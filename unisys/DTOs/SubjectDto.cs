namespace UniSys.DTOs
{
    public class SubjectCreateDto
    {
        public string? Name { get; set; }
        public int CreditHours { get; set; }
        public int TutorId { get; set; } // Pass the ID of the Tutor teaching it
    }
}