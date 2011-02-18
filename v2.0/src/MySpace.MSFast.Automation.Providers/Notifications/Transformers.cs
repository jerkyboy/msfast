//=======================================================================
/* Project: MSFast (MySpace.MSFast.Automation.Client.Providers)
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
using System.Linq;
using System.Text;
using EYF.Providers.Notifications.Transformers;
using EYF.Core.Configuration;
using MySpace.MSFast.Automation.Providers.Results;
using MySpace.MSFast.Automation.Entities.Results;

namespace MySpace.MSFast.Automation.Providers.Notifications
{   
    public class TransformTime : IObjectTransformer
    {   
        public string Transform(object val, string[] args, object state)
        {
            if (val == null || args == null || args.Length != 1)
                return string.Empty;
            
            long max = 0;
            long.TryParse(args[0], out max);

            string str = val.ToString();
            
            double time = 0;
            double.TryParse(str, out time);

            if(time <= 0) 
                return "<font color=\"#CCC\">n/a</font>";

            if (time >= max)
                return String.Format("<font color=\"#b20000\">{0} secs.</font>", (((long)time) / 1000.00));

            return String.Format("{0} secs.", (((long)time) / 1000.00)); 
        }
    }

    public class TransformBackground : IObjectTransformer
    {
        public string Transform(object val, string[] args, object state)
        {
            if (val == null || (val is uint) == false)
                return "FFFFFF";

            return (((uint)val) % 2 == 0) ? "FAFAFA" : "FFFFFF";
        }
    }

    public class GetDate : IObjectTransformer
    {
        public string Transform(object val, string[] args, object state)
        {
            return DateTime.Now.ToString("dddd, dd MMMM yyyy");
        }
    }

    public class TransformCPU : IObjectTransformer
    {
        public string Transform(object val, string[] args, object state)
        {
            if (val == null || args == null || args.Length != 1)
                return string.Empty;
            
            long max = 0;
            long.TryParse(args[0], out max);

            string str = val.ToString();
            
            double time = 0;
            double.TryParse(str, out time);

            if(time <= 0) 
                return "<font color=\"#CCC\">n/a</font>";

            if(time >= max)
                return "<font color=\"#b20000\">" + Math.Round(Math.Min(100,time)) + "%</font>";

            return Math.Round(Math.Min(100, time)) + "%"; 
        }
    }

    public class TransformSize : IObjectTransformer
    {
        public string Transform(object val, string[] args, object state)
        {
            if (val == null || args == null || args.Length != 1)
                return string.Empty;
            
            long max = 0;
            long.TryParse(args[0], out max);

            string str = val.ToString();
            
            double v = 0;
            double.TryParse(str,out v);

            if(v <= 0) 
                return "<font color=\"#CCC\">n/a</font>";

            if (v >= max)
                return String.Format("<font color=\"#b20000\">{0:0,0}kb.</font>", (((long)v) / 1024));

            return String.Format("{0:0,0}kb.", (v / 1024));
        }
    }
    
    public class TransformCount : IObjectTransformer
    {
        public string Transform(object val, string[] args, object state)
        {
            if (val == null || args == null || args.Length != 1)
                return string.Empty;
            
            long max = 0;
            long.TryParse(args[0], out max);

            string str = val.ToString();
            
            double v = 0;
            double.TryParse(str, out v);

            if(v <= 0) 
                return "<font color=\"#CCC\">n/a</font>";

            if(v >= max)
                return "<font color=\"#b20000\">" + Math.Round(v) + " Files</font>";

            return Math.Round(v).ToString() + " Files"; 
        }
    }
     
    
    public class TransformGetThumbnail : IObjectTransformer
    {
        public string Transform(object val, string[] args, object state)
        {
            if (val == null)
                return string.Empty;

            ThumbnailAndTimestamp[] res = ResultsProvider.GetResultsThumbnails(val as ResultsID);

            if (res == null || res.Length == 0)
                return String.Empty;

            return res[res.Length-1].ThumbnailSrc;
        }
    }    
}
