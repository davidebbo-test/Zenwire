@set nunit=..\BuildAndRelease\NUnit\nunit-console.exe
@set dotCover=..\BuildAndRelease\DotCover\dotCover.exe

%dotCover% analyse /TargetExecutable=%nunit% /TargetArguments=Tests.Unit.Zenwire.dll /TargetWorkingDir=..\Tests.Unit.Zenwire\bin\Debug ^
 /Filters=+:module=*;class=*;function=*;-:module=*Tests* /ReportType=HTML /Output=..\Reports\Tests.Unit.Zenwire.html