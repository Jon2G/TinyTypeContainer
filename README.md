# TinyTypeContainer
 [![NuGet version (TinyTypeContainer)](https://img.shields.io/nuget/v/TinyTypeContainer.svg)](https://www.nuget.org/packages/TinyTypeContainer/)
 
 A tiny single file lightweight implementation of a type container for .net

### Sample usage

```c#
//Enable debug logs or not (default false)
Container.Debug = true;

Console.WriteLine("¿Hello, whats your name?");
//Get a stored instance of type DummyType or activate and register a new one if not stored
Container.GetorActivate<DummyType>().Name = Console.ReadLine();
Console.WriteLine($"Hello {Container.GetRequired<DummyType>().Name}");

//Manually build a type instance and register on container
var disposable = new DummyDisposableType();
Container.Register<DummyDisposableType>(disposable);

//----- Some dummy code ----
disposable.Buffer = Container.GetRequired<DummyType>().Name.ToArray().Select(x => (byte)x).ToArray();
Console.WriteLine($"Buffer size: {disposable.Buffer.Length}");

Console.WriteLine("Press any key to finish");
Console.ReadKey();

Console.WriteLine($"See you later,{Container.GetRequired<DummyType>().Name}");

//Be sure to unregister types from de container if no longer required
Container.Unregister<DummyType>();
//If type implements IDisposable Dispose event will be invoked
Container.Unregister<DummyDisposableType>();
```
### Output:
```console
¿Hello, whats your name?
Jon
Hello Jon
Buffer size: 3
Press any key to finish
See you later,Jon
```

### Debug log:
```console
Get 'Sample.Types.DummyType'
Register 'Sample.Types.DummyType'
Get 'Sample.Types.DummyType'
Get 'Sample.Types.DummyDisposableType'
Register 'Sample.Types.DummyDisposableType'
Get 'Sample.Types.DummyType'
Get 'Sample.Types.DummyType'
Unregister 'Sample.Types.DummyType'
Unregister 'Sample.Types.DummyDisposableType'
```

