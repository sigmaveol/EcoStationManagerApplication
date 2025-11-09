using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Models.DTOs
{
    // Supporting classes for return types
    public class PackagingQuantities
    {
        public int QtyNew { get; set; }
        public int QtyInUse { get; set; }
        public int QtyReturned { get; set; }
        public int QtyNeedCleaning { get; set; }
        public int QtyCleaned { get; set; }
        public int QtyDamaged { get; set; }
    }
}
