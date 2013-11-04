@set nunit=..\NUnit\nunit-console.exe
@set dotCover=..\..\Bin\dotCover.exe

%dotCover% analyse /TargetExecutable=%nunit% /TargetArguments=SampleApp.Util.Tests.dll /TargetWorkingDir=..\SampleApp\Bin ^
 /Filters=+:module=*;class=*;function=*;-:module=*Tests* /ReportType=HTML /Output=..\Reports\SampleApp.Util.Tests.html

@start ..\Reports\SampleApp.Util.Tests.html