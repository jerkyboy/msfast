@echo off
ECHO Listing Files...
dir .\Temp\webtestresults /b/s > dirs.tmp
ECHO Files listed!
for /f "tokens=5 delims=\" %%a in (dirs.tmp) do rmdir /S/Q ".\Temp\webtestresults\%%a" | echo %%a Deleted

ECHO Listing Files...
dir .\Temp\client_temp /b/s > dirs.tmp
ECHO Files listed!
for /f "tokens=5 delims=\" %%a in (dirs.tmp) do rmdir /S/Q ".\Temp\client_temp\%%a" | echo %%a Deleted

del .\Temp\webtestresults\*.* /Q
del .\Temp\client_temp\*.* /Q
