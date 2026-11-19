using System.ComponentModel.DataAnnotations;

namespace APIDemoLab.Models;

public class Course
{
    public int IdCourse { get; set; }

    [Required, StringLength(45)]
    public string Name { get; set; } = string.Empty;

    public int Credit { get; set; }

    public bool IsDeleted { get; set; }

    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
