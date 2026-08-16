namespace UniSys.Models
{

public class Student : Person
{
    public string? Major { get; set; }
    public double GPA { get; set; }
    public List<Subject> EnrolledSubjects { get; set; } = new();
}
}