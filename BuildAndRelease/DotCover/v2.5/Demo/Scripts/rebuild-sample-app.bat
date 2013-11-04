@set msbuild="%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"

%msbuild% "%~dp0..\SampleApp\Src\SampleApp.sln" /t:Rebuild