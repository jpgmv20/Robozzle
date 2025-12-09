namespace Robozzle
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Alterado para iniciar pelo HomeForm
            Application.Run(new LoginForm());
        }
    }
}