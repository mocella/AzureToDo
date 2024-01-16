using System;
using System.Collections.Generic;
using System.Linq;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.Coverlet;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.CI.AzurePipelines;

class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    [Solution] readonly Solution Solution;

    public AbsolutePath OutputDirectory => RootDirectory / "output";
    public AbsolutePath TestResultDirectory => OutputDirectory / "test-results";
    
    //Reference to the azure pipeline
    AzurePipelines AzurePipelines => AzurePipelines.Instance;

    public static int Main() => Execute<Build>(x => x.UnitTest);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;
    
    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            DotNetTasks.DotNetClean(s => s
                .SetProject(Solution));
        });

    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetTasks.DotNetRestore(s => s
                .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetTasks.DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .EnableNoRestore());
        });

    Target UnitTest => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            var projects = Solution.GetAllProjects("*Tests*").ToList();
            foreach (var project in projects)
            {
                DotNetTasks.DotNetTest(_ => _
                    .SetProjectFile(project.Path)
                    .SetConfiguration(Configuration)
                    .EnableNoBuild()
                    .SetDataCollector("XPlat Code Coverage")
                    .SetCoverletOutputFormat(CoverletOutputFormat.opencover)
                    .SetLoggers("trx")
                    .SetResultsDirectory(TestResultDirectory)
                    .SetFilter("Category=UnitTest")
                );
            }

            PublishTestResults();
        });
    
    Target AllTests => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            var projects = Solution.GetAllProjects("*Tests*").ToList();
            foreach (var project in projects)
            {
                DotNetTasks.DotNetTest(_ => _
                    .SetProjectFile(project.Path)
                    .SetConfiguration(Configuration)
                    .EnableNoBuild()
                    .SetDataCollector("XPlat Code Coverage")
                    .SetCoverletOutputFormat(CoverletOutputFormat.opencover)
                    .SetLoggers("trx")
                    .SetResultsDirectory(TestResultDirectory)
                );
            }

            PublishTestResults();
        });

    void PublishTestResults(string resultFilePattern = "**/*.trx")
    {
        //get a list of generated test result files
        var testResultFiles = TestResultDirectory.GlobFiles(resultFilePattern).Select(p => p.ToString()).ToList();
        if (null != AzurePipelines)
        {
            try
            {
                Console.WriteLine("Publishing test results to azure.");
 
                AzurePipelines?.PublishTestResults("Test Results",
                    AzurePipelinesTestResultsType.VSTest,
                    testResultFiles,
                    false,
                    "Any CPU",
                    Configuration.ToString(), 
                    true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error publishing code coverage: {ex}");
            }
        }
        else
        {
            Console.WriteLine("AzurePipelines is not set.");
        }
    }
}
