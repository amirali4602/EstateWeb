using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Estate.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string? Name { get; set; }
        [DisplayName("شماره تماس")]
        public string? Number { get; set; }
        [DisplayName("عکس پروفایل")]

        public string? profilePic { get; set; }
        public string? Comment { get; set; }
        public string? Role { get; set; }
        [DisplayName("دسترسی حذف")]

        public bool permisionDelete { get; set; } = false;
        [DisplayName("دسترسی ادیت")]

        public bool permisionEdit { get; set; } = false;
        [DisplayName("آگهی اجاره / فروش")]

        public bool isRent { get; set; } = false;//دسترسی فقط به آگهی فروش ندارد
        [DisplayName("دسترسی به آگهی فروش و اجاره")]

        public bool isOnlyRent { get; set; } = false;//دسترسی به هردو آگهی را دارد
        public string? whatsApp { get; set; }
        public string? instagram { get; set; }
        public string? telegram { get; set; }

        public double minRange { get; set; }
        public double maxRange { get; set; }

        [DisplayName("حداکثر نمایش ویژه مجاز")]
        public int FeaturedMax { get; set; } = 0;
        public int order { get; set; }
        [DisplayName("نمایش در صفحه مشاوران ")]

        public bool IsAgent { get; set; } // مشاور هست یا نه
    }
}
