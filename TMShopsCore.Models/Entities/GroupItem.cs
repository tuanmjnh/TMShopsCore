using System;
using System.Collections.Generic;

namespace TMShopsCore.Models
{
    public partial class GroupItem
    {
        public long Id { get; set; }
        public string AppKey { get; set; }
        public long? GroupId { get; set; }
        public long? ItemId { get; set; }
        public int? Orders { get; set; }
        public string Details { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string DeleteBy { get; set; }
        public DateTime? DeleteAt { get; set; }
        public int? Flag { get; set; }
        public string Extras { get; set; }

        public virtual Group Group { get; set; }
        public virtual Item Item { get; set; }
    }
}
