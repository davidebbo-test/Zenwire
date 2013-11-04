@set nunit=..\NUnit\nunit-console.exe
@set dotCover=..\..\Bin\dotCover.exe

%dotCover% cover /TargetExecutable=%nunit% /TargetArguments=SampleApp.Util.Tests.dll /TargetWorkingDir=..\SampleApp\Bin ^
 /Filters=+:module=*;class=*;function=*;-:module=*Tests* /Output=..\Snapshots\SampleApp.Util.Tests.dcvr