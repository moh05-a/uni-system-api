namespace UniSys.DTOs
{
    public class SubjectResponseDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int CreditHours { get; set; }
        public string? TutorName { get; set; } // Flattens the Tutor object into just a string
        public int StudentCount { get; set; }
    }
}