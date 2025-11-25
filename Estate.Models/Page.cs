using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Estate.Models
{
    public class Page
    {
        [Key]
        public int PageId { get; set; }//ایدی صفحه
        [Required]
        [DisplayName("نوع ملک")]
        public int CategoryId { get; set; }//دسته بندی
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }//دسته بندی

        [ValidateNever]
        [DisplayName("تصویر اصلی")]
        public string? ImageUrl { get; set; }//تصویر
        [ValidateNever]
        [DisplayName("گالری")]
        public string? Gallery { get; set; } = null;//گالری
        [Required]
        [DisplayName("عنوان صفحه")]
        public string Title { get; set; }//عنوان صفحه
        [Required]
        [DisplayName("آدرس")]
        public string Address { get; set; }//آدرس

        [DisplayName("توضیحات")]
        public string Description { get; set; } = "";//توضیحات
        [Required]
        [Range(0, Double.MaxValue)]
        [DisplayName("قیمت کل")]
        public Double PriceTotal { get; set; }//قیمت کل
        [Required]
        [Range(0, Double.MaxValue)]
        [DisplayName("قیمت متری")]
        public Double PriceMeter { get; set; }//قیمت کل
        [Required]
        [Range(0, int.MaxValue)]
        [DisplayName("متراژ")]
        public int Meterage { get; set; }//متراژ
        [Required]
        [Range(0, Double.MaxValue)]
        [DisplayName("ودیعه")]
        public Double Deposit { get; set; } //ودیعه
        [Required]
        [Range(0, Double.MaxValue)]
        [DisplayName("اجاره")]
        public Double Rent { get; set; }//اجاره

        [Range(0, int.MaxValue)]
        [DisplayName("سال ساخت")]
        public int? Year { get; set; } = null;//سال ساخت

        [Range(0, int.MaxValue)]
        [DisplayName("تعداد اتاق")]
        public int? Rooms { get; set; } = null;//تعداد اتاق

        [Range(1, int.MaxValue)]
        [DisplayName("طبقه")]
        public int? Floor { get; set; } = null;//طبقه

        [Range(1, int.MaxValue)]
        [DisplayName("تعداد واحد در طبقه")]
        public int? Units { get; set; } = null;//تعداد واحد در طبقه

        [Range(1, int.MaxValue)]
        [DisplayName("تعداد کل طبقات ساختمان")]
        public int? TotalFloors { get; set; } = null;//تعداد کل طبقات ساختمان


        [Required]
        [DisplayName("آسانسور")]
        public bool Elevator { get; set; }//آسانسور
        [Required]
        [DisplayName("پارکینگ")]
        public bool Parking { get; set; }//پارک
        [Required]
        [DisplayName("انباری")]
        public bool StoreRoom { get; set; }//انباری
        [Required]
        [DisplayName("بالکن")]
        public bool Balcony { get; set; }//بالکن
        [Required]
        [DisplayName("بازسازی شده است")]
        public bool Restored { get; set; }//بازسازی شده است


        [Required]
        [DisplayName("سند")]
        public int DocumentTypeId { get; set; }

        [ForeignKey("DocumentTypeId")]
        [ValidateNever]
        public DocumentType Document { get; set; }//سند

        [Required]
        [DisplayName("جهت ساختمان")]
        public int BuildingDirectionId { get; set; }
        [ForeignKey("BuildingDirectionId")]
        [ValidateNever]
        public BuildingDirection Direction { get; set; }//جهت ساختمان


        [Required]
        [DisplayName("سرویس بهداشتی")]
        public int ToiletId { get; set; }

        [ForeignKey("ToiletId")]
        [ValidateNever]
        public Toilet Toilet { get; set; }//دستشویی
        [Required]
        [DisplayName("سرمایش")]
        public int CoolingId { get; set; }

        [ForeignKey("CoolingId")]
        [ValidateNever]
        public Cooling Cooler { get; set; }//سرمایش
        [Required]
        [DisplayName("گرمایش")]
        public int HeatingId { get; set; }

        [ForeignKey("HeatingId")]
        [ValidateNever]
        public Heating Heater { get; set; }//گرمایش
        [Required]
        [DisplayName("تامیین کننده آب گرم")]
        public int HotWaterSupplierId { get; set; }

        [ForeignKey("HotWaterSupplierId")]
        [ValidateNever]
        public HotWaterSupplier WaterSupplier { get; set; }//تامین کننده آب
        [Required]
        [DisplayName("جنس کف")]
        public int FloorMaterialId { get; set; }

        [ForeignKey("FloorMaterialId")]
        [ValidateNever]
        public FloorMaterial floorMaterial { get; set; }//جنس کف

        [Required]
        [DisplayName("نمایش در سایت")]
        public bool isActive { get; set; } = false;
        [Required]
        [DisplayName("نمایش ویژه")]
        public bool isFeatured { get; set; } = false;
        [Required]
        [DisplayName("اجاره و رهن")]
        public bool isRent { get; set; } = false ;

        public DateTime Date { get; set; }= DateTime.Now;
        public string? CustomerNumber { get; set; } = null;
        public bool ShowCustomerNumber { get; set; } = false;
        [DisplayName("پیام رد آگهی")]
        [ValidateNever]
        public string AdsMessage { get; set; } = "منتظر تایید";
        [DisplayName("فروخته شد")]
        [ValidateNever]
        public bool Sold { get; set; } = false;
        [DisplayName("نام مشتری")]
        [ValidateNever]
        public string Name { get; set; } = "";

    }


}
