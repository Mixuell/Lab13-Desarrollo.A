using System.ComponentModel.DataAnnotations;

namespace APIDemoLab.Models;

public class Grade
{
    public int IdGrade { get; set; }

    [Required, StringLength(45)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(45)]
    public string Description { get; set; } = string.Empty;

    public bool IsDeleted { get; set; }

    public ICollection<Student> Students { get; set; } = new List<Student>();
}
