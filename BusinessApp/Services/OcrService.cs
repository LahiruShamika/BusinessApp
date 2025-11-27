// ...existing code...
using System;
using System.Threading.Tasks;
using Tesseract;
using BusinessApp.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Collections.Generic;

namespace BusinessApp.Services
{
    public class OcrService
    {
        private readonly string _tessDataPath;

        public OcrService(string tessDataPath)
        {
            _tessDataPath = tessDataPath;
        }

        public async Task<OcrResult> ProcessImageAsync(string imagePath)
        {
            // This method is synchronous internally; keep Task signature for compatibility
            using var engine = new TesseractEngine(_tessDataPath, "eng", EngineMode.Default);
            using var img = Pix.LoadFromFile(imagePath);
            using var page = engine.Process(img);

            var text = page.GetText();
            var confidence = page.GetMeanConfidence();

            // Basic attempts to parse amount/date from text can be added later
            return await Task.FromResult(new OcrResult
            {
                ExtractedText = text ?? string.Empty,
                Confidence = confidence
            });
        }

        // Called by PaymentsController: returns list of OCR results (one or more slips)
        public async Task<List<OcrResult>> ProcessOcrFileAsync(IFormFile formFile)
        {
            var results = new List<OcrResult>();
            if (formFile == null) return results;

            // save to temp file and run OCR
            var temp = Path.GetTempFileName();
            try
            {
                await using (var fs = new FileStream(temp, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await formFile.CopyToAsync(fs);
                }

                var r = await ProcessImageAsync(temp);
                results.Add(r);
            }
            finally
            {
                try { File.Delete(temp); } catch { }
            }

            return results;
        }

        // Called by UploadController to process a single payment slip
        public async Task<OcrResult> ProcessPaymentSlipAsync(IFormFile formFile)
        {
            if (formFile == null) return new OcrResult { ExtractedText = string.Empty, Confidence = 0f };

            var temp = Path.GetTempFileName();
            try
            {
                await using (var fs = new FileStream(temp, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await formFile.CopyToAsync(fs);
                }

                return await ProcessImageAsync(temp);
            }
            finally
            {
                try { File.Delete(temp); } catch { }
            }
        }
    }
}
// ...existing code...