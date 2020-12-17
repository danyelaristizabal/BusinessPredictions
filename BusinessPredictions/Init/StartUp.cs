using System;

namespace BusinessPredictions
{
    class StartUp
    {
        [STAThread]
        static void Main()
        {
            App application = new App();
            application.InitializeComponent();
            application.Run();
        }
    }
}
