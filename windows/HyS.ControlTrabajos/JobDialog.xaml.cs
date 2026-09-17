using System.Globalization;
using System.Windows;
using System.Windows.Controls;
namespace HyS.ControlTrabajos;
public partial class JobDialog : Window {
 public Job? Result { get; private set; }
 public JobDialog(){InitializeComponent();DateBox.Text=DateTime.Now.ToString("dd/MM/yyyy");}
 private void Cancel_Click(object sender,RoutedEventArgs e){DialogResult=false;}
 private void Save_Click(object sender,RoutedEventArgs e){
  if(string.IsNullOrWhiteSpace(AreaBox.Text)||string.IsNullOrWhiteSpace(WorkBox.Text)){MessageBox.Show("Área y trabajo son obligatorios.");return;}
  if(!decimal.TryParse(AmountBox.Text,NumberStyles.Number,CultureInfo.CurrentCulture,out var amount)&&!decimal.TryParse(AmountBox.Text,NumberStyles.Number,CultureInfo.InvariantCulture,out amount)){MessageBox.Show("Escribe un importe válido.");return;}
  var status=((ComboBoxItem)StatusBox.SelectedItem).Content?.ToString()??"Pendiente";
  Result=new Job{Date=DateBox.Text.Trim(),Area=AreaBox.Text.Trim(),Work=WorkBox.Text.Trim(),Amount=amount,Status=status};DialogResult=true;
 }
}