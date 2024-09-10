using CoindeskApi.Models;
using CoindeskApi.Models.Validators;
using FluentValidation.TestHelper;

namespace CoindeskApi.Test.Models.Validators;

public class CreateCurrencyDtoValidatorTest
{
    private CreateCurrencyDtoValidator _validator = new CreateCurrencyDtoValidator();

    [Fact(DisplayName = $"測試 {nameof(CreateCurrencyDto.Code)} 為必填")]
    public void Code_Required_Test()
    {
        var request = new CreateCurrencyDto
        {
            Code = string.Empty,
            CurrencyName = "新台幣"
        };

        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Code).WithErrorMessage($"{nameof(CreateCurrencyDto.Code)} 必填");
    }

    [Fact(DisplayName = $"測試 {nameof(CreateCurrencyDto.CurrencyName)} 為必填")]
    public void CurrencyName_Required_Test()
    {
        var request = new CreateCurrencyDto
        {
            Code = "TWD",
            CurrencyName = string.Empty
        };

        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.CurrencyName)
            .WithErrorMessage($"{nameof(CreateCurrencyDto.CurrencyName)} 必填");
    }
}