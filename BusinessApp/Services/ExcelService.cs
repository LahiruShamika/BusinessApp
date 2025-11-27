// ...existing code...
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using BusinessApp.Models;

namespace BusinessApp.Services
{
    public class ExcelService
    {
        // Keep synchronous helper if needed
        public List<PaymentRecord> ReadExcelFile(string filePath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var paymentRecords = new List<PaymentRecord>();

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0]; // Assuming data is in the first worksheet
                var rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++) // Assuming the first row contains headers
                {
                    var idText = worksheet.Cells[row, 1].Text;
                    var amountText = worksheet.Cells[row, 2].Text;
                    var dateText = worksheet.Cells[row, 3].Text;

                    decimal.TryParse(amountText, NumberStyles.Any, CultureInfo.InvariantCulture, out var amount);
                    DateTime.TryParse(dateText, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date);

                    var paymentRecord = new PaymentRecord
                    {
                        Id = idText,
                        Amount = amount,
                        Date = date
                    };
                    paymentRecords.Add(paymentRecord);
                }
            }

            return paymentRecords;
        }

        // Async overload that matches controller usage (reads from uploaded IFormFile)
        public async Task<List<PaymentRecord>> ReadExcelFileAsync(IFormFile formFile)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var ms = new MemoryStream();
            await formFile.CopyToAsync(ms);
            ms.Position = 0;

            var paymentRecords = new List<PaymentRecord>();
            using (var package = new ExcelPackage(ms))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    var idText = worksheet.Cells[row, 1].Text;
                    var amountText = worksheet.Cells[row, 2].Text;
                    var dateText = worksheet.Cells[row, 3].Text;

                    decimal.TryParse(amountText, NumberStyles.Any, CultureInfo.InvariantCulture, out var amount);
                    DateTime.TryParse(dateText, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date);

                    paymentRecords.Add(new PaymentRecord
                    {
                        Id = idText,
                        Amount = amount,
                        Date = date
                    });
                }
            }

            return paymentRecords;
        }
    }
}
// ...existing code...