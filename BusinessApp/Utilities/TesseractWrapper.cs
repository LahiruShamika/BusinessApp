using System;
using System.IO;
using Tesseract;

namespace BusinessApp.Utilities
{
    public class TesseractWrapper
    {
        private readonly string _tessDataPath;

        public TesseractWrapper(string tessDataPath)
        {
            _tessDataPath = tessDataPath;
        }

        public string ExtractTextFromImage(string imagePath)
        {
            if (!File.Exists(imagePath))
            {
                throw new FileNotFoundException("The specified image file does not exist.", imagePath);
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
