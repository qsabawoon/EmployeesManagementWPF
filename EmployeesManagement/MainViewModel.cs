using EmployeesManagement.Models;
using EmployeesManagement.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly DataService _dataService;

    private ObservableCollection<Employee> employees;

    public ObservableCollection<Employee> Employees
    {
        get => employees;
        set
        {
            employees = value;
            OnPropertyChanged(nameof(Employees));
        }
    }

    public MainViewModel(DataService dataService)
    {
        _dataService = dataService;
        Employees = new ObservableCollection<Employee>();
        LoadDataAsync();
    }

    private async void LoadDataAsync()
    {
        var employees = await _dataService.GetEmployeesAsync();
        Employees = new ObservableCollection<Employee>(employees);
    }

    public async Task<bool> AddDataAsync(Employee employee)
    {
        var empCreated = await _dataService.CreateEmployeeAsync(employee);
        if (empCreated != null)
        {
            Employees.Add(empCreated);
            return true;
        }
        return false;
    }

    public async Task<bool> UpdateDataAsync(int empId, Employee employee)
    {
        if (await _dataService.UpdateEmployeeAsync(empId, employee))
            return true;
        return false;
    }

    public async Task<bool> DeleteDataAsync(int empId)
    {
        var isDeleted = await _dataService.DeleteEmployeeAsync(empId);
        if (isDeleted)
        {
            var deletedEmp = Employees.Single(e => e.Id == empId);
            if (deletedEmp != null) {
                Employees.Remove(deletedEmp);
                return true;
            }
        }
        return false;
    }


    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
