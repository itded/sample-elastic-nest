using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Elnes.Entities;

[PrimaryKey("Id")]
public class Teacher
{
    public int Id { get; set; }
    
    [MaxLength(50)]
    [Required]
    public string FirstName { get; set; } = null!;

    [MaxLength(50)]
    [Required]
    public string LastName { get; set; } = null!;
}