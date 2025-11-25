using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Estate.Models
{
    public class DocumentType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("سند")]
        public string Name { get; set; }
    }
}
