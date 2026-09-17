using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using HyS.ControlTrabajos.Data;

namespace HyS.ControlTrabajos;

public partial class MainWindow : Window
{
    public ObservableCollection<Job> Jobs { get; } = new();
    public AuthSession Session { get; }
    private readonly SupabaseJobRepository repository;

    public MainWindow(AuthSession session)
    {
        Session = session;
        repository = new SupabaseJobRepository(session);
        InitializeComponent();
        JobsGrid.ItemsSource = Jobs;
        Title = $"HyS Control de Trabajos - {Session.User.Email}";
        Loaded += async (_, _) => await LoadJobsAsync();
    }

    private async Task LoadJobsAsync()
    {
        try
        {
            var cloudJobs = await repository.GetAllAsync();
            Jobs.Clear();
            foreach (var job in cloudJobs) Jobs.Add(job);
            RefreshTotal();
        }
        catch (Exception ex)
        {
            MessageBox.Show("No se pudieron cargar los trabajos de la nube.\n\n" + ex.Message, "HyS", MessageBoxButton.OK, MessageBoxImage.Warning);
            RefreshTotal();
        }
    }

    private async void NewJob_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new JobDialog { Owner = this };
        if (dialog.ShowDialog() != true || dialog.Result is null) return;
        try
        {
            var saved = await repository.AddAsync(dialog.Result);
            Jobs.Insert(0, saved);
            RefreshTotal();
        }
        catch (Exception ex)
        {
            MessageBox.Show("No se pudo guardar el trabajo en Supabase. No se agregó a la lista para evitar diferencias entre equipos.\n\n" + ex.Message, "HyS", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void RefreshTotal()
    {
        TotalText.Text = Jobs.Where(j => j.Status == "Terminado").Sum(j => j.Amount)
            .ToString("C2", new System.Globalization.CultureInfo("es-MX"));
    }
}

public class Job
{
    public Guid Id { get; set; }
    public string Date { get; set; } = "";
    public string Area { get; set; } = "";
    public string Work { get; set; } = "";
    public decimal Amount { get; set; }
    public string Status { get; set; } = "Pendiente";
}