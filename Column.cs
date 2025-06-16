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
            // delay the start randomly, this way we get a staggered effect           
            Task.Delay(new Random().Next(100, 100000)).Wait();

            iteration = 0;
            while (true)
            {
                Task.Delay(speed).Wait();

                // clear if column is full or random chance occurs
                if (rows >= maxrows || new Random().Next(0, 1000) < 5)
                    await Task.Run(() => Clear());

                await Task.Run(() => Paint(GetRandomCharacter(), rows, Constants.FOREGROUND_COLOR));

                if (iteration >= 100)
                {
                    GC.Collect();
                    iteration = 0;
                }
                if (new Random().Next(0, 1000) < 10)
                    SetRandomSpeed();
            }
        }

        private void Clear()
        {
            if (rows == 0)
                return;

            Paint(lastCharacter, rows - 1, Constants.CLEAR_COLOR);
            Task.Delay(25).Wait();
            Paint(Constants.CLEAR_COLUMN, rows, Constants.BACKGROUND_COLOR);

            iteration++;
            rows = 0;
        }

        private string GetRandomCharacter()
        {
            Random random = new Random();
            return Constants.ALLOWED_CHARS[random.Next(Constants.ALLOWED_CHARS.Length)].ToString();
        }

        private void Paint(string character, int row, Color color)
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
            speed = (new Random().Next(25, 2000));
        }

        public async void Start()
        {
            await Task.Run(() => doTheThings());
        }
    }
}
