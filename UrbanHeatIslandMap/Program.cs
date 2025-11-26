namespace UrbanHeatIsland
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            if (IsNewForm())//select version of program It’s done a bit clumsily ;)
                Application.Run(new Form2()); // new one with interactive map; where two locations (urban and rural) for comparison are selected 
            else
                Application.Run(new Form1()); // old, where CSV - file is loaded only
        }
        static bool IsNewForm ()
        {
            if (File.Exists("Configuration.txt"))
            {
                StreamReader configurationReader = new StreamReader("Configuration.txt");
                string formNumber = configurationReader.ReadLine();
                configurationReader.Close();
                if (formNumber == "1")
                    return false;
            }
            return true;
        }
    }
}