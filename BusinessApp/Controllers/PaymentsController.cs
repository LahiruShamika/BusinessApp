using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessApp.Models;
using BusinessApp.Services;

namespace BusinessApp.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly ExcelService _excelService;
        private readonly OcrService _ocrService;
        private readonly PaymentComparer _paymentComparer;

        public PaymentsController(ExcelService excelService, OcrService ocrService, PaymentComparer paymentComparer)
        {
            _excelService = excelService;
            _ocrService = ocrService;
            _paymentComparer = paymentComparer;
        }

        [HttpPost]
        public async Task<IActionResult> ValidatePayments(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var paymentRecords = await _excelService.ExtractPaymentRecords(file);
            var extractedData = await _ocrService.ProcessPaymentSlip();

            var validationResults = _paymentComparer.ComparePayments(paymentRecords, extractedData);

            return View("Results/Index", validationResults);
        }
    }
}