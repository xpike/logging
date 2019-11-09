# XPike.Settings

[![Build Status](https://dev.azure.com/xpike/xpike/_apis/build/status/xpike-settings?branchName=master)](https://dev.azure.com/xpike/xpike/_build/latest?definitionId=4&branchName=master)
![Nuget](https://img.shields.io/nuget/v/XPike.Settings)

Provides application settings management for xPike.

## Overview

xPike Settings is the recommended source for application settings.

It uses xPike Configuration as its configuration source by default, and other providers can be added.
In addition, it adds a configurable layer of caching, as well as settings refresh capabilities.

Settings Providers are fully DI-managed objects (unlike Configuration Providers).

## Exported Services

### Singletons

- **`ISettingsService`**  
  **=> `SettingsService`**

- **`ISettingsManager<>`**  
  **=> `SettingsManager<>`**

- **`ISettings<>`**  
  **=> `SettingsLoader<>`**

- **`IConfigurationSettingsProvider`**  
  **=> `ConfigurationSettingsProvider`**

### Collections

- **`IEnumerable<ISettingsProvider>`**  
  **Add: `IConfigurationSettingsProvider`**  
  
  ***Note:*** You should not inject this directly - only access it via ISettingsService.
  Injected collections **are not thread-safe**.

## Usage

### Define your Settings POCO

```cs
using XPike.Settings;

namespace MyLibrary
{
    public class MySettings
    {
        public string CompanyName { get; set; }
        public IList<DateTime> UpcomingHolidays { get; set; }
        public IDictionary<string, string> LocationPhoneNumbers { get; set; }
    }
}
```

### Load Automatically from JSON

xPike supports a "touchless" settings system.

**Just inject `ISettings<MySettings>` wherever you want.**

xPike will search for a setting with a matching fully-qualified class name (eg `"MyLibrary.MySettings"`).
The settings will be deserialized from a JSON string.  **Note:** This may vary by Provider.

The value can come from any of the registered Configuration or Settings Providers.
xPike Settings will handle caching and periodic refresh automatically.

### Specifying Options

The built-in Settings Manager allows you to specify a custom configuration key.  You can also specify an action
to be called upon load to further customize your settings.

```
container.RegisterSingleton<ISettingsManager<MySettings>>(provider =>
    new SettingsManager(provider.ResolveDependency<ISettingsService>(),
        "myCustomKey",
        settings =>
        {
            // further customization of settings object

            return settings;
        });
```



### Custom Settings Manager

For the most flexibility, you can specify your own Settings Manager.

```cs
using XPike.Settings;

namespace MyLibrary
{
    public class MySettingsManager
        : SettingsManagerBase<MySettings>
    {
        protected override string ConfigurationKey => $"{nameof(MyLibrary)}::{nameof(MySettings)}";

        public MySettingsManager(ISettingsService settingsService)
            : base(settingsService)
        {
        }

        public override ISettings<TSettings> GetSettings()
        {
            var settings = new MySettings
            {
                CompanyName = GetValue("CompanyName"),
                UpcomingHolidays = GetValue<IList<DateTime>>("UpcomingHolidays"),
                LocationPhoneNumbers = string.Split(new[] {','},
                                                    GetValueOrDefault("LocationPhoneNumbers", "555-1212"))
            };

            return new Settings(ConfigurationKey, settings);
        }
    }
}
```

#### Registering your Custom Settings Manager

`container.RegisterSingleton<ISettingsManager<MySettings>, MySettingsManager>();`

**Note:** If you are using SimpleInjector for dependency injection, then you must be certain
that any custom Settings Managers are registered ***before the Settings Package***.  Since a
Package can load other Packages, this may require that you define them at the start of your
top-level Package.

#### Some things to note:

- The `GetValue()` and `GetValueOrDefault()` overloads on `SettingsManager<TSettings>` prepend the `ConfigurationKey` to the specified `key`.

- The default `ConfigurationKey` is the full class and namespace of the Settings POCO.

- In this example, the call to `GetValue<IList<DateTime>>(...)` loads its value from a JSON string in the `MyLibrary.MySettings::UpcomingHolidays` key.

## Dependencies

- `XPike.Configuration`

## Building and Testing

Building from source and running unit tests requires a Windows machine with:

* .Net Core 3.0 SDK
* .Net Framework 4.6.1 Developer Pack

## Issues

Issues are tracked on [GitHub](https://github.com/xpike/xpike-ioc/issues). Anyone is welcome to file a bug,
an enhancement request, or ask a general question. We ask that bug reports include:

1. A detailed description of the problem
2. Steps to reproduce
3. Expected results
4. Actual results
5. Version of the package xPike
6. Version of the .Net runtime

## Contributing

See our [contributing guidelines](https://github.com/xpike/documentation/blob/master/docfx_project/articles/contributing.md)
in our documentation for information on how to contribute to xPike.

## License

xPike is licensed under the [MIT License](LICENSE).
