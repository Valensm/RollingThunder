@ECHO OFF

SET MSTEST="%VS140COMNTOOLS%\..\IDE\mstest.exe"
SET OPENCOVER=".\packages\OpenCover.4.6.519\tools\OpenCover.Console.exe"
SET REPORTGENERATOR=".\packages\ReportGenerator.2.5.6\tools\ReportGenerator.exe"

SET INPUT=.\BuildOutput
SET OUTPUT=%INPUT%\Coverage
SET RESULTFILE=%OUTPUT%\TestResults.trx
SET OUTPUTFILE=%OUTPUT%\CoverReport.xml


del /Q %RESULTFILE%
del /Q %OUTPUTFILE%
rmdir /S /Q %OUTPUT%
mkdir %OUTPUT%

%OPENCOVER% ^
-register:user ^
-target:%MSTEST% ^
-targetargs:"/testcontainer:\"%INPUT%\Wly.RollingThunder.Logic.Tests.dll\" /resultsfile:\"%RESULTFILE%\"" ^
-filter:"+[Wly.*]* -[Wly.*.Tests]* -[Wly.*]*Exception" ^
-mergebyhash ^
-output:"%OUTPUTFILE%"
rem -skipautoprops ^

%REPORTGENERATOR% "-reports:%OUTPUTFILE%" "-targetdir:%OUTPUT%\Report"


start "report" "%OUTPUT%\Report\index.htm"


