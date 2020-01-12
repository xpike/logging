# Change Log

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