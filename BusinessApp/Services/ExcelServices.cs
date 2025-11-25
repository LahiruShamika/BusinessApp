using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;

namespace BusinessApp.Services
{
    public class ExcelService
    {
        public List<PaymentRecord> ExtractPaymentRecords(string filePath)
        {
            var paymentRecords = new List<PaymentRecord>();

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0]; // Assuming data is in the first worksheet
                var rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++) // Assuming the first row contains headers
                {
                    var paymentRecord = new PaymentRecord
                    {
                        Date = worksheet.Cells[row, 1].GetValue<DateTime>(), // Assuming date is in the first column
                        Narration = worksheet.Cells[row, 2].GetValue<string>(), // Assuming narration is in the second column
                        Amount = worksheet.Cells[row, 3].GetValue<decimal>() // Assuming amount is in the third column
                    };

                    paymentRecords.Add(paymentRecord);
                }
            }

            return paymentRecords;
        }
    }
}