using Microsoft.EntityFrameworkCore;

namespace Elnes.Entities;

[PrimaryKey("Id")]
public class TeacherSubject
{
    public int Id { get; set; }
    
    public int TeacherId { get; set; }
    
    public Teacher Teacher { get; set; } = null!;

    public int SubjectId { get; set; }
    
    public Subject Subject { get; set; } = null!;
}