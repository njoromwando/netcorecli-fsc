using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.DotNet.Tools.Test.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using FluentAssertions;
using static NetcoreCliFsc.Tests.TestSuite;

namespace NetcoreCliFsc.Tests
{
    public class GivenThatIWantANewFSApp : TestBase
    {
        [Theory]
        [InlineData("console")]
        [InlineData("lib")]
        [InlineData("mvc")]
        [InlineData("mstest")]
        [InlineData("xunit")]
        public void Check_default_templates(string type)
        {
            var rootPath = Temp.CreateDirectory().Path;

            Func<string,TestCommand> test = name => new TestCommand(name) { WorkingDirectory = rootPath };

            test("dotnet")
                .Execute($"new {type} -lang F#")
                .Should().Pass();

            test("dotnet")
                .Execute($"restore {RestoreDefaultArgs} {RestoreSourcesArgs(NugetConfigSources)}")
                .Should().Pass();

            test("dotnet")
                .Execute($"build {LogArgs}")
                .Should().Pass();

            if (type == "console")
            {
                test("dotnet")
                    .Execute($"run {LogArgs}")
                    .Should().Pass();
            }

            if (type == "mstest" || type == "xunit")
            {
                test("dotnet")
                    .Execute($"test {LogArgs}")
                    .Should().Pass();
            }
        }


        /*
        [Fact]
        public void When_NewtonsoftJson_dependency_added_Then_project_restores_and_runs()
        {
            var rootPath = Temp.CreateDirectory().Path;
            var projectJsonFile = Path.Combine(rootPath, "project.json");

            new TestCommand("dotnet") { WorkingDirectory = rootPath }
                .Execute("new --lang fsharp");
            
            GivenThatIWantANewCSApp.AddProjectJsonDependency(projectJsonFile, "Newtonsoft.Json", "7.0.1");

            new TestCommand("dotnet") { WorkingDirectory = rootPath }
                .Execute("restore")
                .Should().Pass();

            new TestCommand("dotnet") { WorkingDirectory = rootPath }
                .Execute("run")
                .Should().Pass();
        }
        */
    }
}
