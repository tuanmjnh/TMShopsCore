using System;
using System.Collections.Generic;

namespace TMShopsCore.Models
{
    public partial class SubItem
    {
        public long Id { get; set; }
        public string AppKey { get; set; }
        public long? IdKey { get; set; }
        public Guid? ItemId { get; set; }
        public string MainKey { get; set; }
        public string Value { get; set; }
        public string SubValue { get; set; }
        public string Images { get; set; }
        public string Details { get; set; }
        public int? Orders { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string DeleteBy { get; set; }
        public DateTime? DeleteAt { get; set; }
        public int Flag { get; set; }
        public string Extras { get; set; }

        public virtual Item IdKeyNavigation { get; set; }
    }
}
