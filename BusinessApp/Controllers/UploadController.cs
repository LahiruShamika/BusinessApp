using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using BusinessApp.Services;

namespace BusinessApp.Controllers
{
    public class UploadController : Controller
    {
        private readonly ExcelService _excelService;
        private readonly OcrService _ocrService;

        public UploadController(ExcelService excelService, OcrService ocrService)
        {
            _excelService = excelService;
            _ocrService = ocrService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("File", "Please upload a valid file.");
                return View("Index");
            }

            var filePath = Path.Combine(Path.GetTempPath(), file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var paymentRecords = _excelService.ReadPaymentRecords(filePath);
            // Further processing can be done here, such as invoking OCR and comparing payments.

            return RedirectToAction("Index", "Results"); // Redirect to results page after processing
        }
    }
}