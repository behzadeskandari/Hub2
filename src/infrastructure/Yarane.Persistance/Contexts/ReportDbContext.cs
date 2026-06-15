using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Yarane.Domain.Entities;
using System;

namespace Yarane.Persistance.Contexts
{

    public class ReportDbContext : DbContext
    {
        public ReportDbContext(DbContextOptions<ReportDbContext> options) : base(options) { }

        public DbSet<CardHolderTransaction> CardHolderTransaction { get; set; }
        public DbSet<CardHolderTransactionHistory> CardHolderTransactionHistory { get; set; }
        public DbSet<TransferCardTransAction> TransferCardTransAction { get; set; }
        public DbSet<TransferCardTransActionHistory> TransferCardTransActionHistory { get; set; }
        public DbSet<ClientConfig> ClientConfig { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureCardHolderTransaction(modelBuilder);
            ConfigureCardHolderTransactionHistory(modelBuilder);
            ConfigureTransferCardTransAction(modelBuilder);
            ConfigureTransferCardTransActionHistory(modelBuilder);
            ConfigureClientConfig(modelBuilder);

        }

        private void ConfigureCardHolderTransaction(ModelBuilder mb)
        {
            mb.Entity<CardHolderTransaction>().ToTable("CARDHOLDER_TRANSACTION", "APIGW");

            mb.Entity<CardHolderTransaction>().Property(e => e.Id).HasColumnName("ID");
            mb.Entity<CardHolderTransaction>().Property(e => e.TrackingNumber).HasColumnName("TRACKING_NUMBER");
            mb.Entity<CardHolderTransaction>().Property(e => e.SourcePan).HasColumnName("SOURCE_PAN");
            mb.Entity<CardHolderTransaction>().Property(e => e.DestinationPan).HasColumnName("DESTINATION_PAN");
            mb.Entity<CardHolderTransaction>().Property(e => e.TerminalNumber).HasColumnName("TERMINAL_NUMBER");
            mb.Entity<CardHolderTransaction>().Property(e => e.AcceptorCode).HasColumnName("ACCEPTOR_CODE");
            mb.Entity<CardHolderTransaction>().Property(e => e.Amount).HasColumnName("AMOUNT").HasColumnType("NUMBER(19,4)");
            mb.Entity<CardHolderTransaction>().Property(e => e.ReferenceNumber).HasColumnName("REFERENCE_NUMBER");
            mb.Entity<CardHolderTransaction>().Property(e => e.TerminalType).HasColumnName("TERMINAL_TYPE").HasColumnType("NUMBER(2)");
            mb.Entity<CardHolderTransaction>().Property(e => e.Localization).HasColumnName("LOCALIZATION").HasColumnType("NUMBER(2)");
            mb.Entity<CardHolderTransaction>().Property(e => e.SourceAddress).HasColumnName("SOURCE_ADDRESS");
            mb.Entity<CardHolderTransaction>().Property(e => e.RegistrationDate).HasColumnName("REGISTRATION_DATE").HasColumnType("NUMBER(14)");
            mb.Entity<CardHolderTransaction>().Property(e => e.TransactionDate).HasColumnName("TRANSACTION_DATE").HasColumnType("NUMBER(14)");
            mb.Entity<CardHolderTransaction>().Property(e => e.RRN).HasColumnName("RRN");
            mb.Entity<CardHolderTransaction>().Property(e => e.AdditionalResponseData).HasColumnName("ADDITIONAL_RESPONSE_DATA");
            mb.Entity<CardHolderTransaction>().Property(e => e.CardHolderName).HasColumnName("CARD_HOLDER_NAME");
            mb.Entity<CardHolderTransaction>().Property(e => e.ApprovalCode).HasColumnName("APPROVAL_CODE");
            mb.Entity<CardHolderTransaction>().Property(e => e.Status).HasColumnName("STATUS").HasColumnType("NUMBER(5)");
            mb.Entity<CardHolderTransaction>().Property(e => e.TwoPhaseReferenceCode).HasColumnName("TWO_PHASE_REFERENCE_CODE");
        }

        private void ConfigureCardHolderTransactionHistory(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CardHolderTransactionHistory>()
                .ToTable("CARDHOLDER_TRANSACTION_HISTORY", "APIGW");

            modelBuilder.Entity<CardHolderTransactionHistory>().Property(e => e.Id).HasColumnName("ID");
            modelBuilder.Entity<CardHolderTransactionHistory>().Property(e => e.TrackingNumber).HasColumnName("TRACKING_NUMBER");
            modelBuilder.Entity<CardHolderTransactionHistory>().Property(e => e.SourcePan).HasColumnName("SOURCE_PAN");
            modelBuilder.Entity<CardHolderTransactionHistory>().Property(e => e.DestinationPan).HasColumnName("DESTINATION_PAN");
            modelBuilder.Entity<CardHolderTransactionHistory>().Property(e => e.TerminalNumber).HasColumnName("TERMINAL_NUMBER");
            modelBuilder.Entity<CardHolderTransactionHistory>().Property(e => e.AcceptorCode).HasColumnName("ACCEPTOR_CODE");
            modelBuilder.Entity<CardHolderTransactionHistory>().Property(e => e.Amount).HasColumnName("AMOUNT").HasColumnType("NUMBER(19,4)");
            modelBuilder.Entity<CardHolderTransactionHistory>().Property(e => e.ReferenceNumber).HasColumnName("REFERENCE_NUMBER");
            modelBuilder.Entity<CardHolderTransactionHistory>().Property(e => e.TerminalType).HasColumnName("TERMINAL_TYPE").HasColumnType("NUMBER(2)");
            modelBuilder.Entity<CardHolderTransactionHistory>().Property(e => e.Localization).HasColumnName("LOCALIZATION").HasColumnType("NUMBER(2)");
            modelBuilder.Entity<CardHolderTransactionHistory>().Property(e => e.SourceAddress).HasColumnName("SOURCE_ADDRESS");
            modelBuilder.Entity<CardHolderTransactionHistory>().Property(e => e.RegistrationDate).HasColumnName("REGISTRATION_DATE").HasColumnType("NUMBER(14)");
            modelBuilder.Entity<CardHolderTransactionHistory>().Property(e => e.TransactionDate).HasColumnName("TRANSACTION_DATE").HasColumnType("NUMBER(14)");
            modelBuilder.Entity<CardHolderTransactionHistory>().Property(e => e.RRN).HasColumnName("RRN");
            modelBuilder.Entity<CardHolderTransactionHistory>().Property(e => e.AdditionalResponseData).HasColumnName("ADDITIONAL_RESPONSE_DATA");
            modelBuilder.Entity<CardHolderTransactionHistory>().Property(e => e.CardHolderName).HasColumnName("CARD_HOLDER_NAME");
            modelBuilder.Entity<CardHolderTransactionHistory>().Property(e => e.ApprovalCode).HasColumnName("APPROVAL_CODE");
            modelBuilder.Entity<CardHolderTransactionHistory>().Property(e => e.Status).HasColumnName("STATUS").HasColumnType("NUMBER(5)");
            modelBuilder.Entity<CardHolderTransactionHistory>().Property(e => e.TwoPhaseReferenceCode).HasColumnName("TWO_PHASE_REFERENCE_CODE");
        }

        private void ConfigureTransferCardTransAction(ModelBuilder mb)
        {
            mb.Entity<TransferCardTransAction>().ToTable("TRANSFERCARD_TRANSACTION", "APIGW");

            mb.Entity<TransferCardTransAction>().HasKey(x => x.Id);

            mb.Entity<TransferCardTransAction>()
                .Property(e => e.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            mb.Entity<TransferCardTransAction>()
                .Property(e => e.TrackingNumber)
                .HasColumnName("TRACKING_NUMBER")
                .HasColumnType("NVARCHAR2(64)")
                .IsRequired();

            mb.Entity<TransferCardTransAction>()
                .Property(e => e.SourcePan)
                .HasColumnName("SOURCE_PAN")
                .HasColumnType("NVARCHAR2(19)");

            mb.Entity<TransferCardTransAction>()
                .Property(e => e.DestinationPan)
                .HasColumnName("DESTINATION_PAN")
                .HasColumnType("NVARCHAR2(19)");

            mb.Entity<TransferCardTransAction>()
                .Property(e => e.TerminalNumber)
                .HasColumnName("TERMINAL_NUMBER")
                .HasColumnType("NVARCHAR2(10)");

            mb.Entity<TransferCardTransAction>()
                .Property(e => e.AcceptorCode)
                .HasColumnName("ACCEPTOR_CODE")
                .HasColumnType("NVARCHAR2(15)");

            mb.Entity<TransferCardTransAction>()
                .Property(e => e.Amount)
                .HasColumnName("AMOUNT")
                .HasColumnType("NUMBER(19)");

            mb.Entity<TransferCardTransAction>()
                .Property(e => e.ReferenceNumber)
                .HasColumnName("REFERENCE_NUMBER")
                .HasColumnType("NVARCHAR2(30)");

            mb.Entity<TransferCardTransAction>()
                .Property(e => e.TerminalType)
                .HasColumnName("TERMINAL_TYPE")
                .HasColumnType("NUMBER(2)");

            mb.Entity<TransferCardTransAction>()
                .Property(e => e.Localization)
                .HasColumnName("LOCALIZATION")
                .HasColumnType("NUMBER(2)");

            mb.Entity<TransferCardTransAction>()
                .Property(e => e.RegistrationDate)
                .HasColumnName("REGISTRATION_DATE")
                .HasColumnType("NUMBER(14)");

            mb.Entity<TransferCardTransAction>()
                .Property(e => e.TransactionDate)
                .HasColumnName("TRANSACTION_DATE")
                .HasColumnType("NUMBER(14)");

            mb.Entity<TransferCardTransAction>()
                .Property(e => e.Stan)
                .HasColumnName("STAN")
                .HasColumnType("NUMBER(10)");

            mb.Entity<TransferCardTransAction>()
                .Property(e => e.RRN)
                .HasColumnName("RRN")
                .HasColumnType("NVARCHAR2(12)");

            mb.Entity<TransferCardTransAction>()
                .Property(e => e.AdditionalResponseData)
                .HasColumnName("ADDITIONAL_RESPONSE_DATA")
                .HasColumnType("NVARCHAR2(100)");

            mb.Entity<TransferCardTransAction>()
                .Property(e => e.ApprovalCode)
                .HasColumnName("APPROVAL_CODE")
                .HasColumnType("NVARCHAR2(64)");

            mb.Entity<TransferCardTransAction>()
                .Property(e => e.Status)
                .HasColumnName("STATUS")
                .HasColumnType("NUMBER(2)");

            mb.Entity<TransferCardTransAction>()
                .Property(e => e.ErrorJson)
                .HasColumnName("ERRORS_JSON")
                .HasColumnType("CLOB");

            mb.Entity<TransferCardTransAction>()
                .Property(e => e.SourceAddress)
                .HasColumnName("SOURCE_ADDRESS")
                .HasColumnType("NVARCHAR2(15)");

            mb.Entity<TransferCardTransAction>()
                .Property(e => e.TransactionType)
                .HasColumnName("TRANSACTION_TYPE")
                .HasColumnType("INTEGER");

            mb.Entity<TransferCardTransAction>()
                .Property(e => e.MerchantID)
                .HasColumnName("MERCHANT_ID")
                .HasColumnType("NVARCHAR2(15)");
        }

        private void ConfigureTransferCardTransActionHistory(ModelBuilder modelBuilder)
        {
            var e = modelBuilder.Entity<TransferCardTransActionHistory>();

            e.ToTable("TRANSFERCARD_TRANSACTION_HISTORY", "APIGW");

            e.HasKey(x => x.Id);

            e.Property(x => x.Id).HasColumnName("ID");

            e.Property(x => x.TrackingNumber)
                .HasColumnName("TRACKING_NUMBER")
                .HasColumnType("NVARCHAR2(64)");

            e.Property(x => x.SourcePan)
                .HasColumnName("SOURCE_PAN")
                .HasColumnType("NVARCHAR2(19)");

            e.Property(x => x.DestinationPan)
                .HasColumnName("DESTINATION_PAN")
                .HasColumnType("NVARCHAR2(19)");

            e.Property(x => x.TerminalNumber)
                .HasColumnName("TERMINAL_NUMBER")
                .HasColumnType("NVARCHAR2(10)");

            e.Property(x => x.AcceptorCode)
                .HasColumnName("ACCEPTOR_CODE")
                .HasColumnType("NVARCHAR2(15)");

            e.Property(x => x.Amount)
                .HasColumnName("AMOUNT")
                .HasColumnType("NUMBER(19,4)");

            e.Property(x => x.ReferenceNumber)
                .HasColumnName("REFERENCE_NUMBER")
                .HasColumnType("NVARCHAR2(30)");

            e.Property(x => x.TerminalType)
                .HasColumnName("TERMINAL_TYPE")
                .HasColumnType("NUMBER(2)");

            e.Property(x => x.Localization)
                .HasColumnName("LOCALIZATION")
                .HasColumnType("NUMBER(2)");

            e.Property(x => x.RegistrationDate)
                .HasColumnName("REGISTRATION_DATE")
                .HasColumnType("NUMBER(14)");

            e.Property(x => x.TransactionDate)
                .HasColumnName("TRANSACTION_DATE")
                .HasColumnType("NUMBER(14)");

            e.Property(x => x.Stan)
                .HasColumnName("STAN")
                .HasColumnType("NUMBER(10)");

            e.Property(x => x.RRN)
                .HasColumnName("RRN")
                .HasColumnType("NVARCHAR2(12)");

            e.Property(x => x.AdditionalResponseData)
                .HasColumnName("ADDITIONAL_RESPONSE_DATA")
                .HasColumnType("NVARCHAR2(100)");

            e.Property(x => x.ApprovalCode)
                .HasColumnName("APPROVAL_CODE")
                .HasColumnType("NVARCHAR2(64)");

            e.Property(x => x.Status)
                .HasColumnName("STATUS")
                .HasColumnType("NUMBER(5)");

            e.Property(x => x.ErrorJson)
                .HasColumnName("ERRORS_JSON")
                .HasColumnType("CLOB");

            e.Property(x => x.SourceAddress)
                .HasColumnName("SOURCE_ADDRESS")
                .HasColumnType("NVARCHAR2(15)");

            e.Property(x => x.TransactionType)
                .HasColumnName("TRANSACTION_TYPE")
                .HasColumnType("INTEGER");

            e.Property(x => x.MerchantID)
                .HasColumnName("MERCHANT_ID")
                .HasColumnType("NVARCHAR2(15)");
        }

        private void ConfigureClientConfig(ModelBuilder mb)
        {
            mb.Entity<ClientConfig>().ToTable("CLIENT_CONFIG", "APIGW");
            mb.Entity<ClientConfig>().Property(e => e.Id).HasColumnName("ID");
            mb.Entity<ClientConfig>().Property(e => e.ConsumerName).HasColumnName("CONSUMER_NAME");
            mb.Entity<ClientConfig>().Property(e => e.TerminalNumber).HasColumnName("TERMINAL_NUMBER");
            mb.Entity<ClientConfig>().Property(e => e.TerminalType).HasColumnName("TERMINAL_TYPE");
            mb.Entity<ClientConfig>().Property(e => e.AcceptorCode).HasColumnName("ACCEPTOR_CODE");
        }
    }
}
