@set dotCover=..\..\Bin\dotCover.exe

%dotCover% report /Source=..\Snapshots\SampleApp.Util.Tests.dcvr /ReportType=HTML /Output=..\Reports\SampleApp.Util.Tests.html

@start ..\Reports\SampleApp.Util.Tests.html