# Obaki.LocalStorageCache
This is a simple library that allows you to easily cache data in the browser's local storage. It provides a simple API for storing and retrieving data  based on a specified time-to-live (TTL). The library make use of [Blazored LocalStorage](https://github.com/Blazored/LocalStorage) for storing cache data.

Overall, Obaki.LocalStorageCache is a simple and easy-to-use library that can help you improve the performance of your web application by caching data locally in the browser.

## Installing

To install the package add the following line inside your csproj file with the latest version.

```
<PackageReference Include="Obaki.LocalStorageCache" Version="x.x.x" />
```

An alternative is to install via the .NET CLI with the following command:

```
dotnet add package Obaki.LocalStorageCache
```

For more information you can check the [nuget package](https://www.nuget.org/packages/Obaki.LocalStorageCache).

## Setup
Register the needed services in your Program.cs file as **Scoped**

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddLocalStorageCacheAsScoped();
}
``` 

Or as **Singleton**

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddLocalStorageCacheAsSingleton();
}
```
## Usage 
**Asynchronous(Non-Blocking)**
```c#
using Obaki.LocalStorageCache;

public class TestAsync {
  private readonly ILocalStorageCache _localStorageCache;
  
  public TestAsync(ILocalStorageCache localStorageCache) {
    _localStorageCache = localStorageCache;
  }

  public async Task<TCacheData> GetDataAsync() {
    var cache = await _localStorageCache.GetOrCreateCacheAsync(
      Key, //Define Key
      TimeSpan.FromHours(1), //TTL
       async () =>
         return await GetNewData<TCacheData>(); //Refresh cache data.
      });
      
    return cache ?? default;
  }
}
```
**Synchronous(Blocking)**
```c#
using Obaki.LocalStorageCache;

public class TestSync {
  private readonly ILocalStorageCacheSync _localStorageCacheSync;
  
  public TestSync(ILocalStorageCacheSync localStorageCacheSync) {
    _localStorageCacheSync = localStorageCacheSync;
  }

  public TCacheData GetDataSync() {
    var cache = _localStorageCacheSync.GetOrCreateCache(
      Key, //Define Key
      TimeSpan.FromHours(1), //TTL
        () =>
         return GetNewData<TCacheData>(); //Refresh cache data.
      });
      
    return cache ?? default;
  }
}
```
## Disclaimer
Use only this library for caching non sensitive data.
If you are working with highly private and confidential data , you should not be storing this data in your client's browser.
# License
MIT License
