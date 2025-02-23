using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dagnyr.api.Entities;

    public record Product
    {
        public int Id { get; set; }
        public string ItemNumber { get; set; }
        public string ProductName { get; set; }
        public double PricePerPiece { get; set; }
        public int PackQuantity { get; set; }
        public double WeightKg { get; set; }

        public DateOnly ProductionDate { get; set; }
        public DateOnly ExpiryDate { get; set; }

    
    }
