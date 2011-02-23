@echo off
ECHO Listing Files...
dir \MSFastAutomation_Temp\webtestresults /b/s > dirs.tmp
ECHO Files listed!
for /f "tokens=4 delims=\" %%a in (dirs.tmp) do rmdir /S/Q "\MSFastAutomation_Temp\webtestresults\%%a" | echo %%a Deleted

ECHO Listing Files...
dir \MSFastAutomation_Temp\client_temp /b/s > dirs.tmp
ECHO Files listed!
for /f "tokens=4 delims=\" %%a in (dirs.tmp) do rmdir /S/Q "\MSFastAutomation_Temp\client_temp\%%a" | echo %%a Deleted

del \MSFastAutomation_Temp\webtestresults\*.* /Q
del \MSFastAutomation_Temp\client_temp\*.* /Q
