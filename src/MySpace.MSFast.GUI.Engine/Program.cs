//=======================================================================
/* Project: MSFast (MySpace.MSFast.GUI.Engine)
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
using System.Windows.Forms;
using MySpace.MSFast.ImportExportsMgrs;
using System.IO;
using MySpace.MSFast.DataProcessors;
using MySpace.MSFast.DataProcessors.Render;
using MySpace.MSFast.DataProcessors.Download;

namespace MySpace.MSFast.GUI.Engine
{
	static class Program
	{
		[STAThread]
		static void Main()
		{
            string[] args = Environment.GetCommandLineArgs();
            String openFile = null;

            if (args != null && args.Length == 2)
                openFile = args[1];

          //  Open(@"C:\Users\yramot\Desktop\homepage tests\1st Iteration\1.msf");
            Open(@"C:\Users\yramot\Desktop\homepage tests\1st Iteration\2.msf");
            Open(@"C:\Users\yramot\Desktop\homepage tests\1st Iteration\3.msf");
            Open(@"C:\Users\yramot\Desktop\homepage tests\1st Iteration\4.msf");
            Open(@"C:\Users\yramot\Desktop\homepage tests\1st Iteration\5.msf");

            Open(@"C:\Users\yramot\Desktop\homepage tests\2nd Iteration\1.msf");
            Open(@"C:\Users\yramot\Desktop\homepage tests\2nd Iteration\2.msf");
            Open(@"C:\Users\yramot\Desktop\homepage tests\2nd Iteration\3.msf");
            Open(@"C:\Users\yramot\Desktop\homepage tests\2nd Iteration\4.msf");
            Open(@"C:\Users\yramot\Desktop\homepage tests\2nd Iteration\5.msf");


            Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(openFile));

		}

        private static void Open(string p)
        {
            Stream s = File.Open(p, FileMode.Open);
            
            MSFImportExportsManager msfHandler = new MSFImportExportsManager();
            ProcessedDataPackage pdd = msfHandler.LoadProcessedDataPackage(s);
            s.Close();
            DownloadData dd = pdd[typeof(DownloadData)] as DownloadData;
            RenderData rd = pdd[typeof(RenderData)] as RenderData;
            
            Console.WriteLine(p);
            Console.WriteLine("Total Download : " + dd.Count);
            
            int css = 0;
            int js = 0;
            int images = 0;

            int csssize = 0;
            int jssize = 0;
            int imagessize = 0;
            int all = 0;

            foreach (DownloadState ds in dd)
            {
                all += ds.TotalReceived;
                if (ds.URLType == URLType.CSS)
                {
                    css++;
                    csssize += ds.TotalReceived;
                }
                else if (ds.URLType == URLType.Image)
                {
                    images++;
                    imagessize += ds.TotalReceived;
                }
                else if (ds.URLType == URLType.JS)
                {
                    js++;
                    jssize += ds.TotalReceived;
                }                
            }

            Console.WriteLine("All      " + dd.Count + "     " + all);
            Console.WriteLine("CSS      " + css + "     " + csssize);
            Console.WriteLine("JS       " + js + "     " + jssize);
            Console.WriteLine("Images   " + images + "     " + imagessize);

            Console.WriteLine("Max " + rd.MaxRenderTime);
            Console.WriteLine("Avg " + rd.AvgRenderTime);

        }

	}

}
