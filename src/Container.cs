using System;
using System.Collections.Generic;

namespace TinyTypeContainer
{
#nullable enable
    public static class Container
    {
        public static bool Debug { get; set; } = false;

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
            DebugLog.WriteLine($"{nameof(Has)} '{type.FullName}'\n");
            return InternalCollection.ContainsKey(type);
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
        public static void Unregister(Type type)
        {

            DebugLog.WriteLine(nameof(Unregister) + type.FullName);

            if (InternalCollection.ContainsKey(type))
            {
                if (InternalCollection[type] is IDisposable disposable)
                {
                    disposable.Dispose();
                }
                InternalCollection.Remove(type);
            }
        }
        public static object? Register(Type type, object? value)
        {
            DebugLog.WriteLine(nameof(Register) + type.FullName);

            if (InternalCollection.ContainsKey(type))
            {
                InternalCollection.Remove(type);
            }
            if (value is null) { return value; }
            InternalCollection.Add(type, value);
            return value;
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


        public static object? Get(Type type)
        {
            DebugLog.WriteLine($"{nameof(Get)} '{type.FullName}'");
            if (InternalCollection.ContainsKey(type))
            {
                return InternalCollection[type];
            }
            return null;
        }
        public static T? Get<T>()
        {
            return (T?)Get(typeof(T));
        }

        public static T? Patch<T>(Func<T?, T?> action)
        {
            DebugLog.WriteLine(nameof(Patch) + typeof(T).FullName);
            return Register<T>(action(Get<T>()));
        }
    }
#nullable disable
}
