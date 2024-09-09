using System.ComponentModel.DataAnnotations;

namespace CoindeskApi.Models;

// TODO 改用 FluentValidation

public class CreateCurrencyDto
{
    [Required]
    public string Code { get; set; }

    [Required]
    public string CurrencyName { get; set; }
}

public class UpdateCurrencyDto
{
    [Required]
    public string Code { get; set; }

    [Required]
    public string CurrencyName { get; set; }
}

public class DeleteCurrencyDto
{
    [Required]
    public string Code { get; set; }
}