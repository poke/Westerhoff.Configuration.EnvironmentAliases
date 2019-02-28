using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Westerhoff.Configuration.EnvironmentAliases
{
    /// <summary>
    /// Configuration source for environment variable aliases.
    /// </summary>
    public class EnvironmentAliasesConfigurationSource : IConfigurationSource
    {
        /// <summary>
        /// Mapping of environment variable names to configuration paths.
        /// </summary>
        public IDictionary<string, string> KeyMapping
        { get; set; } = new Dictionary<string, string>();

        /// <inheritdoc />
        public IConfigurationProvider Build(IConfigurationBuilder builder)
            => new EnvironmentAliasesConfigurationProvider(KeyMapping);
    }
}
