using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartNewspaper.Models
{
    public class ItemContent
    {
        public long ItemID { get; set; }
        public string Content { get; set; }
        public virtual Item Item { get; set; }
    }
}