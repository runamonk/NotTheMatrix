using System;
using System.Windows.Forms;

namespace NotTheMatrix
{
    public partial class NotTheMatrix : Form
    {
        private int maxcols;
        private int width;
        private int height;
        private List<Column> Columns;

        public NotTheMatrix(string runType)
        {
            Cursor.Hide();
            Columns = new List<Column>();
            FormBorderStyle = FormBorderStyle.None;
               
            TopMost = true;
            ShowInTaskbar = false;
            ControlBox = false;

            if (runType == "fullscreen")
            {
                foreach (var s in Screen.AllScreens)
                    width += s.WorkingArea.Width;

                foreach (var s in Screen.AllScreens)
                    height += s.WorkingArea.Height;

                WindowState = FormWindowState.Maximized;
            }
            else
            {
                StartPosition = FormStartPosition.Manual;
                Location = new Point(0, 0); 
                width = 500; 
                height = 500; 
                Size = new Size(width, height);            
            }

            BackColor = Constants.BACKGROUND_COLOR;
            ForeColor = Constants.FOREGROUND_COLOR;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            maxcols = width / Constants.DEFAULT_SIZE;

            Task.Run(async () => await StartColumns());
        }

        private async Task StartColumns()
        {
            for (int i = 0; i < maxcols; i++)
            {
                Columns.Add(new Column(i, height));
                await Task.Run(() => Columns[i].Start());
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            Stop();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            Stop();
        }

        private void Stop()
        {
            Cursor.Show();
            Columns.Clear();          
            Close();
        }
    }
}
