using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using Yarane.Contract.Dtos.Pagination;
using Yarane.Contract.Dtos.Transaction;

namespace Yarane.Application.Services
{
    public interface IReportService
    {
        public Task<PagedResult<TransactionReportVm>> Search(TransactionSearchRequest req);
        public string Escape(string value);
        public string FixPersian(string text);
        public Task<IQueryable<TransactionReportVm>> ExportAll();
        public Task<PagedResult<TransactionReportVm>> GetAll(int Page, int PageSize);
        public ExcelWorksheet CreateSheet(ExcelPackage package, int index);
        PdfPCell CreateCell(string text, Font font);
    }
}
