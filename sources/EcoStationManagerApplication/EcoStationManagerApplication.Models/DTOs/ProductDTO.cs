using EcoStationManagerApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Models.DTOs
{
	// OrderDTO.cs
	public class ProductDTO
	{
		public int ProductId { get; set; }
		public string SKU { get; set; }
		public string Name { get; set; }
		public ProductType ProductType { get; set; }
		public string Unit { get; set; }
		public decimal Price { get; set; }
		public decimal MinStockLevel { get; set; }
		public int? CategoryId { get; set; }
		public string CategoryName { get; set; }
		public bool IsActive { get; set; }
		public DateTime CreatedDate { get; set; }
	}

	public class CreateProductDTO
	{
		public string SKU { get; set; }
		public string Name { get; set; }
		public ProductType ProductType { get; set; }
		public string Unit { get; set; }
		public decimal Price { get; set; }
		public decimal MinStockLevel { get; set; }
		public int? CategoryId { get; set; }
	}

	public class UpdateProductDTO
	{
		public int ProductId { get; set; }
		public string Name { get; set; }
		public ProductType ProductType { get; set; }
		public string Unit { get; set; }
		public decimal Price { get; set; }
		public decimal MinStockLevel { get; set; }
		public int? CategoryId { get; set; }
		public bool IsActive { get; set; }
	}
}
