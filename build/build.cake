#addin "Cake.FileHelpers&version=3.1.0"
#addin nuget:?package=Cake.ArgumentHelpers&version=0.3.0
#tool "nuget:?package=OpenCover&version=4.7.922"
#tool "nuget:?package=ReportGenerator&version=4.0.14"

using System.Xml.XPath;

// Get Build Arguments
var runtime = ArgumentOrEnvironmentVariable("Runtime", "") ?? "win81-x64";

// Setup Parameters
var configuration = "Release";

// Log Parameters
Information("========================================");
Information("BUILD PARAMETERS");
Information("========================================");
Information($"Configuration: {configuration}");
Information($"Runtime: {runtime}");

DotNetCoreTestSettings TestSettings
{
    get
    {
        return new DotNetCoreTestSettings
        {
            Configuration = configuration,
            NoRestore = true,
            NoBuild = true,
            ArgumentCustomization = args => args.Append("-v normal"),
        };
    }
}

Task("Clean")
    .Does(() =>
    {
        CleanDirectories("../**/**/bin");
        Information("Cleaning 'bin' folders.");
        CleanDirectories("../**/**/obj");
        Information("Cleaning 'obj' folders.");
        CleanDirectories("../publish");
        Information("Cleaning 'publish' folder.");
        CleanDirectories("../Coverage");
        Information("Cleaning 'coverage' folder.");
    });

Task("Restore")
    .Does(() =>
    {
        DotNetCoreRestore(
            $"../Palindromes.sln",
            new DotNetCoreRestoreSettings
            {
                Sources = new[]
                {
                    "https://api.nuget.org/v3/index.json",
                },
                Runtime = runtime,
            });
    });

Task("BuildForTests")
    .Does(() =>
    {
        DotNetCoreBuild(
            $"../Palindromes.sln",
            new DotNetCoreBuildSettings
            {
                Configuration = configuration,
                NoRestore = true,
            });
    });

Task("UnitTests")
    .Does(() =>
    {
        var testSettings = TestSettings;
        testSettings.Filter = "TestCategory=Unit";
        DotNetCoreTest(
            "../test/Palindromes.Tests/Palindromes.Tests.csproj",
        testSettings);
    });

Task("Coverage")
    .Does(() =>
    {
        var outputFolder = "../coverage";
        var reportFile = new FilePath($"{outputFolder}/coverage.xml");
        EnsureDirectoryExists(outputFolder);
        OpenCover(tool =>
        {
            tool.DotNetCoreTest(
                $"../Palindromes.sln",
                TestSettings);
        },
        reportFile,
        new OpenCoverSettings()
        {
            ArgumentCustomization = args => args.Append("-oldstyle"),
        }
        .WithFilter($"+[Palindromes*]*")
        .WithFilter("-[*Tests*]*")
        .ExcludeByAttribute("*.ExcludeFromCodeCoverageAttribute"));
        try
        {
            ReportGenerator(reportFile, outputFolder);
        }
        catch (Exception e)
        {
            Information("Generating coverage report failed.");
            Information(e);
        }
    });

Task("BuildConsoleApp")
    .Does(() =>
    {
        DotNetCorePublish(
            "../src/Palindromes/Palindromes.csproj",
            new DotNetCorePublishSettings
            {
                Framework = "netcoreapp2.2",
                Configuration = configuration,
                Runtime = runtime,
                OutputDirectory = "../publish",
                SelfContained = false,
            });
    });

Task("Test")
    .IsDependentOn("BuildForTests")
    .IsDependentOn("Coverage");

Task("Build")
    .IsDependentOn("BuildConsoleApp");

Task("Default")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .IsDependentOn("Test")
    .IsDependentOn("Build");

var target = Argument("target", "Default");

RunTarget(target);