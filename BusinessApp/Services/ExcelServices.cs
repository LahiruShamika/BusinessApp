using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using BusinessApp.Models;

namespace BusinessApp.Services
{
    public class ExcelServices
    {
        public ExcelServices()
        {
            // Required for EPPlus 5+ when used in noncommercial projects
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        // Example: read payments from an Excel stream
        public IEnumerable<PaymentRecord> ReadPayments(Stream excelStream)
        {
            var results = new List<PaymentRecord>();

            using var package = new ExcelPackage(excelStream);
            var ws = package.Workbook.Worksheets.Count > 0 ? package.Workbook.Worksheets[0] : null;
            if (ws == null || ws.Dimension == null) return results;

            int startRow = 2; // assume row 1 = headers
            int endRow = ws.Dimension.End.Row;

            for (int r = startRow; r <= endRow; r++)
            {
                // Map columns to your PaymentRecord properties. Adjust indexes as needed.
                var record = new PaymentRecord
                {
                    // Example column mapping (change property names/types to match your model):
                    // Date = DateTime.TryParse(ws.Cells[r, 1].Text, out var d) ? d : (DateTime?)null,
                    // Narration = ws.Cells[r, 2].Text,
                    // Amount = decimal.TryParse(ws.Cells[r, 3].Text, out var a) ? a : 0m
                };

                // Only add if meaningful values present; adjust validation as needed
                results.Add(record);
            }

            return results;
        }
    }
}