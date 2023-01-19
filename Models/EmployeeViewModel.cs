using System.ComponentModel;

namespace TestApp.Models
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        [DisplayName ("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [DisplayName("Address")]
        public string Address { get; set; }
      
        [DisplayName("Gross Salary")]
        public float GrossSalary { get; set; }
      
        [DisplayName("Position")]
        public string Position { get; set; }
    }
}
