using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestApp.Models.DBEntities
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column(TypeName="varchar(50)")]
        public string FirstName     { get; set; }
        public string LastName { get; set; }    
        public string Address { get; set; }
        public float GrossSalary    { get; set; }
        public string Position { get; set; }    
         
    }
}
