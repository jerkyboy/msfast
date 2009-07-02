using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections;

namespace ComIterop
{
    [TypeLibType(4112)]
    [InterfaceType(2)]
    [Guid("3050F613-98B5-11CF-BB82-00AA00BDCE0B")]
    public interface HTMLDocumentEvents2
    {
        [DispId(1044)]
        void onactivate(IHTMLEventObj pEvtObj);
        [DispId(-2147418107)]
        void onafterupdate(IHTMLEventObj pEvtObj);
        [DispId(1047)]
        bool onbeforeactivate(IHTMLEventObj pEvtObj);
        [DispId(1034)]
        bool onbeforedeactivate(IHTMLEventObj pEvtObj);
        [DispId(1027)]
        void onbeforeeditfocus(IHTMLEventObj pEvtObj);
        [DispId(-2147418108)]
        bool onbeforeupdate(IHTMLEventObj pEvtObj);
        [DispId(-2147418078)]
        void oncellchange(IHTMLEventObj pEvtObj);
        [DispId(-600)]
        bool onclick(IHTMLEventObj pEvtObj);
        [DispId(1023)]
        bool oncontextmenu(IHTMLEventObj pEvtObj);
        [DispId(1036)]
        bool oncontrolselect(IHTMLEventObj pEvtObj);
        [DispId(-2147418097)]
        void ondataavailable(IHTMLEventObj pEvtObj);
        [DispId(-2147418098)]
        void ondatasetchanged(IHTMLEventObj pEvtObj);
        [DispId(-2147418096)]
        void ondatasetcomplete(IHTMLEventObj pEvtObj);
        [DispId(-601)]
        bool ondblclick(IHTMLEventObj pEvtObj);
        [DispId(1045)]
        void ondeactivate(IHTMLEventObj pEvtObj);
        [DispId(-2147418101)]
        bool ondragstart(IHTMLEventObj pEvtObj);
        [DispId(-2147418099)]
        bool onerrorupdate(IHTMLEventObj pEvtObj);
        [DispId(1048)]
        void onfocusin(IHTMLEventObj pEvtObj);
        [DispId(1049)]
        void onfocusout(IHTMLEventObj pEvtObj);
        [DispId(-2147418102)]
        bool onhelp(IHTMLEventObj pEvtObj);
        [DispId(-602)]
        void onkeydown(IHTMLEventObj pEvtObj);
        [DispId(-603)]
        bool onkeypress(IHTMLEventObj pEvtObj);
        [DispId(-604)]
        void onkeyup(IHTMLEventObj pEvtObj);
        [DispId(-605)]
        void onmousedown(IHTMLEventObj pEvtObj);
        [DispId(-606)]
        void onmousemove(IHTMLEventObj pEvtObj);
        [DispId(-2147418103)]
        void onmouseout(IHTMLEventObj pEvtObj);
        [DispId(-2147418104)]
        void onmouseover(IHTMLEventObj pEvtObj);
        [DispId(-607)]
        void onmouseup(IHTMLEventObj pEvtObj);
        [DispId(1033)]
        bool onmousewheel(IHTMLEventObj pEvtObj);
        [DispId(-2147418093)]
        void onpropertychange(IHTMLEventObj pEvtObj);
        [DispId(-609)]
        void onreadystatechange(IHTMLEventObj pEvtObj);
        [DispId(-2147418105)]
        void onrowenter(IHTMLEventObj pEvtObj);
        [DispId(-2147418106)]
        bool onrowexit(IHTMLEventObj pEvtObj);
        [DispId(-2147418080)]
        void onrowsdelete(IHTMLEventObj pEvtObj);
        [DispId(-2147418079)]
        void onrowsinserted(IHTMLEventObj pEvtObj);
        [DispId(1037)]
        void onselectionchange(IHTMLEventObj pEvtObj);
        [DispId(-2147418100)]
        bool onselectstart(IHTMLEventObj pEvtObj);
        [DispId(1026)]
        bool onstop(IHTMLEventObj pEvtObj);
    }

    [InterfaceType(2)]
    [Guid("3050F625-98B5-11CF-BB82-00AA00BDCE0B")]
    [TypeLibType(4112)]
    public interface HTMLWindowEvents2
    {
        [DispId(1025)]
        void onafterprint(IHTMLEventObj pEvtObj);
        [DispId(1024)]
        void onbeforeprint(IHTMLEventObj pEvtObj);
        [DispId(1017)]
        void onbeforeunload(IHTMLEventObj pEvtObj);
        [DispId(-2147418112)]
        void onblur(IHTMLEventObj pEvtObj);
        [DispId(1002)]
        void onerror(string description, string url, int line);
        [DispId(-2147418111)]
        void onfocus(IHTMLEventObj pEvtObj);
        [DispId(-2147418102)]
        bool onhelp(IHTMLEventObj pEvtObj);
        [DispId(1003)]
        void onload(IHTMLEventObj pEvtObj);
        [DispId(1016)]
        void onresize(IHTMLEventObj pEvtObj);
        [DispId(1014)]
        void onscroll(IHTMLEventObj pEvtObj);
        [DispId(1008)]
        void onunload(IHTMLEventObj pEvtObj);
    }

    [Guid("3050F32D-98B5-11CF-BB82-00AA00BDCE0B")]
    [TypeLibType(4160)]
    public interface IHTMLEventObj
    {
        [DispId(1002)]
        bool altKey { get; }
        [DispId(1012)]
        int button { get; }
        [DispId(1008)]
        bool cancelBubble { get; set; }
        [DispId(1020)]
        int clientX { get; }
        [DispId(1021)]
        int clientY { get; }
        [DispId(1003)]
        bool ctrlKey { get; }
        [DispId(1009)]
        IHTMLElement fromElement { get; }
        [DispId(1011)]
        int keyCode { get; set; }
        [DispId(1022)]
        int offsetX { get; }
        [DispId(1023)]
        int offsetY { get; }
        [DispId(1014)]
        string qualifier { get; }
        [DispId(1015)]
        int reason { get; }
        [DispId(1007)]
        object returnValue { get; set; }
        [DispId(1024)]
        int screenX { get; }
        [DispId(1025)]
        int screenY { get; }
        [DispId(1004)]
        bool shiftKey { get; }
        [DispId(1001)]
        IHTMLElement srcElement { get; }
        [DispId(1026)]
        object srcFilter { get; }
        [DispId(1010)]
        IHTMLElement toElement { get; }
        [DispId(1013)]
        string type { get; }
        [DispId(1005)]
        int x { get; }
        [DispId(1006)]
        int y { get; }
    }

    [Guid("D30C1661-CDAF-11D0-8A3E-00C04FC9E26E")]
    [TypeLibType(4176)]
    public interface IWebBrowser2 : IWebBrowserApp
    {
        [DispId(555)]
        bool AddressBar { get; set; }
        [DispId(200)]
        object Application { get; }
        [DispId(212)]
        bool Busy { get; }
        [DispId(202)]
        object Container { get; }
        [DispId(203)]
        object Document { get; }
        [DispId(400)]
        string FullName { get; }
        [DispId(407)]
        bool FullScreen { get; set; }
        [DispId(209)]
        int Height { get; set; }
        [DispId(-515)]
        int HWND { get; }
        [DispId(206)]
        int Left { get; set; }
        [DispId(210)]
        string LocationName { get; }
        [DispId(211)]
        string LocationURL { get; }
        [DispId(406)]
        bool MenuBar { get; set; }
        [DispId(0)]
        string Name { get; }
        [DispId(550)]
        bool Offline { get; set; }
        [DispId(201)]
        object Parent { get; }
        [DispId(401)]
        string Path { get; }
        [DispId(-525)]
        tagREADYSTATE ReadyState { get; }
        [DispId(552)]
        bool RegisterAsBrowser { get; set; }
        [DispId(553)]
        bool RegisterAsDropTarget { get; set; }
        [DispId(556)]
        bool Resizable { get; set; }
        [DispId(551)]
        bool Silent { get; set; }
        [DispId(403)]
        bool StatusBar { get; set; }
        [DispId(404)]
        string StatusText { get; set; }
        [DispId(554)]
        bool TheaterMode { get; set; }
        [DispId(405)]
        int ToolBar { get; set; }
        [DispId(207)]
        int Top { get; set; }
        [DispId(204)]
        bool TopLevelContainer { get; }
        [DispId(205)]
        string Type { get; }
        [DispId(402)]
        bool Visible { get; set; }
        [DispId(208)]
        int Width { get; set; }

        [DispId(301)]
        void ClientToWindow(ref int pcx, ref int pcy);
        [DispId(502)]
        void ExecWB(OLECMDID cmdID, OLECMDEXECOPT cmdexecopt, ref object pvaIn, ref object pvaOut);
        [DispId(303)]
        object GetProperty(string Property);
        [DispId(100)]
        void GoBack();
        [DispId(101)]
        void GoForward();
        [DispId(102)]
        void GoHome();
        [DispId(103)]
        void GoSearch();
        [DispId(104)]
        void Navigate(string URL, ref object Flags, ref object TargetFrameName, ref object PostData, ref object Headers);
        [DispId(500)]
        void Navigate2(ref object URL, ref object Flags, ref object TargetFrameName, ref object PostData, ref object Headers);
        [DispId(302)]
        void PutProperty(string Property, object vtValue);
        [DispId(501)]
        OLECMDF QueryStatusWB(OLECMDID cmdID);
        [DispId(300)]
        void Quit();
        [DispId(-550)]
        void Refresh();
        [DispId(105)]
        void Refresh2(ref object Level);
        [DispId(503)]
        void ShowBrowserBar(ref object pvaClsid, ref object pvarShow, ref object pvarSize);
        [DispId(106)]
        void Stop();
    }

    public enum OLECMDF
    {
        OLECMDF_SUPPORTED = 1,
        OLECMDF_ENABLED = 2,
        OLECMDF_LATCHED = 4,
        OLECMDF_NINCHED = 8,
        OLECMDF_INVISIBLE = 16,
        OLECMDF_DEFHIDEONCTXTMENU = 32,
    }

    public enum OLECMDID
    {
        OLECMDID_OPEN = 1,
        OLECMDID_NEW = 2,
        OLECMDID_SAVE = 3,
        OLECMDID_SAVEAS = 4,
        OLECMDID_SAVECOPYAS = 5,
        OLECMDID_PRINT = 6,
        OLECMDID_PRINTPREVIEW = 7,
        OLECMDID_PAGESETUP = 8,
        OLECMDID_SPELL = 9,
        OLECMDID_PROPERTIES = 10,
        OLECMDID_CUT = 11,
        OLECMDID_COPY = 12,
        OLECMDID_PASTE = 13,
        OLECMDID_PASTESPECIAL = 14,
        OLECMDID_UNDO = 15,
        OLECMDID_REDO = 16,
        OLECMDID_SELECTALL = 17,
        OLECMDID_CLEARSELECTION = 18,
        OLECMDID_ZOOM = 19,
        OLECMDID_GETZOOMRANGE = 20,
        OLECMDID_UPDATECOMMANDS = 21,
        OLECMDID_REFRESH = 22,
        OLECMDID_STOP = 23,
        OLECMDID_HIDETOOLBARS = 24,
        OLECMDID_SETPROGRESSMAX = 25,
        OLECMDID_SETPROGRESSPOS = 26,
        OLECMDID_SETPROGRESSTEXT = 27,
        OLECMDID_SETTITLE = 28,
        OLECMDID_SETDOWNLOADSTATE = 29,
        OLECMDID_STOPDOWNLOAD = 30,
        OLECMDID_ONTOOLBARACTIVATED = 31,
        OLECMDID_FIND = 32,
        OLECMDID_DELETE = 33,
        OLECMDID_HTTPEQUIV = 34,
        OLECMDID_HTTPEQUIV_DONE = 35,
        OLECMDID_ENABLE_INTERACTION = 36,
        OLECMDID_ONUNLOAD = 37,
        OLECMDID_PROPERTYBAG2 = 38,
        OLECMDID_PREREFRESH = 39,
        OLECMDID_SHOWSCRIPTERROR = 40,
        OLECMDID_SHOWMESSAGE = 41,
        OLECMDID_SHOWFIND = 42,
        OLECMDID_SHOWPAGESETUP = 43,
        OLECMDID_SHOWPRINT = 44,
        OLECMDID_CLOSE = 45,
        OLECMDID_ALLOWUILESSSAVEAS = 46,
        OLECMDID_DONTDOWNLOADCSS = 47,
        OLECMDID_UPDATEPAGESTATUS = 48,
        OLECMDID_PRINT2 = 49,
        OLECMDID_PRINTPREVIEW2 = 50,
        OLECMDID_SETPRINTTEMPLATE = 51,
        OLECMDID_GETPRINTTEMPLATE = 52,
        OLECMDID_PAGEACTIONBLOCKED = 55,
        OLECMDID_PAGEACTIONUIQUERY = 56,
        OLECMDID_FOCUSVIEWCONTROLS = 57,
        OLECMDID_FOCUSVIEWCONTROLSQUERY = 58,
        OLECMDID_SHOWPAGEACTIONMENU = 59,
        OLECMDID_ADDTRAVELENTRY = 60,
        OLECMDID_UPDATETRAVELENTRY = 61,
        OLECMDID_UPDATEBACKFORWARDSTATE = 62,
        OLECMDID_OPTICAL_ZOOM = 63,
        OLECMDID_OPTICAL_GETZOOMRANGE = 64,
        OLECMDID_WINDOWSTATECHANGED = 65,
    }

    public enum OLECMDEXECOPT
    {
        OLECMDEXECOPT_DODEFAULT = 0,
        OLECMDEXECOPT_PROMPTUSER = 1,
        OLECMDEXECOPT_DONTPROMPTUSER = 2,
        OLECMDEXECOPT_SHOWHELP = 3,
    }

    [Guid("0002DF05-0000-0000-C000-000000000046")]
    [TypeLibType(4176)]
    public interface IWebBrowserApp : IWebBrowser
    {
        [DispId(200)]
        object Application { get; }
        [DispId(212)]
        bool Busy { get; }
        [DispId(202)]
        object Container { get; }
        [DispId(203)]
        object Document { get; }
        [DispId(400)]
        string FullName { get; }
        [DispId(407)]
        bool FullScreen { get; set; }
        [DispId(209)]
        int Height { get; set; }
        [DispId(-515)]
        int HWND { get; }
        [DispId(206)]
        int Left { get; set; }
        [DispId(210)]
        string LocationName { get; }
        [DispId(211)]
        string LocationURL { get; }
        [DispId(406)]
        bool MenuBar { get; set; }
        [DispId(0)]
        string Name { get; }
        [DispId(201)]
        object Parent { get; }
        [DispId(401)]
        string Path { get; }
        [DispId(403)]
        bool StatusBar { get; set; }
        [DispId(404)]
        string StatusText { get; set; }
        [DispId(405)]
        int ToolBar { get; set; }
        [DispId(207)]
        int Top { get; set; }
        [DispId(204)]
        bool TopLevelContainer { get; }
        [DispId(205)]
        string Type { get; }
        [DispId(402)]
        bool Visible { get; set; }
        [DispId(208)]
        int Width { get; set; }

        [DispId(301)]
        void ClientToWindow(ref int pcx, ref int pcy);
        [DispId(303)]
        object GetProperty(string Property);
        [DispId(100)]
        void GoBack();
        [DispId(101)]
        void GoForward();
        [DispId(102)]
        void GoHome();
        [DispId(103)]
        void GoSearch();
        [DispId(104)]
        void Navigate(string URL, ref object Flags, ref object TargetFrameName, ref object PostData, ref object Headers);
        [DispId(302)]
        void PutProperty(string Property, object vtValue);
        [DispId(300)]
        void Quit();
        [DispId(-550)]
        void Refresh();
        [DispId(105)]
        void Refresh2(ref object Level);
        [DispId(106)]
        void Stop();
    }


    [Guid("EAB22AC1-30C1-11CF-A7EB-0000C05BAE0B")]
    [TypeLibType(4176)]
    public interface IWebBrowser
    {
        [DispId(200)]
        object Application { get; }
        [DispId(212)]
        bool Busy { get; }
        [DispId(202)]
        object Container { get; }
        [DispId(203)]
        object Document { get; }
        [DispId(209)]
        int Height { get; set; }
        [DispId(206)]
        int Left { get; set; }
        [DispId(210)]
        string LocationName { get; }
        [DispId(211)]
        string LocationURL { get; }
        [DispId(201)]
        object Parent { get; }
        [DispId(207)]
        int Top { get; set; }
        [DispId(204)]
        bool TopLevelContainer { get; }
        [DispId(205)]
        string Type { get; }
        [DispId(208)]
        int Width { get; set; }

        [DispId(100)]
        void GoBack();
        [DispId(101)]
        void GoForward();
        [DispId(102)]
        void GoHome();
        [DispId(103)]
        void GoSearch();
        [DispId(104)]
        void Navigate(string URL, ref object Flags, ref object TargetFrameName, ref object PostData, ref object Headers);
        [DispId(-550)]
        void Refresh();
        [DispId(105)]
        void Refresh2(ref object Level);
        [DispId(106)]
        void Stop();
    }

    [TypeLibType(16)]
    [ComVisible(false)]
    public interface DWebBrowserEvents2_Event
    {
        event DWebBrowserEvents2_BeforeNavigate2EventHandler BeforeNavigate2;
        event DWebBrowserEvents2_ClientToHostWindowEventHandler ClientToHostWindow;
        event DWebBrowserEvents2_CommandStateChangeEventHandler CommandStateChange;
        event DWebBrowserEvents2_DocumentCompleteEventHandler DocumentComplete;
        event DWebBrowserEvents2_DownloadBeginEventHandler DownloadBegin;
        event DWebBrowserEvents2_DownloadCompleteEventHandler DownloadComplete;
        event DWebBrowserEvents2_FileDownloadEventHandler FileDownload;
        event DWebBrowserEvents2_NavigateComplete2EventHandler NavigateComplete2;
        event DWebBrowserEvents2_NavigateErrorEventHandler NavigateError;
        event DWebBrowserEvents2_NewWindow2EventHandler NewWindow2;
        event DWebBrowserEvents2_NewWindow3EventHandler NewWindow3;
        event DWebBrowserEvents2_OnFullScreenEventHandler OnFullScreen;
        event DWebBrowserEvents2_OnMenuBarEventHandler OnMenuBar;
        event DWebBrowserEvents2_OnQuitEventHandler OnQuit;
        event DWebBrowserEvents2_OnStatusBarEventHandler OnStatusBar;
        event DWebBrowserEvents2_OnTheaterModeEventHandler OnTheaterMode;
        event DWebBrowserEvents2_OnToolBarEventHandler OnToolBar;
        event DWebBrowserEvents2_OnVisibleEventHandler OnVisible;
        event DWebBrowserEvents2_PrintTemplateInstantiationEventHandler PrintTemplateInstantiation;
        event DWebBrowserEvents2_PrintTemplateTeardownEventHandler PrintTemplateTeardown;
        event DWebBrowserEvents2_PrivacyImpactedStateChangeEventHandler PrivacyImpactedStateChange;
        event DWebBrowserEvents2_ProgressChangeEventHandler ProgressChange;
        event DWebBrowserEvents2_PropertyChangeEventHandler PropertyChange;
        event DWebBrowserEvents2_SetPhishingFilterStatusEventHandler SetPhishingFilterStatus;
        event DWebBrowserEvents2_SetSecureLockIconEventHandler SetSecureLockIcon;
        event DWebBrowserEvents2_StatusTextChangeEventHandler StatusTextChange;
        event DWebBrowserEvents2_TitleChangeEventHandler TitleChange;
        event DWebBrowserEvents2_UpdatePageStatusEventHandler UpdatePageStatus;
        event DWebBrowserEvents2_WindowClosingEventHandler WindowClosing;
        event DWebBrowserEvents2_WindowSetHeightEventHandler WindowSetHeight;
        event DWebBrowserEvents2_WindowSetLeftEventHandler WindowSetLeft;
        event DWebBrowserEvents2_WindowSetResizableEventHandler WindowSetResizable;
        event DWebBrowserEvents2_WindowSetTopEventHandler WindowSetTop;
        event DWebBrowserEvents2_WindowSetWidthEventHandler WindowSetWidth;
        event DWebBrowserEvents2_WindowStateChangedEventHandler WindowStateChanged;
    }

[ComVisible(false)]
public delegate void DWebBrowserEvents2_BeforeNavigate2EventHandler(object pDisp, ref object URL, ref object Flags, ref object TargetFrameName, ref object PostData, ref object Headers, ref bool Cancel);

[ComVisible(false)]
public delegate void DWebBrowserEvents2_ClientToHostWindowEventHandler(ref int CX, ref int CY);

[ComVisible(false)]
public delegate void DWebBrowserEvents2_CommandStateChangeEventHandler(int Command, bool Enable);

[ComVisible(false)]
public delegate void DWebBrowserEvents2_DocumentCompleteEventHandler(object pDisp, ref object URL);

[ComVisible(false)]
public delegate void DWebBrowserEvents2_DownloadBeginEventHandler();

[ComVisible(false)]
public delegate void DWebBrowserEvents2_DownloadCompleteEventHandler();

[ComVisible(false)]
public delegate void DWebBrowserEvents2_FileDownloadEventHandler(bool ActiveDocument, ref bool Cancel);

[ComVisible(false)]
public delegate void DWebBrowserEvents2_NavigateComplete2EventHandler(object pDisp, ref object URL);

[ComVisible(false)]
public delegate void DWebBrowserEvents2_NavigateErrorEventHandler(object pDisp, ref object URL, ref object Frame, ref object StatusCode, ref bool Cancel);

[ComVisible(false)]
public delegate void DWebBrowserEvents2_OnFullScreenEventHandler(bool FullScreen);

[ComVisible(false)]
public delegate void DWebBrowserEvents2_NewWindow3EventHandler(ref object ppDisp, ref bool Cancel, uint dwFlags, string bstrUrlContext, string bstrUrl);

[ComVisible(false)]
public delegate void DWebBrowserEvents2_OnMenuBarEventHandler(bool MenuBar);

[ComVisible(false)]
public delegate void DWebBrowserEvents2_OnQuitEventHandler();

[ComVisible(false)]
public delegate void DWebBrowserEvents2_OnStatusBarEventHandler(bool StatusBar);

[ComVisible(false)]
public delegate void DWebBrowserEvents2_OnTheaterModeEventHandler(bool TheaterMode);

[ComVisible(false)]
public delegate void DWebBrowserEvents2_OnToolBarEventHandler(bool ToolBar);

[ComVisible(false)]
public delegate void DWebBrowserEvents2_OnVisibleEventHandler(bool Visible);

[ComVisible(false)]
public delegate void DWebBrowserEvents2_PrintTemplateInstantiationEventHandler(object pDisp);

[ComVisible(false)]
public delegate void DWebBrowserEvents2_PrintTemplateTeardownEventHandler(object pDisp);

[ComVisible(false)]
public delegate void DWebBrowserEvents2_PrivacyImpactedStateChangeEventHandler(bool bImpacted);

[ComVisible(false)]
public delegate void DWebBrowserEvents2_ProgressChangeEventHandler(int Progress, int ProgressMax);

[ComVisible(false)]
public delegate void DWebBrowserEvents2_PropertyChangeEventHandler(string szProperty);

[ComVisible(false)]
public delegate void DWebBrowserEvents2_SetPhishingFilterStatusEventHandler(int PhishingFilterStatus);

[ComVisible(false)]
public delegate void DWebBrowserEvents2_SetSecureLockIconEventHandler(int SecureLockIcon);

[ComVisible(false)]
public delegate void DWebBrowserEvents2_StatusTextChangeEventHandler(string Text);

[ComVisible(false)]
public delegate void DWebBrowserEvents2_TitleChangeEventHandler(string Text);

[ComVisible(false)]
public delegate void DWebBrowserEvents2_UpdatePageStatusEventHandler(object pDisp, ref object nPage, ref object fDone);

[ComVisible(false)]
public delegate void DWebBrowserEvents2_WindowClosingEventHandler(bool IsChildWindow, ref bool Cancel);

[ComVisible(false)]
public delegate void DWebBrowserEvents2_WindowSetHeightEventHandler(int Height);

[ComVisible(false)]
public delegate void DWebBrowserEvents2_WindowSetLeftEventHandler(int Left);

[ComVisible(false)]
public delegate void DWebBrowserEvents2_WindowSetResizableEventHandler(bool Resizable);

[ComVisible(false)]
public delegate void DWebBrowserEvents2_WindowSetTopEventHandler(int Top);

[ComVisible(false)]
public delegate void DWebBrowserEvents2_WindowSetWidthEventHandler(int Width);

[ComVisible(false)]
public delegate void DWebBrowserEvents2_WindowStateChangedEventHandler(uint dwWindowStateFlags, uint dwValidFlagsMask);

[ComVisible(false)]
public delegate void DWebBrowserEvents2_NewWindow2EventHandler(ref object ppDisp, ref bool Cancel);

    public struct tagPOINT
    {
        public int x;
        public int y;
    }

    public struct tagRECT
    {
        public int bottom;
        public int left;
        public int right;
        public int top;
    }

    public enum tagREADYSTATE
    {
        READYSTATE_UNINITIALIZED = 0,
        READYSTATE_LOADING = 1,
        READYSTATE_LOADED = 2,
        READYSTATE_INTERACTIVE = 3,
        READYSTATE_COMPLETE = 4,
    }

    [Guid("626FC520-A41E-11CF-A731-00A0C9082637")]
    [TypeLibType(4160)]
    public interface IHTMLDocument
    {
        [DispId(1001)]
        object Script { get; }
    }

    [TypeLibType(4160)]
    [Guid("332C4425-26CB-11D0-B483-00C04FD90119")]
    public interface IHTMLDocument2 : IHTMLDocument
    {
        [DispId(1005)]
        IHTMLElement activeElement { get; }
        [DispId(1022)]
        object alinkColor { get; set; }
        [DispId(1003)]
        IHTMLElementCollection all { get; }
        [DispId(1007)]
        IHTMLElementCollection anchors { get; }
        [DispId(1008)]
        IHTMLElementCollection applets { get; }
        [DispId(-501)]
        object bgColor { get; set; }
        [DispId(1004)]
        IHTMLElement body { get; }
        [DispId(1032)]
        string charset { get; set; }
        [DispId(1030)]
        string cookie { get; set; }
        [DispId(1033)]
        string defaultCharset { get; set; }
        [DispId(1014)]
        string designMode { get; set; }
        [DispId(1029)]
        string domain { get; set; }
        [DispId(1015)]
        IHTMLElementCollection embeds { get; }
        [DispId(1031)]
        bool expando { get; set; }
        [DispId(-2147413110)]
        object fgColor { get; set; }
        [DispId(1043)]
        string fileCreatedDate { get; }
        [DispId(1044)]
        string fileModifiedDate { get; }
        [DispId(1042)]
        string fileSize { get; }
        [DispId(1045)]
        string fileUpdatedDate { get; }
        [DispId(1010)]
        IHTMLElementCollection forms { get; }
        [DispId(1019)]
        object frames { get; }
        [DispId(1011)]
        IHTMLElementCollection images { get; }
        [DispId(1028)]
        string lastModified { get; }
        [DispId(1024)]
        object linkColor { get; set; }
        [DispId(1009)]
        IHTMLElementCollection links { get; }
        [DispId(1026)]
        object location { get; }
        [DispId(1041)]
        string mimeType { get; }
        [DispId(1048)]
        string nameProp { get; }
        [DispId(-2147412090)]
        object onafterupdate { get; set; }
        [DispId(-2147412091)]
        object onbeforeupdate { get; set; }
        [DispId(-2147412104)]
        object onclick { get; set; }
        [DispId(-2147412103)]
        object ondblclick { get; set; }
        [DispId(-2147412077)]
        object ondragstart { get; set; }
        [DispId(-2147412074)]
        object onerrorupdate { get; set; }
        [DispId(-2147412099)]
        object onhelp { get; set; }
        [DispId(-2147412107)]
        object onkeydown { get; set; }
        [DispId(-2147412105)]
        object onkeypress { get; set; }
        [DispId(-2147412106)]
        object onkeyup { get; set; }
        [DispId(-2147412110)]
        object onmousedown { get; set; }
        [DispId(-2147412108)]
        object onmousemove { get; set; }
        [DispId(-2147412111)]
        object onmouseout { get; set; }
        [DispId(-2147412112)]
        object onmouseover { get; set; }
        [DispId(-2147412109)]
        object onmouseup { get; set; }
        [DispId(-2147412087)]
        object onreadystatechange { get; set; }
        [DispId(-2147412093)]
        object onrowenter { get; set; }
        [DispId(-2147412094)]
        object onrowexit { get; set; }
        [DispId(-2147412075)]
        object onselectstart { get; set; }
        [DispId(1034)]
        object parentWindow { get; }
        [DispId(1021)]
        IHTMLElementCollection plugins { get; }
        [DispId(1047)]
        string protocol { get; }
        [DispId(1018)]
        string readyState { get; }
        [DispId(1027)]
        string referrer { get; }
        [DispId(1001)]
        object Script { get; }
        [DispId(1013)]
        IHTMLElementCollection scripts { get; }
        [DispId(1046)]
        string security { get; }
        [DispId(1017)]
        object selection { get; }
        [DispId(1069)]
        object styleSheets { get; }
        [DispId(1012)]
        string title { get; set; }
        [DispId(1025)]
        string url { get; set; }
        [DispId(1023)]
        object vlinkColor { get; set; }

        [DispId(1058)]
        void clear();
        [DispId(1057)]
        void close();
        [DispId(1067)]
        IHTMLElement createElement(string eTag);
        [DispId(1071)]
        IHTMLStyleSheet createStyleSheet(string bstrHref, int lIndex);
        [DispId(1068)]
        IHTMLElement elementFromPoint(int x, int y);
        [DispId(1065)]
        bool execCommand(string cmdID, bool showUI, object value);
        [DispId(1066)]
        bool execCommandShowHelp(string cmdID);
        [DispId(1056)]
        object open(string url, object name, object features, object replace);
        [DispId(1060)]
        bool queryCommandEnabled(string cmdID);
        [DispId(1062)]
        bool queryCommandIndeterm(string cmdID);
        [DispId(1061)]
        bool queryCommandState(string cmdID);
        [DispId(1059)]
        bool queryCommandSupported(string cmdID);
        [DispId(1063)]
        string queryCommandText(string cmdID);
        [DispId(1064)]
        object queryCommandValue(string cmdID);
        [DispId(1070)]
        string toString();
        [DispId(1054)]
        void write(params object[] psarray);
        [DispId(1055)]
        void writeln(params object[] psarray);
    }

    [TypeLibType(4160)]
    [Guid("3050F21F-98B5-11CF-BB82-00AA00BDCE0B")]
    public interface IHTMLElementCollection : IEnumerable
    {
        [DispId(1500)]
        int length { get; set; }

        [TypeLibFunc(65)]
        [DispId(-4)]
        IEnumerator GetEnumerator();
        [DispId(0)]
        object item(object name, object index);
        [DispId(1502)]
        object tags(object tagName);
        [DispId(1501)]
        string toString();
    }

    [Guid("3050F2E3-98B5-11CF-BB82-00AA00BDCE0B")]
    [TypeLibType(4160)]
    public interface IHTMLStyleSheet
    {
        [DispId(1014)]
        string cssText { get; set; }
        [DispId(-2147418036)]
        bool disabled { get; set; }
        [DispId(1006)]
        string href { get; set; }
        [DispId(1008)]
        string id { get; }
        [DispId(1005)]
        object imports { get; }
        [DispId(1013)]
        string media { get; set; }
        [DispId(1003)]
        IHTMLElement owningElement { get; }
        [DispId(1002)]
        IHTMLStyleSheet parentStyleSheet { get; }
        [DispId(1004)]
        bool readOnly { get; }
        [DispId(1015)]
        object rules { get; }
        [DispId(1001)]
        string title { get; set; }
        [DispId(1007)]
        string type { get; }

        [DispId(1009)]
        int addImport(string bstrUrl, int lIndex);
        [DispId(1010)]
        int addRule(string bstrSelector, string bstrStyle, int lIndex);
        [DispId(1011)]
        void removeImport(int lIndex);
        [DispId(1012)]
        void removeRule(int lIndex);
    }

    
}


