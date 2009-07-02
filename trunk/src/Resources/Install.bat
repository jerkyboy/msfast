@echo off
CLS
REM *************************************************************************

REM		Project: MSFast
REM		Original author: Yadid Ramot (e.yadid@gmail.com)
REM		Copyright (C) 2009 MySpace.com 

REM		This file is part of MSFast.
REM		MSFast is free software: you can redistribute it and/or modify
REM		it under the terms of the GNU General Public License as published by
REM		the Free Software Foundation, either version 3 of the License, or
REM		(at your option) any later version.
REM 
REM		MSFast is distributed in the hope that it will be useful,
REM		but WITHOUT ANY WARRANTY; without even the implied warranty of
REM		MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
REM		GNU General Public License for more details.
 
REM		You should have received a copy of the GNU General Public License
REM		along with MSFast.  If not, see <http://www.gnu.org/licenses/>.

REM *************************************************************************

echo *************************************************************************
echo *  
echo *  This batch is a utility for creating a REG file that holds all of 
echo *  the registry keys required by MSFast. This batch will automatically 
echo *  install the REG file when its done.
echo *  
echo *  Use:
echo *  CreateRegFile.bat "<BIN FOLDER>" "<TEMP FOLDER>"
echo *  
echo *  Both "<BIN FOLDER>" and "<TEMP FOLDER>" should be an absolute path
echo *  
echo *  for example: CreateRegFile.bar "C:\MSFast" "C:\Temp"
echo *  
echo *************************************************************************
echo.
SET REGISTRY_BAND_CLSID={AAE91B90-296A-471e-9926-2D4505F8EF5B}
SET REGISTRY_BAND_BUTTON_CLSID={AAE91B90-296A-471e-9926-2D4505F8EF5A}
SET BIN_FOLDER=%1
SET TEMP_FOLDER=%2

SET BIN_FOLDER=###%BIN_FOLDER%###
SET BIN_FOLDER=%BIN_FOLDER:"###=%
SET BIN_FOLDER=%BIN_FOLDER:###"=%
SET BIN_FOLDER=%BIN_FOLDER:###=%

SET TEMP_FOLDER=###%TEMP_FOLDER%###
SET TEMP_FOLDER=%TEMP_FOLDER:"###=%
SET TEMP_FOLDER=%TEMP_FOLDER:###"=%
SET TEMP_FOLDER=%TEMP_FOLDER:###=%

echo *************************************************************************
echo.
echo Bin Folder - %BIN_FOLDER%
echo Temp Folder - %TEMP_FOLDER%
echo.
echo *************************************************************************
echo.
echo Is this correct (Y/N)?
CHOICE /N 
if errorlevel 2 goto END

IF EXIST "tmp.reg" DEL tmp.reg

SET BIN_FOLDER=%BIN_FOLDER:\=\\%
SET TEMP_FOLDER=%TEMP_FOLDER:\=\\%

echo Windows Registry Editor Version 5.00 >> tmp.reg
echo. >> tmp.reg
echo [HKEY_CLASSES_ROOT\CLSID\%REGISTRY_BAND_CLSID%] >> tmp.reg
echo "HelpText"="MySpace's Performance Tracker" >> tmp.reg
echo "MenuText"="MySpace's Performance Tracker" >> tmp.reg
echo. >> tmp.reg
echo [HKEY_LOCAL_MACHINE\SOFTWARE\MySpace] >> tmp.reg
echo. >> tmp.reg
echo [HKEY_LOCAL_MACHINE\SOFTWARE\MySpace\PerformanceTracker] >> tmp.reg
echo "InstallPath"="%BIN_FOLDER%" >> tmp.reg
echo "TemporaryFolder"="%TEMP_FOLDER%" >> tmp.reg
echo "DumpFolder"="%TEMP_FOLDER%" >> tmp.reg
echo "VersionUpdateURL"="http://msfast.myspace.com/version.txt" >> tmp.reg
echo "CurrentPackageVersion"="" >> tmp.reg
echo "DefaultProxyPort"="8085|8086|8087|8088|8089|8090|8091" >> tmp.reg
echo "PageValidation"=dword:00000001 >> tmp.reg
echo "PageGraph"=dword:00000001 >> tmp.reg
echo "ClearCacheBeforeTest"=dword:00000001 >> tmp.reg
echo "Start Menu Folder"="MySpace\\MySpace's Performance Tracker" >> tmp.reg
echo "LatestPackageVersion"="" >> tmp.reg
echo "LatestCollection"=dword:00000030 >> tmp.reg
echo. >> tmp.reg
echo [HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Internet Explorer\Explorer Bars\%REGISTRY_BAND_CLSID%] >> tmp.reg
echo "BarSize"=hex:06,01,00,00,00,00,00,00 >> tmp.reg
echo. >> tmp.reg
echo [HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Internet Explorer\Extensions\%REGISTRY_BAND_BUTTON_CLSID%] >> tmp.reg
echo "CLSID"="{E0DD6CAB-2D10-11D2-8F1A-0000F87ABD16}" >> tmp.reg
echo "Default Visible"="Yes" >> tmp.reg
echo "ButtonText"="MySpace's Performance Tracker" >> tmp.reg
echo "Icon"="%BIN_FOLDER%\\icon.ico" >> tmp.reg
echo "HotIcon"="%BIN_FOLDER%\\icon.ico" >> tmp.reg
echo "BandCLSID"="%REGISTRY_BAND_CLSID%" >> tmp.reg
echo "MenuText"="MySpace's Performance Tracker" >> tmp.reg
echo "ToolTip"="MySpace's Performance Tracker" >> tmp.reg
echo "MenuStatusBar"="MySpace's Performance Tracker" >> tmp.reg
echo. >> tmp.reg

CLS
echo.
echo *************************************************************************
echo *  The following keys will be created:
echo *************************************************************************
echo.
type tmp.reg
echo.
echo *************************************************************************
echo.
echo Continue (Y/N)?
CHOICE /N 
if errorlevel 2 goto END


%WinDir%\regedit.exe /S "tmp.reg"

DEL tmp.reg

:END
