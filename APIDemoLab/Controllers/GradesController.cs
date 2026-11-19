using APIDemoLab.Data;
using APIDemoLab.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIDemoLab.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GradesController(ApplicationDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Grade>>> GetAll() =>
        await db.Grades.AsNoTracking().Where(x => !x.IsDeleted).OrderBy(x => x.Name).ToListAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Grade>> GetById(int id)
    {
        var grade = await db.Grades.AsNoTracking().FirstOrDefaultAsync(x => x.IdGrade == id && !x.IsDeleted);
        return grade is null ? NotFound() : Ok(grade);
    }

    [HttpPost]
    public async Task<ActionResult<Grade>> Create(Grade grade)
    {
        grade.IdGrade = 0;
        grade.IsDeleted = false;
        db.Grades.Add(grade);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = grade.IdGrade }, grade);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Grade input)
    {
        if (id != input.IdGrade) return BadRequest("El ID de la ruta y del recurso no coinciden.");
        var grade = await db.Grades.FirstOrDefaultAsync(x => x.IdGrade == id && !x.IsDeleted);
        if (grade is null) return NotFound();
        grade.Name = input.Name;
        grade.Description = input.Description;
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var grade = await db.Grades.FirstOrDefaultAsync(x => x.IdGrade == id && !x.IsDeleted);
        if (grade is null) return NotFound();
        grade.IsDeleted = true;
        await db.SaveChangesAsync();
        return NoContent();
    }
}
