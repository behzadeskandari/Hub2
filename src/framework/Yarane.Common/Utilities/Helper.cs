using CsvHelper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;


namespace Yarane.Common.Utilities
{
    public class Helper
    {
        private readonly IConfiguration _configuration;

        public Helper(
            IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ComputeSHA256Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(bytes);
                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        public string SaveCsv<T>(List<T> dto, string uploadAddress)
            where T : class
        {
            var models = dto.ToList();

            var csvData = ToCsv(models);

            var fileName = "people.csv";

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", fileName);

            File.WriteAllText(filePath, csvData);

            return fileName;
        }

        public string ToCsv<T>(IEnumerable<T> model)
            where T : class
        {
            using (var writer = new StringWriter())
            using (var csv = new CsvWriter(writer, CultureInfo.CurrentCulture))
            {
                csv.WriteRecords(model);
                return writer.ToString();
            }
        }

        public string CheckSha256SumOfFile(string filePath)
        {
            using (var sha256 = SHA256.Create())
            {
                using (var fileStream = File.OpenRead(filePath))
                {
                    byte[] hashBytes = sha256.ComputeHash(fileStream);
                    string hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                    return hashString;
                }
            }
        }
        public bool ValidateLuhn(string number)
        {
            int sum = 0;
            bool alternate = false;

            for (int i = number.Length - 1; i >= 0; i--)
            {
                int digit = int.Parse(number[i].ToString());

                if (alternate)
                {
                    digit *= 2;
                    if (digit > 9)
                    {
                        digit -= 9;
                    }
                }

                sum += digit;
                alternate = !alternate;
            }

            return sum % 10 == 0;
        }

        public bool CheckSheba(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            // حذف فاصله‌ها و تبدیل به حروف کوچک
            input = input.Replace(" ", "").ToLower();

            // تعریف عبارت منظم برای چک کردن صحت شماره شبا
            var regex = new Regex("^[a-zA-Z]{2}\\d{2}(\\d{4}\\s?){5}\\d{0,2}$");

            // چک کردن صحت رشته با استفاده از عبارت منظم
            var isMatch = regex.IsMatch(input);

            if (!isMatch)
            {
                return false;
            }

            // اعتبار سنجی شماره شبا
            var reversedSheba = input.Substring(4) + input.Substring(0, 4);
            var shebaDigits = string.Join("", reversedSheba.Select(c => char.IsDigit(c) ? c.ToString() : (char.ToLower(c) - 87).ToString()));
            var shebaValue = decimal.Parse(shebaDigits);
            if (shebaValue % 97 != 1)
            {
                return false;
            }

            return true;
        }
        public string GetPersianCurrentDateWithSeporator()
        {
            DateTime today = DateTime.Now;
            PersianCalendar pc = new PersianCalendar();
            string year = pc.GetYear(today).ToString();
            string month = pc.GetMonth(today).ToString().PadLeft(2, '0');
            string day = pc.GetDayOfMonth(today).ToString().PadLeft(2, '0');
            return year + "/" + month + "/" + day;
        }
        public string GetPersianCurrentDateWithoutSeporator()
        {
            DateTime today = DateTime.Now;
            PersianCalendar pc = new PersianCalendar();
            string year = pc.GetYear(today).ToString();
            string month = pc.GetMonth(today).ToString().PadLeft(2, '0');
            string day = pc.GetDayOfMonth(today).ToString().PadLeft(2, '0');
            return year + month + day;
        }
        public string GetPersianDateWithSeporator(DateTime dateTime)
        {
            PersianCalendar pc = new PersianCalendar();
            string year = pc.GetYear(dateTime).ToString();
            string month = pc.GetMonth(dateTime).ToString().PadLeft(2, '0');
            string day = pc.GetDayOfMonth(dateTime).ToString().PadLeft(2, '0');
            return year + "/" + month + "/" + day;
        }
        public string GetPersianDateWithoutSeporator(DateTime dateTime)
        {
            PersianCalendar pc = new PersianCalendar();
            string year = pc.GetYear(dateTime).ToString();
            string month = pc.GetMonth(dateTime).ToString().PadLeft(2, '0');
            string day = pc.GetDayOfMonth(dateTime).ToString().PadLeft(2, '0');
            return year + month + day;
        }
        public string GetPersianDateTimeWithSeporator(DateTime dateTime)
        {
            TimeZoneInfo timeZone = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? TimeZoneInfo.FindSystemTimeZoneById("Iran Standard Time") : TimeZoneInfo.FindSystemTimeZoneById("Asia/Tehran");
            DateTime iranTime = TimeZoneInfo.ConvertTime(dateTime, timeZone);
            PersianCalendar pc = new PersianCalendar();
            string year = pc.GetYear(iranTime).ToString();
            string month = pc.GetMonth(iranTime).ToString().PadLeft(2, '0');
            string day = pc.GetDayOfMonth(iranTime).ToString().PadLeft(2, '0');
            string hour = iranTime.Hour.ToString("00");
            string minute = iranTime.Minute.ToString("00");
            string second = iranTime.Second.ToString("00");
            string millisecond = iranTime.Millisecond.ToString().PadLeft(3, '0');

            return $"{year}/{month}/{day} - {hour}:{minute}:{second}";
            //return $"{year}/{month}/{day} - {hour}:{minute}:{second}.{millisecond}";
        }
        public string GenerateTransactionId(string customerNumber)
        {
            DateTime now = DateTime.Now;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            string randomString = new string(Enumerable.Repeat(chars, 20)
              .Select(s => s[random.Next(s.Length)]).ToArray());


            string str = "8833-23699" + randomString + "-" + now.ToString("yyyyMMddHHmmssfff");
            string inputString = str.Replace("-", "");
            int sum = 0;
            foreach (char c in inputString)
            {
                sum += c;
            }
            return str + "-" + sum.ToString();
        }
        public string GenerateStatementName(string statementType, DateTime fromDate, DateTime toDate)
        {
            var receiptStatement = _configuration.GetSection("IranInsuranceConfigs").GetSection("AccountNumbers").GetSection("Receipt").Value;
            var paymentStatement = _configuration.GetSection("IranInsuranceConfigs").GetSection("AccountNumbers").GetSection("Payment").Value;
            string statementName = "";
            if (statementType.Trim().ToLower() == "receipt")
            {
                statementName = $"{receiptStatement}_{GetPersianDateWithoutSeporator(fromDate)}_{GetPersianDateWithoutSeporator(toDate)}.csv";
            }
            else if (statementType.Trim().ToLower() == "payment")
            {
                statementName = $"{paymentStatement}_{GetPersianDateWithoutSeporator(fromDate)}_{GetPersianDateWithoutSeporator(toDate)}.csv";
            }
            return statementName;
        }

        public string GenerateCreateaccountFileName(string statementType, DateTime fromDate, DateTime toDate)
        {
            var receiptStatement = _configuration.GetSection("IranInsuranceConfigs").GetSection("AccountNumbers").GetSection("Receipt").Value;
            var paymentStatement = _configuration.GetSection("IranInsuranceConfigs").GetSection("AccountNumbers").GetSection("Payment").Value;
            string statementName = "";
            if (statementType.Trim().ToLower() == "receipt")
            {
                statementName = $"{receiptStatement}_{GetPersianDateWithoutSeporator(fromDate)}_{GetPersianDateWithoutSeporator(toDate)}.csv";
            }
            else if (statementType.Trim().ToLower() == "payment")
            {
                statementName = $"{paymentStatement}_{GetPersianDateWithoutSeporator(fromDate)}_{GetPersianDateWithoutSeporator(toDate)}.csv";
            }
            return statementName;
        }


        public string GetDefaultBranch()
        {
            var defaultBranchValue = _configuration.GetSection("BranchConfig").GetSection("DefaultBranch").Value;
            return string.IsNullOrEmpty(defaultBranchValue) ? "0064" : defaultBranchValue;
        }


    }
}
