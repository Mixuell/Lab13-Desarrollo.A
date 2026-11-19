using APIDemoLab.Data;
using APIDemoLab.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIDemoLab.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController(ApplicationDbContext db) : ControllerBase
{
    // Endpoint 8: filters are optional and can be combined.
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Student>>> Search([FromQuery] string? name, [FromQuery] string? lastName, [FromQuery] string? email)
    {
        var query = db.Students.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(name)) query = query.Where(s => s.FirstName.Contains(name));
        if (!string.IsNullOrWhiteSpace(lastName)) query = query.Where(s => s.LastName.Contains(lastName));
        if (!string.IsNullOrWhiteSpace(email)) query = query.Where(s => s.Email.Contains(email));
        return await query.OrderByDescending(s => s.LastName).ThenBy(s => s.FirstName).ToListAsync();
    }

    // Endpoint 9: one result per enrollment, ordered by its course name.
    [HttpGet("by-grade")]
    public async Task<ActionResult<IEnumerable<object>>> ByNameAndGrade([FromQuery] string? name, [FromQuery] int? gradeId)
    {
        var query = db.Students.AsNoTracking().Include(s => s.Grade).Include(s => s.Enrollments).ThenInclude(e => e.Course).AsQueryable();
        if (!string.IsNullOrWhiteSpace(name)) query = query.Where(s => s.FirstName.Contains(name) || s.LastName.Contains(name));
        if (gradeId.HasValue) query = query.Where(s => s.Grade_idGrade == gradeId.Value);
        var students = await query.ToListAsync();
        var result = students.SelectMany(s => s.Enrollments.DefaultIfEmpty(), (s, e) => new { Student = s, Enrollment = e })
            .OrderByDescending(x => x.Enrollment?.Course?.Name)
            .Select(x => new { x.Student.IdStudent, x.Student.Grade_idGrade, x.Student.FirstName, x.Student.LastName, x.Student.Phone, x.Student.Email, Grade = x.Student.Grade,
                Enrollment = x.Enrollment is null ? null : new { x.Enrollment.IdEnrollment, x.Enrollment.Date, Course = x.Enrollment.Course } });
        return Ok(result);
    }
}
