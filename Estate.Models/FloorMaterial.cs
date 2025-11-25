using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Estate.Models
{
    public class FloorMaterial
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("جنس کف")]
        public string Name { get; set; }
    }

}
