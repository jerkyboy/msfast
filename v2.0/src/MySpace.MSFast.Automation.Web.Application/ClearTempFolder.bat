@echo off
ECHO Listing Files...
dir \MSFastAutomation\Temp\*. /b/s > \dirs.tmp
ECHO Files listed!
for /f "tokens=4 delims=\" %%a in (\dirs.tmp) do rmdir /S/Q "\MSFastAutomation\Temp\%%a" | echo %%a Deleted

