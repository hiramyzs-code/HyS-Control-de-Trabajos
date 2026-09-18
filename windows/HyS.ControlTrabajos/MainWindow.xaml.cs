using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace HyS.ControlTrabajos;
public partial class MainWindow : Window
{
    private readonly ObservableCollection<JobRow> _jobs = new();
    public MainWindow(){InitializeComponent();Refresh();}
    private void Refresh(){var status=DoneRadio?.IsChecked==true?"Terminado":"Pendiente";SectionTitle.Text=status=="Pendiente"?"Trabajos pendientes":"Trabajos terminados";var q=(SearchBox?.Text??"").Trim();JobsGrid.ItemsSource=_jobs.Where(x=>x.Status==status&&(q.Length==0||x.Area.Contains(q,StringComparison.OrdinalIgnoreCase)||x.Work.Contains(q,StringComparison.OrdinalIgnoreCase))).ToList();}
    private void Filter_Changed(object sender,RoutedEventArgs e){if(IsLoaded)Refresh();}
    private void SearchBox_TextChanged(object sender,TextChangedEventArgs e){if(IsLoaded)Refresh();}
    private void NewJob_Click(object sender,RoutedEventArgs e)=>MessageBox.Show("El formulario de nuevo trabajo se conectará a la base de datos compartida en el siguiente bloque.","HyS Control de trabajos");
    private void JobsGrid_MouseDoubleClick(object sender,MouseButtonEventArgs e){if(JobsGrid.SelectedItem is JobRow j)MessageBox.Show($"{j.Area}\n\n{j.Work}","Detalle del trabajo");}
}
public sealed class JobRow{public DateTime WorkDate{get;set;}=DateTime.Today;public string Area{get;set;}="";public string Work{get;set;}="";public decimal Amount{get;set;}public string Status{get;set;}="Pendiente";}