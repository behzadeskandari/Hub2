using System;
using System.ComponentModel.DataAnnotations;

namespace Yarane.Domain.Entities
{
    public class BaseEntity<TKey>
    {
        public TKey Id { get; set; }
       // public DateTime? CreateAt { get; set; }
       // public DateTime? ModifiedAt { get; set; }

        //[ConcurrencyCheck]
        //public Guid RecordVersion { get; set; }
    }
    public class BaseEntity : BaseEntity<Guid>
    {

    }
}
