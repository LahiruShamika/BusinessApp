// ...existing code...
namespace BusinessApp.Models
{
    public class OcrResult
    {
        public string ExtractedText { get; set; } = string.Empty;
        public float Confidence { get; set; }

        // Make nullable because OCR may not parse these reliably
        public decimal? Amount { get; set; }
        public DateTime? Date { get; set; }
        public string? Narration { get; set; }
    }
}
// ...existing code...