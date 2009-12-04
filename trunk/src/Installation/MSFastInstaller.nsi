!ifndef PROJECT_DIR
  !define PROJECT_DIR '.\'
!endif
!ifndef RESOURCES_DIR
  !define RESOURCES_DIR '${PROJECT_DIR}\resources'
!endif
!ifndef APP_NAME
  !define APP_NAME "MySpace's Performance Tracker"
!endif
!ifndef APP_PACKAGE_VERSION
  !define APP_PACKAGE_VERSION "1.0.0.130 (BETA)"
!endif
!ifndef REGISTRY_ROOT
  !define REGISTRY_ROOT 'HKLM'
!endif
!ifndef REGISTRY_KEY
  !define REGISTRY_KEY 'Software\MySpace\PerformanceTracker'
!endif
!ifndef REGISTRY_BAND_CLSID
  !define REGISTRY_BAND_CLSID '{AAE91B90-296A-471e-9926-2D4505F8EF5B}'
!endif
!ifndef REGISTRY_BAND_BUTTON_CLSID
  !define REGISTRY_BAND_BUTTON_CLSID '{AAE91B90-296A-471e-9926-2D4505F8EF5A}'
!endif
!ifndef REGISTRY_CLASS_NAME
  !define REGISTRY_CLASS_NAME 'MySpace.MSFast.SysImpl.Win32.InternetExplorer.MSFastBrowserBand'
!endif

;Minimum .NET version
!define DOT_MAJOR 2
!define DOT_MINOR 0


VIAddVersionKey /LANG=${LANG_ENGLISH} "ProductName" "${APP_NAME}"
VIAddVersionKey /LANG=${LANG_ENGLISH} "CompanyName" "MySpace, Inc."
VIAddVersionKey /LANG=${LANG_ENGLISH} "LegalTrademarks" "This Application is a trademark of MySpace Inc."
VIAddVersionKey /LANG=${LANG_ENGLISH} "LegalCopyright" "MySpace, Inc."
VIAddVersionKey /LANG=${LANG_ENGLISH} "FileDescription" "Installation package for ${APP_NAME}."
VIAddVersionKey /LANG=${LANG_ENGLISH} "FileVersion" "${APP_PACKAGE_VERSION}"
VIAddVersionKey /LANG=${LANG_ENGLISH} "ProductID" ""
VIProductVersion "${APP_PACKAGE_VERSION}"


;--------------------------------
;General
	
    Name "${APP_NAME}"

    ; The file to write
    ; OutFile "Setup_MySpace_PerformanceTracker.0.0.0.0.exe"

    ; The default installation directory
    InstallDir $PROGRAMFILES\MySpace\PerformanceTracker
    InstallDirRegKey HKLM "${REGISTRY_KEY}" "InstallPath"

    SetCompress force
    SetCompressor /SOLID lzma

    ; Request application privileges for Windows Vista
    RequestExecutionLevel admin
	
;--------------------------------
;Include 

  !include "MUI.nsh"
 
;--------------------------------
;Variables

  Var MUI_TEMP
  Var STARTMENU_FOLDER

;--------------------------------
;Interface Settings

  Icon "${RESOURCES_DIR}\arrow2-install.ico"
  UninstallIcon "${RESOURCES_DIR}\arrow2-uninstall.ico" 
  !define MUI_HEADERIMAGE
  !define MUI_ICON "${RESOURCES_DIR}\arrow2-install.ico"
  !define MUI_UNICON "${RESOURCES_DIR}\arrow2-uninstall.ico" 
  !define MUI_WELCOMEFINISHPAGE_BITMAP "${RESOURCES_DIR}\welcome.bmp"
  !define MUI_HEADERIMAGE_BITMAP "${RESOURCES_DIR}\top.bmp" ; optional
  !define MUI_ABORTWARNING
;--------------------------------
;Pages
  !define MUI_WELCOMEPAGE_TITLE "${APP_NAME} Setup Wizard"
  !define MUI_WELCOMEPAGE_TEXT "This wizard will guide you through the installation of ${APP_NAME}\r\n\r\n$_CLICK"
  !insertmacro MUI_PAGE_LICENSE "License.txt"
  ;Start Menu Folder Page Configuration
  !define MUI_STARTMENUPAGE_DEFAULTFOLDER "MySpace\${APP_NAME}"
  !define MUI_STARTMENUPAGE_REGISTRY_ROOT "HKLM" 
  !define MUI_STARTMENUPAGE_REGISTRY_KEY "${REGISTRY_KEY}" 
  !define MUI_STARTMENUPAGE_REGISTRY_VALUENAME "Start Menu Folder"

  !define MUI_FINISHPAGE_RUN
  !define MUI_FINISHPAGE_RUN_TEXT "Launch Internet Explorer"
  !define MUI_FINISHPAGE_RUN_FUNCTION "LaunchLink"

  !insertmacro MUI_PAGE_WELCOME
  !insertmacro MUI_PAGE_STARTMENU Application $STARTMENU_FOLDER
  !insertmacro MUI_PAGE_INSTFILES
  !insertmacro MUI_PAGE_FINISH
  
  !insertmacro MUI_UNPAGE_CONFIRM
  !insertmacro MUI_UNPAGE_INSTFILES
  !insertmacro MUI_UNPAGE_FINISH
;--------------------------------
;Languages
 
  !insertmacro MUI_LANGUAGE "English"

;--------------------------------
;Installer Sections



Function .onInit
	
   Call IsDotNetInstalled
   Call IsFlashInstalled
   Call IsIERunning
FunctionEnd


; The stuff to install
Section "IE Plugin" SecIEPlugin ;No components page, name is not important
    
    ;Register

    WriteRegStr HKCR "MSFastResults" "" "MSFast Results File"
    WriteRegStr HKCR "MSFastResults\DefaultIcon" "" "$INSTDIR\Resources\icon2.ico"
    WriteRegStr HKCR "MSFastResults\shell\edit\command" "" '"$INSTDIR\msfast.exe" "%1"'
    WriteRegStr HKCR "MSFastResults\shell\open\command" "" '"$INSTDIR\msfast.exe" "%1"'
    WriteRegStr HKCR ".msf" "" "MSFastResults"

    WriteRegStr HKLM "${REGISTRY_KEY}" "InstallPath" "$INSTDIR\"
	WriteRegStr HKLM "${REGISTRY_KEY}" "TemporaryFolder" "$INSTDIR\dump"
	WriteRegStr HKLM "${REGISTRY_KEY}" "DumpFolder" "$INSTDIR\dump"
    WriteRegStr HKLM "${REGISTRY_KEY}" "VersionUpdateURL" "http://msfast.myspace.com/version.txt"
    WriteRegStr HKLM "${REGISTRY_KEY}" "CurrentPackageVersion" "${APP_PACKAGE_VERSION}"
	WriteRegStr HKLM "${REGISTRY_KEY}" "DefaultProxyPort" "8085|8086|8087|8088|8089|8090|8091"
    WriteRegDWORD HKLM "${REGISTRY_KEY}" "PageValidation" 0x00000001
    WriteRegDWORD HKLM "${REGISTRY_KEY}" "PageGraph" 0x00000001
    WriteRegDWORD HKLM "${REGISTRY_KEY}" "ClearCacheBeforeTest" 0x00000001
       
	WriteRegBin HKLM "SOFTWARE\Microsoft\Internet Explorer\Explorer Bars\${REGISTRY_BAND_CLSID}" "BarSize" 0601000000000000
	
	WriteRegStr HKLM "SOFTWARE\Microsoft\Internet Explorer\Extensions\${REGISTRY_BAND_BUTTON_CLSID}" "CLSID" "{E0DD6CAB-2D10-11D2-8F1A-0000F87ABD16}"
	WriteRegStr HKLM "SOFTWARE\Microsoft\Internet Explorer\Extensions\${REGISTRY_BAND_BUTTON_CLSID}" "Default Visible" "Yes"
	WriteRegStr HKLM "SOFTWARE\Microsoft\Internet Explorer\Extensions\${REGISTRY_BAND_BUTTON_CLSID}" "ButtonText" "${APP_NAME}"
	WriteRegStr HKLM "SOFTWARE\Microsoft\Internet Explorer\Extensions\${REGISTRY_BAND_BUTTON_CLSID}" "Icon" "$INSTDIR\Resources\icon.ico"
	WriteRegStr HKLM "SOFTWARE\Microsoft\Internet Explorer\Extensions\${REGISTRY_BAND_BUTTON_CLSID}" "HotIcon" "$INSTDIR\Resources\icon.ico"
	WriteRegStr HKLM "SOFTWARE\Microsoft\Internet Explorer\Extensions\${REGISTRY_BAND_BUTTON_CLSID}" "BandCLSID" "${REGISTRY_BAND_CLSID}"
	WriteRegStr HKLM "SOFTWARE\Microsoft\Internet Explorer\Extensions\${REGISTRY_BAND_BUTTON_CLSID}" "MenuText" "${APP_NAME}"
	WriteRegStr HKLM "SOFTWARE\Microsoft\Internet Explorer\Extensions\${REGISTRY_BAND_BUTTON_CLSID}" "ToolTip" "${APP_NAME}"
	WriteRegStr HKLM "SOFTWARE\Microsoft\Internet Explorer\Extensions\${REGISTRY_BAND_BUTTON_CLSID}" "MenuStatusBar" "${APP_NAME}"
	
	WriteRegStr HKCR "${REGISTRY_CLASS_NAME}" "" "${REGISTRY_CLASS_NAME}"
	WriteRegStr HKCR "${REGISTRY_CLASS_NAME}\CLSID" "" "${REGISTRY_BAND_CLSID}"
	
	WriteRegStr HKCR "CLSID\${REGISTRY_BAND_CLSID}" "" "${REGISTRY_CLASS_NAME}"
	WriteRegStr HKCR "CLSID\${REGISTRY_BAND_CLSID}" "HelpText" "${APP_NAME}"
	WriteRegStr HKCR "CLSID\${REGISTRY_BAND_CLSID}" "MenuText" "${APP_NAME}"
	WriteRegStr HKCR "CLSID\${REGISTRY_BAND_CLSID}\InprocServer32" "" "mscoree.dll"
	WriteRegStr HKCR "CLSID\${REGISTRY_BAND_CLSID}\InprocServer32" "ThreadingModel" "Both"
	WriteRegStr HKCR "CLSID\${REGISTRY_BAND_CLSID}\InprocServer32" "Class" "${REGISTRY_CLASS_NAME}"
	WriteRegStr HKCR "CLSID\${REGISTRY_BAND_CLSID}\InprocServer32" "Assembly" "MySpace.MSFast.SysImpl.Win32.InternetExplorer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
	WriteRegStr HKCR "CLSID\${REGISTRY_BAND_CLSID}\InprocServer32" "RuntimeVersion" "v2.0.50727"
	WriteRegStr HKCR "CLSID\${REGISTRY_BAND_CLSID}\InprocServer32" "CodeBase" "file:///$INSTDIR\MySpace.MSFast.SysImpl.Win32.InternetExplorer.dll"
	WriteRegStr HKCR "CLSID\${REGISTRY_BAND_CLSID}\InprocServer32\1.0.0.0" "Class" "${REGISTRY_CLASS_NAME}"
	WriteRegStr HKCR "CLSID\${REGISTRY_BAND_CLSID}\InprocServer32\1.0.0.0" "Assembly" "MySpace.MSFast.SysImpl.Win32.InternetExplorer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
	WriteRegStr HKCR "CLSID\${REGISTRY_BAND_CLSID}\InprocServer32\1.0.0.0" "RuntimeVersion" "v2.0.50727"
	WriteRegStr HKCR "CLSID\${REGISTRY_BAND_CLSID}\InprocServer32\1.0.0.0" "CodeBase" "file:///$INSTDIR\MySpace.MSFast.SysImpl.Win32.InternetExplorer.dll"
	WriteRegStr HKCR "CLSID\${REGISTRY_BAND_CLSID}\ProgId" "" "${REGISTRY_CLASS_NAME}"
	WriteRegStr HKCR "CLSID\${REGISTRY_BAND_CLSID}\Implemented Categories\{62C8FE65-4EBB-45E7-B440-6E39B2CDBF29}" "" ""
	WriteRegStr HKCR "CLSID\${REGISTRY_BAND_CLSID}\Implemented Categories\{00021494-0000-0000-C000-000000000046}" "" ""
    
    CreateDirectory "$INSTDIR";
    CreateDirectory "$INSTDIR\dump";
    CreateDirectory "$INSTDIR\conf";
    
    !include ApplicationFiles.nsi
    !include ConfigurationFiles.nsi
    !include JSTemplate.nsi
    !include JSShell.nsi
	!include Resources.nsi
	
	;Configure the windows firewall. 
	nsExec::ExecToLog '$WINDIR\system32\netsh.exe firewall add allowedprogram "$INSTDIR\engine.exe" "MySpace Performance Tracker"'
	
	;Install Native Assemblies
	nsExec::ExecToLog '"$WINDIR\Microsoft.NET\Framework\v2.0.50727\ngen.exe" install "$INSTDIR\engine.exe" /AppBase:"$INSTDIR" /queue:1'
	nsExec::ExecToLog '"$WINDIR\Microsoft.NET\Framework\v2.0.50727\ngen.exe" install "$INSTDIR\configuration.exe" /AppBase:"$INSTDIR" /queue:1'
	nsExec::ExecToLog '"$WINDIR\Microsoft.NET\Framework\v2.0.50727\ngen.exe" install "$INSTDIR\MySpace.Performance.Tracker.Collectors.Client.MSFast.Hosts.Toolbar.IE.dll" /AppBase:"$INSTDIR" /queue:1'
	nsExec::ExecToLog '"$WINDIR\Microsoft.NET\Framework\v2.0.50727\ngen.exe" executeQueuedItems 1'
	
	
	;Flash Security
	CreateDirectory "$WINDIR\System32\Macromed\Flash\FlashPlayerTrust"
	Push "$INSTDIR"
	Push "$WINDIR\System32\Macromed\Flash\FlashPlayerTrust\MySpacePerfTracker.cfg" ;file to write to 
	Call WriteToFile
	
    WriteUninstaller "$INSTDIR\uninstall.exe"
    
    ;Create shortcuts
    
    !insertmacro MUI_STARTMENU_WRITE_BEGIN Application
    
		CreateDirectory "$SMPROGRAMS\$STARTMENU_FOLDER"
		CreateShortCut "$SMPROGRAMS\$STARTMENU_FOLDER\Configuration.lnk" "$INSTDIR\configuration.exe" 
		CreateShortCut "$SMPROGRAMS\$STARTMENU_FOLDER\MSFast.lnk" "$INSTDIR\msfast.exe" 
		CreateShortCut "$SMPROGRAMS\$STARTMENU_FOLDER\Uninstall.lnk" "$INSTDIR\Uninstall.exe" 
    
    !insertmacro MUI_STARTMENU_WRITE_END
    

	;Refresh IE's installtion cache
    DeleteRegKey HKCU "SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Discardable\PostSetup\Component Categories\{00021494-0000-0000-C000-000000000046}"
    
    
SectionEnd ; end the section





Section "Uninstall"
	;Remove firewall 
	nsExec::ExecToLog '$WINDIR\system32\netsh.exe firewall delete allowedprogram "$INSTDIR\engine.exe"'

    SetRebootFlag true
    ;;Delete Files
    Delete "$INSTDIR\*.*"
    RMDir "$INSTDIR"
    Delete "$WINDIR\System32\Macromed\Flash\FlashPlayerTrust\MySpacePerfTracker.cfg"
    
    DeleteRegKey HKLM "${REGISTRY_KEY}"
	DeleteRegKey HKLM "SOFTWARE\Microsoft\Internet Explorer\Explorer Bars\${REGISTRY_BAND_CLSID}"
	DeleteRegKey HKLM "SOFTWARE\Microsoft\Internet Explorer\Extensions\${REGISTRY_BAND_BUTTON_CLSID}"
	DeleteRegKey HKCR "${REGISTRY_CLASS_NAME}"
	DeleteRegKey HKCR "CLSID\${REGISTRY_BAND_CLSID}"
    
    ;Process Uninstall
    
    !insertmacro MUI_STARTMENU_GETFOLDER Application $MUI_TEMP
    Delete "$SMPROGRAMS\$MUI_TEMP\Uninstall.lnk"
    Delete "$SMPROGRAMS\$MUI_TEMP\Configuration.lnk"

    ;Delete empty start menu parent diretories
    StrCpy $MUI_TEMP "$SMPROGRAMS\$MUI_TEMP"

startMenuDeleteLoop:
    ClearErrors
    RMDir $MUI_TEMP
    GetFullPathName $MUI_TEMP "$MUI_TEMP\.."

    IfErrors startMenuDeleteLoopDone

    StrCmp $MUI_TEMP $SMPROGRAMS startMenuDeleteLoopDone startMenuDeleteLoop

startMenuDeleteLoopDone:
    RMDir /r /REBOOTOK "$INSTDIR"
SectionEnd



Function LaunchLink
  ExecShell "" "iexplore.exe" "http://msfast.myspace.com"
FunctionEnd



Function WriteToFile
 Exch $0 ;file to write to
 Exch
 Exch $1 ;text to write

 IfFileExists $0 deletefile  

 continue:
  FileOpen $0 $0 a #open file
   FileSeek $0 0 END #go to end
   FileWrite $0 $1 #write to file
  FileClose $0
  Goto end

 deletefile:
   Delete $0
   Goto continue

 end: 

 Pop $1
 Pop $0
FunctionEnd

!macro WriteToFile String File
 Push "${String}"
 Push "${File}"
  Call WriteToFile
!macroend
!define WriteToFile "!insertmacro WriteToFile"



Function IsIERunning
;0   = Process was not found 
;1   = Process was found 
;605 = Unable to search for process 
;606 = Unable to identify system type 
;607 = Unsupported OS 
;632 = Process name is invalid 
   
   CheckIfIERunning:
   
   FindProcDLL::FindProc "iexplore.exe"
   
   StrCmp $R0 0 0 error 
     Goto end 

   error:
	MessageBox MB_OKCANCEL|MB_ICONEXCLAMATION "Internet Explorer must be closed during this installation. $\r$\nPlease close all instances of IE." IDCANCEL "IERunningQuit" 
	GoTo CheckIfIERunning
   
   IERunningQuit:
	MessageBox MB_YESNO|MB_ICONEXCLAMATION "Are you sure you want to abort ${APP_NAME} Setup?"  IDNO "CheckIfIERunning" 
	Abort
   
   end: 
	
	
FunctionEnd

;comment


Function IsFlashInstalled

   
   ClearErrors
   
   ReadRegStr $0 HKCR CLSID\{d27cdb6e-ae6d-11cf-96b8-444553540000} ""
   
   IfErrors FlashNotInstalled
   Goto FlashInstalled
   
   FlashNotInstalled:
	MessageBox MB_YESNO|MB_ICONEXCLAMATION "${APP_NAME} require Adobe Flash Player$\r$\n Would you like to install it now?"  IDNO "FlashNotInstalledQuit" 
	ExecShell "" "iexplore.exe" "http://get.adobe.com/flashplayer/"
	Abort
	
   FlashNotInstalledQuit:
	MessageBox MB_YESNO|MB_ICONEXCLAMATION "Are you sure you want to abort ${APP_NAME} Setup?"  IDNO "FlashNotInstalled" 
	Abort
   
   FlashInstalled:

FunctionEnd


; Usage
; Define in your script two constants:
;   DOT_MAJOR "(Major framework version)"
;   DOT_MINOR "{Minor frameword version)"
; 
; Call IsDotNetInstalled
; This function will abort the installation if the required version 
; or higher version of the .NETFramework is not installed.  Place it in
; either your .onInit function or your first install section before 
; other code.
Function IsDotNetInstalled
 
  StrCpy $0 "0"
  StrCpy $1 "SOFTWARE\Microsoft\.NETFramework" ;registry entry to look in.
  StrCpy $2 0
 
  StartEnum:
    ;Enumerate the versions installed.
    EnumRegKey $3 HKLM "$1\policy" $2
 
    ;If we don't find any versions installed, it's not here.
    StrCmp $3 "" noDotNet notEmpty
 
    ;We found something.
    notEmpty:
      ;Find out if the RegKey starts with 'v'.  
      ;If it doesn't, goto the next key.
      StrCpy $4 $3 1 0
      StrCmp $4 "v" +1 goNext
      StrCpy $4 $3 1 1
 
      ;It starts with 'v'.  Now check to see how the installed major version
      ;relates to our required major version.
      ;If it's equal check the minor version, if it's greater, 
      ;we found a good RegKey.
      IntCmp $4 ${DOT_MAJOR} +1 goNext yesDotNetReg
      ;Check the minor version.  If it's equal or greater to our requested 
      ;version then we're good.
      StrCpy $4 $3 1 3
      IntCmp $4 ${DOT_MINOR} yesDotNetReg goNext yesDotNetReg
 
    goNext:
      ;Go to the next RegKey.
      IntOp $2 $2 + 1
      goto StartEnum
 
  yesDotNetReg:
    ;Now that we've found a good RegKey, let's make sure it's actually
    ;installed by getting the install path and checking to see if the 
    ;mscorlib.dll exists.
    EnumRegValue $2 HKLM "$1\policy\$3" 0
    ;$2 should equal whatever comes after the major and minor versions 
    ;(ie, v1.1.4322)
    StrCmp $2 "" noDotNet
    ReadRegStr $4 HKLM $1 "InstallRoot"
    ;Hopefully the install root isn't empty.
    StrCmp $4 "" noDotNet
    ;build the actuall directory path to mscorlib.dll.
    StrCpy $4 "$4$3.$2\mscorlib.dll"
    IfFileExists $4 yesDotNet noDotNet
 
  noDotNet:
    ;Nope, something went wrong along the way.  Looks like the 
    ;proper .NETFramework isn't installed.  
    MessageBox MB_YESNO|MB_ICONEXCLAMATION "${APP_NAME} requiere Microsoft .NET Framework 2.0$\r$\n Would you like to install it now?"  IDNO "DotNetNotInstalledQuit" 
    ExecShell "" "iexplore.exe" "http://www.microsoft.com/downloads/details.aspx?FamilyID=0856eacb-4362-4b0d-8edd-aab15c5e04f5&displaylang=en"
    Abort
    
  DotNetNotInstalledQuit:  
	MessageBox MB_YESNO|MB_ICONEXCLAMATION "Are you sure you want to abort ${APP_NAME} Setup?"  IDNO "noDotNet" 
	Abort
 
  yesDotNet:
    ;Everything checks out.  Go on with the rest of the installation.
 
FunctionEnd