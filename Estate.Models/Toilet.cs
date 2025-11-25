using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Estate.Models
{
    public class Toilet
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("سرویس بهداشتی")]
        public string Name { get; set; }
    }
}
