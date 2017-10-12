using System;
using System.Collections.Generic;

namespace TMShopsCore.Models
{
    public partial class Item
    {
        public Item()
        {
            GroupItem = new HashSet<GroupItem>();
            SubItem = new HashSet<SubItem>();
        }

        public long Id { get; set; }
        public string AppKey { get; set; }
        public string IdKey { get; set; }
        public string CodeKey { get; set; }
        public string Title { get; set; }
        public long? Quantity { get; set; }
        public long? QuantityTotal { get; set; }
        public decimal? PriceOld { get; set; }
        public decimal? Price { get; set; }
        public string Images { get; set; }
        public byte[] Image { get; set; }
        public string Details { get; set; }
        public int? Orders { get; set; }
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

        public virtual ICollection<GroupItem> GroupItem { get; set; }
        public virtual ICollection<SubItem> SubItem { get; set; }
    }
}
