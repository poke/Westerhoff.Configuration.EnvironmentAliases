using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Westerhoff.Configuration.EnvironmentAliases.Test
{
    public class ProviderTests
    {
        [Fact]
        public void MappingIsApplied()
        {
            // arrange
            var mapping = new Dictionary<string, string>
            {
                ["key1"] = "Foo:Bar:Baz",
                ["key2"] = "Foo:Bar:Qux",
                ["key3"] = "ConnectionStrings:Default",
            };
            var environmentVariables = new Hashtable
            {
                ["key1"] = "Foo value",
                ["key2"] = "Bar value",
                ["key3"] = "Baz value",
            };

            // act
            var provider = new EnvironmentAliasesConfigurationProvider(mapping);
            provider.Load(environmentVariables);

            // assert
            Assert.True(provider.TryGet("Foo:Bar:Baz", out string value));
            Assert.Equal("Foo value", value);
            Assert.True(provider.TryGet("Foo:Bar:Qux", out value));
            Assert.Equal("Bar value", value);
            Assert.True(provider.TryGet("ConnectionStrings:Default", out value));
            Assert.Equal("Baz value", value);

            Assert.False(provider.TryGet("key1", out value));
            Assert.False(provider.TryGet("key2", out value));
            Assert.False(provider.TryGet("key3", out value));
        }

        [Fact]
        public void UnconfiguredKeysAreIgnored()
        {
            // arrange
            var mapping = new Dictionary<string, string>
            {
                ["key2"] = "Foo:Bar",
                ["key4"] = "Foo:Baz",
            };
            var environmentVariables = new Hashtable
            {
                ["key1"] = "Foo",
                ["key2"] = "Bar",
                ["key3"] = "Baz",
                ["key4"] = "Baz",
            };

            // act
            var provider = new EnvironmentAliasesConfigurationProvider(mapping);
            provider.Load(environmentVariables);

            // assert
            var keyCount = provider.GetChildKeys(Enumerable.Empty<string>(), parentPath: null).Count();
            Assert.Equal(2, keyCount);

            Assert.True(provider.TryGet("Foo:Bar", out _));
            Assert.True(provider.TryGet("Foo:Baz", out _));
        }

        [Fact]
        public void MissingEnvironmentVariablesAreIgnored()
        {
            // arrange
            var mapping = new Dictionary<string, string>
            {
                ["key1"] = "Foo:Bar:Baz",
                ["key2"] = "Foo:Bar:Qux",
                ["key3"] = "ConnectionStrings:Default",
            };
            var environmentVariables = new Hashtable
            {
                ["key2"] = "Bar",
            };

            // act
            var provider = new EnvironmentAliasesConfigurationProvider(mapping);
            provider.Load(environmentVariables);

            // assert
            var keyCount = provider.GetChildKeys(Enumerable.Empty<string>(), parentPath: null).Count();
            Assert.Equal(1, keyCount);

            Assert.True(provider.TryGet("Foo:Bar:Qux", out var value));
            Assert.Equal("Bar", value);
        }

        [Fact]
        public void EmptyMappingReturnsEmptyConfiguration()
        {
            // arrange
            var mapping = new Dictionary<string, string>();
            var environmentVariables = new Hashtable
            {
                ["key1"] = "Foo",
                ["key2"] = "Bar",
                ["key3"] = "Baz",
                ["key4"] = "Baz",
            };

            // act
            var provider = new EnvironmentAliasesConfigurationProvider(mapping);
            provider.Load(environmentVariables);

            // assert
            var keyCount = provider.GetChildKeys(Enumerable.Empty<string>(), parentPath: null).Count();
            Assert.Equal(0, keyCount);
        }

        [Fact]
        public void EmptyEnvironmentVariablesReturnsEmptyConfiguration()
        {
            // arrange
            var mapping = new Dictionary<string, string>
            {
                ["key2"] = "Foo:Bar",
                ["key4"] = "Foo:Baz",
            };
            var environmentVariables = new Hashtable();

            // act
            var provider = new EnvironmentAliasesConfigurationProvider(mapping);
            provider.Load(environmentVariables);

            // assert
            var keyCount = provider.GetChildKeys(Enumerable.Empty<string>(), parentPath: null).Count();
            Assert.Equal(0, keyCount);
        }

        [Fact]
        public void OverwritesPreviousData()
        {
            // arrange
            var mapping = new Dictionary<string, string>
            {
                ["key1"] = "Foo:Bar:Baz",
                ["key2"] = "Foo:Bar:Qux",
                ["key3"] = "ConnectionStrings:Default",
            };
            var environmentVariables = new Hashtable
            {
                ["key1"] = "Foo value",
                ["key2"] = "Bar value",
                ["key3"] = "Baz value",
            };

            // act
            var provider = new EnvironmentAliasesConfigurationProvider(mapping);
            provider.Load(environmentVariables);

            var keyCount = provider.GetChildKeys(Enumerable.Empty<string>(), parentPath: null).Count();
            Assert.NotEqual(0, keyCount);

            environmentVariables = new Hashtable
            {
                ["key3"] = "Baz",
            };
            provider.Load(environmentVariables);

            // assert
            keyCount = provider.GetChildKeys(Enumerable.Empty<string>(), parentPath: null).Count();
            Assert.Equal(1, keyCount);

            Assert.True(provider.TryGet("ConnectionStrings:Default", out var value));
            Assert.Equal("Baz", value);
        }

        [Fact]
        public void DuplicateKeysWithDifferentCasing()
        {
            // arrange
            var mapping = new Dictionary<string, string>
            {
                ["key1"] = "Foo:Bar:Baz",
                ["key2"] = "Foo:Bar:Qux",
            };
            var environmentVariables = new Hashtable
            {
                ["key1"] = "Foo value 1",
                ["Key1"] = "Foo value 2",
                ["kEY1"] = "Foo value 3",
            };

            // act
            var provider = new EnvironmentAliasesConfigurationProvider(mapping);
            provider.Load(environmentVariables);

            // assert
            var keyCount = provider.GetChildKeys(Enumerable.Empty<string>(), parentPath: null).Count();
            Assert.Equal(1, keyCount);

            Assert.True(provider.TryGet("Foo:Bar:Baz", out var value));
            Assert.Equal("Foo value", value.Substring(0, 9));
        }
    }
}
