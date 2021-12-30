using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EAS.Data.Entities
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public GenderType Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Position { get; set; }
        public DateTime DateOfEmployment { get; set; }
        public string StaffId { get; set; }
        //Navigation
        public IEnumerable<Attendance>? Attendances { get; set; }
    }
    public enum GenderType { Male, Female, Others}
}
