namespace UniSys.Models
{

public class Subject
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int CreditHours { get; set; }
    public Tutor? Tutor { get; private set; }
    public List<Student> Students { get; set; } = new();
}
}