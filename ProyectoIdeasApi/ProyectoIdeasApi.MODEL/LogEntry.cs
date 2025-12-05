using ProyectoIdeasApi.MODEL.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoIdeasApi.MODEL
{
    public class LogEntry
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }

        public string Level { get; set; } = default!;
        public string Message { get; set; } = default!;
        public string? Exception { get; set; }

        public string? FileName { get; set; }
        public string? MemberName { get; set; }
        public int? LineNumber { get; set; }

        public string? TraceId { get; set; }
    }
}
