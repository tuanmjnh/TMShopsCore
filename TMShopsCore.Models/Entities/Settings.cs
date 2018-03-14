using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TMShopsCore.Models
{
    [Table("Settings")]
    public partial class Settings
    {
        public long Id { get; set; }
        public string ModuleKey { get; set; }
        public string SubKey { get; set; }
        public string Value { get; set; }
        public string SubValue { get; set; }
        public string Details { get; set; }
        public int? Orders { get; set; }
        public int? Flag { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string DeleteBy { get; set; }
        public DateTime? DeleteAt { get; set; }
        public string Extras { get; set; }
    }
}
