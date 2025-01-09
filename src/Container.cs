using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TinyTypeContainer
{
#nullable enable
    public static class Container
    {
        public static bool Debug { get; set; } = false;
        public static bool IsEmpty => InternalCollection.Count == 0;
        private static Dictionary<Type, object?> _internalCollection = new Dictionary<Type, object?>();

        public static Dictionary<Type, object?> InternalCollection
        {
            get
            {
                if (_internalCollection is null)
                {
                    _internalCollection = new();
                }
                return _internalCollection;
            }
        }
        public static bool Has<T>()
        {
            return Has(typeof(T));
        }
        public static bool Has(Type type)
        {
            lock (InternalCollectionLock)
            {
                DebugLog.WriteLine($"{nameof(Has)} '{type.FullName}'\n");
                return InternalCollection.ContainsKey(type);
            }
        }
        public static bool Has(object val)
        {
            if (val is null) { return false; }
            return Has(val.GetType());
        }

        public static void Unregister<T>()
        {
            Unregister(typeof(T));
        }
        public static void Unregister(object? val)
        {
            if (val is null) { return; }
            Unregister(val.GetType());
        }

        public static void Unregister(KeyValuePair<Type, object>? type)
        {
            if (type.HasValue)
            {
                if (InternalCollection[type.Value.Key] is IDisposable disposable)
                {
                    disposable.Dispose();
                }
                InternalCollection.Remove(type.Value.Key);
            }
        }
        public static void Unregister(Type type)
        {
            DebugLog.WriteLine(nameof(Unregister) + type.FullName);
            KeyValuePair<Type, object>? registeredMatchingType = GetKeyPair(type);
            if (registeredMatchingType?.Key is not null)
            {
                Unregister(registeredMatchingType);
                return;
            }


            throw new InvalidOperationException($"Type '{type.FullName}' not found in container");
        }

        private static KeyValuePair<Type, object?>? GetKeyPair(Type type)
        {
            return InternalCollection.FirstOrDefault(x => x.Key == type
                                                          || x.Key.GetTypeInfo().IsAssignableFrom(type)
                                                          || type.GetTypeInfo().IsAssignableFrom(x.Key)
            );
        }

        public static object? Register(Type type, object? value)
        {
            lock (InternalCollectionLock)
            {
                DebugLog.WriteLine(nameof(Register) + type.FullName);

                var registeredMatchingType = GetKeyPair(type);
                if (registeredMatchingType.Value.Key is not null)
                {
                    Unregister(registeredMatchingType);
                }

                if (value is null) { return value; }
                InternalCollection.Add(type, value);
                return value;
            }
        }

        public static T? Register<T>(T? value)
        {
            return (T?)Register(typeof(T), value);
        }


        public static T GetorActivate<T>() where T : new()
        {
            var result = Get<T>();
            if (result is null)
            {
                result = new T();
                Register(result);
            }
            return result;
        }
        public static T GetRequired<T>()
        {
            var result = Get<T>();
            if (result is null)
            {
                throw new InvalidOperationException($"Type '{typeof(T).FullName}' not found in container");
            }
            return result;
        }

        public static readonly object InternalCollectionLock = new object();
        public static object? Get(Type type)
        {
            lock (InternalCollectionLock)
            {
                DebugLog.WriteLine($"{nameof(Get)} '{type.FullName}'");
                var registeredMatchingType = GetKeyPair(type);

                if (registeredMatchingType?.Key is not null)
                {
                    return InternalCollection[registeredMatchingType.Value.Key];
                }
                return null;
            }
        }
        public static T? Get<T>()
        {
            return (T?)Get(typeof(T));
        }

        /// <summary>
        /// Copies the values of src object into the current instance of registred value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src">Source object</param>
        /// <returns></returns>
        public static T? Patch<T>(T? src)
        {
            T? current = Get<T>();
            if (src is null || current is null)
            {
                return current;
            }
            foreach (var property in typeof(T).GetProperties())
            {
                if (property.CanWrite && property.SetMethod != null)
                {
                    property.SetValue(current, property.GetValue(src));
                }
            }
            return Register<T>(current);
        }
        public static T? Patch<T>(Func<T?, T?> action)
        {
            DebugLog.WriteLine(nameof(Patch) + typeof(T).FullName);
            return Register<T>(action(Get<T>()));
        }

        public static void Clear()
        {
            InternalCollection.Clear();
        }
    }
#nullable disable
}
