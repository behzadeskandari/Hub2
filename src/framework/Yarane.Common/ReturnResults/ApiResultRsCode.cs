using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Yarane.Common.ReturnResults
{
    public enum ApiResultRsCode
    {
        [Display(Name = "شبکه در دسترس نمی باشد")]
        NetworkUnAvailable = -1,

        [Display(Name = "عملیات با موفقیت انجام پذیرفت")]
        Success = 0,

        [Display(Name = "هیچ موردی یافت نشد")]
        DataIsNull = 1,
        // testr
        [Display(Name = "عملیات با شکست مواجه شد")]
        OperationFailed = 2,

        [Display(Name = "سرویس کور بانکینگ در دسترس نمی باشد")]
        ServiceUnAvailable = 3,

        [Display(Name = "Your IP address is not allowed")]
        IpAddressUnAvailable = 4,

        [Display(Name = "ارسال شده به سیستم کور بانکینگ")]
        SentToCoreBanking = 100,

        [Display(Name = "اطلاعات ورودی نامعتبر است")]
        BadRequest = 400,

        [Display(Name = "یافت نشد")]
        NotFound = 404,

        [Display(Name = "خطای سمت سرور")]
        ServerError = 500,

        [Display(Name = "خطای گیتوی")]
        BadGatway = 502,

        [Display(Name = "دیتای موجود نیست")]
        NullData = 1600,

        [Display(Name = "موجودی حساب شما برای انجام تراکنش کافی نیست ")]
        NotEnoughBalanceForTransaction = 101,

        [Display(Name = "شبای ارسالی در تراکنش معتبر نمی باشد ")]
        InvalidIbanInTransaction = 102,

        [Display(Name = "نام کاربری تکراری می باشد ")]
        UserExist = 103,

        [Display(Name = "کاربر مورد نظر هنوز فعال نشده است ")]
        NotActivated = 104,

        [Display(Name = "نام کاربری یا کلمه عبور صحیح نمی باشد ")]
        IncorrectData = 105,

        [Display(Name = "کاربر احراز هویت نشده است")]
        UnAuthenticate = 106,

        [Display(Name = "توسط  کاربر دیگری درحال پردازش می باشد")]
        Inprogress = 107,

        [Display(Name = "توسط  کاربر دیگری درحال پردازش می باشد")]
        Dupplicate = 108,

        [Display(Name = "وضعیت تراکنش نامعتبر می باشد")]
        InvalidTransactionStatus = 109,

        [Display(Name = "هیچ رکوردی برای ارسال به نهضت یافت نشد")]
        NoRecordsForSend = 110,

        [Display(Name = "کاربر مورد نظر قفل شده است")]
        Locked = 111,

        [Display(Name = "رمز یکبار مصرف یا شناسه توکن معتبر نمی باشد")]
        InCorrectOtp = 112,

        [Display(Name = "توکن ارسالی منقضی شده است")]
        TokenExpierd = 113,

        [Display(Name = "کاربر دسترسی لازم را ندارد")]
        UnAuthorize = 114,

        [Display(Name = "کد امنیتی صحیح نمی باشد")]
        UnvalidCaptcha = 115,

    }
}
