using System.ComponentModel.DataAnnotations;

namespace CoindeskApi.Models;

// TODO 改用 FluentValidation

public class UpdateCurrencyDto
{
    [Required]
    public string Code { get; set; }

    [Required]
    public string CurrencyName { get; set; }
}