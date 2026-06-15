using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Yarane.Persistance.Contexts
{
    public class ExceptionContext : DbContext
    {
        public ExceptionContext(DbContextOptions<ExceptionContext> options) : base(options)
        {

        }
        public DbSet<ExceptionLog> ExceptionLog { get; set; }

    }

    public class ExceptionLog
    {
        public int Id { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public bool? IsActive { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string Source { get; set; }
        public string ExceptionType { get; set; }
        public string ClassName { get; set; }
        public string MethodName { get; set; }
    }
}
