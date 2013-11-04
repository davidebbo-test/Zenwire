Using the ‘SampleApp’ solution you can get familiar with dotCover Console Runner and find out the most useful ways of getting coverage reports.

dotCover Console Runner provides two ways of performing coverage analysis and getting reports:
1. analyse
2. cover - merge – report


The first one can be used in a simple scenario when you have, for example, one test project and want to get a single coverage report.

The second one provides more fine-grained control over coverage process. It can be used in continuous integration 
environments when coverage analysis should be executed several times, but we'd like to get a single report. 
You can use the “merge” command to explicitly combine several coverage snapshots into one and process the result with the "report" command afterwards. 

To see the difference between two ways, use the two test projects in the ‘SampleApp’ solution. 

To see all commands supported by dotCover, type ‘dotCover help’ in the command line. 
You can also take a look at blog posts and screencasts (http://blogs.jetbrains.com/dotnet/tag/dotcover/ and http://tv.jetbrains.net/channel/dotcover).


Unit Testing Framework
======================

NUnit is used in both test projects as a testing framework, so NUnit binaries are included in the package and located in the ‘NUnit’ subfolder. 
dotCover also works with other testing frameworks such as MSTest, xUnit, MSpec, etc.


Available Scripts
==============

rebuild-sample-app.bat  – Rebuilds the sample application.

analyse.bat 		– Runs dotCover Console Runner with the ‘analyse' command for the ‘SampleApp.Util.Tests’ project.

cover.bat 		– Runs dotCover Console Runner with the ‘cover' command for the ‘SampleApp.Util.Tests’ project.

cover-another-tests.bat - Runs dotCover Console Runner with the ‘cover' command for the ‘SampleApp.Data.Tests’ project.

merge.bat 		– Runs dotCover Console Runner with ‘merge’ command for the results that were obtained after running cover.bat and cover-another-tests.bat

report.bat 		– Runs dotCover Console Runner with ‘report’ command for results that were obtained after running cover.bat


Similar scripts are stored in the ‘XmlBasedConfiguration’ folder. They show how to configure dotCover Console Runner using xml configuration file.

dotCover supports two types of configuration: through the command line parameters and through the xml configuration file. You can choose the one 
which is more appropiate for your purposes or you can use both. In this case parameters passed though command line override the ones 
in the xml configuration file.



Projects
========

SampleApp                - Project that we want to get covered with tests.

SampleApp.Data.Tests     - Project containing tests for the sample data structures.

SampleApp.Util.Tests     - Project containing tests for the sample algorithms.



Steps
=====

1. Run analyse.bat
2. Examine Reports\SampleApp.Util.Tests.html for the output coverage report.
3. Repeat the example with consequent execution of cover.bat and report.bat

Notes
=====

1. If you work with sample application in the 'Program Files' folder, make sure that the scripts are executed with administrative privileges. 
2. All snapshots are saved to the 'Snapshots' folder. You can open them using dotCover right from Visual Studio.


  
