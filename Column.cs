using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;


namespace NotTheMatrix
{
    public class Column
    {
        private int speed = 0;
        private int column = 0;
        private int maxrows = 0;
        private int height = 0;
        private int rows = 0;
        private int iteration = 0;
        private string lastCharacter = string.Empty;

        [DllImport("User32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);
        [DllImport("User32.dll")]
        public static extern void ReleaseDC(IntPtr hwnd, IntPtr dc);

        public Column(int column, int height) 
        {
            this.column = column;
            this.height = height;
            maxrows = height / Constants.DEFAULT_SIZE;      
            SetRandomSpeed();
        }

        async Task doTheThings()
        {
            iteration = 0;
            while (true)
            {
                // clear if column is full or random chance occurs
                if (rows >= maxrows || new Random().Next(0, 1000) < 5)
                    Clear();

                PaintCharacter(GetRandomCharacter(), rows, Constants.FOREGROUND_COLOR);

                if (iteration >= 100)
                {
                    GC.Collect();
                    iteration = 0;
                }

                await Task.Delay(speed);
            }
        }

        private void Clear()
        {
            if (rows == 0)
                return;

            PaintCharacter(lastCharacter, rows - 1, Constants.CLEAR_COLOR);
            Thread.Sleep(100);
            PaintCharacter(Constants.CLEAR_COLUMN, rows, Constants.BACKGROUND_COLOR);

            iteration++;
            rows = 0;
            SetRandomSpeed();
        }

        private string GetRandomCharacter()
        {
            Random random = new Random();
            return Constants.ALLOWED_CHARS[random.Next(Constants.ALLOWED_CHARS.Length)].ToString();
        }

        private void PaintCharacter(string character, int row, Color color)
        {
            try
            {
                IntPtr hdc = GetDC(IntPtr.Zero);
                if (hdc != IntPtr.Zero)
                {
                    using (Graphics g = Graphics.FromHdc(hdc))
                    {
                        // clear the column.
                        if (character == Constants.CLEAR_COLUMN)
                        {
                            g.FillRectangle(new SolidBrush(Constants.BACKGROUND_COLOR), new Rectangle(column * Constants.DEFAULT_SIZE, 0, Constants.DEFAULT_SIZE, height));
                        }
                        else
                        {
                            g.DrawString(character, Constants.FONT, new SolidBrush(color), column * Constants.DEFAULT_SIZE, row * Constants.DEFAULT_SIZE);
                        }
                    }
                    ReleaseDC(IntPtr.Zero, hdc);
                    lastCharacter = character;
                    rows++;
                    iteration++;
                }
            }
            catch { }
        }

        private void SetRandomSpeed()
        {
            speed = (new Random().Next(100, 2000));
        }

        public async void Start()
        {
            // delay the start randomly, this way we get a staggered effect           
            await Task.Delay(new Random().Next(100, 100000));
            await Task.Run(() => doTheThings());
        }

    }
}
