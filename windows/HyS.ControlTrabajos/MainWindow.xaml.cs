using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
namespace HyS.ControlTrabajos;
public partial class MainWindow : Window {
 public ObservableCollection<Job> Jobs { get; } = new();
 public MainWindow(){ InitializeComponent(); JobsGrid.ItemsSource=Jobs; RefreshTotal(); }
 private void NewJob_Click(object sender,RoutedEventArgs e){ MessageBox.Show("Formulario de Nuevo Trabajo en construcción. La versión final usará la misma base de datos en la nube que Android.","HyS Control de Trabajos"); }
 private void RefreshTotal(){ TotalText.Text=Jobs.Where(j=>j.Status=="Terminado").Sum(j=>j.Amount).ToString("C2",new System.Globalization.CultureInfo("es-MX")); }
}
public class Job { public string Date {get;set;}=""; public string Area {get;set;}=""; public string Work {get;set;}=""; public decimal Amount {get;set;} public string Status {get;set;}="Pendiente"; }
