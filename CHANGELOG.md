# Change Log

## [2.0.0]

### Breaking Changes

- Interface changes to `ITraceContext` and `ITraceContextProvider` to make implementations thread-safe.
  - `IDictionary<string,string>` has been replace with `IReadOnlyDictionary<string,string>`
  - Thread-safe `GetXXXX(...)` methods were added. 

### Bug Fixes

- NetUtil is now thread-safe

## [1.3.1]

- Added request logging middleware

## [1.3.0]

- Updated to latest IoC and Configuration packages.
- Simplified setup to a single call, similar to IoC and Configuration.

## [1.2.2]

- Dual support for IHostBuilder and IWebHostBuilder

## [1.2.1]

- Additional cleanup of registration/configuration extension methods

## [1.2.0]

- Added a few package registration extension methods
- Added comments to extension methods
- Pulled latest config package
- Changed default Package registration to leave existing ILogProviders intact

## [1.1.1]

- Pulled latest config package

## [1.1.0]

- Removed dependency on `XPike.Settings`.
- Configuration now accessed via `IConfigManager<T>`.
- `ConfigureAwait(false)` is now called wherever `await` is used.

## [1.0.0]

Initial release.