// ...existing code...
using System.Collections.Generic;

namespace BusinessApp.Models
{
    public class ComparisonItem
    {
        public bool IsMatch { get; set; }
        public string Message { get; set; } = string.Empty;
        public PaymentRecord? Record { get; set; }
        public OcrResult? Ocr { get; set; }

        public ComparisonItem(bool isMatch, string message, PaymentRecord? record, OcrResult? ocr)
        {
            IsMatch = isMatch;
            Message = message;
            Record = record;
            Ocr = ocr;
        }
    }

    public class ComparisonResult
    {
        public List<ComparisonItem> Matches { get; } = new();
        public List<ComparisonItem> Mismatches { get; } = new();

        public ComparisonResult() { }

        // Keep constructor used by PaymentComparer
        public ComparisonResult(bool isMatch, string message, PaymentRecord? record, OcrResult? ocr)
        {
            var item = new ComparisonItem(isMatch, message, record, ocr);
            if (isMatch) Matches.Add(item); else Mismatches.Add(item);
        }
    }
}
// ...existing code...