using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;


namespace NotTheMatrix
{
    public class Column
    {
        private int speed;
        private int column = 0;
        private int maxrows = 0;
        private int height;
        private int rows;
        private int iteration;

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

        private string GetRandomCharacter()
        {
            Random random = new Random();
            return Constants.ALLOWED_CHARS[random.Next(Constants.ALLOWED_CHARS.Length)].ToString();
        }
        private void SetRandomSpeed()
        {
            speed = (new Random().Next(100, 2000));
        }
        public void Clear()
        {
            SetRandomSpeed();
            rows = 0;
            try
            {
                IntPtr hdc = GetDC(IntPtr.Zero);
                if (hdc != IntPtr.Zero)
                {
                    using (Graphics g = Graphics.FromHdc(hdc))
                    {
                        g.FillRectangle(new SolidBrush(Constants.BACKGROUND_COLOR), new Rectangle(column * Constants.DEFAULT_SIZE, 0, Constants.DEFAULT_SIZE, height));
                    }
                    ReleaseDC(IntPtr.Zero, hdc);
                    iteration++;
                }
            }
            catch {}
        }

        async Task doTheThings()
        {
            iteration = 0;
            while (true)
            {
                // clear if column is full or random chance occurs
                if (rows >= maxrows || new Random().Next(0, 1000) < 5)
                {
                    Clear();
                }
                else
                {
                    try
                    {
                        IntPtr hdc = GetDC(IntPtr.Zero);
                        if (hdc != IntPtr.Zero)
                        {
                            using (Graphics g = Graphics.FromHdc(hdc))
                            {
                                g.DrawString(GetRandomCharacter(), Constants.FONT, new SolidBrush(Constants.FOREGROUND_COLOR), column * Constants.DEFAULT_SIZE, rows * Constants.DEFAULT_SIZE);
                            }
                            ReleaseDC(IntPtr.Zero, hdc);
                            iteration++;
                        }
                    }
                    catch {}
                    rows++;
                }
                if (iteration >= 100)
                {
                    GC.Collect();
                    iteration = 0;
                }
                
                await Task.Delay(speed);
            }
        }

        public async void Start()
        {
            // delay the start randomly, this way we get a staggered effect
            await Task.Delay(new Random().Next(100, 100000));
            await Task.Run(() => doTheThings());
        }
    }
}
