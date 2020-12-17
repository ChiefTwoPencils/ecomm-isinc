using EComm.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EComm.Web.Models
{
    public class ProductEditViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Products must have a name!")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Products must have a price!")]
        [Range(1.00, 500.00, ErrorMessage = "Products must have a price between 1.00 and 500.00!")]
        public decimal? UnitPrice { get; set; }
        public string Package { get; set; }
        public bool IsDiscontinued { get; set; }

        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }
        public IEnumerable<Supplier> Suppliers { get; set; }
        public IEnumerable<SelectListItem> SupplierItems => Suppliers?
            .Select(s => new SelectListItem
            {
                Text = s.CompanyName,
                Value = s.Id.ToString(),
                Selected = s.Id == SupplierId
            })
            .OrderBy(i => i.Text);
    }
}
