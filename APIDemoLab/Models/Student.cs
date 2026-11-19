using System.ComponentModel.DataAnnotations;

namespace APIDemoLab.Models;

public class Student
{
    public int IdStudent { get; set; }
    public int Grade_idGrade { get; set; }

    [Required, StringLength(45)]
    public string FirstName { get; set; } = string.Empty;

    [Required, StringLength(45)]
    public string LastName { get; set; } = string.Empty;

    [Required, StringLength(45)]
    public string Phone { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(45)]
    public string Email { get; set; } = string.Empty;

    public Grade? Grade { get; set; }
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
