namespace ERPPlus.IntegrationTests.Data.Requests
{
    public class RegisterModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string CellPhone { get; set; }
        public string OfficePhone { get; set; }
        public string NetworkId { get; set; }
        public string Status { get; set; }
        public string Supervisor { get; set; }
        public string Password { get; set; }
        public bool ReportTo { get; set; }
        public string AccessLevel { get; set; }
        public string UnitOffice { get; set; }
        public string Department { get; set; }
    }
}

