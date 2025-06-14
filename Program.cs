namespace NotTheMatrix
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            if (args != null && args.Length == 1 && args[0].ToLower() == "/p")
            {
                Application.Run(new NotTheMatrix("preview"));
            }
            else
            if (args != null && args.Length == 1 && args[0].ToLower() == "/s")
            {
                Application.Run(new NotTheMatrix("fullscreen"));
            }
            else
                Application.Exit();
        }
    }
}