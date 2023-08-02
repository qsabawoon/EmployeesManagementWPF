using EmployeesManagement.Models;
using EmployeesManagement.Services;
using Microsoft.Extensions.Configuration;
using System.Windows;
using System.Windows.Controls;

namespace EmployeesManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainViewModel? mainViewModel;

        public MainWindow()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var dataService = new DataService(configuration);
            mainViewModel = new MainViewModel(dataService);

            DataContext = mainViewModel;
            InitializeComponent();
        }

        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtName.Text) || string.IsNullOrEmpty(TxtEmail.Text))
            {
                MessageBox.Show("Please enter both Name and Email.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var newEmp = new Employee()
            {
                Name = TxtName.Text,
                Gender = CmbGender.SelectedValue.ToString() ?? "male",
                Email = TxtEmail.Text,
                Status = "active"
            };

            var result = await mainViewModel.AddDataAsync(newEmp);
            if (result)
            {
                MessageBox.Show("Employee added successfully.", "Added",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private async void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Button btnDel = (Button)sender;
            Employee emp = (Employee)btnDel.CommandParameter;

            var result = await mainViewModel.UpdateDataAsync(emp.Id, emp);
            if (result)
            {
                MessageBox.Show("Employee updated successfully.", "Saved",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private async void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            Button btnDel = (Button)sender;
            Employee emp = (Employee)btnDel.CommandParameter;

            var result = await mainViewModel.DeleteDataAsync(emp.Id);
            if (result)
            {
                MessageBox.Show("Employee deleted successfully.", "Deleted",
                MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
