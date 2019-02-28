using System;
using System.Collections.Generic;
using Westerhoff.Configuration.EnvironmentAliases;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// Extension methods for registering <see cref="EnvironmentAliasesConfigurationProvider"/>
    /// with <see cref="IConfigurationBuilder"/>.
    /// </summary>
    public static class EnvironmentAliasesExtensions
    {
        /// <summary>
        /// Add a configuration provider that reads configuration values from
        /// environment variables after applying a key mapping.
        /// </summary>
        /// <param name="configurationBuilder">Configuration builder.</param>
        /// <param name="keyMapping">Mapping of environment variable names to configuration paths.</param>
        /// <returns>Configuration builder.</returns>
        public static IConfigurationBuilder AddEnvironmentAliases(this IConfigurationBuilder configurationBuilder, IDictionary<string, string> keyMapping)
        {
            configurationBuilder.Add(new EnvironmentAliasesConfigurationSource()
            {
                KeyMapping = keyMapping,
            });
            return configurationBuilder;
        }

        /// <summary>
        /// Add a configuration provider that reads configuration values from
        /// environment variables after applying a key mapping.
        /// </summary>
        /// <param name="builder">Configuration builder.</param>
        /// <param name="configureSource">Configures the source.</param>
        /// <returns>Configuration builder.</returns>
        public static IConfigurationBuilder AddEnvironmentAliases(this IConfigurationBuilder builder, Action<EnvironmentAliasesConfigurationSource> configureSource)
            => builder.Add(configureSource);
    }
}
