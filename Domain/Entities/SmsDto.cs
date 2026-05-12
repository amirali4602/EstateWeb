using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class SmsDto
{
    [Key]
    public int Id { get; set; }

    public string? PhoneNumber { get; set; }
    public DateTime date { get; set; }
    public int FailedTimes { get; set; }
    public string? sentStatus { get; set; }

}
