using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Elnes.Entities;

[PrimaryKey("Id")]
public class Subject
{
    public int Id { get; set; }
    
    [MaxLength(50)]
    [Required]
    public string Name { get; set; } = null!;
}