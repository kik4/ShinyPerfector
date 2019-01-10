using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ShinyPerfecter
{

    public partial class Form1 : Form
    {
        private const int MOUSEEVENTF_LEFTDOWN = 0x2;
        private const int MOUSEEVENTF_LEFTUP = 0x4;

        [DllImport("USER32.dll", CallingConvention = CallingConvention.StdCall)]
        static extern void SetCursorPos(int X, int Y);

        [DllImport("USER32.dll", CallingConvention = CallingConvention.StdCall)]
        static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        Point cursor;
        Color color;
        int state = 0;
        int delta = 19;

        public Form1()
        {
            cursor = Cursor.Position;
            color = Color.White;

            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (state == 0)
            {
                var p = Cursor.Position;
                var c = GetColor(p);
                label1.Text = $"(cursor, color) = ({p}, {c})";
            }
            else
            {
                // click
                if (state == 1)
                {
                    var c = GetColor(cursor);

                    if (!(c.ToArgb() == color.ToArgb()))
                    {
                        SetCursorPos(cursor.X + delta, cursor.Y);
                        mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                        mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                        label3.Text = "clicked!";
                        state++;
                    }
                }
                else if (state >= 2)
                {
                    state++;
                    if (state > 100)
                    {
                        label3.Text = "waiting...";
                        state = 0;
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cursor = Cursor.Position;
            cursor.X -= delta;
            color = GetColor(cursor);
            label2.Text = $"(cursor, color) = ({cursor}, {color})";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            state++;
        }

        private Color GetColor(Point pt)
        {
            using (var bmp = new Bitmap(1, 1))
            using (var g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(pt, new Point(0, 0), new Size(1, 1));
                return bmp.GetPixel(0, 0);
            }
        }
    }
}
