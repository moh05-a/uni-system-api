namespace UniSys.Models{

public class University
{
    public int Id { get; set; } 
    public string? Name { get; set; }
    public string? Location { get; set; }
    public List<Student> Students { get; set; } = new();
    public List<Tutor> Tutors { get; set; } = new();
    public List<Subject> Subjects { get; set; } = new();
}
} 