# Obaki.LocalStorageCache
This is a simple library that allows you to easily cache data in the browser's local storage. It provides a simple API for storing and retrieving data  based on a specified time-to-live (TTL). The library make use of [Blazored LocalStorage](https://github.com/Blazored/LocalStorage) for storing cache data.

Overall, Obaki.LocalStorageCache is a simple and easy-to-use library that can help you improve the performance of your web application by caching data locally in the browser.

**NOTE:** As of this writing the library has a bare minimum method that only fetch the cache data or refresh the cache based on the specified time-to-live asynchronously.
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
Register the needed services in your Program.cs file as **Scope**

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
```c#
using Obaki.LocalStorageCache;

public class Test {
  private readonly ILocalStorageCache _localStorageCache;
  
  public Test(ILocalStorageCache localStorageCache) {
    _localStorageCache = localStorageCache;
  }

  public async Task<TCacheData> GetData() {
    var cache = await _localStorageCache.GetOrCreateCacheAsync(
      Key, //Define Key
      TimeSpan.FromHours(1), //TTL
       async () =>
         return await GetNewData(); //Refresh cache data.
      });
      
    return cache ?? default;
  }
}
```
## What's Next?
Working to mask the cache data inside the  local storage. Masking != Secure.  

# License
MIT License
