namespace UniSys.Models{

public class Tutor : Person
{
    public string? Department { get; set; }
    public decimal Salary { get; private set; }
    public List<Subject> TeachingSubjects { get; set; } = new();
}
}