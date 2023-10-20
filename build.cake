const string ENGINE_PACKAGE_ID = "TestCentric.Engine";
const string ENGINE_CORE_PACKAGE_ID = "TestCentric.Engine.Core";
const string ENGINE_API_PACKAGE_ID = "TestCentric.Engine.Api";

const string TEST_BED_EXE = "test-bed.exe";

// Load the recipe
#load nuget:?package=TestCentric.Cake.Recipe&version=1.1.0-dev00054
// Comment out above line and uncomment below for local tests of recipe changes
//#load ../TestCentric.Cake.Recipe/recipe/*.cake

using System.Xml;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Threading.Tasks;

//////////////////////////////////////////////////////////////////////
// INITIALIZE BUILD SETTINGS
//////////////////////////////////////////////////////////////////////

BuildSettings.Initialize(
	Context,
	"TestCentric.Engine",
	solutionFile: "testcentric-engine.sln",
	githubRepository: "testcentric-engine",
	unitTests: "**/*.tests.exe|**/*.tests.dll"
);

//////////////////////////////////////////////////////////////////////
// DEFINE PACKAGE
//////////////////////////////////////////////////////////////////////

BuildSettings.Packages.Add(new NuGetPackage(
	id: "TestCentric.Engine.Core",
	title: "TestCentric Engine Core Assembly",
	description: "This package includes the TestCentric engine.core assembly, which forms part of the TestCentric engine. It is provided in a separate package use in creating pluggable agents.",
	basePath: "src/TestEngine/testcentric.engine.core/bin/" + BuildSettings.Configuration,
	packageContent: new PackageContent(
		new FilePath[] { "../../../../../LICENSE.txt", "../../../../../testcentric.png" },
		new DirectoryContent("lib/net20").WithFiles(
			"net20/testcentric.engine.core.dll", "net20/testcentric.engine.core.pdb", "net20/testcentric.engine.api.dll",
			"net20/testcentric.engine.metadata.dll", "net20/testcentric.extensibility.dll", "net20/testcentric.extensibility.api.dll" ),
		new DirectoryContent("lib/net462").WithFiles(
			"net462/testcentric.engine.core.dll", "net462/testcentric.engine.core.pdb", "net462/testcentric.engine.api.dll",
			"net462/testcentric.engine.metadata.dll", "net462/testcentric.extensibility.dll", "net462/testcentric.extensibility.api.dll" ),
		new DirectoryContent("lib/netstandard2.0").WithFiles(
			"netstandard2.0/testcentric.engine.core.dll", "netstandard2.0/testcentric.engine.core.pdb", "netstandard2.0/testcentric.engine.api.dll",
			"netstandard2.0/testcentric.engine.metadata.dll", "netstandard2.0/testcentric.extensibility.dll", "netstandard2.0/testcentric.extensibility.api.dll" ),
		new DirectoryContent("lib/netcoreapp3.1").WithFiles(
			"netcoreapp3.1/testcentric.engine.core.dll", "netcoreapp3.1/testcentric.engine.core.pdb", "netcoreapp3.1/testcentric.engine.api.dll",
			"netcoreapp3.1/testcentric.engine.metadata.dll", "netcoreapp3.1/testcentric.extensibility.dll", "netcoreapp3.1/testcentric.extensibility.api.dll",
			"netcoreapp3.1/Microsoft.Extensions.DependencyModel.dll" )),
	checks:new PackageCheck[] {
		HasFiles("LICENSE.txt", "testcentric.png"),
		HasDirectory("lib/net20").WithFiles(
			"testcentric.engine.core.dll", "testcentric.engine.core.pdb", "testcentric.engine.api.dll",
			"testcentric.engine.metadata.dll", "testcentric.extensibility.dll", "testcentric.extensibility.api.dll"),
		HasDirectory("lib/net462").WithFiles(
			"testcentric.engine.core.dll", "testcentric.engine.core.pdb", "testcentric.engine.api.dll",
			"testcentric.engine.metadata.dll", "testcentric.extensibility.dll", "testcentric.extensibility.api.dll"),
		HasDirectory("lib/netstandard2.0").WithFiles(
			"testcentric.engine.core.dll", "testcentric.engine.core.pdb", "testcentric.engine.api.dll",
			"testcentric.engine.metadata.dll", "testcentric.extensibility.dll", "testcentric.extensibility.api.dll"),
		HasDirectory("lib/netcoreapp3.1").WithFiles(
			"testcentric.engine.core.dll", "testcentric.engine.core.pdb", "testcentric.engine.api.dll",
			"testcentric.engine.metadata.dll", "testcentric.extensibility.dll", "testcentric.extensibility.api.dll",
			"Microsoft.Extensions.DependencyModel.dll")
	}));
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("AppVeyor")
	.Description("Targets to run on AppVeyor")
	.IsDependentOn("DumpSettings")
	.IsDependentOn("Build")
	.IsDependentOn("Test")
	.IsDependentOn("Package")
	.IsDependentOn("Publish")
	.IsDependentOn("CreateDraftRelease")
	.IsDependentOn("CreateProductionRelease");

Task("Travis")
	.Description("Targets to run on Travis")
    .IsDependentOn("Build")
    .IsDependentOn("Test");

Task("Default")
    .IsDependentOn("Build");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(CommandLineOptions.Target);
