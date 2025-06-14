using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotTheMatrix
{
    public static class Constants
    {
        public const string ALLOWED_CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        public const int DEFAULT_SIZE = 23;
        public static readonly Color BACKGROUND_COLOR = Color.Black;
        public static readonly Color FOREGROUND_COLOR = Color.Lime;
        public static readonly Font FONT = new Font("Cascadia Mono", DEFAULT_SIZE, FontStyle.Bold, GraphicsUnit.Pixel);
    }
}
