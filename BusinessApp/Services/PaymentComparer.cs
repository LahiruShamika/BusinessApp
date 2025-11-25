using System;
using System.Collections.Generic;
using System.Linq;
using BusinessApp.Models;

namespace BusinessApp.Services
{
    public class PaymentComparer
    {
        public List<ValidationResult> ComparePayments(List<PaymentRecord> excelRecords, List<PaymentRecord> extractedRecords)
        {
            var results = new List<ValidationResult>();

            foreach (var extracted in extractedRecords)
            {
                var matchingRecord = excelRecords.FirstOrDefault(record =>
                    record.Date == extracted.Date && record.Narration == extracted.Narration);

                if (matchingRecord != null)
                {
                    results.Add(new ValidationResult
                    {
                        IsValid = true,
                        ErrorMessage = string.Empty
                    });
                }
                else
                {
                    results.Add(new ValidationResult
                    {
                        IsValid = false,
                        ErrorMessage = $"Invalid payment: {extracted.Date} - {extracted.Narration}"
                    });
                }
            }

            return results;
        }
    }
}