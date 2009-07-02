//=======================================================================
/* Project: MSFast (MySpace.MSFast.Engine)
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
using System.Text.RegularExpressions;
using System.IO;

namespace MySpace.MSFast.Engine.SuProxy.Pipes.Collect
{
	public class ChunkedPage
	{
		public enum ResultsType : byte
		{
			Unknown = 0,
			StartToHead = 1,
			HeadContent = 2,
			HeadToBody = 4,
			Content = 8,
			BodyToPostBody = 16,
			PostBodyContent = 32,
			PostBodyToEnd = 64
		}

		#region Regex

		private const String tag = "</?[^>]*?>";
        private const String remark = "<\\s*(?i:\\!\\s*-\\s*-).*?(?i:-\\s*-)[^>]*>";
        private const String specialCharacter = "&[a-zA-Z0-9]{1,4};";
        private const String noscript = "<\\s*(?i:n\\s*o\\s*s\\s*c\\s*r\\s*i\\s*p\\s*t)[^>]*>.*?<\\s*/?\\s*(?i:n\\s*o\\s*s\\s*c\\s*r\\s*i\\s*p\\s*t)[^>]*>";
        private const String style = "<\\s*(?i:s\\s*t\\s*y\\s*l\\s*e)[^>]*>.*?<\\s*/?\\s*(?i:\\/\\s*s\\s*t\\s*y\\s*l\\s*e)[^>]*>";
		private const String script = "<\\s*(?i:s\\s*c\\s*r\\s*i\\s*p\\s*t)[^>]*>.*?<\\s*/?\\s*(?i:s\\s*c\\s*r\\s*i\\s*p\\s*t)[^>]*>";
		private const String iframe = "<\\s*(?i:i\\s*f\\s*r\\s*a\\s*m\\s*e)[^>]*>.*?<\\s*/?\\s*(?i:\\/\\s*i\\s*f\\s*r\\s*a\\s*m\\s*e)[^>]*>";
		private const String objct = "<\\s*(?i:o\\s*b\\s*j\\s*e\\s*c\\s*t)[^>]*>.*?<\\s*/?\\s*(?i:o\\s*b\\s*j\\s*e\\s*c\\s*t)[^>]*>";
		private const String textarea = "<\\s*(?i:t\\s*e\\s*x\\s*t\\s*a\\s*r\\s*e\\s*a)[^>]*>.*?<\\s*/?\\s*(?i:t\\s*e\\s*x\\s*t\\s*a\\s*r\\s*e\\s*a)[^>]*>";
		private const String button = "<\\s*(?i:b\\s*u\\s*t\\s*t\\s*o\\s*n)[^>]*>.*?<\\s*/?\\s*(?i:b\\s*u\\s*t\\s*t\\s*o\\s*n)[^>]*>";
		private const String conditionalRemarks = "<\\s*(?i:\\!\\s*-\\s*-\\s*\\[\\s*if)[^>]*>.*?<\\s*(?i:\\!\\s*\\[\\s*endif\\s*\\]\\s*-\\s*-)[^>]*>";

        private static readonly Regex htmlSTag = new Regex("<\\s*(?i:h\\s*t\\s*m\\s*l)[^>]*>", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex htmlETag = new Regex("<\\s*/?\\s*(?i:\\/\\s*h\\s*t\\s*m\\s*l)[^>]*>", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex headSTag = new Regex("<\\s*(?i:h\\s*e\\s*a\\s*d)[^>]*>", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex headETag = new Regex("<\\s*/?\\s*(?i:\\/\\s*h\\s*e\\s*a\\s*d)[^>]*>", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex bodySTag = new Regex("<\\s*(?i:b\\s*o\\s*d\\s*y)[^>]*>", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex bodyETag = new Regex("<\\s*/?\\s*(?i:\\/\\s*b\\s*o\\s*d\\s*y)[^>]*>", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);

		private static readonly Regex bodyChunks = new Regex("((" +
                                                                                                            iframe + ")|(" +																								
                                                                                                            conditionalRemarks + ")|(" +
																											specialCharacter + ")|(" +
																											objct + ")|(" +
																											script + ")|(" +
																											noscript + ")|(" +
                                                                                                            style + ")|(" +
																											button + ")|(" +
																											textarea + ")|(" +
                                                                                                            remark + ")|(" +
																											tag + "))", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);

		/*/ (...<html>...<head>)...</head>...<body>...</body>...</html>...
		private static readonly Regex startToHead = new Regex(".*?" + htmlSTag + ".*?" + headSTag, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);

		// ...<html>...<head>(...)</head>...<body>...</body>...</html>...
		private static readonly Regex headContent = new Regex(headSTag + "(.*?)" + headETag, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);

		// ...<html>...<head>...(</head>...<body>)...</body>...</html>...
		private static readonly Regex headToBody = new Regex(headETag + "(.*?)" + bodySTag, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);

		// ...<html>...<head>...</head>...<body>(...)</body>...</html>...
		private static readonly Regex bodyContent = new Regex(bodySTag + "(.*?)" + bodyETag, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);

		// ...<html>...<head>...</head>...<body>...(</body>)...</html>...
		private static readonly Regex bodyToPostBody = new Regex(bodyETag, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);

		// ...<html>...<head>...</head>...<body>...</body>(...)</html>...
		private static readonly Regex postBodyContent = new Regex(bodyETag + "(.*?)" + htmlETag, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);

		// ...<html>...<head>...</head>...<body>...</body>...(</html>...)
		private static readonly Regex postBodyToEnd = new Regex(htmlETag + ".*", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
        */

		#endregion

		public String Header = "";
		public bool IsParsed = false;
	
		public String StartToHead = String.Empty;
		public String HeadContent = String.Empty;
		public String HeadToBody = String.Empty;
		public BodyChunk[] Body = null;
		public String BodyToPostBody = String.Empty;
		public String PostBodyContent = String.Empty;
		public String PostBodyToEnd = String.Empty;

		public ChunkedPage(String header)
		{
			this.Header = header;
		}

		public void Parse(String page, int resolution)
        {
			IsParsed = true;

            // ...<html>...<head>(X)...</head>...<body>...</body>...</html>...
            int startHead = 0;

            // ...<html>...<head>...(X)</head>...<body>...</body>...</html>...
            int endHead = 0;

            // ...<html>...<head>...</head>...<body>(X)...</body>...</html>...
            int startBody = 0;



            // ...<html>...<head>...</head>...<body>...(X)</body>...</html>...
            int endBody = page.Length;

            // ...<html>...<head>...</head>...<body>...</body>(X)...</html>...
            int startPostBody = page.Length;

            // ...<html>...<head>...</head>...<body>...</body>...(X)</html>...
            int endPostBody = page.Length;


            Match match = null;
            match = bodySTag.Match(page);

            if (match.Success)
            {
                startHead = match.Index;    // ...<html>...<head>...</head>...(X)<body>...</body>...</html>...
                endHead = match.Index;// ...<html>...<head>...</head>...(X)<body>...</body>...</html>...
                startBody = match.Index + match.Length;// ...<html>...<head>...</head>...<body>(X)...</body>...</html>...
            }

            match = headETag.Match(page);
            if (match.Success)
            {
                startHead = match.Index;
                endHead = match.Index;
                startBody = Math.Max(startBody, match.Index + match.Length);
            }

            match = headSTag.Match(page);
            if (match.Success)
            {
                startHead = match.Index + match.Length;
                endHead = Math.Max(endHead, match.Index + match.Length);
                startBody = Math.Max(startBody, match.Index + match.Length);
            }

            match = htmlSTag.Match(page);
            if (match.Success)
            {
                startHead = Math.Max(startHead, match.Index + match.Length);
                endHead = Math.Max(endHead, match.Index + match.Length);
                startBody = Math.Max(startBody, match.Index + match.Length);
            }


            match = bodyETag.Match(page);
            if (match.Success)
            {
                endBody = match.Index;
                startPostBody = match.Index + match.Length;
                endPostBody = match.Index + match.Length;
            }

            match = htmlETag.Match(page);
            if (match.Success)
            {
                endBody = Math.Min(endBody, match.Index);
                startPostBody = Math.Min(startPostBody, match.Index);
                endPostBody = match.Index;
            }


            // Find <head>
			this.StartToHead = page.Substring(0, startHead);
			this.HeadContent = page.Substring(startHead, endHead - startHead);
			this.HeadToBody = page.Substring(endHead, startBody - endHead);
			this.BodyToPostBody = page.Substring(endBody, startPostBody - endBody);
			this.PostBodyContent = page.Substring(startPostBody, endPostBody - startPostBody);
			this.PostBodyToEnd = page.Substring(endPostBody, page.Length - endPostBody);


			LinkedList<BodyChunk> bodyChunksList = new LinkedList<BodyChunk>(); ;
			BodyChunk bc = new BodyChunk();

			String body = page.Substring(startBody, endBody - startBody);
			MatchCollection mc = bodyChunks.Matches(body);

            int interval = body.Length / resolution;
			int index = 0;
			int length = 0;
			int i = 0;

			for (; index < body.Length; )
			{
				length = Math.Min(Math.Abs(body.Length - index), interval);

				for (; i < mc.Count; i++)
				{
					if (index + length <= mc[i].Index)
						break;

					if (mc[i].Index < (index + length) && (index + length) < (mc[i].Index + mc[i].Length))
					{
						if (mc[i].Index == index || (index + length) - mc[i].Index > (mc[i].Index + mc[i].Length) - (index + length))
						{
							length = (mc[i].Index + mc[i].Length) - index;
						}
						else
						{
							length = mc[i].Index - index;
						}
						break;
					}
				}

				if (length != 0)
				{
					bc.Content = body.Substring(index, length);
				}
				else
				{
					bc.Content = String.Empty;
				}

				bc.StartIndex = index;
				bc.EndIndex = (index + length);
				bodyChunksList.AddLast(bc);
				bc = new BodyChunk();

				index += length;
			}
			BodyChunk[] arr = new BodyChunk[bodyChunksList.Count];
			bodyChunksList.CopyTo(arr, 0);
			this.Body = arr;

		}

		#region Save To Disc
		public bool SaveToDisc(String sourceFilename, String breaksFilename)
		{
			StringBuilder sb = new StringBuilder();
			MemoryStream ms = new MemoryStream();

			int rid = 0;

			int i = 0;

			i = sb.Length;
			sb.Append(AddCRIfNeeded(this.StartToHead));
			ms.Write(GetIndexLengthTypeAndId(i, sb.Length, ResultsType.StartToHead, rid++), 0, 10);

			i = sb.Length;
			sb.Append(AddCRIfNeeded(this.HeadContent));
			ms.Write(GetIndexLengthTypeAndId(i, sb.Length, ResultsType.HeadContent, rid++), 0, 10);

			i = sb.Length;
			sb.Append(AddCRIfNeeded(this.HeadToBody));
			ms.Write(GetIndexLengthTypeAndId(i, sb.Length, ResultsType.HeadToBody, rid++), 0, 10);

			foreach (BodyChunk bc in this.Body)
			{
				i = sb.Length;
				sb.Append(AddCRIfNeeded(bc.Content));
				ms.Write(GetIndexLengthTypeAndId(i, sb.Length, ResultsType.Content, rid++), 0, 10);
			}

			i = sb.Length;
			sb.Append(AddCRIfNeeded(this.BodyToPostBody));
			ms.Write(GetIndexLengthTypeAndId(i, sb.Length, ResultsType.BodyToPostBody, rid++), 0, 10);

			i = sb.Length;
			sb.Append(AddCRIfNeeded(this.PostBodyContent));
			ms.Write(GetIndexLengthTypeAndId(i, sb.Length, ResultsType.PostBodyContent, rid++), 0, 10);

			i = sb.Length;
			sb.Append(AddCRIfNeeded(this.PostBodyToEnd));
			ms.Write(GetIndexLengthTypeAndId(i, sb.Length, ResultsType.PostBodyToEnd, rid++), 0, 10);
			try
			{
				StreamWriter sw = new StreamWriter(sourceFilename, false, Encoding.UTF8);
				sw.Write(sb.ToString());
				sw.Flush();
				sw.Close();

				BinaryWriter bsw = new BinaryWriter(File.Create(breaksFilename));
				bsw.Write(ms.ToArray());
				bsw.Flush();
				bsw.Close();
			}
			catch {
				return false;
			}
			return true;
		}

		#region Fix Rows
		private static Regex rowsFormatterA = new Regex("\r([^\n]{1})");
		private static Regex rowsFormatterB = new Regex("([^\r]{1})\n");
		private static MatchEvaluator rowsFormatterDelegateA = new MatchEvaluator(RowsFormatterMatchEvaluatorA);
		private static MatchEvaluator rowsFormatterDelegateB = new MatchEvaluator(RowsFormatterMatchEvaluatorB);

		private static String AddCRIfNeeded(String s)
		{
			if (s.StartsWith("\n"))
				s = "\r" + s;

			if (s.EndsWith("\r"))
				s = s + "\n";

			while (rowsFormatterA.IsMatch(s))
			{
				s = rowsFormatterA.Replace(s, rowsFormatterDelegateA);
			}
			while (rowsFormatterB.IsMatch(s))
			{
				s = rowsFormatterB.Replace(s, rowsFormatterDelegateB);
			}

			return s;
		}

		private static String RowsFormatterMatchEvaluatorA(Match r)
		{
			return "\r\n" + r.Groups[1].Value;
		}

		private static String RowsFormatterMatchEvaluatorB(Match r)
		{
			return r.Groups[1].Value + "\r\n";
		}
		#endregion

		private byte[] tempBuffer = new byte[10];
		private byte[] GetIndexLengthTypeAndId(int index, int totalLength, ResultsType resultsType, int id)
		{
			int length = totalLength - index;
			tempBuffer[0] = (byte)resultsType;
			tempBuffer[1] = (byte)id;
			tempBuffer[2] = (byte)(index >> 24);
			tempBuffer[3] = (byte)(index >> 16);
			tempBuffer[4] = (byte)(index >> 8);
			tempBuffer[5] = (byte)(index);
			tempBuffer[6] = (byte)(length >> 24);
			tempBuffer[7] = (byte)(length >> 16);
			tempBuffer[8] = (byte)(length >> 8);
			tempBuffer[9] = (byte)(length);
			return tempBuffer;
		}
		#endregion
	}

	public class BodyChunk
	{
		public String Content;
		public int StartIndex = 0;
		public int EndIndex = 0;
	}
}
