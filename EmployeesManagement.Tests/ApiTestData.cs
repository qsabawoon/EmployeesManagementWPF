namespace EmployeesManagement.Tests
{
    public static class ApiTestData
    {
        public const string EmployeeList = @"
        [
            {
                ""id"": 1,
                ""name"": ""Sabawoon"",
                ""gender"": ""Male"",
                ""email"": ""qsabawoon@gmail.com"",
                ""status"": ""active"",
                ""createdBy"": ""loggedin.user""
            },
            {
                ""id"": 2,
                ""name"": ""Minhaj"",
                ""gender"": ""Male"",
                ""email"": ""minhajm@gmail.com"",
                ""status"": ""active"",
                ""createdBy"": ""loggedin.user""
            },
            {
                ""id"": 3,
                ""name"": ""Jola"",
                ""gender"": ""Female"",
                ""email"": ""jola.johnson@example.com"",
                ""status"": ""active"",
                ""createdBy"": ""loggedin.user""
            }
        ]";

        public const string SingleEmployee = @"
        {
            ""id"": 1,
            ""name"": ""Sabawoon"",
            ""gender"": ""Male"",
            ""email"": ""qsabawoon@gmail.com"",
            ""status"": ""active"",
            ""createdBy"": ""loggedin.user""
        }";

        public const string CreatedEmployee = @"
        {
            ""id"": 4,
            ""name"": ""Haroon"",
            ""gender"": ""Male"",
            ""email"": ""haroon@hotmail.com"",
            ""status"": ""active"",
            ""createdBy"": ""loggedin.user""
        }";

        public const string UpdatedEmployee = @"
        {
            ""id"": 4,
            ""name"": ""Najm"",
            ""gender"": ""Male"",
            ""email"": ""najm.haroon@hotmail.com"",
            ""status"": ""active"",
            ""modifiedBy"": ""loggedin.user""
        }";

        public const string DeletionSuccess = @"
        {
            ""code"": 204,
            ""meta"": null
        }";
    }
}
