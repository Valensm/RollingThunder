@ECHO OFF
SET OUTPUT="%1"

IF %OUTPUT%=="" (
    SET OUTPUT="%cd%\BuildOutput"
    )

ECHO Using output directory: %OUTPUT%.

SET NUGET=packages\NuGet.CommandLine.3.5.0\tools\NuGet.exe
SET PROJECT=Logic\Logic.csproj 

%NUGET% pack %PROJECT% -build -properties Configuration=Release -sym -OutputDirectory %OUTPUT% -suffix Alpha1