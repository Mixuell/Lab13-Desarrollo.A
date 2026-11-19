using APIDemoLab.Data;
using APIDemoLab.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIDemoLab.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController(ApplicationDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Course>>> GetAll() =>
        await db.Courses.AsNoTracking().Where(x => !x.IsDeleted).OrderBy(x => x.Name).ToListAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Course>> GetById(int id)
    {
        var course = await db.Courses.AsNoTracking().FirstOrDefaultAsync(x => x.IdCourse == id && !x.IsDeleted);
        return course is null ? NotFound() : Ok(course);
    }

    [HttpPost]
    public async Task<ActionResult<Course>> Create(Course course)
    {
        course.IdCourse = 0;
        course.IsDeleted = false;
        db.Courses.Add(course);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = course.IdCourse }, course);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Course input)
    {
        if (id != input.IdCourse) return BadRequest("El ID de la ruta y del recurso no coinciden.");
        var course = await db.Courses.FirstOrDefaultAsync(x => x.IdCourse == id && !x.IsDeleted);
        if (course is null) return NotFound();
        course.Name = input.Name;
        course.Credit = input.Credit;
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var course = await db.Courses.FirstOrDefaultAsync(x => x.IdCourse == id && !x.IsDeleted);
        if (course is null) return NotFound();
        course.IsDeleted = true;
        await db.SaveChangesAsync();
        return NoContent();
    }
}
