using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Westerhoff.Configuration.EnvironmentAliases.Test
{
    public class IntegrationTests
    {
        [Fact]
        public void VariablesAreMapped()
        {
            Environment.SetEnvironmentVariable("APP_TEST_DATABASE", "Data Source=mydatabase.db");
            Environment.SetEnvironmentVariable("APP_TEST_NUMBER", "12345");
            Environment.SetEnvironmentVariable("APP_TEST_STUFF", "Foo bar baz");
            Environment.SetEnvironmentVariable("APP_TEST_NOTMAPPED", "This should not appear anywhere");

            var configuration = new ConfigurationBuilder()
                .AddEnvironmentAliases(new Dictionary<string, string>()
                {
                    ["APP_TEST_DATABASE"] = "ConnectionStrings:Default",
                    ["APP_TEST_NUMBER"] = "Settings:Nested:Number",
                    ["APP_TEST_STUFF"] = "Stuff",
                    ["APP_TEST_NOTSPECIFIED"] = "Settings:Text",
                })
                .Build();

            Assert.Equal(6, configuration.AsEnumerable().Count());

            var keys = configuration.AsEnumerable().Select(kv => kv.Key).ToList();
            Assert.Contains("ConnectionStrings", keys);
            Assert.Contains("ConnectionStrings:Default", keys);
            Assert.Contains("Settings", keys);
            Assert.Contains("Settings:Nested", keys);
            Assert.Contains("Settings:Nested:Number", keys);
            Assert.Contains("Stuff", keys);

            Assert.Equal("Data Source=mydatabase.db", configuration.GetConnectionString("Default"));
            Assert.Equal("12345", configuration["Settings:Nested:Number"]);
            Assert.Equal("Foo bar baz", configuration["Stuff"]);
        }
    }
}
