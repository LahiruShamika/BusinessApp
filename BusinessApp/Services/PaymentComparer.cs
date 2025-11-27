// ...existing code...
using System.Collections.Generic;
using BusinessApp.Models;

namespace BusinessApp.Services
{
    public class PaymentComparer
    {
        // Accept single OCR result (UploadController uses this)
        public ComparisonResult Compare(List<PaymentRecord> paymentRecords, OcrResult ocrResult)
        {
            var list = new List<OcrResult>();
            if (ocrResult != null) list.Add(ocrResult);
            return Compare(paymentRecords, list);
        }

        // Accept multiple OCR results (PaymentsController uses this)
        public ComparisonResult Compare(List<PaymentRecord> paymentRecords, List<OcrResult> ocrResults)
        {
            var result = new ComparisonResult();

            if (paymentRecords == null) return result;
            ocrResults ??= new List<OcrResult>();

            foreach (var record in paymentRecords)
            {
                bool matched = false;

                foreach (var ocr in ocrResults)
                {
                    // simple matching: amount + same date (date comparison tolerant to day)
                    var amountMatches = ocr?.Amount.HasValue == true && record.Amount == ocr!.Amount!.Value;
                    var dateMatches = ocr?.Date.HasValue == true && record.Date.Date == ocr!.Date!.Value.Date;

                    if (amountMatches && dateMatches)
                    {
                        // Add a matching item
                        result.Matches.Add(new ComparisonItem(true, "Matched by amount and date", record, ocr));
                        matched = true;
                        break;
                    }
                }

                if (!matched)
                {
                    result.Mismatches.Add(new ComparisonItem(false, "No matching OCR found", record, null));
                }
            }

            return result;
        }
    }
}
// ...existing code...