using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Estate.Models
{
    public class BuildingDirection
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("جهت ساختمان")]
        public string Name { get; set; }
    }
}
