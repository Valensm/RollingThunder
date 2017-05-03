@ECHO OFF
ECHO.

SET OUTPUT="%1"

IF %OUTPUT%=="" (
    SET OUTPUT="%cd%\BuildOutput"
    )

ECHO Using output directory: %OUTPUT%.

IF EXIST %OUTPUT% (
    rmdir /S /Q %OUTPUT%
	
	IF %ERRORLEVEL% NEQ 0 (
		ECHO     Cannot delete directory %OUTPUT%.
		GOTO END
		)
		
	ECHO     Directory %OUTPUT% has been deleted.
	) 

IF EXIST "%ProgramFiles(x86)%\Windows Installer XML v3.5\bin" (
	SET HEAT="%ProgramFiles(x86)%\Windows Installer XML v3.5\bin\heat.exe"
	)
	
IF %HEAT%=="" (
	SET HEAT="heat.exe"
	)
	
REM ECHO Using Heat path: %HEAT%

IF EXIST "%ProgramFiles(x86)%\MSBUILD\14.0" (
	SET MSBUILD="C:\Program Files (x86)\MSBuild\14.0\Bin\msbuild.exe"
	ECHO Using the v14 MsBuild - Roslyn.
	GOTO END1
	)

IF EXIST "%ProgramFiles(x86)%\MSBUILD\12.0" (
	SET MSBUILD="C:\Program Files (x86)\MSBuild\12.0\Bin\msbuild.exe"
	ECHO Using the v12 MsBuild.
	GOTO END1
	)

IF EXIST "%ProgramFiles(x86)%" (
	SET MSBUILD=%WinDir\Microsoft.NET\Framework\v4.0.30319\msbuild.exe
	ECHO Using the v4.0.30319 MsBuild from '%WinDir%' directory.
	GOTO END1
	)

SET MSBUILD=C:\WINNT\Microsoft.NET\Framework\v4.0.30319\msbuild.exe
ECHO Using the v4.0.30319 MsBuild from 'C:\WINNT' directory.
GOTO END1

:END1
ECHO.

ECHO Building solution ...
%MSBUILD% RollingThunder.sln /t:Rebuild /p:Configuration=Release;OutputPath=%OUTPUT%

IF %ERRORLEVEL% NEQ 0 (
			ECHO Build failed.
			GOTO END
		)
		
ECHO Build complete.
GOTO END
		


:END
ECHO.

rem PAUSE 

