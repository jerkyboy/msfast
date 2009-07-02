//=======================================================================
/* Project: MSFast (MySpace.MSFast.SysImpl.Win32)
*  Original author: Yadid Ramot (e.yadid@gmail.com)
*  Copyright (C) 2009 MySpace.com 
*
*  This file is part of MSFast.
*  MSFast is free software: you can redistribute it and/or modify
*  it under the terms of the GNU General Public License as published by
*  the Free Software Foundation, either version 3 of the License, or
*  (at your option) any later version.
*
*  MSFast is distributed in the hope that it will be useful,
*  but WITHOUT ANY WARRANTY; without even the implied warranty of
*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*  GNU General Public License for more details.
* 
*  You should have received a copy of the GNU General Public License
*  along with MSFast.  If not, see <http://www.gnu.org/licenses/>.
*/
//=======================================================================

//Imports
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using mshtml;
using System.Runtime.InteropServices.ComTypes;

namespace MySpace.MSFast.SysImpl.Win32.ComIterop
{
    #region DBIM enum
    /// <summary>
    /// Interop definition for DBIM
    /// </summary>
    [Flags]
    public enum DBIM : uint
    {
        MINSIZE = 0x0001,
        MAXSIZE = 0x0002,
        INTEGRAL = 0x0004,
        ACTUAL = 0x0008,
        TITLE = 0x0010,
        MODEFLAGS = 0x0020,
        BKCOLOR = 0x0040
    }
    #endregion

    #region DBIMF enum
    /// <summary>
    /// Interop definition for DBIMF
    /// </summary>
    [Flags]
    public enum DBIMF : uint
    {
        NORMAL = 0x0000,
        FIXED = 0x0001,
        FIXEDBMP = 0x0004,   // a fixed background bitmap (if supported)
        VARIABLEHEIGHT = 0x0008,
        UNDELETEABLE = 0x0010,
        DEBOSSED = 0x0020,
        BKCOLOR = 0x0040,
        USECHEVRON = 0x0080,
        BREAK = 0x0100,
        ADDTOFRONT = 0x0200,
        TOPALIGN = 0x0400
    }
    #endregion

    #region DESKBANDINFO struct
    /// <summary>
    /// Interop definition for DESKBANDINFO
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct DESKBANDINFO
    {
        public DBIM dwMask;
        public Point ptMinSize;
        public Point ptMaxSize;
        public Point ptIntegral;
        public Point ptActual;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 255)]
        public String wszTitle;
        public DBIMF dwModeFlags;
        public Int32 crBkgnd;
    };
    #endregion

    #region POINT struct
    /// <summary>
    /// Interop definition for POINT
    /// </summary>
    public struct POINT
    {
        public Int32 x;
        public Int32 y;
    }
    #endregion

    #region MSG struct
    /// <summary>
    /// Interop definition for MSG
    /// </summary>
    public struct MSG
    {
        public IntPtr hwnd;
        public UInt32 message;
        public UInt32 wParam;
        public Int32 lParam;
        public UInt32 time;
        public POINT pt;
    }
    #endregion

    #region HRESULT class
    /// <summary>
    /// Interop definition for HRESULT
    /// </summary>
    public class HRESULT
    {
        public static readonly int S_OK = 0;
        public static readonly int S_FALSE = 1;
        public static readonly int E_NOTIMPL = 2;
        public static readonly int E_INVALIDARG = 3;
        public static readonly int E_FAIL = 4;
    }
    #endregion

    #region ICustomDoc interface
    /// <summary>
    /// Interop definition for ICustomDoc
    /// </summary>
    [ComImport(), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), GuidAttribute("3050f3f0-98b5-11cf-bb82-00aa00bdce0b")]
    public interface ICustomDoc
    {
        [PreserveSig]
        void SetUIHandler(IDocHostUIHandler pUIHandler);
    }
    #endregion

    #region IDeskBand interface
    /// <summary>
    /// Interop definition for IDeskBand
    /// </summary>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("EB0FE172-1A3A-11D0-89B3-00A0C90A90AC")]
    public interface IDeskBand
    {
        void GetWindow(out System.IntPtr phwnd);
        void ContextSensitiveHelp([In] bool fEnterMode);

        void ShowDW([In] bool fShow);
        void CloseDW([In] UInt32 dwReserved);

        void ResizeBorderDW(
            IntPtr prcBorder,
            [In, MarshalAs(UnmanagedType.IUnknown)] Object punkToolbarSite,
            bool fReserved);

        void GetBandInfo(
            UInt32 dwBandID,
            UInt32 dwViewMode,
            ref DESKBANDINFO pdbi);
    }
    #endregion

    #region DOCHOSTUITYPE enum
    /// <summary>
    /// Interop definition for DOCHOSTUITYPE
    /// </summary>
    public enum DOCHOSTUITYPE
    {
        DOCHOSTUITYPE_BROWSE = 0,
        DOCHOSTUITYPE_AUTHOR = 1
    }
    #endregion

    #region DOCHOSTUIDBLCLK enum
    /// <summary>
    /// Interop definition for DOCHOSTUIDBLCLK
    /// </summary>
    public enum DOCHOSTUIDBLCLK
    {
        DOCHOSTUIDBLCLK_DEFAULT = 0,
        DOCHOSTUIDBLCLK_SHOWPROPERTIES = 1,
        DOCHOSTUIDBLCLK_SHOWCODE = 2
    }
    #endregion

    #region DOCHOSTUIFLAG enum
    /// <summary>
    /// Interop definition for DOCHOSTUIFLAG
    /// </summary>
    public enum DOCHOSTUIFLAG
    {
        DOCHOSTUIFLAG_DIALOG = 0x00000001,
        DOCHOSTUIFLAG_DISABLE_HELP_MENU = 0x00000002,
        DOCHOSTUIFLAG_NO3DBORDER = 0x00000004,
        DOCHOSTUIFLAG_SCROLL_NO = 0x00000008,
        DOCHOSTUIFLAG_DISABLE_SCRIPT_INACTIVE = 0x00000010,
        DOCHOSTUIFLAG_OPENNEWWIN = 0x00000020,
        DOCHOSTUIFLAG_DISABLE_OFFSCREEN = 0x00000040,
        DOCHOSTUIFLAG_FLAT_SCROLLBAR = 0x00000080,
        DOCHOSTUIFLAG_DIV_BLOCKDEFAULT = 0x00000100,
        DOCHOSTUIFLAG_ACTIVATE_CLIENTHIT_ONLY = 0x00000200,
        DOCHOSTUIFLAG_OVERRIDEBEHAVIORFACTORY = 0x00000400,
        DOCHOSTUIFLAG_CODEPAGELINKEDFONTS = 0x00000800,
        DOCHOSTUIFLAG_URL_ENCODING_DISABLE_UTF8 = 0x00001000,
        DOCHOSTUIFLAG_URL_ENCODING_ENABLE_UTF8 = 0x00002000,
        DOCHOSTUIFLAG_ENABLE_FORMS_AUTOCOMPLETE = 0x00004000,
        DOCHOSTUIFLAG_ENABLE_INPLACE_NAVIGATION = 0x00010000,
        DOCHOSTUIFLAG_IME_ENABLE_RECONVERSION = 0x00020000,
        DOCHOSTUIFLAG_THEME = 0x00040000,
        DOCHOSTUIFLAG_NOTHEME = 0x00080000,
        DOCHOSTUIFLAG_NOPICS = 0x00100000,
        DOCHOSTUIFLAG_NO3DOUTERBORDER = 0x00200000,
        DOCHOSTUIFLAG_DELEGATESIDOFDISPATCH = 0x00400000
    }
    #endregion

    #region DOCHOSTUIINFO struct
    /// <summary>
    /// Interop definition for DOCHOSTUIINFO
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DOCHOSTUIINFO
    {
        public uint cbSize;
        public uint dwFlags;
        public uint dwDoubleClick;
        [MarshalAs(UnmanagedType.BStr)]
        public string pchHostCss;
        [MarshalAs(UnmanagedType.BStr)]
        public string pchHostNS;
    }
    #endregion

    #region tagMSG struct
    /// <summary>
    /// Interop definition for tagMSG
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct tagMSG
    {
        public IntPtr hwnd;
        public int lParam;
        public uint message;
        public tagPOINT pt;
        public uint time;
        public uint wParam;
    }
    #endregion

    #region IDocHostUIHandler interface
    /// <summary>
    /// Interop definition for IDocHostUIHandler
    /// </summary>
    [ComImport(), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), GuidAttribute("bd3f23c0-d43e-11cf-893b-00aa00bdce1a")]
    public interface IDocHostUIHandler
    {
        [PreserveSig]
        void ShowContextMenu(uint dwID, ref tagPOINT ppt,
                                    [MarshalAs(UnmanagedType.IUnknown)]  object pcmdtReserved,
                                    [MarshalAs(UnmanagedType.IDispatch)] object pdispReserved);

        [PreserveSig]
        void GetHostInfo(ref DOCHOSTUIINFO pInfo);

        [PreserveSig]
        void ShowUI(uint dwID, IntPtr pActiveObject, IntPtr pCommandTarget, IntPtr pFrame, IntPtr pDoc);

        [PreserveSig]
        void HideUI();

        [PreserveSig]
        void UpdateUI();

        [PreserveSig]
        void EnableModeless(int fEnable);

        [PreserveSig]
        void OnDocWindowActivate(int fActivate);

        [PreserveSig]
        void OnFrameWindowActivate(int fActivate);

        [PreserveSig]
        void ResizeBorder(ref tagRECT prcBorder,
                              IntPtr pUIWindow,
                              int fRameWindow);

        [PreserveSig]
        uint TranslateAccelerator(ref tagMSG lpMsg, ref Guid pguidCmdGroup, uint nCmdID);

        [PreserveSig]
        void GetOptionKeyPath(ref string pchKey, uint dw);

        [PreserveSig]
        void GetDropTarget(IntPtr pDropTarget, IntPtr ppDropTarget);

        [PreserveSig]
        void GetExternal([Out, MarshalAs(UnmanagedType.IDispatch)] out object ppDispatch);

        [PreserveSig]
        void TranslateUrl(uint dwTranslate,
                                [MarshalAs(UnmanagedType.BStr)]  string pchURLIn,
                                [MarshalAs(UnmanagedType.BStr)]  ref string ppchURLOut);

        [PreserveSig]
        void FilterDataObject(IDataObject pDO, ref IDataObject ppDORet);
    }
    #endregion

    #region IDocHostUIHandler2 interface
    /// <summary>
    /// Interop definition for IDocHostUIHandler2
    /// </summary>
    [ComImport(), GuidAttribute("3050f6d0-98b5-11cf-bb82-00aa00bdce0b")]
    public interface IDocHostUIHandler2 : IDocHostUIHandler
    {
        [PreserveSig]
        void GetOverrideKeyPath(ref string pchKey, uint dw);
    }
    #endregion

    #region IDockingWindow interface
    /// <summary>
    /// Interop definition for IDockingWindow
    /// </summary>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("012dd920-7b26-11d0-8ca9-00a0c92dbfe8")]
    public interface IDockingWindow
    {
        void GetWindow(out System.IntPtr phwnd);
        void ContextSensitiveHelp([In] bool fEnterMode);

        void ShowDW([In] bool fShow);
        void CloseDW([In] UInt32 dwReserved);
        void ResizeBorderDW(
            IntPtr prcBorder,
            [In, MarshalAs(UnmanagedType.IUnknown)] Object punkToolbarSite,
            bool fReserved);
    }
    #endregion

    #region IInputObject interface
    /// <summary>
    /// Interop definition for IInputObject
    /// </summary>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("68284faa-6a48-11d0-8c78-00c04fd918b4")]
    public interface IInputObject
    {
        void UIActivateIO(Int32 fActivate, ref MSG msg);

        [PreserveSig]
        //[return:MarshalAs(UnmanagedType.Error)]
        Int32 HasFocusIO();

        [PreserveSig]
        Int32 TranslateAcceleratorIO(ref MSG msg);
    }
    #endregion

    #region IInputObjectSite interface
    /// <summary>
    /// Interop definition for IInputObjectSite
    /// </summary>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("f1db8392-7331-11d0-8c99-00a0c92dbfe8")]
    public interface IInputObjectSite
    {
        [PreserveSig]
        Int32 OnFocusChangeIS([MarshalAs(UnmanagedType.IUnknown)] Object punkObj, Int32 fSetFocus);
    }
    #endregion

    #region IObjectWithSite interface
    /// <summary>
    /// Interop definition for IObjectWithSite
    /// </summary>
    [ComImport(), Guid("fc4801a3-2ba9-11cf-a229-00aa003d7352")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IObjectWithSite
    {

        void SetSite([In, MarshalAs(UnmanagedType.IUnknown)] object pUnkSite);
        void GetSite(ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppvSite);

    }
    #endregion

    #region IObserver interface
    /// <summary>
    /// Interop definition for IObserver
    /// </summary>
    [GuidAttribute("181C179B-7CC9-4457-8C1D-4B45E7C8589D")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsDual)]
    public interface IObserver
    {
    }
    #endregion

    #region OLECMDTEXT struct
    /// <summary>
    /// Interop definition for OLECMDTEXT
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct OLECMDTEXT
    {
        public uint cmdtextf;
        public uint cwActual;
        public uint cwBuf;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public char rgwz;
    }
    #endregion

    #region OLECMD struct
    /// <summary>
    /// Interop definition for OLECMD
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct OLECMD
    {
        public uint cmdID;
        public uint cmdf;
    }
    #endregion

    #region OLEVARIANT class
    /// <summary>
    /// Interop definition for OLEVARIANT
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public class OLEVARIANT
    {
        [FieldOffset(0)]
        public Int16 vt;
        [FieldOffset(2)]
        public Int16 wReserved1;
        [FieldOffset(4)]
        public Int16 wReserved2;
        [FieldOffset(6)]
        public Int16 wReserved3;
        [FieldOffset(8)]
        public int lVal;
        [FieldOffset(8)]
        public short iVal;
        [FieldOffset(8)]
        public IntPtr bstrVal;
        [FieldOffset(8)]
        public IntPtr pUnkVal;
        [FieldOffset(8)]
        public IntPtr pArray;
        [FieldOffset(8)]
        public IntPtr pvRecord;
        [FieldOffset(12)]
        public IntPtr pRecInfo;
    }
    #endregion

    #region OLEVARIANT_TYPE enum
    /// <summary>
    /// Interop definition for OLEVARIANT_TYPE
    /// </summary>
    public enum OLEVARIANT_TYPE : int
    {
        VT_EMPTY = 0,
        VT_UNKNOWN = 1,
        VT_NULL = 2,
        VT_BOOL = 3,
        VT_UI2 = 4,
        VT_I1 = 5,
        VT_UI1 = 6,
        VT_I2 = 7,

        VT_I4 = 9,
        VT_UI4 = 10,
        VT_I8 = 11,
        VT_UI8 = 12,
        VT_R4 = 13,
        VT_R8 = 14,
        VT_DECIMAL = 15,
        VT_DATE = 16,
        VT_BSTR = 18
    }
    #endregion

    #region IOleCommandTarget interface
    /// <summary>
    /// Interop definition for IOleCommandTarget. 
    /// </summary>
    [ComImport,
    Guid("b722bccb-4e68-101b-a2bc-00aa00404770"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOleCommandTarget
    {
        //IMPORTANT: The order of the methods is critical here. You
        //perform early binding in most cases, so the order of the methods
        //here MUST match the order of their vtable layout (which is determined
        //by their layout in IDL). The interop calls key off the vtable ordering,
        //not the symbolic names. Therefore, if you switched these method declarations
        //and tried to call the Exec method on an IOleCommandTarget interface from your
        //application, it would translate into a call to the QueryStatus method instead.
        void QueryStatus(ref Guid pguidCmdGroup, UInt32 cCmds, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] OLECMD[] prgCmds, ref OLECMDTEXT CmdText);
        void Exec(ref Guid pguidCmdGroup, uint nCmdId, uint nCmdExecOpt, ref object pvaIn, ref object pvaOut);
    }
    #endregion

    #region IOleObject interface
    /// <summary>
    /// Interop definition for IOleObject
    /// </summary>
    [ComImport, Guid("00000112-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOleObject
    {
        void SetClientSite(IOleClientSite pClientSite);
        void GetClientSite(ref IOleClientSite ppClientSite);
        void SetHostNames(object szContainerApp, object szContainerObj);
        void Close(uint dwSaveOption);
        void SetMoniker(uint dwWhichMoniker, object pmk);
        void GetMoniker(uint dwAssign, uint dwWhichMoniker, ref object ppmk);
        void InitFromData(IDataObject pDataObject, bool fCreation, uint dwReserved);
        void GetClipboardData(uint dwReserved, ref IDataObject ppDataObject);
        void DoVerb(uint iVerb, uint lpmsg, object pActiveSite, uint lindex, uint hwndParent, uint lprcPosRect);
        void EnumVerbs(ref object ppEnumOleVerb);
        void Update();
        void IsUpToDate();
        void GetUserClassID(uint pClsid);
        void GetUserType(uint dwFormOfType, uint pszUserType);
        void SetExtent(uint dwDrawAspect, uint psizel);
        void GetExtent(uint dwDrawAspect, uint psizel);
        void Advise(object pAdvSink, uint pdwConnection);
        void Unadvise(uint dwConnection);
        void EnumAdvise(ref object ppenumAdvise);
        void GetMiscStatus(uint dwAspect, uint pdwStatus);
        void SetColorScheme(object pLogpal);
    };
    #endregion

    #region IOleClientSite interface
    /// <summary>
    /// Interop definition for IOleClientSite
    /// </summary>
    [ComImport, ComVisible(true)]
    [Guid("00000118-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOleClientSite
    {
        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int SaveObject();

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int GetMoniker(
             [In, MarshalAs(UnmanagedType.U4)]         uint dwAssign,
             [In, MarshalAs(UnmanagedType.U4)]         uint dwWhichMoniker,
             [Out, MarshalAs(UnmanagedType.Interface)] out IMoniker ppmk);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int GetContainer(
             [Out, MarshalAs(UnmanagedType.Interface)] out IOleContainer ppContainer);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int ShowObject();

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int OnShowWindow([In, MarshalAs(UnmanagedType.Bool)] bool fShow);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int RequestNewObjectLayout();
    }
    #endregion

    #region IOleContainer interface
    /// <summary>
    /// Interop definition for IOleContainer
    /// </summary>
    [ComImport(), ComVisible(true),
    Guid("0000011B-0000-0000-C000-000000000046"),
    InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOleContainer
    {
        //IParseDisplayName
        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int ParseDisplayName(
             [In, MarshalAs(UnmanagedType.Interface)] object pbc,
             [In, MarshalAs(UnmanagedType.BStr)]      string pszDisplayName,
             [Out, MarshalAs(UnmanagedType.LPArray)] int[] pchEaten,
             [Out, MarshalAs(UnmanagedType.LPArray)] object[] ppmkOut);

        //IOleContainer
        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int EnumObjects(
             [In, MarshalAs(UnmanagedType.U4)] tagOLECONTF grfFlags,
             out IEnumUnknown ppenum);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int LockContainer(
             [In, MarshalAs(UnmanagedType.Bool)] Boolean fLock);
    }
    #endregion

    #region tagOLECONTF enum
    /// <summary>
    /// Interop definition for tagOLECONTF
    /// </summary>
    public enum tagOLECONTF
    {
        OLECONTF_EMBEDDINGS = 1,
        OLECONTF_LINKS = 2,
        OLECONTF_OTHERS = 4,
        OLECONTF_ONLYUSER = 8,
        OLECONTF_ONLYIFRUNNING = 16
    }
    #endregion

    #region IOleWindow interface
    /// <summary>
    /// Interop definition for IOleWindow
    /// </summary>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("00000114-0000-0000-C000-000000000046")]
    public interface IOleWindow
    {
        void GetWindow(out System.IntPtr phwnd);
        void ContextSensitiveHelp([In] bool fEnterMode);
    }
    #endregion

    #region IPersist interface
    /// <summary>
    /// Interop definition for IPersist
    /// </summary>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("0000010c-0000-0000-C000-000000000046")]
    public interface IPersist
    {
        void GetClassID(out Guid pClassID);
    }
    #endregion

    #region IPersistFile interface
    /// <summary>
    /// Interop definition for IPersistFile
    /// </summary>
    [Guid("0000010b-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    public interface IPersistFile
    {
        void GetClassID(out Guid pClassID);
        void GetCurFile(out String ppszFileName);
        int IsDirty();
        void Load(String pszFileName, int dwMode);
        void Save(String pszFileName, bool fRemember);
        void SaveCompleted(String pszFileName);

    }
    #endregion

    #region IPersistStream interface
    /// <summary>
    /// Interop definition for IPersistStream
    /// </summary>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("00000109-0000-0000-C000-000000000046")]
    public interface IPersistStream
    {
        void GetClassID(out Guid pClassID);

        void IsDirty();

        void Load([In, MarshalAs(UnmanagedType.Interface)] Object pStm);

        void Save([In, MarshalAs(UnmanagedType.Interface)] Object pStm, [In] bool fClearDirty);

        void GetSizeMax([Out] out UInt64 pcbSize);
    }
    #endregion

    #region IPersistStreamInit Interface
    //MIDL_INTERFACE("7FD52380-4E07-101B-AE2D-08002B2EC713")
    //IPersistStreamInit : public IPersist
    //{
    //public:
    //    virtual HRESULT STDMETHODCALLTYPE IsDirty( void) = 0;

    //    virtual HRESULT STDMETHODCALLTYPE Load( 
    //        /* [in] */ LPSTREAM pStm) = 0;

    //    virtual HRESULT STDMETHODCALLTYPE Save( 
    //        /* [in] */ LPSTREAM pStm,
    //        /* [in] */ BOOL fClearDirty) = 0;

    //    virtual HRESULT STDMETHODCALLTYPE GetSizeMax( 
    //        /* [out] */ ULARGE_INTEGER *pCbSize) = 0;

    //    virtual HRESULT STDMETHODCALLTYPE InitNew( void) = 0;

    //};
    [ComVisible(true), ComImport(),
    Guid("7FD52380-4E07-101B-AE2D-08002B2EC713"),
    InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPersistStreamInit
    {
        void GetClassID(
            [In, Out] ref Guid pClassID);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int IsDirty();

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int Load(
            [In, MarshalAs(UnmanagedType.Interface)] 
            System.Runtime.InteropServices.ComTypes.IStream pstm);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int Save(
            [In, MarshalAs(UnmanagedType.Interface)]
            System.Runtime.InteropServices.ComTypes.IStream pstm,
            [In, MarshalAs(UnmanagedType.Bool)] bool fClearDirty);

        void GetSizeMax(
            [Out, MarshalAs(UnmanagedType.LPArray)] long pcbSize);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int InitNew();
    }
    #endregion

    #region IServiceProvider Interface
    //MIDL_INTERFACE("6d5140c1-7436-11ce-8034-00aa006009fa")
    //IServiceProvider : public IUnknown
    //{
    //public:
    //    virtual /* [local] */ HRESULT STDMETHODCALLTYPE QueryService( 
    //        /* [in] */ REFGUID guidService,
    //        /* [in] */ REFIID riid,
    //        /* [out] */ void __RPC_FAR *__RPC_FAR *ppvObject) = 0;

    //    template <class Q>
    //    HRESULT STDMETHODCALLTYPE QueryService(REFGUID guidService, Q** pp)
    //    {
    //        return QueryService(guidService, __uuidof(Q), (void **)pp);
    //    }
    //};
    [ComImport, ComVisible(true)]
    [Guid("6d5140c1-7436-11ce-8034-00aa006009fa")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IServiceProvider
    {
        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int QueryService(
            [In] ref Guid guidService,
            [In] ref Guid riid,
            [Out] out IntPtr ppvObject);
        //This does not work i.e.-> ppvObject = (INewWindowManager)this
        //[Out, MarshalAs(UnmanagedType.Interface)] out object ppvObject);
    }
    #endregion
}
