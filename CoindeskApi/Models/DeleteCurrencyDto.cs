using System.ComponentModel.DataAnnotations;

namespace CoindeskApi.Models;

// TODO 改用 FluentValidation

public class DeleteCurrencyDto
{
    [Required]
    public string Code { get; set; }
}