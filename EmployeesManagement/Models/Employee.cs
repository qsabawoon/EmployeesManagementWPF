using EmployeesManagement.Models.Base;

namespace EmployeesManagement.Models
{
    public class Employee : /*AuditFields,*/ IModelBase
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string Gender { get; set; } = "male";
        public string? Email { get; set; }
        public string Status { get; set; } = "active";
    }
}
