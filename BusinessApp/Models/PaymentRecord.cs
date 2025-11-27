// ...existing code...
namespace BusinessApp.Models
{
    public class PaymentRecord
    {
        // Use string for Id to match Excel text cells
        public string Id { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }

        // Make optional to avoid CS8618 warnings
        public string? PayerName { get; set; }
        public string? ReferenceNumber { get; set; }
    }
}
// ...existing code...