namespace EmployeesManagement.Misc
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public Meta Meta { get; set; }
    }

    public class Meta
    {
        public int Pagination { get; set; }
    }
}
