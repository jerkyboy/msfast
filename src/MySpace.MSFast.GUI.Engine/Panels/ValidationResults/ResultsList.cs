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
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;
using MySpace.MSFast.DataValidators;
using BrightIdeasSoftware;

namespace MySpace.MSFast.GUI.Engine.Panels.ValidationResults
{
	public class ResultsList : ObjectListView
	{
		private OLVColumn ls_content_column_name;
		//private OLVColumn ls_content_column_occurrences;
		private OLVColumn ls_content_column_results;

        private List<IValidationResults> results = null;

        public delegate void OnResultsSelected(IValidationResults results);
		public event OnResultsSelected ResultsSelected;

		public void Initialize()
		{
 

            this.ls_content_column_results = new OLVColumn();
			this.ls_content_column_results.AspectName = "Score";
			this.ls_content_column_results.Text = "Score";
			this.ls_content_column_results.UseInitialLetterForGroup = true;
			this.ls_content_column_results.Width = 40;
            this.ls_content_column_results.TextAlign = HorizontalAlignment.Center;
            this.ls_content_column_results.RendererDelegate = new RenderDelegate(this.ResultsRenderDelegate);

            this.ls_content_column_name = new OLVColumn();
			this.ls_content_column_name.Text = "Name";
			this.ls_content_column_name.UseInitialLetterForGroup = true;
			this.ls_content_column_name.Width = 300;
            this.ls_content_column_name.RendererDelegate = new RenderDelegate(this.NameRenderDelegate);

            this.OwnerDraw = true;
			this.AllColumns.Add(this.ls_content_column_name);
			this.AllColumns.Add(this.ls_content_column_results);

			this.Columns.AddRange(new ColumnHeader[] {ls_content_column_results,ls_content_column_name });

            this.Location = new System.Drawing.Point(0, 0);
            this.TabIndex = 0;
            this.EmptyListMsg = "Click Start";
			this.FullRowSelect = true;
            this.MultiSelect = false;
            this.RowHeight = 25;
            this.ShowGroups = false;
            ImageList il = new ImageList();
            il.ImageSize = new Size(1, this.RowHeight);
            this.SmallImageList = il;

			this.SelectedIndexChanged += new EventHandler(BufferValidatorSummeryList_SelectionChanged);
			
            results = new List<IValidationResults>();

			this.SetObjects(results);
		}

        Brush[] textColors = new Brush[]{
            new SolidBrush(Color.FromArgb(61,177,10)),
            new SolidBrush(Color.FromArgb(178,207,38)),
            new SolidBrush(Color.FromArgb(222,199,40)),
            new SolidBrush(Color.FromArgb(225,144,50)),
            new SolidBrush(Color.FromArgb(225,50,50))};


        Brush bruBackground = new SolidBrush(Color.White);
        Brush bruSelectBackground = new SolidBrush(Color.FromArgb(234,245,255));
        Font listFont = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        int listFontP = -1;
        Pen gridPen = new Pen(new SolidBrush(Color.FromArgb(245, 245, 245)),1);

        private void ResultsRenderDelegate(DrawListViewSubItemEventArgs e, Graphics g, Rectangle r, Object rowObject)
        {
            if (e == null || e.Item == null || (e.Item is OLVListItem) == false || (((OLVListItem)e.Item).RowObject is IValidationResults) == false)
            {
                TextRenderDelegate(e, g, r, rowObject, null,0);
            }
            else
            {
                TextRenderDelegate(e, g, r, rowObject,
                    ((IValidationResults)((OLVListItem)e.Item).RowObject).Score.ToString(),
                    ((IValidationResults)((OLVListItem)e.Item).RowObject).Score);
            }
        }

        private void NameRenderDelegate(DrawListViewSubItemEventArgs e, Graphics g, Rectangle r, Object rowObject)
        {
            if (e == null || e.Item == null || (e.Item is OLVListItem) == false || (((OLVListItem)e.Item).RowObject is IValidationResults) == false)
            {
                TextRenderDelegate(e, g, r, rowObject, null,0);
            }
            else
            {
                TextRenderDelegate(e, g, r, rowObject,
                    ((IValidationResults)((OLVListItem)e.Item).RowObject).Validator.Name,
                    ((IValidationResults)((OLVListItem)e.Item).RowObject).Score);
            }
        }

        private void TextRenderDelegate(DrawListViewSubItemEventArgs e, Graphics g, Rectangle r, Object rowObject,String text,int score)
        {
            bool sel = (e.ItemState & ListViewItemStates.Selected) == ListViewItemStates.Selected;
            g.FillRectangle((sel) ? bruSelectBackground : bruBackground, r);
            g.DrawLine(gridPen, r.Left, r.Bottom-1, r.Right, r.Bottom-1);

            if (String.IsNullOrEmpty(text))
                return;
            int bi = 0;
            if (score < 100)
                bi++;
            if (score < 90)
                bi++;
            if (score < 80)
                bi++;
            if (score < 70)
                bi++;

            if (listFontP == -1)
            {
                listFontP = (int)g.MeasureString("X", this.listFont).Height / 2;
            }

            g.DrawString(text, this.listFont, this.textColors[bi], r.Left+2, r.Top + 4, StringFormat.GenericTypographic);

        }



        void BufferValidatorSummeryList_SelectionChanged(object sender, EventArgs e)
		{
			if (ResultsSelected != null)
			{
                ResultsSelected((IValidationResults)this.GetSelectedObject());
			}
		}

        internal void Add(IValidationResults msg)
		{
			this.BeginUpdate();
			results.Add(msg);
			this.SetObjects(results);
			this.EndUpdate();
		}

		internal void ClearList()
		{
			results.Clear();
			this.SetObjects(results);
		}
	}
    public class PLabel : Label
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }
    }
}