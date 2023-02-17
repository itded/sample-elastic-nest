using Elnes.Common;
using Elnes.Data;
using Elnes.Elastic.Documents;
using Microsoft.EntityFrameworkCore;
using Nest;

namespace Elnes.Commands;

public class CopyDataToElasticCommand : IDataCommand
{
    private readonly AppDbContext _appDbContext;
    private readonly Uri _esNodeUri;
    private readonly int? _teacherId;

    public CopyDataToElasticCommand(AppDbContext appDbContext, Uri esNodeUri, int? teacherId)
    {
        _appDbContext = appDbContext;
        _esNodeUri = esNodeUri;
        _teacherId = teacherId;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var esConfig = new ConnectionSettings(_esNodeUri);
        var esClient = new ElasticClient(esConfig);

        if (_teacherId != null)
        {
            await CreateTeacherDocument(cancellationToken, esClient, _teacherId.Value);
        }
        else
        {
            await CreateTeacherDocuments(cancellationToken, esClient);
        }
        
    }

    private async Task CreateTeacherDocument(CancellationToken cancellationToken, ElasticClient esClient, int teacherId)
    {
        var teacherSubjects = await _appDbContext.TeacherSubjects.AsNoTracking().Include(x => x.Subject)
            .Include(x => x.Teacher).Where(t => t.TeacherId == teacherId).Select(x => x).ToListAsync(cancellationToken);

        if (!teacherSubjects.Any())
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Teacher subjects with TeacherId = {teacherId} were not found");
            Console.ForegroundColor = ConsoleColor.White;
            return;
        }

        var teacher = teacherSubjects.First().Teacher;
        var subjectNames = teacherSubjects.Select(x => x.Subject.Name);
        var document = new TeacherDocument()
        {
            FirstName = teacher.FirstName,
            LastName = teacher.LastName,
            Subjects = string.Join(", ", subjectNames)
        };

        var createResult = await esClient.CreateAsync(document, c => c
            .Index(Constants.ElasticTeacherIndexName)
            .Id(teacher.Id), cancellationToken);

        Console.WriteLine($"{teacherId}: {createResult.Result}");
    }

    private async Task CreateTeacherDocuments(CancellationToken cancellationToken, ElasticClient esClient)
    {
        var teacherSubjects = await _appDbContext.TeacherSubjects.AsNoTracking().Include(x => x.Subject)
            .Include(x => x.Teacher).Select(x => x).ToListAsync(cancellationToken);
        var teacherSubjectGroups = teacherSubjects.GroupBy(x => x.TeacherId);

        foreach (var teacherSubjectGroup in teacherSubjectGroups)
        {
            var teacher = teacherSubjectGroup.First().Teacher;
            var subjectNames = teacherSubjectGroup.Select(x => x.Subject.Name);
            var document = new TeacherDocument()
            {
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                Subjects = string.Join(", ", subjectNames)
            };

            var createResult = await esClient.CreateAsync(document, c => c
                .Index(Constants.ElasticTeacherIndexName)
                .Id(teacher.Id), cancellationToken);

            Console.WriteLine($"{teacher.Id}: {createResult.Result}");
        }
    }
}