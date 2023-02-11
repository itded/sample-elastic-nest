using Nest;

namespace Elnes.Elastic.Indices;

[ElasticsearchType(IdProperty = "Id", RelationName = "sample_teacher")]
public class TeacherIndex
{
    public int Id { get; set; }
    
    [Keyword(Name="first_name")]
    public string FirstName { get; set; }
    
    [Keyword(Name="last_name")]
    public string LastName { get; set; }
    
    [Text(Name = "subjects")]
    public string Subjects { get; set; }
}