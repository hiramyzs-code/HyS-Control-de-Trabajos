using System.Windows;
using HyS.ControlTrabajos.Data;

namespace HyS.ControlTrabajos;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        if (!SupabaseConfig.IsConfigured)
        {
            MessageBox.Show("Falta la configuración de conexión de HyS. Configure HYS_SUPABASE_PUBLISHABLE_KEY antes de iniciar.", "HyS Control de Trabajos", MessageBoxButton.OK, MessageBoxImage.Information);
            Shutdown();
            return;
        }

        var login = new LoginWindow();
        if (login.ShowDialog() != true || login.Session is null)
        {
            Shutdown();
            return;
        }

        var main = new MainWindow(login.Session);
        MainWindow = main;
        main.Show();
    }
}