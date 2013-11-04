using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Zenwire.Domain
{
    public class Product //: Catelog
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public List<string> Items { get; set; }

        public decimal Cost { get; set; }
        public decimal Price { get; set; }
        public decimal Commission { get; set; }
    }
}