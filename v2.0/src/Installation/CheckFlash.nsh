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