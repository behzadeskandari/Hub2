using Yarane.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Yarane.Contract.Dtos
{
    public class UserLogResponse
    {
        public List<UserLogResultData> ResultData { get; set; }
        public int Count { get; set; }
    }

    public class UserLogResultData
    {
        public long Id { get; set; }
        public Guid? UserId { get; set; }
        public string? UserName { get; set; }
        public DateTime? CreateAt { get; set; }
        public string? Ip { get; set; }
        public short? Status { get; set; }
    }
}
