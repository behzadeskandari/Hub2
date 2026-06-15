using System.Net.NetworkInformation;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NehzatMelli.Api.Controllers;
using OfficeOpenXml;
using Yarane.Application.Services;
using Yarane.Contract.Dtos.Pagination;
using Yarane.Contract.Dtos.Transaction;

namespace Yarane.Api.Controllers.v1
{

    public class ReportController : BaseController
    {
        private readonly IReportService _service;

        public ReportController(IReportService service)
        {
            _service = service;
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] TransactionSearchRequest request)
        {
            var result = await _service.Search(request);
            return Ok(result);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] PagedRequest request)
        {
            var result = await _service.GetAll(request.Page, request.PageSize);
            return Ok(result);
        }




        [HttpGet("export/excel")]
        public async Task<IActionResult> ExportExcel()
        {
            var query = await _service.ExportAll();

            const int maxRowsPerSheet = 1_000_000;

            using var package = new ExcelPackage();

            int sheetIndex = 1;
            int row = 2;
            int globalRow = 0;

            ExcelWorksheet ws = _service.CreateSheet(package, sheetIndex);

            var headers = new[]
            {
                "شماره رهگیری",
                "RRN",
                "مبلغ",
                "شماره ترمینال",
                "وضعیت",
                "نوع ترمینال",
                "تاریخ تراکنش",
                "کارت مبدآ",
                "کارت مقصد",
                "کد پذیرنده",
                "کد تایید",
            };
            for (int i = 0; i < headers.Length; i++)
            {
                var cell = ws.Cells[1, i + 1];
                cell.Value = headers[i];

                cell.Style.Font.Bold = true;
                cell.Style.Font.Color.SetColor(System.Drawing.Color.White);
                cell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(45, 62, 80));
                cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            }
            ws.Row(1).Height = 22;

            // Freeze header
            ws.View.FreezePanes(2, 1);

            await foreach (var item in query.AsAsyncEnumerable())
            {
                // 👉 create new sheet when limit reached
                if (globalRow > 0 && globalRow % maxRowsPerSheet == 0)
                {
                    sheetIndex++;
                    ws = _service.CreateSheet(package, sheetIndex);
                    row = 2;
                }
                ws.Cells[2, 1].Value = item.TrackingNumber;
                ws.Cells[2, 2].Value = item.RRN ?? "";
                ws.Cells[2, 3].Value = item.Amount;
                ws.Cells[2, 5].Value = item.TerminalNumber;
                ws.Cells[2, 6].Value = item.TerminalType;
                ws.Cells[2, 7].Value = item.TransactionDate;
                ws.Cells[2, 8].Value = item.SourcePan;
                ws.Cells[2, 9].Value = item.DestinationPan;
                ws.Cells[2, 10].Value = item.AcceptorCode;
                ws.Cells[2, 11].Value = item.ApprovalCode;
                ws.Cells[row, 3].Style.Numberformat.Format = "#,##0";
                ws.Cells[row, 4].Value = item.Status == 1 ? "موفق" : "ناموفق";

                var statusCell = ws.Cells[row, 4];
                statusCell.Style.Font.Bold = true;

                if (item.Status == 1)
                    statusCell.Style.Font.Color.SetColor(System.Drawing.Color.Green);
                else
                    statusCell.Style.Font.Color.SetColor(System.Drawing.Color.Red);


                string transactionDateFormatted = "";

                if (item.TransactionDate.HasValue && item.TransactionDate.Value > 0)
                {
                    try
                    {
                        // Convert from milliseconds since 1970
                        var dateTime = DateTimeOffset.FromUnixTimeMilliseconds(item.TransactionDate.Value)
                                                     .ToLocalTime()
                                                     .DateTime;

                        // Format as readable date (you can customize this)
                        transactionDateFormatted = $"\"{dateTime:yyyy/MM/dd HH:mm:ss}\"";
                    }
                    catch
                    {
                        transactionDateFormatted = $"\"{item.TransactionDate.Value}\"";
                    }
                }
                else
                {
                    transactionDateFormatted = "\"\"";
                }
                ws.Cells[row, 5].Value = item.TransactionDate?.ToString("yyyy-MM-dd HH:mm");

                row++;
                globalRow++;
            }

            // -----------------------------
            // 3. BORDER (table look)
            // -----------------------------
            var range = ws.Cells[1, 1, row - 1, 11];
            range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

            // -----------------------------
            // 4. AUTO FIT
            // -----------------------------
            ws.Cells[ws.Dimension.Address].AutoFitColumns();

            // Add min width for readability
            ws.Column(1).Width = 25;
            ws.Column(2).Width = 20;
            ws.Column(3).Width = 15;
            ws.Column(4).Width = 12;
            ws.Column(5).Width = 22;

            var stream = new MemoryStream(package.GetAsByteArray());

            return File(stream,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "transactions.xlsx");
        }
        [HttpPost("export/row-excel")]
        public IActionResult ExportSingleRow([FromBody] TransactionReportVm item)
        {
            if (item == null)
                return NotFound();

            using var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add("Transaction");

            // -----------------------------
            // HEADER STYLE
            // -----------------------------
            var headers = new[]
            {
                "شماره تراکنش",
                "RRN",
                "مبلغ",
                "شماره ترمینال",
                "نوع ترمینال",
                "تاریخ تراکنش",
                "کارت مبدآ",
                "کارت مقصد",
                "کد پذیرنده",
                "کد تایید",
                "وضعیت"
            };

            for (int i = 0; i < headers.Length; i++)
            {
                var cell = ws.Cells[1, i + 1];
                cell.Value = headers[i];

                cell.Style.Font.Bold = true;
                cell.Style.Font.Color.SetColor(System.Drawing.Color.White);
                cell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(45, 62, 80));
                cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            }

            ws.Row(1).Height = 22;
            ws.View.FreezePanes(2, 1);

            // -----------------------------
            // DATA ROW
            // -----------------------------
            ws.Cells[2, 1].Value = item.TrackingNumber;
            ws.Cells[2, 2].Value = item.RRN ?? "";
            ws.Cells[2, 3].Value = item.Amount;
            ws.Cells[2, 5].Value = item.TerminalNumber;
            ws.Cells[2, 6].Value = item.TerminalType;
            ws.Cells[2, 7].Value = item.TransactionDate;
            ws.Cells[2, 8].Value = item.SourcePan;
            ws.Cells[2, 9].Value = item.DestinationPan;
            ws.Cells[2, 10].Value = item.AcceptorCode;
            ws.Cells[2, 11].Value = item.ApprovalCode;
            ws.Cells[2, 3].Style.Numberformat.Format = "#,##0";

            ws.Cells[2, 4].Value = item.Status == 1 ? "موفق" : "ناموفق";

            var statusCell = ws.Cells[2, 4];
            statusCell.Style.Font.Bold = true;
            statusCell.Style.Font.Color.SetColor(
                item.Status == 1 ? System.Drawing.Color.Green : System.Drawing.Color.Red
            );

            // -----------------------------
            // BORDER + AUTO FIT
            // -----------------------------
            var range = ws.Cells[1, 1, 2, 4];

            range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

            ws.Cells[ws.Dimension.Address].AutoFitColumns();

            // -----------------------------
            // RETURN FILE
            // -----------------------------
            var stream = new MemoryStream(package.GetAsByteArray());

            return File(stream,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"transaction-{item.TrackingNumber}.xlsx");
        }

        [HttpGet("export/pdf")]
        public async Task<IActionResult> ExportPdf()
        {
            var data = await _service.ExportAll();
            using var stream = new MemoryStream();

            var document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4.Rotate(), 12, 12, 25, 25);
            var writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, stream);
            document.Open();

            // Fonts
            var baseFont = BaseFont.CreateFont(@"C:\Windows\Fonts\tahoma.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            var titleFont = new Font(baseFont, 18, Font.BOLD);
            var headerFont = new Font(baseFont, 9, Font.BOLD, BaseColor.WHITE);
            var normalFont = new Font(baseFont, 7f, Font.NORMAL);   // Even smaller for long fields

            // Title
            var title = new Paragraph(_service.FixPersian("گزارش تراکنش"), titleFont);
            title.Alignment = Element.ALIGN_CENTER;
            title.SpacingAfter = 15f;
            document.Add(title);

            // === 9 COLUMNS - MATCHING YOUR HEADER ORDER ===
            var table = new PdfPTable(9);
            table.WidthPercentage = 100;
            table.SetWidths(new float[] { 0.7f, 2.0f, 3.0f, 2.4f, 1.6f, 3.1f, 3.1f, 1.4f, 2.7f });
            table.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            table.DefaultCell.Padding = 5;

            // Header Helper
            void AddHeader(string text)
            {
                var cell = new PdfPCell(new Phrase(text, headerFont));
                cell.BackgroundColor = new BaseColor(45, 62, 80);
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.Padding = 8;
                cell.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                table.AddCell(cell);
            }

            // Correct Header Order (matching your latest PDF screenshot)
            AddHeader("ردیف");
            AddHeader("شماره ترمینال");
            AddHeader("شماره پیگیری");
            AddHeader("RRN");
            AddHeader("مبلغ");
            AddHeader("آدرس مبدا");
            AddHeader("آدرس مقصد");
            AddHeader("وضعیت");
            AddHeader("تاریخ تراکنش");

            int rowNumber = 1;

            await foreach (var item in data.AsAsyncEnumerable())
            {
                // 1. Row Number
                var rowCell = new PdfPCell(new Phrase(rowNumber.ToString(), normalFont));
                rowCell.HorizontalAlignment = Element.ALIGN_CENTER;
                rowCell.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                table.AddCell(rowCell);

                // 2. Terminal Number
                table.AddCell(_service.CreateCell(item.TerminalNumber ?? "", normalFont));

                // 3. Tracking Number (شماره پیگیری)
                table.AddCell(_service.CreateCell(item.TrackingNumber ?? "", normalFont));

                // 4. RRN
                table.AddCell(_service.CreateCell(item.RRN ?? "", normalFont));

                // 5. Amount
                var amtCell = _service.CreateCell(item.Amount.ToString(), normalFont);
                amtCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(amtCell);

                // 6. Source PAN (آدرس مبدا)
                table.AddCell(_service.CreateCell(item.SourcePan ?? "", normalFont));

                // 7. Destination PAN (آدرس مقصد)
                table.AddCell(_service.CreateCell(item.DestinationPan ?? "", normalFont));

                // 8. Status
                var statusText = item.Status == 1 ? "موفق" : "ناموفق";
                var statusCell = _service.CreateCell(statusText, normalFont);
                statusCell.HorizontalAlignment = Element.ALIGN_CENTER;
                statusCell.BackgroundColor = item.Status == 1
                    ? new BaseColor(200, 255, 200)
                    : new BaseColor(255, 200, 200);
                table.AddCell(statusCell);

                // 9. Date
                string dateStr = item.TransactionDate.HasValue && item.TransactionDate.Value > 0
                    ? DateTimeOffset.FromUnixTimeMilliseconds(item.TransactionDate.Value)
                                     .ToLocalTime()
                                     .ToString("yyyy/MM/dd HH:mm:ss")
                    : "-";
                var dateCell = _service.CreateCell(dateStr, normalFont);
                dateCell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(dateCell);

                rowNumber++;
            }

            document.Add(table);
            document.Close();

            return File(stream.ToArray(), "application/pdf", "transactions.pdf");
        }
        [HttpGet("export/csv")]
        public async Task<IActionResult> ExportCsv()
        {
            var data = await _service.ExportAll();
            var sb = new StringBuilder();
            sb.Append('\uFEFF'); // BOM for Persian support

            // ====================== HEADER ======================
            sb.AppendLine("شماره پیگیری,RRN,ApprovalCode,آدرس مبدا,آدرس مقصد,شماره ترمینال,نوع ترمینال,تاریخ ثبت,شماره پیگیری,مبلغ,وضعیت,تاریخ تراکنش");

            await foreach (var item in data.AsAsyncEnumerable())
            {
                var status = item.Status == 1 ? "موفق" : "ناموفق";

                // Date Formatting
                string transactionDateFormatted = "";
                string registrationDateFormatted = "";

                if (item.TransactionDate.HasValue && item.TransactionDate.Value > 0)
                {
                    try
                    {
                        var regDate = DateTimeOffset.FromUnixTimeMilliseconds(item.RegistrationDate ?? 0)
                                                    .ToLocalTime().DateTime;
                        var transDate = DateTimeOffset.FromUnixTimeMilliseconds(item.TransactionDate.Value)
                                                      .ToLocalTime().DateTime;

                        registrationDateFormatted = regDate.ToString("yyyy/MM/dd HH:mm:ss");
                        transactionDateFormatted = transDate.ToString("yyyy/MM/dd HH:mm:ss");
                    }
                    catch
                    {
                        registrationDateFormatted = item.RegistrationDate?.ToString() ?? "";
                        transactionDateFormatted = item.TransactionDate?.ToString() ?? "";
                    }
                }

                // Force long PAN numbers to be treated as TEXT in Excel
                string sourcePanText = string.IsNullOrEmpty(item.SourcePan) ? "" : $"'{item.SourcePan}";
                string destPanText = string.IsNullOrEmpty(item.DestinationPan) ? "" : $"'{item.DestinationPan}";

                sb.AppendLine(
                    $"{_service.Escape(item.TrackingNumber)}," +
                    $"{_service.Escape(item.RRN)}," +
                    $"{_service.Escape(item.ApprovalCode)}," +
                    $"{_service.Escape(sourcePanText)}," +           // Fixed: Force as text
                    $"{_service.Escape(destPanText)}," +             // Fixed: Force as text
                    $"{_service.Escape(item.TerminalNumber)}," +
                    $"{_service.Escape(item.TerminalType?.ToString())}," +
                    $"{_service.Escape(registrationDateFormatted)}," +
                    $"{_service.Escape(item.TrackingNumber?.ToString())}," +
                    $"{item.Amount}," +
                    $"{status}," +
                    $"{transactionDateFormatted}"
                );
            }

            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
            var stream = new MemoryStream(bytes);
            return File(stream, "text/csv", "transactions.csv");
        }



    }
}
