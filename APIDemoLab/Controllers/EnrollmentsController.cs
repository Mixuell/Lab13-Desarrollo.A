using APIDemoLab.Data;
using APIDemoLab.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIDemoLab.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnrollmentsController(ApplicationDbContext db) : ControllerBase
{
    [HttpGet("by-course")]
    public async Task<ActionResult<IEnumerable<Enrollment>>> ByCourse([FromQuery] string? courseName)
    {
        var query = db.Enrollments.AsNoTracking().Include(e => e.Student).ThenInclude(s => s!.Grade).Include(e => e.Course).AsQueryable();
        if (!string.IsNullOrWhiteSpace(courseName)) query = query.Where(e => e.Course!.Name.Contains(courseName));
        return await query.OrderBy(e => e.Course!.Name).ThenBy(e => e.Student!.LastName).ToListAsync();
    }

    [HttpGet("by-grade")]
    public async Task<ActionResult<IEnumerable<Enrollment>>> ByGrade([FromQuery] int? gradeId)
    {
        var query = db.Enrollments.AsNoTracking().Include(e => e.Student).ThenInclude(s => s!.Grade).Include(e => e.Course).AsQueryable();
        if (gradeId.HasValue) query = query.Where(e => e.Student!.Grade_idGrade == gradeId.Value);
        return await query.OrderBy(e => e.Course!.Name).ThenBy(e => e.Student!.LastName).ToListAsync();
    }
}
