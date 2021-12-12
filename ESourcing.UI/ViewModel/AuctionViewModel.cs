using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ESourcing.UI.ViewModel
{
    public class AuctionViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please fill the Name place.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please fill the Description")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Please fill the Product Id")]
        public string ProductId { get; set; }
        [Required(ErrorMessage = "Please fill the quantity")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Please fill the Start Date")]
        public DateTime StartedAt { get; set; }
        [Required(ErrorMessage = "Please fill the Finish Date")]
        public DateTime FinishedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Status { get; set; }
        public int SellerId { get; set; }
        public List<string> IncludedSellers { get; set; }
    }
}
