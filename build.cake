//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// GLOBALS
//////////////////////////////////////////////////////////////////////

const string SLN = "./mystery-island.sln";
const string CSPROJ = "./src/MysteryIsland/MysteryIsland.csproj";
const string ARTIFACTS = "./artifacts";

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("build")
    .Does(() =>
{
    DotNetCoreBuild(SLN,
        new DotNetCoreBuildSettings
        {
            Configuration = configuration
        }
    );
});

Task("publish")
    .Does(() =>
{
    var runtimes = new string[]{
        "win-x64", "linux-x64", "osx-x64"
    };
    foreach(var runtime in runtimes)
    {
        var dir = System.IO.Path.Join(ARTIFACTS, runtime);
        DotNetCorePublish(CSPROJ,
            new DotNetCorePublishSettings
            {
                Configuration = configuration,
                OutputDirectory = dir,
                SelfContained = true,
                Runtime = runtime
            }
        );

        StartProcess("zip", new ProcessSettings {
            Arguments = new ProcessArgumentBuilder()
                .Append($"-r {dir}")
                .Append($"{ARTIFACTS}/mystery-island-{runtime}.zip")
                .Append(dir)
        });
    }
});


//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("build");

Task("azure-pipelines")
    .IsDependentOn("publish");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
