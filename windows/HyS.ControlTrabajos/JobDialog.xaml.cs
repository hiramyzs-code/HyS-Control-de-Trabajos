using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace HyS.ControlTrabajos;

public partial class JobDialog : Window
{
    public Job? Result { get; private set; }
    private readonly Guid existingId;

    public JobDialog(Job? existing = null)
    {
        InitializeComponent();
        if (existing is null)
        {
            DateBox.Text = DateTime.Now.ToString("dd/MM/yyyy");
            return;
        }

        existingId = existing.Id;
        Title = "Editar trabajo";
        DateBox.Text = existing.Date;
        AreaBox.Text = existing.Area;
        WorkBox.Text = existing.Work;
        AmountBox.Text = existing.Amount.ToString(CultureInfo.CurrentCulture);
        foreach (ComboBoxItem item in StatusBox.Items)
        {
            if (string.Equals(item.Content?.ToString(), existing.Status, StringComparison.OrdinalIgnoreCase))
            {
                StatusBox.SelectedItem = item;
                break;
            }
        }
    }

    private void Cancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(AreaBox.Text) || string.IsNullOrWhiteSpace(WorkBox.Text))
        {
            MessageBox.Show("Área y trabajo son obligatorios.");
            return;
        }

        if (!DateTime.TryParseExact(DateBox.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
        {
            MessageBox.Show("La fecha debe tener el formato dd/MM/yyyy.");
            return;
        }

        if (!decimal.TryParse(AmountBox.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out var amount) &&
            !decimal.TryParse(AmountBox.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out amount))
        {
            MessageBox.Show("Escribe un importe válido.");
            return;
        }

        var status = ((ComboBoxItem)StatusBox.SelectedItem).Content?.ToString() ?? "Pendiente";
        Result = new Job
        {
            Id = existingId,
            Date = DateBox.Text.Trim(),
            Area = AreaBox.Text.Trim(),
            Work = WorkBox.Text.Trim(),
            Amount = amount,
            Status = status
        };
        DialogResult = true;
    }
}