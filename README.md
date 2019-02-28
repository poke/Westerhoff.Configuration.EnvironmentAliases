# Westerhoff.Configuration.EnvironmentAliases

[![See this package on NuGet](https://img.shields.io/nuget/v/Westerhoff.Configuration.EnvironmentAliases.svg?style=flat-square)](https://www.nuget.org/packages/Westerhoff.Configuration.EnvironmentAliases)

A configuration provider for [`Microsoft.Extension.Configuration`](https://www.nuget.org/packages/Microsoft.Extensions.Configuration) that reads configuration values from environment variables according to a mapping to configuration paths. This does essentially the same as [`Microsoft.Extensions.Configuration.EnvironmentVariables`](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.EnvironmentVariables) except that not all (prefixed) environments variables are loaded but just the ones that are explicitly mapped to a configuration path. In addition, this allows to use aliases for more complex configuration paths.

## Instructions

To use this configuration provider, call the `AddEnvironmentAliases` extension method on an `IConfigurationBuilder`. For example, when using ASP.NET Core, call the `ConfigureAppConfiguration` method on the web host builder to add the provider:

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(config => {

                config.AddEnvironmentAliases(new Dictionary<string, string>
                {
                    ["APP_DATABASE"] = "ConnectionStrings:Default",
                    ["APP_NUMBER"] = "Settings:Number",
                    ["LOGGING_VERBOSITY"] = "Logging:LogLevel:Default",
                });

            })
            .UseStartup<Startup>();

Afterwards, environment variables `APP_DATABASE`, `APP_NUMBER` and `LOGGING_VERBOSITY` can be used to configure the values at the specified configuration paths.
