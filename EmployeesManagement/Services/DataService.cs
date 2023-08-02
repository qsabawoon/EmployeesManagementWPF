using EmployeesManagement.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows;

namespace EmployeesManagement.Services
{
    public class DataService
    {
        private readonly IConfiguration? _config;
        private readonly string _apiUrl;
        private readonly string _apiToken;

        /// <summary>
        /// Constructor that takes an IConfiguration object as a parameter.
        /// It initializes the private fields _config, _apiUrl, and _apiToken based on the provided configuration.
        /// </summary>
        /// <param name="config"></param>
        public DataService(IConfiguration config)
        {
            _config = config;
            _apiUrl = config["ApiConfig:ApiUrl"] ?? "";
            _apiToken = config["ApiConfig:ApiToken"] ?? "";
        }

        /// <summary>
        /// Default constructor with no parameters.
        /// It sets default values for _apiUrl and _apiToken for cases where a configuration is not provided.
        /// </summary>
        public DataService()
        {
            _apiUrl = "https://gorest.co.in/public/v2";
            _apiToken = "MockToken";
        }

        /// <summary>
        /// A method that returns an instance of HttpClient configured with the API token for making HTTP requests.
        /// It sets the "Authorization" header with the provided API token.
        /// </summary>
        /// <returns></returns>
        protected virtual HttpClient GetHttpClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _apiToken);
            return client;
        }

        /// <summary>
        /// Asynchronous method to retrieve a list of employees from the API.
        /// It makes an HTTP GET request to the API and deserializes the JSON response into a list of Employee objects.
        /// </summary>
        /// <param name="page">The optional parameter page specifies the page number of the results.</param>
        /// <returns></returns>
        public async Task<List<Employee>> GetEmployeesAsync(int page = 1)
        {
            using var client = GetHttpClient();
            var response = await client.GetAsync($"{_apiUrl}/users?page={page}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var employees = JsonConvert.DeserializeObject<List<Employee>>(json);

            return employees;
        }

        /// <summary>
        /// Asynchronous method to retrieve an individual employee by their ID from the API.
        /// It makes an HTTP GET request to the API with the specified ID and deserializes the JSON response into an Employee object.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            using var client = GetHttpClient();
            var response = await client.GetAsync($"{_apiUrl}/users/{id}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var emp = JsonConvert.DeserializeObject<Employee>(json);
            return emp;
        }

        /// <summary>
        /// Asynchronous method to create a new employee on the API.
        /// It serializes the provided Employee object to JSON and sends an HTTP POST request with the serialized data.
        /// It then deserializes the JSON response into an Employee object representing the newly created employee.
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public async Task<Employee> CreateEmployeeAsync(Employee employee)
        {
            try
            {
                using var client = GetHttpClient();
                var serializedEmployee = JsonConvert.SerializeObject(employee);

                var content = new StringContent(serializedEmployee, System.Text.Encoding.Default, "application/json");
                var response = await client.PostAsync($"{_apiUrl}/users", content);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var emp = JsonConvert.DeserializeObject<Employee>(json);
                return emp;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong on the server.\n\n" + ex.Message,
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        /// <summary>
        /// Asynchronous method to update an existing employee on the API.
        /// It serializes the provided Employee object to JSON and sends an HTTP PUT request with the serialized data to the specified employee ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="employee"></param>
        /// <returns>It returns a boolean indicating whether the update was successful or not.</returns>
        public async Task<bool> UpdateEmployeeAsync(int id, Employee employee)
        {
            try
            {
                using var client = GetHttpClient();
                var serializedEmployee = JsonConvert.SerializeObject(employee);

                var content = new StringContent(serializedEmployee, System.Text.Encoding.Default, "application/json");
                var response = await client.PutAsync($"{_apiUrl}/users/{id}", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong on the server.\n\n" + ex.Message,
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
}

        /// <summary>
        /// Asynchronous method to delete an employee from the API by their ID.
        /// It sends an HTTP DELETE request to the API with the specified employee ID and returns a boolean indicating whether the deletion was successful or not.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            try
            {
                using var client = GetHttpClient();
                var response = await client.DeleteAsync($"{_apiUrl}/users/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong on the server.\n\n" + ex.Message,
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
