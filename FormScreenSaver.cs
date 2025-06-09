using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NotTheMatrix
{
    public partial class FormScreenSaver : Form
    {
        private const string ALLOWED_CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        
        private Color BackgroundColor = Color.Black;
        private Color ForegroundColor = Color.Lime;
        
        private bool cursorIsHidden = false;
        private int defaultSpeed = 10;
        private int defaultSize = 23;
        private int maxColumns;
        private int maxRows;
        private int noFullColumns = 0;
        private int maxFullColumns = 0;
        private bool doRunScreensaver = true;
        private int[] Columns = Array.Empty<int>();

        public FormScreenSaver(string runType)
        {
            if (runType == "preview")
            {
                Height = 500;
                Width = 500;
                StartPosition = FormStartPosition.Manual;
                Location = new Point(0, Height / 2);
            }
            else
            {
                WindowState = FormWindowState.Maximized;
                FormBorderStyle = FormBorderStyle.None;
                TopMost = true;
                ShowInTaskbar = false;
                ControlBox = false;
            }

            Font = new Font("Cascadia Mono", defaultSize, FontStyle.Bold, GraphicsUnit.Pixel);
            BackColor = BackgroundColor;
            ForeColor = ForegroundColor;  
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Cursor.Hide();
            maxColumns = Width / defaultSize;
            maxRows = Height / defaultSize;
            Columns = new int[maxColumns];
            maxFullColumns = Columns.Length / 4;
        }

        string GetRandomCharacter()
        {
            Random random = new Random();
            return ALLOWED_CHARS[random.Next(ALLOWED_CHARS.Length)].ToString();
        }

        void ClearColumn(int col, Graphics g)
        {
            Columns[col] = 0;
            g.FillRectangle(new SolidBrush(BackgroundColor), new Rectangle(col * defaultSize, 0, defaultSize, Height));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            
            Graphics g = e.Graphics;
            g.Clear(BackgroundColor);

            while (doRunScreensaver)
            {
                int nextCol = (new Random()).Next(0, maxColumns);
                
                while (true)
                {
                    if (Columns[nextCol] < maxRows)
                    {
                        break;
                    }

                    if (Columns[nextCol] >= maxRows)
                    {
                        if (noFullColumns == maxFullColumns)
                        {
                            ClearColumn(nextCol, g);
                            noFullColumns--;
                            break;
                        }
                        else
                        {
                            noFullColumns++;
                        }
                    }
                }

                if (new Random().Next(0, 256) < 5)
                {
                    int clearCol = new Random().Next(0, maxColumns);
                    if (clearCol != nextCol)
                        ClearColumn(clearCol, g);
                }

                string rowStr = (Columns[nextCol] * defaultSize + defaultSize).ToString();
                string newChar = GetRandomCharacter();
                g.DrawString(newChar, Font, new SolidBrush(ForegroundColor), nextCol * defaultSize, Columns[nextCol] * defaultSize);

                Columns[nextCol]++;
                Application.DoEvents();
                Thread.Sleep(defaultSpeed);
            }
            Cursor.Show();
            Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            doRunScreensaver = false;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            doRunScreensaver = false;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            doRunScreensaver = false;
        }
    }
}
