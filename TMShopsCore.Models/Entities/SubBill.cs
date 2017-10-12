using System;
using System.Collections.Generic;

namespace TMShopsCore.Models
{
    public partial class SubBill
    {
        public long Id { get; set; }
        public string IdKey { get; set; }
        public string CodeKey { get; set; }
        public Guid? ItemId { get; set; }
        public string Title { get; set; }
        public long? Quantity { get; set; }
        public decimal? PriceOld { get; set; }
        public decimal? Price { get; set; }
        public decimal? TotalPrice { get; set; }
        public int? Orders { get; set; }
        public string Details { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string DeleteBy { get; set; }
        public DateTime? DeleteAt { get; set; }
        public int? Flag { get; set; }
        public string Extras { get; set; }

        public virtual Bill IdKeyNavigation { get; set; }
    }
}
