# Obaki.LocalSorageCache
This is a simple library that allows you to easily cache data in the browser's local storage. It provides a simple method for storing and retrieving data  based on a specified time-to-live (TTL). The library make use of [Blazored LocalStorage](https://github.com/Blazored/LocalStorage) for storing cache data.

Overall, Obaki.LocalStorageCache is a simple and easy-to-use library that can help you improve the performance of your web application by caching data locally in the browser.

**NOTE:** As of this writing the library has a bare minimum method that only fetch the cache data and refresh the cache based on the specified time-to-live(hrs)
## Installing

To install the package add the following line inside your csproj file with the latest version.:

```
<PackageReference Include="Obaki.LocalSorageCache" Version="x.x.x" />
```

An alternative is to install via the .NET CLI with the following command:

```
dotnet add package Obaki.LocalSorageCache
```

For more information you can check the [nuget package](https://www.nuget.org/packages/Obaki.LocalSorageCache).

## Setup
# License
MIT License
