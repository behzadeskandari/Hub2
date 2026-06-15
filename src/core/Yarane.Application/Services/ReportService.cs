using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Polly.Caching;
using Yarane.Contract.Dtos.Pagination;
using Yarane.Contract.Dtos.Report;
using Yarane.Contract.Dtos.Transaction;
using Yarane.Persistance.Contexts;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Yarane.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly ReportDbContext _context;
        public ReportService(ReportDbContext context)
        {
            _context = context;
        }
        public ExcelWorksheet CreateSheet(ExcelPackage package, int index)
        {
            var ws = package.Workbook.Worksheets.Add($"Transactions_{index}");
            var headers = new[]
            {
                "شماره رهگیری", "RRN", "مبلغ", "وضعیت", "تاریخ تراکنش"
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
            return ws;
        }
        public async Task<IQueryable<TransactionReportVm>> ExportAll()
        {
            //var cardData = _context.CardHolderTransaction
            //    .AsNoTracking()
            //    .Select(x => new TransactionReportVm
            //    {
            //        Source = "نگه دارنده",
            //        Amount = x.Amount,
            //        Status = x.Status,
            //        TransactionDate = x.TransactionDate,
            //        TrackingNumber = x.TrackingNumber ?? "",
            //        ApprovalCode = x.ApprovalCode ?? "",
            //        RRN = x.RRN ?? "",
            //        SourcePan = x.SourcePan,
            //        ReferenceNumber = x.ReferenceNumber,
            //        DestinationPan = x.DestinationPan,
            //        TerminalType = x.TerminalType ?? 0,
            //        TerminalNumber = x.TerminalNumber,
            //        RegistrationDate = x.RegistrationDate ?? 0,
            //        CardHolderName = x.CardHolderName ?? "",
            //        Error_Json = ""
            //    });

            //var cardHistoryData = _context.CardHolderTransactionHistory
            //    .AsNoTracking()
            //    .Select(x => new TransactionReportVm
            //    {
            //        Source = "نگه دارنده",
            //        Amount = x.Amount,
            //        Status = x.Status,
            //        TransactionDate = x.TransactionDate,
            //        TrackingNumber = x.TrackingNumber ?? "",
            //        ApprovalCode = x.ApprovalCode ?? "",
            //        RRN = x.RRN ?? "",
            //        SourcePan = x.SourcePan,
            //        ReferenceNumber = x.ReferenceNumber,
            //        DestinationPan = x.DestinationPan,
            //        TerminalType = x.TerminalType ?? 0,
            //        TerminalNumber = x.TerminalNumber,
            //        RegistrationDate = x.RegistrationDate ?? 0,
            //        CardHolderName = x.CardHolderName ?? "",
            //        Error_Json = ""
            //    });

            //var transferData = _context.TransferCardTransAction
            //    .AsNoTracking()
            //    .Select(x => new TransactionReportVm
            //    {
            //        Source = "واریز کننده",
            //        Amount = x.Amount ?? 0m,
            //        Status = x.Status ?? 0,
            //        TransactionDate = x.TransactionDate ?? 0,
            //        TrackingNumber = x.TrackingNumber ?? "",
            //        ApprovalCode = x.ApprovalCode ?? "",
            //        RRN = x.RRN ?? "",
            //        SourcePan = null,
            //        ReferenceNumber = null,
            //        DestinationPan = null,
            //        TerminalType = x.TerminalType.HasValue ? Convert.ToInt32(x.TerminalType.Value) : Convert.ToInt32(0),
            //        TerminalNumber = null,
            //        RegistrationDate = x.RegistrationDate ?? 0,
            //        CardHolderName = "",
            //        Error_Json = x.ErrorJson ?? ""
            //    });

            //var transferHistoryData = _context.TransferCardTransActionHistory
            //    .AsNoTracking()
            //    .Select(x => new TransactionReportVm
            //    {
            //        Source = "واریز کننده",
            //        Amount = x.Amount ?? 0m,
            //        Status = x.Status ?? 0,
            //        TransactionDate = x.TransactionDate ?? 0,
            //        TrackingNumber = x.TrackingNumber ?? "",
            //        ApprovalCode = x.ApprovalCode ?? "",
            //        RRN = x.RRN ?? "",
            //        SourcePan = null,
            //        ReferenceNumber = null,
            //        DestinationPan = null,

            //        TerminalType = x.TerminalType.HasValue ? Convert.ToInt32(x.TerminalType.Value) : Convert.ToInt32(0),
            //        TerminalNumber = null,
            //        RegistrationDate = x.RegistrationDate ?? 0,
            //        CardHolderName = "",
            //        Error_Json = x.ErrorJson ?? ""
            //    });

            //return cardData
            //    .Concat(cardHistoryData)
            //    .Concat(transferData)
            //    .Concat(transferHistoryData);
            var query = from card in _context.CardHolderTransaction.AsNoTracking()

                        join cardHist in _context.CardHolderTransactionHistory.AsNoTracking()
                            on card.TrackingNumber equals cardHist.TrackingNumber into cardHistGroup
                        from cardHist in cardHistGroup.DefaultIfEmpty()

                        join transfer in _context.TransferCardTransAction.AsNoTracking()
                            on card.TrackingNumber equals transfer.TrackingNumber into transferGroup
                        from transfer in transferGroup.DefaultIfEmpty()

                        join transferHist in _context.TransferCardTransActionHistory.AsNoTracking()
                            on card.TrackingNumber equals transferHist.TrackingNumber into transferHistGroup
                        from transferHist in transferHistGroup.DefaultIfEmpty()

                            // === FIXED CLIENT_CONFIG JOIN ===
                        join client in _context.ClientConfig.AsNoTracking()
                            on new
                            {
                                TerminalNumber = card.TerminalNumber ?? "",
                                AcceptorCode = card.AcceptorCode ?? ""
                            }
                            equals new
                            {
                                TerminalNumber = client.TerminalNumber ?? "",
                                AcceptorCode = client.AcceptorCode.ToString() ?? ""
                            }
                            into clientGroup
                        from client in clientGroup.DefaultIfEmpty()

                        select new TransactionReportVm
                        {
                            Source = transfer != null || transferHist != null
                              ? "واریز کننده"
                              : "نگه دارنده",
                            TrackingNumber = card.TrackingNumber ?? "",
                            RRN = card.RRN ?? "",
                            Amount = card.Amount,
                            Status = card.Status,
                            TransactionDate = card.TransactionDate,
                            ApprovalCode = card.ApprovalCode ?? "",
                            SourcePan = card.SourcePan ?? "",
                            DestinationPan = card.DestinationPan ?? "",
                            TerminalNumber = card.TerminalNumber ?? "",
                            AcceptorCode = card.AcceptorCode ?? "",
                            ReferenceNumber = card.ReferenceNumber ?? "",
                            TerminalType = card.TerminalType ?? 0,
                            Localization = card.Localization ?? 0,
                            SourceAddress = card.SourceAddress ?? "",
                            RegistrationDate = card.RegistrationDate ?? 0,
                            AdditionalResponseData = card.AdditionalResponseData ?? "",
                            CardHolderName = card.CardHolderName ?? "",
                            TwoPhaseReferenceCode = card.TwoPhaseReferenceCode ?? "",

                            // Client Config
                            ConsumerName = client.ConsumerName ?? "",

                            // Error Handling
                            Error_Json = !string.IsNullOrEmpty(transfer.ErrorJson) ? transfer.ErrorJson :
                                         !string.IsNullOrEmpty(transfer.ErrorJson) ? transfer.ErrorJson :
                                         !string.IsNullOrEmpty(transferHist.ErrorJson) ? transferHist.ErrorJson : "",

                            TransferAmount = transfer.Amount,
                            TransferStatus = transfer.Status,
                            TransferErrorJson = transfer.ErrorJson ?? transferHist.ErrorJson,
                            TransferTerminalType = transfer != null && transfer.TerminalType.HasValue
                                ? Convert.ToInt32(transfer.TerminalType.Value) : 0,
                        };

            return query.OrderByDescending(x => x.TransactionDate);
        }



        public async Task<PagedResult<TransactionReportVm>> GetAll(int Page, int PageSize)
        {
            var query = from card in _context.CardHolderTransaction.AsNoTracking()

                        join cardHist in _context.CardHolderTransactionHistory.AsNoTracking()
                            on card.TrackingNumber equals cardHist.TrackingNumber into cardHistGroup
                        from cardHist in cardHistGroup.DefaultIfEmpty()

                        join transfer in _context.TransferCardTransAction.AsNoTracking()
                            on card.TrackingNumber equals transfer.TrackingNumber into transferGroup
                        from transfer in transferGroup.DefaultIfEmpty()

                        join transferHist in _context.TransferCardTransActionHistory.AsNoTracking()
                            on card.TrackingNumber equals transferHist.TrackingNumber into transferHistGroup
                        from transferHist in transferHistGroup.DefaultIfEmpty()

                            // === FIXED CLIENT_CONFIG JOIN ===
                        join client in _context.ClientConfig.AsNoTracking()
                            on new
                            {
                                TerminalNumber = card.TerminalNumber ?? "",
                                AcceptorCode = card.AcceptorCode ?? ""
                            }
                            equals new
                            {
                                TerminalNumber = client.TerminalNumber ?? "",
                                AcceptorCode = client.AcceptorCode.ToString() ?? ""
                            }
                            into clientGroup
                        from client in clientGroup.DefaultIfEmpty()

                        select new TransactionReportVm
                        {
                            Source = transfer != null || transferHist != null
                              ? "واریز کننده"
                              : "نگه دارنده",
                            TrackingNumber = card.TrackingNumber ?? "",
                            RRN = card.RRN ?? "",
                            Amount = card.Amount,
                            Status = transfer.Status,
                            TransactionDate = card.TransactionDate,
                            ApprovalCode = card.ApprovalCode ?? "",
                            SourcePan = card.SourcePan ?? "",
                            DestinationPan = card.DestinationPan ?? "",
                            TerminalNumber = card.TerminalNumber ?? "",
                            AcceptorCode = card.AcceptorCode ?? "",
                            ReferenceNumber = card.ReferenceNumber ?? "",
                            TerminalType = card.TerminalType ?? 0,
                            Localization = card.Localization ?? 0,
                            SourceAddress = card.SourceAddress ?? "",
                            RegistrationDate = card.RegistrationDate ?? 0,
                            AdditionalResponseData = card.AdditionalResponseData ?? "",
                            CardHolderName = card.CardHolderName ?? "",
                            TwoPhaseReferenceCode = card.TwoPhaseReferenceCode ?? "",

                            ConsumerName = client.ConsumerName ?? "",

                            Error_Json = !string.IsNullOrEmpty(transfer.ErrorJson) ? transfer.ErrorJson :
                                         !string.IsNullOrEmpty(transfer.ErrorJson) ? transfer.ErrorJson :
                                         !string.IsNullOrEmpty(transferHist.ErrorJson) ? transferHist.ErrorJson : "",

                            TransferAmount = transfer.Amount,
                            TransferStatus = transfer.Status,
                            TransferErrorJson = transfer.ErrorJson ?? transferHist.ErrorJson,
                            TransferTerminalType = transfer != null && transfer.TerminalType.HasValue
                                ? Convert.ToInt32(transfer.TerminalType.Value) : 0,
                        };

            query = query.OrderByDescending(x => x.TransactionDate);

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((Page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            return new PagedResult<TransactionReportVm>
            {
                Items = items,
                TotalCount = totalCount
            };
        }

        //public async Task<PagedResult<TransactionReportVm>> GetAll(int Page, int PageSize)
        //{
        //    var query = from card in _context.CardHolderTransaction.AsNoTracking()
        //                join cardHist in _context.CardHolderTransactionHistory.AsNoTracking()
        //                    on card.TrackingNumber equals cardHist.TrackingNumber into cardHistGroup
        //                from cardHist in cardHistGroup.DefaultIfEmpty()   // LEFT JOIN

        //                join transfer in _context.TransferCardTransAction.AsNoTracking()
        //                    on card.TrackingNumber equals transfer.TrackingNumber into transferGroup
        //                from transfer in transferGroup.DefaultIfEmpty()   // LEFT JOIN

        //                join transferHist in _context.TransferCardTransActionHistory.AsNoTracking()
        //                    on card.TrackingNumber equals transferHist.TrackingNumber into transferHistGroup
        //                from transferHist in transferHistGroup.DefaultIfEmpty()   // LEFT JOIN

        //                select new TransactionReportVm
        //                {
        //                    // CardHolderTransaction (Primary source)
        //                    Source = "نگه دارنده",
        //                    TrackingNumber = card.TrackingNumber ?? "",
        //                    RRN = card.RRN ?? "",
        //                    Amount = card.Amount,
        //                    Status = card.Status,
        //                    TransactionDate = card.TransactionDate,
        //                    ApprovalCode = card.ApprovalCode ?? "",
        //                    SourcePan = card.SourcePan ?? "",
        //                    DestinationPan = card.DestinationPan ?? "",
        //                    TerminalNumber = card.TerminalNumber ?? "",
        //                    AcceptorCode = card.AcceptorCode ?? "",
        //                    ReferenceNumber = card.ReferenceNumber ?? "",
        //                    TerminalType = card.TerminalType ?? 0,
        //                    Localization = card.Localization ?? 0,
        //                    SourceAddress = card.SourceAddress ?? "",
        //                    RegistrationDate = card.RegistrationDate ?? 0,
        //                    AdditionalResponseData = card.AdditionalResponseData ?? "",
        //                    CardHolderName = card.CardHolderName ?? "",
        //                    TwoPhaseReferenceCode = card.TwoPhaseReferenceCode ?? "",

        //                    // From History
        //                    Error_Json = cardHist != null ? "" : "", // or map specific fields if needed

        //                    // Transfer data (you can add more fields to Vm if needed)
        //                    TransferAmount = transfer != null ? transfer.Amount : null,
        //                    TransferStatus = transfer != null ? transfer.Status : null,
        //                    TransferErrorJson = transfer != null ? transfer.ErrorJson : null,
        //                    TransferTerminalType = transfer != null && transfer.TerminalType.HasValue
        //                        ? Convert.ToInt32(transfer.TerminalType.Value) : 0,
        //                    // Transfer History
        //                    TransferHistErrorJson = transferHist != null ? transferHist.ErrorJson : null,

        //                    // You can add more fields from each table as needed
        //                };

        //    // Optional: Add ordering
        //    query = query.OrderByDescending(x => x.TransactionDate);

        //    var totalCount = await query.CountAsync();

        //    var items = await query
        //        .Skip((Page - 1) * PageSize)
        //        .Take(PageSize)
        //        .ToListAsync();

        //    return new PagedResult<TransactionReportVm>
        //    {
        //        Items = items,
        //        TotalCount = totalCount
        //    };

        //    // var clientConfig = _context.ClientConfig.AsNoTracking();
        //    // var cardDataQuery = _context.CardHolderTransaction
        //    //.AsNoTracking()
        //    //.Select(x => new TransactionReportVm
        //    //{
        //    //    Source = "نگه دارنده",
        //    //    TrackingNumber = x.TrackingNumber ?? "",
        //    //    RRN = x.RRN ?? "",
        //    //    Amount = x.Amount,
        //    //    Status = x.Status,
        //    //    TransactionDate = x.TransactionDate,
        //    //    ApprovalCode = x.ApprovalCode ?? "",
        //    //    SourcePan = x.SourcePan ?? "",
        //    //    DestinationPan = x.DestinationPan ?? "",
        //    //    TerminalNumber = x.TerminalNumber ?? "",
        //    //    AcceptorCode = x.AcceptorCode ?? "",
        //    //    ReferenceNumber = x.ReferenceNumber ?? "",
        //    //    TerminalType = x.TerminalType ?? 0,
        //    //    Localization = x.Localization ?? 0,
        //    //    SourceAddress = x.SourceAddress ?? "",
        //    //    RegistrationDate = x.RegistrationDate ?? 0,
        //    //    AdditionalResponseData = x.AdditionalResponseData ?? "",
        //    //    CardHolderName = x.CardHolderName ?? "",
        //    //    TwoPhaseReferenceCode = x.TwoPhaseReferenceCode ?? "",
        //    //    Error_Json = ""
        //    //});

        //    // var cardHistoryQuery = _context.CardHolderTransactionHistory
        //    //     .AsNoTracking()
        //    //     .Select(x => new TransactionReportVm
        //    //     {
        //    //         Source = "نگه دارنده",
        //    //         TrackingNumber = x.TrackingNumber ?? "",
        //    //         RRN = x.RRN ?? "",
        //    //         Amount = x.Amount,
        //    //         Status = x.Status,
        //    //         TransactionDate = x.TransactionDate,
        //    //         ApprovalCode = x.ApprovalCode ?? "",
        //    //         SourcePan = x.SourcePan ?? "",
        //    //         DestinationPan = x.DestinationPan ?? "",
        //    //         TerminalNumber = x.TerminalNumber ?? "",
        //    //         AcceptorCode = "",                    // not available
        //    //         ReferenceNumber = x.ReferenceNumber ?? "",
        //    //         TerminalType = x.TerminalType ?? 0,
        //    //         Localization = 0,                     // not available
        //    //         SourceAddress = "",                   // not available
        //    //         RegistrationDate = x.RegistrationDate ?? 0,
        //    //         AdditionalResponseData = "",          // not available
        //    //         CardHolderName = x.CardHolderName ?? "",
        //    //         TwoPhaseReferenceCode = "",           // not available
        //    //         Error_Json = ""
        //    //     });

        //    // var transferQuery = _context.TransferCardTransAction
        //    //     .AsNoTracking()
        //    //     .Select(x => new TransactionReportVm
        //    //     {
        //    //         Source = "واریز کننده",
        //    //         TrackingNumber = x.TrackingNumber ?? "",
        //    //         RRN = x.RRN ?? "",
        //    //         Amount = x.Amount ?? 0m,
        //    //         Status = x.Status ?? 0,
        //    //         TransactionDate = x.TransactionDate ?? 0,
        //    //         ApprovalCode = x.ApprovalCode ?? "",
        //    //         SourcePan = "",                       // adjust if available
        //    //         DestinationPan = "",                  // adjust if available
        //    //         TerminalNumber = "",
        //    //         AcceptorCode = "",
        //    //         ReferenceNumber = "",
        //    //         TerminalType = x.TerminalType.HasValue ? Convert.ToInt32(x.TerminalType.Value) : 0,
        //    //         Localization = 0,
        //    //         SourceAddress = "",
        //    //         RegistrationDate = x.RegistrationDate ?? 0,
        //    //         AdditionalResponseData = "",
        //    //         CardHolderName = "",
        //    //         TwoPhaseReferenceCode = "",
        //    //         Error_Json = x.ErrorJson ?? ""
        //    //     });

        //    // var transferHistoryQuery = _context.TransferCardTransActionHistory
        //    //     .AsNoTracking()
        //    //     .Select(x => new TransactionReportVm
        //    //     {
        //    //         Source = "واریز کننده",
        //    //         TrackingNumber = x.TrackingNumber ?? "",
        //    //         RRN = x.RRN ?? "",
        //    //         Amount = x.Amount ?? 0m,
        //    //         Status = x.Status ?? 0,
        //    //         TransactionDate = x.TransactionDate ?? 0,
        //    //         ApprovalCode = x.ApprovalCode ?? "",
        //    //         SourcePan = "",
        //    //         DestinationPan = "",
        //    //         TerminalNumber = "",
        //    //         AcceptorCode = "",
        //    //         ReferenceNumber = "",
        //    //         TerminalType = x.TerminalType.HasValue ? Convert.ToInt32(x.TerminalType.Value) : 0,
        //    //         Localization = 0,
        //    //         SourceAddress = "",
        //    //         RegistrationDate = x.RegistrationDate ?? 0,
        //    //         AdditionalResponseData = "",
        //    //         CardHolderName = "",
        //    //         TwoPhaseReferenceCode = "",
        //    //         Error_Json = x.ErrorJson ?? ""
        //    //     });

        //    // // Combine at DB level (one query)
        //    // var allDataQuery = cardDataQuery
        //    //     .Concat(cardHistoryQuery)
        //    //     .Concat(transferQuery)
        //    //     .Concat(transferHistoryQuery);

        //    // // Optional: Add ordering here
        //    // allDataQuery = allDataQuery.OrderByDescending(x => x.TransactionDate);

        //    // var totalCount = await allDataQuery.CountAsync();

        //    // var items = await allDataQuery
        //    //     .Skip((Page - 1) * PageSize)
        //    //     .Take(PageSize)
        //    //     .ToListAsync();

        //    // return new PagedResult<TransactionReportVm>
        //    // {
        //    //     Items = items,
        //    //     TotalCount = totalCount
        //    // };
        //}

        public async Task<PagedResult<TransactionReportVm>> Search(TransactionSearchRequest req)
        {
            var cutoffMillis = DateTimeOffset.UtcNow.AddDays(-3).ToUnixTimeMilliseconds();

            long? fromMillis = null;
            long? toMillis = null;

            if (req.FromDate.HasValue)
            {
                var fromDate = DateTime.SpecifyKind(req.FromDate.Value.Date, DateTimeKind.Utc);
                fromMillis = new DateTimeOffset(fromDate).ToUnixTimeMilliseconds();
            }

            if (req.ToDate.HasValue)
            {
                if (req.FromDate.HasValue && req.FromDate.Value.Date == req.ToDate.Value.Date)
                {
                    // Same day → search full 24 hours
                    var endOfDay = DateTime.SpecifyKind(req.ToDate.Value.Date.AddDays(1).AddMilliseconds(-1), DateTimeKind.Utc);
                    toMillis = new DateTimeOffset(endOfDay).ToUnixTimeMilliseconds();
                }
                else
                {
                    // Different days or only ToDate → end of ToDate
                    var endOfDay = DateTime.SpecifyKind(req.ToDate.Value.Date.AddDays(1).AddMilliseconds(-1), DateTimeKind.Utc);
                    toMillis = new DateTimeOffset(endOfDay).ToUnixTimeMilliseconds();
                }
            }
            bool IsCardFiltered = false;
            // Decide which tables to search
            bool searchCurrent = true;
            bool searchHistory = false;

            if (toMillis.HasValue && toMillis.Value < cutoffMillis)
            {
                searchCurrent = false;
                searchHistory = true;
            }
            else if (fromMillis.HasValue && fromMillis.Value < cutoffMillis)
            {
                searchCurrent = true;
                searchHistory = true;
            }

            IQueryable<TransactionReportVm> query = null;

            // =====================================================
            // CURRENT DATA
            // =====================================================
            if (searchCurrent)
            {
                var cardQuery = _context.CardHolderTransaction
                  .AsNoTracking()
                  .Select(x => new TransactionReportVm
                  {
                      Source = "نگه دارنده",
                      TrackingNumber = x.TrackingNumber ?? "",
                      RRN = x.RRN ?? "",
                      Amount = x.Amount,
                      Status = x.Status,
                      TransactionDate = x.TransactionDate,
                      ApprovalCode = x.ApprovalCode ?? "",
                      SourcePan = x.SourcePan ?? "",
                      DestinationPan = x.DestinationPan ?? "",
                      TerminalNumber = x.TerminalNumber ?? "",
                      AcceptorCode = x.AcceptorCode ?? "",
                      ReferenceNumber = x.ReferenceNumber ?? "",
                      TerminalType = x.TerminalType ?? 0,
                      Localization = x.Localization ?? 0,
                      SourceAddress = x.SourceAddress ?? "",
                      RegistrationDate = x.RegistrationDate ?? 0,
                      AdditionalResponseData = x.AdditionalResponseData ?? "",
                      CardHolderName = x.CardHolderName ?? "",
                      TwoPhaseReferenceCode = x.TwoPhaseReferenceCode ?? "",
                      Error_Json = ""
                  });
                var transferQuery = _context.TransferCardTransAction
                       .AsNoTracking()
                       .Select(x => new TransactionReportVm
                       {
                           Source = "واریز کننده",
                           TrackingNumber = x.TrackingNumber ?? "",
                           RRN = x.RRN ?? "",
                           Amount = x.Amount ?? 0m,
                           Status = x.Status ?? 0,
                           TransactionDate = x.TransactionDate ?? 0,
                           ApprovalCode = x.ApprovalCode ?? "",
                           SourcePan = "",                       // adjust if available
                           DestinationPan = "",                  // adjust if available
                           TerminalNumber = "",
                           AcceptorCode = "",
                           ReferenceNumber = "",
                           TerminalType = x.TerminalType.HasValue ? Convert.ToInt32(x.TerminalType.Value) : 0,
                           Localization = 0,
                           SourceAddress = "",
                           RegistrationDate = x.RegistrationDate ?? 0,
                           AdditionalResponseData = "",
                           CardHolderName = "",
                           TwoPhaseReferenceCode = "",
                           Error_Json = x.ErrorJson ?? ""
                       });


                if (!string.IsNullOrWhiteSpace(req.TrackingNumber))
                {
                    cardQuery = cardQuery.Where(x => x.TrackingNumber.Contains(req.TrackingNumber));
                    transferQuery = transferQuery.Where(x => x.TrackingNumber.Contains(req.TrackingNumber));
                }

                if (!string.IsNullOrWhiteSpace(req.RRN))
                {
                    cardQuery = cardQuery.Where(x => x.RRN.Contains(req.RRN));
                    transferQuery = transferQuery.Where(x => x.RRN.Contains(req.RRN));
                }

                if (req.Status.HasValue)
                {
                    cardQuery = cardQuery.Where(x => x.Status == req.Status);
                    transferQuery = transferQuery.Where(x => x.Status == req.Status);
                }

                if (req.AmountFrom.HasValue)
                {
                    cardQuery = cardQuery.Where(x => x.Amount >= req.AmountFrom.Value);
                    transferQuery = transferQuery.Where(x => x.Amount >= req.AmountFrom.Value);
                }

                if (req.AmountTo.HasValue)
                {
                    cardQuery = cardQuery.Where(x => x.Amount <= req.AmountTo.Value);
                    transferQuery = transferQuery.Where(x => x.Amount <= req.AmountTo.Value);
                }

                if (fromMillis.HasValue)
                {
                    cardQuery = cardQuery.Where(x => x.TransactionDate >= fromMillis.Value);
                    transferQuery = transferQuery.Where(x => x.TransactionDate >= fromMillis.Value);
                    if(cardQuery.Count() > 0)
                    {
                        IsCardFiltered = true;
                    }
                }
                if (toMillis.HasValue && !IsCardFiltered)
                {
                    cardQuery = cardQuery.Where(x => x.TransactionDate <= toMillis.Value);
                    transferQuery = transferQuery.Where(x => x.TransactionDate <= toMillis.Value);
                }

                var currentCards = cardQuery.Select(x => new TransactionReportVm
                {
                    Source = "نگه دارنده",
                    TrackingNumber = x.TrackingNumber != null ? x.TrackingNumber : "",
                    RRN = x.RRN != null ? x.RRN : "",
                    Amount = x.Amount != null ? x.Amount : 0,
                    Status = x.Status != null ? x.Status : 0,
                    TransactionDate = x.TransactionDate,
                    ApprovalCode = x.ApprovalCode !=null  ? x.ApprovalCode : "",

                    AcceptorCode = x.AcceptorCode != null ? x.AcceptorCode : "",
                    AdditionalResponseData = x.AdditionalResponseData != null ? x.AdditionalResponseData : "",
                    CardHolderName = x.CardHolderName != null ? x.CardHolderName : "",
                    DestinationPan = x.DestinationPan != null ? x.DestinationPan : "",
                    SourceAddress = x.SourceAddress != null ? x.SourceAddress : "",
                    Localization = x.Localization.HasValue ? x.Localization.Value : 1,
                    ReferenceNumber = x.ReferenceNumber != null ? x.ReferenceNumber : "",
                    SourcePan = x.SourcePan != null ? x.SourcePan : "",
                    Error_Json = x.Error_Json != null ? x.Error_Json : "",
                    RegistrationDate = x.RegistrationDate.HasValue ? x.RegistrationDate.Value : 1,
                    TerminalNumber = x.TerminalNumber != null ? x.TerminalNumber : "",
                    TerminalType = x.TerminalType.HasValue ? x.TerminalType.Value : 1,
                    TwoPhaseReferenceCode = x.TwoPhaseReferenceCode != null ? x.TwoPhaseReferenceCode : "",

                }); 

                var currentTransfers = transferQuery.Select(x => new TransactionReportVm
                {
                    Source = "واریز کننده",
                    TrackingNumber = x.TrackingNumber != null ?x.TrackingNumber : "",
                    RRN = x.RRN != null ? x.RRN : "",
                    Amount = x.Amount != null ? x.Amount: 0,
                    Status = x.Status != null ? x.Status : 0,
                    TransactionDate = x.TransactionDate != null ? x.TransactionDate : 0,
                    ApprovalCode = x.ApprovalCode != null ? x.ApprovalCode : "",
                    // MUST MATCH FULL SHAPE
                    AcceptorCode = "",
                    AdditionalResponseData = "",
                    CardHolderName = "",
                    DestinationPan = "",
                    SourceAddress = "",
                    Localization = 0,
                    ReferenceNumber = "",
                    SourcePan = "",
                    Error_Json = "",
                    RegistrationDate = 0,
                    TerminalNumber = "",
                    TerminalType = 0,
                    TwoPhaseReferenceCode = ""
                });

                query = currentCards.Concat(currentTransfers);
            }

            // =====================================================
            // HISTORY DATA
            // =====================================================
            if (searchHistory)
            {
                var cardQuery = _context.CardHolderTransaction
                   .AsNoTracking()
                   .Select(x => new TransactionReportVm
                   {
                       Source = "نگه دارنده",
                       TrackingNumber = x.TrackingNumber ?? "",
                       RRN = x.RRN ?? "",
                       Amount = x.Amount,
                       Status = x.Status,
                       TransactionDate = x.TransactionDate,
                       ApprovalCode = x.ApprovalCode ?? "",
                       SourcePan = x.SourcePan ?? "",
                       DestinationPan = x.DestinationPan ?? "",
                       TerminalNumber = x.TerminalNumber ?? "",
                       AcceptorCode = x.AcceptorCode ?? "",
                       ReferenceNumber = x.ReferenceNumber ?? "",
                       TerminalType = x.TerminalType ?? 0,
                       Localization = x.Localization ?? 0,
                       SourceAddress = x.SourceAddress ?? "",
                       RegistrationDate = x.RegistrationDate ?? 0,
                       AdditionalResponseData = x.AdditionalResponseData ?? "",
                       CardHolderName = x.CardHolderName ?? "",
                       TwoPhaseReferenceCode = x.TwoPhaseReferenceCode ?? "",
                       Error_Json = ""
                   });

                var cardHistoryQuery = _context.CardHolderTransactionHistory
                    .AsNoTracking()
                    .Select(x => new TransactionReportVm
                    {
                        Source = "نگه دارنده",
                        TrackingNumber = x.TrackingNumber ?? "",
                        RRN = x.RRN ?? "",
                        Amount = x.Amount,
                        Status = x.Status,
                        TransactionDate = x.TransactionDate,
                        ApprovalCode = x.ApprovalCode ?? "",
                        SourcePan = x.SourcePan ?? "",
                        DestinationPan = x.DestinationPan ?? "",
                        TerminalNumber = x.TerminalNumber ?? "",
                        AcceptorCode = "",                    // not available
                        ReferenceNumber = x.ReferenceNumber ?? "",
                        TerminalType = x.TerminalType ?? 0,
                        Localization = 0,                     // not available
                        SourceAddress = "",                   // not available
                        RegistrationDate = x.RegistrationDate ?? 0,
                        AdditionalResponseData = "",          // not available
                        CardHolderName = x.CardHolderName ?? "",
                        TwoPhaseReferenceCode = "",           // not available
                        Error_Json = ""
                    });

                var transferQuery = _context.TransferCardTransAction
                    .AsNoTracking()
                    .Select(x => new TransactionReportVm
                    {
                        Source = "واریز کننده",
                        TrackingNumber = x.TrackingNumber ?? "",
                        RRN = x.RRN ?? "",
                        Amount = x.Amount ?? 0m,
                        Status = x.Status ?? 0,
                        TransactionDate = x.TransactionDate ?? 0,
                        ApprovalCode = x.ApprovalCode ?? "",
                        SourcePan = "",                       // adjust if available
                        DestinationPan = "",                  // adjust if available
                        TerminalNumber = "",
                        AcceptorCode = "",
                        ReferenceNumber = "",
                        TerminalType = x.TerminalType.HasValue ? Convert.ToInt32(x.TerminalType.Value) : 0,
                        Localization = 0,
                        SourceAddress = "",
                        RegistrationDate = x.RegistrationDate ?? 0,
                        AdditionalResponseData = "",
                        CardHolderName = "",
                        TwoPhaseReferenceCode = "",
                        Error_Json = x.ErrorJson ?? ""
                    });

                var transferHistoryQuery = _context.TransferCardTransActionHistory
                    .AsNoTracking()
                    .Select(x => new TransactionReportVm
                    {
                        Source = "واریز کننده",
                        TrackingNumber = x.TrackingNumber ?? "",
                        RRN = x.RRN ?? "",
                        Amount = x.Amount ?? 0m,
                        Status = x.Status ?? 0,
                        TransactionDate = x.TransactionDate ?? 0,
                        ApprovalCode = x.ApprovalCode ?? "",
                        SourcePan = "",
                        DestinationPan = "",
                        TerminalNumber = "",
                        AcceptorCode = "",
                        ReferenceNumber = "",
                        TerminalType = x.TerminalType.HasValue ? Convert.ToInt32(x.TerminalType.Value) : 0,
                        Localization = 0,
                        SourceAddress = "",
                        RegistrationDate = x.RegistrationDate ?? 0,
                        AdditionalResponseData = "",
                        CardHolderName = "",
                        TwoPhaseReferenceCode = "",
                        Error_Json = x.ErrorJson ?? ""
                    });

                if (!string.IsNullOrWhiteSpace(req.TrackingNumber))
                {
                    cardQuery = cardQuery.Where(x => x.TrackingNumber.Contains(req.TrackingNumber));
                    transferQuery = transferQuery.Where(x => x.TrackingNumber.Contains(req.TrackingNumber));
                    cardHistoryQuery = cardHistoryQuery.Where(x => x.TrackingNumber.Contains(req.TrackingNumber));
                    transferHistoryQuery = transferHistoryQuery.Where(x => x.TrackingNumber.Contains(req.TrackingNumber));
                }

                if (!string.IsNullOrWhiteSpace(req.RRN))
                {
                    cardQuery = cardQuery.Where(x => x.RRN.Contains(req.RRN));
                    transferQuery = transferQuery.Where(x => x.RRN.Contains(req.RRN));
                    cardHistoryQuery = cardHistoryQuery.Where(x => x.RRN.Contains(req.RRN));
                    transferHistoryQuery = transferHistoryQuery.Where(x => x.RRN.Contains(req.RRN));
                }

                if (req.Status.HasValue)
                {
                    cardQuery = cardQuery.Where(x => x.Status == req.Status);
                    transferQuery = transferQuery.Where(x => x.Status == req.Status);
                    cardHistoryQuery = cardHistoryQuery.Where(x => x.Status == req.Status);
                    transferHistoryQuery = transferHistoryQuery.Where(x => x.Status == req.Status);
                }

                if (req.AmountFrom.HasValue)
                {
                    cardQuery = cardQuery.Where(x => x.Amount >= req.AmountFrom.Value);
                    transferQuery = transferQuery.Where(x => x.Amount >= req.AmountFrom.Value);
                    cardHistoryQuery = cardHistoryQuery.Where(x => x.Amount >= req.AmountFrom.Value);
                    transferHistoryQuery = transferHistoryQuery.Where(x => x.Amount >= req.AmountFrom.Value);
                }

                if (req.AmountTo.HasValue)
                {
                    cardQuery = cardQuery.Where(x => x.Amount <= req.AmountTo.Value);
                    transferQuery = transferQuery.Where(x => x.Amount <= req.AmountTo.Value);
                    cardHistoryQuery = cardHistoryQuery.Where(x => x.Amount <= req.AmountTo.Value);
                    transferHistoryQuery = transferHistoryQuery.Where(x => x.Amount <= req.AmountTo.Value);
                }

                if (fromMillis.HasValue)
                {
                    cardQuery = cardQuery.Where(x => x.TransactionDate >= fromMillis.Value);
                    transferQuery = transferQuery.Where(x => x.TransactionDate >= fromMillis.Value);
                    cardHistoryQuery = cardHistoryQuery.Where(x => x.TransactionDate >= fromMillis.Value);
                    transferHistoryQuery = transferHistoryQuery.Where(x => x.TransactionDate >= fromMillis.Value);
                    if (cardQuery.Count() > 0 || 
                        cardHistoryQuery.Count() > 0 || 
                        cardHistoryQuery.Count() > 0 || 
                        transferHistoryQuery.Count() > 0
                        )
                    {
                        IsCardFiltered = true;
                    }
                }

                if (toMillis.HasValue && !IsCardFiltered)
                {
                    cardQuery = cardQuery.Where(x => x.TransactionDate <= toMillis.Value);
                    transferQuery = transferQuery.Where(x => x.TransactionDate <= toMillis.Value);
                    cardHistoryQuery = cardHistoryQuery.Where(x => x.TransactionDate <= toMillis.Value);
                    transferHistoryQuery = transferHistoryQuery.Where(x => x.TransactionDate <= toMillis.Value);
                }

                var historyCards = cardHistoryQuery.Select(x => new TransactionReportVm
                {
                    Source = "نگه دارنده",
                    TrackingNumber = x.TrackingNumber,
                    RRN = x.RRN,
                    Amount = x.Amount,
                    Status = x.Status,
                    TransactionDate = x.TransactionDate,
                    ApprovalCode = x.ApprovalCode ?? "",

                    AcceptorCode = x.AcceptorCode ?? "",
                    AdditionalResponseData = x.AdditionalResponseData ?? "",
                    CardHolderName = x.CardHolderName ?? "",
                    DestinationPan = x.DestinationPan ?? "",
                    SourceAddress = x.SourceAddress ?? "",
                    Localization = x.Localization.HasValue ? x.Localization.Value : 1,
                    ReferenceNumber = x.ReferenceNumber ?? "",
                    SourcePan = x.SourcePan ?? "",
                    Error_Json = "",
                    RegistrationDate = x.RegistrationDate.HasValue ? x.RegistrationDate.Value : 1,
                    TerminalNumber = x.TerminalNumber ?? "",
                    TerminalType = x.TerminalType.HasValue ? x.TerminalType.Value : 1,
                    TwoPhaseReferenceCode = x.TwoPhaseReferenceCode ?? ""
                });

                var historyTransfers = transferHistoryQuery.Select(x => new TransactionReportVm
                {
                    Source = "واریز کننده",
                    TrackingNumber = x.TrackingNumber,
                    RRN = x.RRN,
                    Amount = x.Amount,
                    Status = x.Status,
                    TransactionDate = x.TransactionDate,
                    ApprovalCode = x.ApprovalCode ?? "",

                    AcceptorCode = "",
                    AdditionalResponseData = "",
                    CardHolderName = "",
                    DestinationPan = "",
                    SourceAddress = "",
                    Localization = 0,
                    ReferenceNumber = "",
                    SourcePan = "",
                    Error_Json = "",
                    RegistrationDate = 0,
                    TerminalNumber = "",
                    TerminalType = 0,
                    TwoPhaseReferenceCode = ""
                });

                var historyData = historyCards.Concat(historyTransfers);

                query = query == null ? historyData : query.Concat(historyData);
            }

            // =====================================================
            // EXECUTE
            // =====================================================
            if (query == null)
            {
                query = Enumerable.Empty<TransactionReportVm>().AsQueryable();
            }

            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(x => x.TransactionDate)
                .Skip((req.PageNumber - 1) * req.PageSize)
                .Take(req.PageSize)
                .ToListAsync();

            return new PagedResult<TransactionReportVm>
            {
                Items = items,
                TotalCount = total
            };
        }
        public string FixPersian(string text)
        {
            if (string.IsNullOrEmpty(text)) return "";
            return iTextSharp.text.pdf.BidiLine.ProcessLTR(text, 0, 1);
        }
        public string Escape(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";

            // Escape double quotes and wrap in quotes if needed
            string escaped = value.Replace("\"", "\"\"");

            if (escaped.Contains(",") || escaped.Contains("\"") || escaped.Contains("\n"))
            {
                escaped = $"\"{escaped}\"";
            }

            return escaped;
        }

        public PdfPCell CreateCell(string text, Font font)
        {
            var cell = new PdfPCell(new Phrase(text, font));
            cell.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Padding = 5;
            return cell;
        }
    }
}
