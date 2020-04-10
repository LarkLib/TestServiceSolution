using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Dynamic;

namespace TestWindowsFormsApplication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Test2();
        }

        private void Test2()
        {
            dynamic d = new ExpandoObject();
            //d["aaa"]="abc";
            Console.WriteLine(d.aaa);
        }

        private void Test1()
        {
            var image = new Bitmap(180, 121);
            var g = Graphics.FromImage(image);
            for (int i = 0; i < 120; i += 12)
            {
                g.DrawLine(new Pen(Color.Red), 0, i, 180, i);
            }
            g.DrawString(DateTime.Now.ToString("yyyyMMdd HH:mm:ss.fff"), DefaultFont, new SolidBrush(Color.Blue), new PointF(20, 20));
            pictureBox1.Image = image;
        }

        private void tableLayoutPanel1_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            //var g = this.tableLayoutPanel1.CreateGraphics();
            //g.DrawRectangle(new Pen(Color.Red), 50, 50, 50, 50);
            var offset = 3;
            // 单元格重绘 
            Pen pen = new Pen(Color.Red);
            var bursh = new SolidBrush(Color.Blue);
            //e.Graphics.DrawRectangle(pp, e.CellBounds.X + offset, e.CellBounds.Y + offset, e.CellBounds.X + e.CellBounds.Width - offset, e.CellBounds.Y + e.CellBounds.Height - offset);
            e.Graphics.DrawRectangle(pen, e.CellBounds.X + offset, e.CellBounds.Y + offset, e.CellBounds.Width - 2 * offset, e.CellBounds.Height - 2 * offset);
            e.Graphics.FillRectangle(bursh, e.CellBounds.X + 2 * offset, e.CellBounds.Y + 2 * offset, e.CellBounds.Width - 4 * offset, e.CellBounds.Height - 4 * offset);

            System.Text.StringBuilder messageBoxCS = new System.Text.StringBuilder();
            messageBoxCS.AppendFormat("{0} = {1}", "CellBounds", e.CellBounds);
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", "Row", e.Row);
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", "Column", e.Column);
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", "ClipRectangle", e.ClipRectangle);
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", "Graphics", e.Graphics);
            messageBoxCS.AppendLine();
            //MessageBox.Show(messageBoxCS.ToString(), "CellPaint Event");
            label1.Text = messageBoxCS.ToString();
            label1.Text = DateTime.Now.ToString();
        }
        Point? GetRowColIndex(TableLayoutPanel tlp, Point point)
        {
            if (point.X > tlp.Width || point.Y > tlp.Height)
                return null;

            int w = tlp.Width;
            int h = tlp.Height;
            int[] widths = tlp.GetColumnWidths();

            int i;
            for (i = widths.Length - 1; i >= 0 && point.X < w; i--)
                w -= widths[i];
            int col = i + 1;

            int[] heights = tlp.GetRowHeights();
            for (i = heights.Length - 1; i >= 0 && point.Y < h; i--)
                h -= heights[i];

            int row = i + 1;

            return new Point(col, row);
        }
        private void tableLayoutPanel1_Click(object sender, EventArgs e)
        {
            var cellPos = GetRowColIndex(
                tableLayoutPanel1,
                tableLayoutPanel1.PointToClient(Cursor.Position));
            label1.Text = string.Format("x={0},y={1}", cellPos.Value.X, cellPos.Value.Y);
        }
    }
}
