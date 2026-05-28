using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace GamingBooster_Pro
{
    public partial class App : Application
    {
        private static Mutex? _singleInstanceMutex;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (e.Args.Any(a => string.Equals(a, "--selftest", StringComparison.OrdinalIgnoreCase)))
            {
                ShutdownMode = ShutdownMode.OnExplicitShutdown;
                int code = RedlineSelfTest.RunAll();
                Environment.Exit(code);
                return;
            }

            _singleInstanceMutex = new Mutex(true, RedlineInstallHelper.AppMutexName, out bool onlyInstance);
            if (!onlyInstance)
            {
                MessageBox.Show(
                    "Redline Gaming Optimizer läuft bereits.\nBitte das geöffnete Fenster nutzen oder für ein Update die App schließen.",
                    "Redline",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                Shutdown();
                return;
            }

            MainWindow window = new MainWindow();
            TextOptions.SetTextFormattingMode(window, TextFormattingMode.Display);
            TextOptions.SetTextRenderingMode(window, TextRenderingMode.ClearType);
            window.Show();
        }
    }
}
