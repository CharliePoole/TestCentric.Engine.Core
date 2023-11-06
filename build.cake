// Load the recipe
#load nuget:?package=TestCentric.Cake.Recipe&version=1.1.0-dev00058
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
	solutionFile: "TestCentric.Engine.Core.sln",
	githubRepository: "TestCentric.Engine.Core",
	unitTests: "**/*.tests.exe|**/*.tests.dll"
);

//////////////////////////////////////////////////////////////////////
// DEFINE PACKAGE
//////////////////////////////////////////////////////////////////////

BuildSettings.Packages.Add(new NuGetPackage(
	id: "TestCentric.Engine.Core",
	title: "TestCentric Engine Core Assembly",
	source: "nuget/TestCentric.Engine.Core.nuspec",
	basePath: "src/TestCentric.Engine.Core/bin/" + BuildSettings.Configuration,
	checks:new PackageCheck[] {
		HasFiles("LICENSE.txt", "README.md", "testcentric.png"),
		HasDirectory("lib/net20").WithFiles(
			"testcentric.engine.core.dll", "testcentric.engine.core.pdb"),
		HasDirectory("lib/net462").WithFiles(
			"testcentric.engine.core.dll", "testcentric.engine.core.pdb"),
		HasDirectory("lib/netstandard2.0").WithFiles(
			"testcentric.engine.core.dll", "testcentric.engine.core.pdb"),
		HasDirectory("lib/netcoreapp3.1").WithFiles(
			"testcentric.engine.core.dll", "testcentric.engine.core.pdb")
	}));

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
