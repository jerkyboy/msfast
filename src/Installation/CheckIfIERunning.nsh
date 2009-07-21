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
