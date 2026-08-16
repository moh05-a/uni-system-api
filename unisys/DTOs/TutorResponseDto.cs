namespace UniSys.DTOs
{
    public class TutorResponseDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Department { get; set; }
        public List<string> SubjectNames { get; set; } = new();
    }
}