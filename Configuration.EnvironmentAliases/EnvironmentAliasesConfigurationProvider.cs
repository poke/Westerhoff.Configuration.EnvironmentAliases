using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Westerhoff.Configuration.EnvironmentAliases
{
    /// <summary>
    /// Configuration provider for environment variable aliases.
    /// </summary>
    public class EnvironmentAliasesConfigurationProvider : ConfigurationProvider
    {
        /// <summary>
        /// Mapping of environment variable names to configuration paths.
        /// </summary>
        public IDictionary<string, string> KeyMapping
        { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Create a new provider with an empty mapping.
        /// </summary>
        public EnvironmentAliasesConfigurationProvider()
        { }

        /// <summary>
        /// Create a new provider with the specified mapping.
        /// </summary>
        /// <param name="mapping">Existing mapping.</param>
        public EnvironmentAliasesConfigurationProvider(IDictionary<string, string> mapping)
        {
            if (mapping != null)
            {
                foreach (var item in mapping)
                    KeyMapping.Add(item);
            }
        }

        /// <inheritdoc />
        public override void Load()
        {
            Load(Environment.GetEnvironmentVariables());
        }

        /// <summary>
        /// Load configuration from environment variables.
        /// </summary>
        /// <param name="environmentVariables">Environment variables.</param>
        public void Load(IDictionary environmentVariables)
        {
            Data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (DictionaryEntry entry in environmentVariables)
            {
                if (KeyMapping.ContainsKey((string)entry.Key))
                    Data[KeyMapping[(string)entry.Key]] = (string)entry.Value;
            }
        }
    }
}
