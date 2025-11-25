using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Estate.Models
{
    public class HotWaterSupplier
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("تامین کننده آب گرم")]
        public string Name { get; set; }
    }
}
