using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TShirt.API.Model;

namespace TShirt.API.Model
{
	//public class Shirt
 //   {
	//	public int TShirtId { get; set; }
	//	public string Gender { get; set; }
	//	public string Made { get; set; }
	//	public decimal Price { get; set; }
	//	public string Color { get; set; }
 //       public Size Size { get; set; }
 //       public Style Style { get; set; }
 //       public string Description { get; set; }
	//	public string ActualFileName { get; set; }
	//	public string NewFileName { get; set; } 
	//	public string FileExtension { get; set; }
	//	public int FileSizeInKB { get; set; }
	//	public int CreatedByUserId { get; set; }
	//	public DateTime CreatedOn { get; set; }

	//	public string ImagePath { get; set; }

 //   }

	public class Shirt
    {
        public int TShirtId { get; set; }
        public string Gender { get; set; }
        public string Made { get; set; }
        public decimal Price { get; set; }
        public string Color { get; set; }
        public int SizeId { get; set; }
        public string SizeName { get; set; }
        public int StyleId { get; set; }
        public string StyleName { get; set; }
        public string Description { get; set; }
        public string ActualFileName { get; set; }
        public string NewFileName { get; set; }
        public string FileExtension { get; set; }
        public int? FileSizeInKB { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public IFormFile? Image { get; set; } 
        public string ImageFileUrl { get; set; }
	}
}



