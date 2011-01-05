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

namespace MySpace.MSFast.SysImpl.Win32.Utils
{
	public class ScreenGrabber
	{
		public static Image GrabScreen(IntPtr hWnd, Point location, Size size) 
		{
			Image myImage = new Bitmap(size.Width, size.Height);
			Graphics g = Graphics.FromImage(myImage);
			IntPtr destDeviceContext = g.GetHdc();
			IntPtr srcDeviceContext = Win32API.GetWindowDC(hWnd);
			Win32API.BitBlt(destDeviceContext, 0, 0, size.Width, size.Height, srcDeviceContext, location.X, location.Y, Win32API.SRCCOPY);
			Win32API.ReleaseDC(hWnd, srcDeviceContext);
			g.ReleaseHdc(destDeviceContext);

			return myImage;		
		}

		public static Bitmap GrabScreen(IntPtr appHWND, IntPtr grabHWND)
		{
			return GrabScreen(appHWND,grabHWND,true);
		}

		public static Bitmap GrabScreen(IntPtr appHWND, IntPtr grabHWND, bool maximize)
		{
			if (grabHWND == IntPtr.Zero || appHWND == IntPtr.Zero)
				return null;

			if (maximize)
				Win32API.ShowWindow(appHWND, Win32API.WindowShowStyle.Maximize);//show it
			
			Win32API.SendMessage(grabHWND, Win32API.WindowsMessages.WM_PAINT, 0, 0); 

			RECT rc;

			Win32API.GetWindowRect(grabHWND, out rc);

			int width = rc.Right - rc.Left;
			int height = rc.Bottom - rc.Top;

			IntPtr hdc = Win32API.GetDC(IntPtr.Zero);
			IntPtr memDC = Win32API.CreateCompatibleDC(hdc);
			IntPtr memBM = Win32API.CreateCompatibleBitmap(hdc, width, height);
			IntPtr hOld = Win32API.SelectObject(memDC, memBM);
			IntPtr hBmp = IntPtr.Zero;

			Bitmap img = null;

			try
			{
				int bpp = Win32API.GetDeviceCaps(hdc, Win32API.DeviceCap.BITSPIXEL);
				int size = (bpp / 8) * (width * height);

				byte[] lpBits1 = new byte[size];

				bool ret = true;


				if (ret = Win32API.PrintWindow(grabHWND, memDC, 0))
				{
					Win32API.GetBitmapBits(memBM, size, lpBits1);
					hBmp = Win32API.CreateBitmap(width, height, 1, (uint)bpp, lpBits1);
					img = Image.FromHbitmap(hBmp);
					Win32API.DeleteObject(hBmp);
				}
			}
			catch
			{
			}
			finally
			{
				Win32API.SelectObject(memDC, hOld);
				Win32API.DeleteObject(memBM);
				Win32API.DeleteObject(hBmp);
				Win32API.DeleteDC(memDC);
				Win32API.ReleaseDC(IntPtr.Zero, hdc);
			}
			return img;

		}
	}
}
