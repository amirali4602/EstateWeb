using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estate.Models
{
    public class SmsDto
    {
        [Key]
        public int Id { get; set; }

        public string? PhoneNumber { get; set; }
        public DateTime date { get; set; }
        public int FailedTimes { get; set; }
        public string? sentStatus { get; set; }

    }
}
