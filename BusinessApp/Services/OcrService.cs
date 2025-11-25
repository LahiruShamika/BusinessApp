using System;
using System.Drawing;
using System.IO;
using Tesseract;

namespace BusinessApp.Services
{
    public class OcrService
    {
        private readonly string _tessDataPath;

        public OcrService(string tessDataPath)
        {
            _tessDataPath = tessDataPath;
        }

        public string ExtractTextFromImage(string imagePath)
        {
            if (!File.Exists(imagePath))
            {
                throw new FileNotFoundException("Image file not found.", imagePath);
            }

            using (var engine = new TesseractEngine(_tessDataPath, "eng", EngineMode.Default))
            {
                using (var img = Pix.LoadFromFile(imagePath))
                {
                    using (var page = engine.Process(img))
                    {
                        return page.GetText();
                    }
                }
            }
        }
    }
}