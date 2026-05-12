using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Page : BaseEntity
{
    [Key]
    public int PageId { get; set; }//ایدی صفحه
    [Required]
    [DisplayName("نوع محصول")]
    public int CategoryId { get; set; }
    [ForeignKey("CategoryId")]
    [ValidateNever]
    public Category Category { get; set; }//دسته بندی

    [Required]
    [DisplayName("عنوان صفحه")]
    public string Title { get; set; }

    [DisplayName("توضیحات")]
    public string Description { get; set; } = "";

    [DisplayName("قیمت")]
    [Range(0, double.MaxValue)]
    public decimal Price { get; set; } // قیمت

    //Features
    [DisplayName("ابعاد")]
    public string Dimensions { get; set; }

    [DisplayName("ضخامت")]
    [Range(0.1, 50.0)]
    public double Thickness { get; set; }

    [DisplayName("وزن")]
    [Range(0.1, 50.0)]
    public double Mass { get; set; }

    [Required]
    [DisplayName("کد رنگ")]
    public string ColorCode { get; set; } 


    //images
    [ValidateNever]
    [DisplayName("تصویر اصلی")]
    public string? ImageUrl { get; set; }//تصویر
    [Required]
    [DisplayName("نمایش در سایت")]
    public bool isActive { get; set; } = false;
    [Required]
    [DisplayName("نمایش ویژه")]
    public bool isFeatured { get; set; } = false;

    public DateTime Date { get; set; }= DateTime.Now;
    public string? CustomerNumber { get; set; } = null;
    public bool ShowCustomerNumber { get; set; } = false;
    
    [DisplayName("فروخته شد")]
    [ValidateNever]
    public bool Sold { get; set; } = false;
    [DisplayName("نام مشتری")]
    [ValidateNever]
    public string Name { get; set; } = ""; 
    [ValidateNever]
    [DisplayName("گالری")]
    public string? Gallery { get; set; } = null;//گالری
}
