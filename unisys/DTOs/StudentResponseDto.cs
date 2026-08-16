namespace UniSys.DTOs
{
    public class StudentResponseDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Major { get; set; }
        public double GPA { get; set; }
        public List<string> EnrolledSubjectNames { get; set; } = new(); // Just the names, no loops!
    }
}