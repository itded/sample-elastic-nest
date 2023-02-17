using Bogus;
using Elnes.Data;
using Elnes.Entities;
using Microsoft.EntityFrameworkCore;

namespace Elnes.Commands;

/// <summary>
/// Seeds faked data using Bogus
/// </summary>
public class SeedFakeDataCommand : IDataCommand
{
    private readonly AppDbContext _appDbContext;

    public SeedFakeDataCommand(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        const int teacherSeed = 5000;
        const int subjectSeed = 6000;
        
        var rnd = new Random(subjectSeed);
        
        var subjectNames = new []
        {
            "Anthropology",
            "Art",
            "Biology",
            "Business Administration",
            "Chemistry",
            "Communication",
            "English",
            "Economics",
            "German",
            "History",
            "Mathematics",
            "Media Studies",
            "Psychology",
            "Physics",
            "Sociology",
            "Spanish"
        };

        var subjectList = new List<Subject>();
        foreach (var subjectName in subjectNames)
        {
            var subjectExists = await _appDbContext.Subjects.AsNoTracking().AnyAsync(x => x.Name == subjectName, cancellationToken);
            if (subjectExists)
            {
                continue;
            }
            
            var subject = await _appDbContext.Subjects.AddAsync(new Subject()
            {
                Name = subjectName
            }, cancellationToken);
            await _appDbContext.SaveChangesAsync(cancellationToken);
            subjectList.Add(subject.Entity);
        }

        var teacherFaker = new Faker<Teacher>().UseSeed(teacherSeed)
            .RuleFor(x => x.FirstName, (f, p) => f.Name.FirstName())
            .RuleFor(x => x.LastName, (f, p) => f.Name.LastName());
        
        // each subject has two teachers
        foreach (var subject in subjectList)
        {
            var teachers= teacherFaker.Generate(2);
            
            await _appDbContext.Teachers.AddRangeAsync(teachers, cancellationToken);

            foreach (var teacher in teachers)
            {
                await _appDbContext.TeacherSubjects.AddAsync(new TeacherSubject()
                {
                    Teacher = teacher,
                    Subject = subject
                }, cancellationToken);

                // another  subject can be assigned to the teacher
                var newSubjectIdx = rnd.Next(0, subjectList.Count);
                var newSubject = subjectList[newSubjectIdx];

                if (newSubject.Name != subject.Name)
                {
                    await _appDbContext.TeacherSubjects.AddAsync(new TeacherSubject()
                    {
                        Teacher = teacher,
                        Subject = newSubject
                    }, cancellationToken);
                }
            }

            await _appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}