using System.Windows;
using HyS.ControlTrabajos.Data;

namespace HyS.ControlTrabajos;

public partial class LoginWindow : Window
{
    private readonly SupabaseAuthService auth = new();
    public AuthSession? Session { get; private set; }

    public LoginWindow() => InitializeComponent();

    private async void Login_Click(object sender, RoutedEventArgs e)
    {
        StatusText.Text = "Conectando...";
        try
        {
            Session = await auth.SignInAsync(EmailBox.Text.Trim(), PasswordBox.Password);
            DialogResult = true;
        }
        catch (Exception ex)
        {
            StatusText.Text = "No se pudo iniciar sesión. " + ex.Message;
        }
    }
}