using EmployeesManagement.Models;
using EmployeesManagement.Services;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using System.Net;

namespace EmployeesManagement.Tests;

[TestFixture]
public class DataServiceTests
{
    [Test]
    public async Task GetEmployeesAsync_ShouldReturnListOfEmployees()
    {
        // Arrange
        HttpClientHandlerMock handlerMock = new(ApiTestData.EmployeeList);
        var dataServiceMock = new Mock<DataService>() { CallBase = true };

        dataServiceMock
            .Protected()
            .Setup<HttpClient>("GetHttpClient")
            .Returns(() => new HttpClientMock(handlerMock));

        // Act
        var employees = await dataServiceMock.Object.GetEmployeesAsync();

        // Assert
        Assert.That(employees, Has.Count.EqualTo(3));
        Assert.That(employees[0].Name, Is.EqualTo("Sabawoon"));
    }

    [Test]
    public async Task GetEmployeeByIdAsync_ShouldReturnEmployee()
    {
        // Arrange
        HttpClientHandlerMock handlerMock = new(ApiTestData.SingleEmployee);
        var dataServiceMock = new Mock<DataService>() { CallBase = true };

        dataServiceMock
            .Protected()
            .Setup<HttpClient>("GetHttpClient")
            .Returns(() => new HttpClientMock(handlerMock));

        // Act
        var employee = await dataServiceMock.Object.GetEmployeeByIdAsync(1);

        // Assert
        Assert.That(employee, Is.Not.Null);
        Assert.That(employee.Name, Is.EqualTo("Sabawoon"));
        Assert.That(employee.Gender, Is.EqualTo("Male"));
    }

    [Test]
    public async Task CreateEmployeeAsync_ShouldReturnCreatedEmployee()
    {
        // Arrange
        HttpClientHandlerMock handlerMock = new(ApiTestData.CreatedEmployee);
        var dataServiceMock = new Mock<DataService>() { CallBase = true };

        dataServiceMock
            .Protected()
            .Setup<HttpClient>("GetHttpClient")
            .Returns(() => new HttpClientMock(handlerMock));

        // Act
        var newEmployee = new Employee {
            Name = "Haroon",
            Gender = "Male",
            Email = "haroon@hotmail.com",
            Status = "active"
            //CreatedBy = "loggedin.user"
        };
        var createdEmployee = await dataServiceMock.Object.CreateEmployeeAsync(newEmployee);

        // Assert
        Assert.That(createdEmployee, Is.Not.Null);
        Assert.That(createdEmployee.Name, Is.EqualTo("Haroon"));
    }

    [Test]
    public async Task UpdateEmployeeAsync_ShouldReturnTrueIfUpdateSucceeded()
    {
        // Arrange
        HttpClientHandlerMock handlerMock = new(ApiTestData.UpdatedEmployee);
        var dataServiceMock = new Mock<DataService>() { CallBase = true };

        dataServiceMock
            .Protected()
            .Setup<HttpClient>("GetHttpClient")
            .Returns(() => new HttpClientMock(handlerMock));

        // Act
        var existingEmployee = new Employee {
            Id = 4, Name = "Haroon", Gender = "Male",
            Email = "najm.haroon@hotmail.com",
            Status = "active"
            //CreatedBy = "loggedin.user",
            //ModifiedBy = "loggedin.user"
        };
        var isUpdated = await dataServiceMock.Object.UpdateEmployeeAsync(existingEmployee.Id, existingEmployee);

        // Assert
        Assert.That(isUpdated, Is.True);
    }

    [Test]
    public async Task DeleteEmployeeAsync_ShouldReturnTrueIfDeleteSucceeded()
    {
        // Arrange
        HttpClientHandlerMock handlerMock = new(ApiTestData.DeletionSuccess);
        var dataServiceMock = new Mock<DataService>() { CallBase = true };

        dataServiceMock
            .Protected()
            .Setup<HttpClient>("GetHttpClient")
            .Returns(() => new HttpClientMock(handlerMock));

        // Act
        var isDeleted = await dataServiceMock.Object.DeleteEmployeeAsync(1);

        // Assert
        Assert.That(isDeleted, Is.True);
    }


    #region Mock HTTP Client and Handler ----------------------------------------
    private class HttpClientMock : HttpClient
    {
        public HttpClientMock(HttpMessageHandler handler) : base(handler)
        {

        }

        public static HttpClient GetHttpClientMock(string mockApiResponse)
        {
            var httpClientHandlerMock = new HttpClientHandlerMock(mockApiResponse);
            var httpClient = new HttpClient(httpClientHandlerMock)
            {
                //DefaultRequestHeaders = { Authorization = new AuthenticationHeaderValue("Bearer", ApiToken) }
            };
            return httpClient;
        }
    }

    private class HttpClientHandlerMock : HttpMessageHandler
    {
        private readonly string mockApiResponse;

        public HttpClientHandlerMock(string mockApiResponse)
        {
            this.mockApiResponse = mockApiResponse;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(mockApiResponse)
            };

            await Task.Delay(1, cancellationToken);
            return response;
        }
    }
    #endregion
}