namespace APIDemoLab.Models;

public class Enrollment
{
    public int IdEnrollment { get; set; }
    public int Student_idStudent { get; set; }
    public int Course_idCourse { get; set; }
    public DateTime Date { get; set; }

    public Student? Student { get; set; }
    public Course? Course { get; set; }
}
