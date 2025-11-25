using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Estate.Models
{
    public class Cooling
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("سرمایش")]
        public string Name { get; set; }
    }
}
