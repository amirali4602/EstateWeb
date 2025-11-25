using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Estate.Models
{
    public class Heating
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("گرمایش")]
        public string Name { get; set; }
    }
}
