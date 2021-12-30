using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EAS.Data.Entities
{
    public class Attendance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey(nameof(Employee))]
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public AttendanceMode Mode { get; set; }
        //Relational Data
        public Employee Employee { get; set; }
    }
    public enum AttendanceMode { Online,Physical, Hybrid}
}
