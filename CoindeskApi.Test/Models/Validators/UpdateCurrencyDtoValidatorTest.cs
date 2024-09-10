using CoindeskApi.Models;
using CoindeskApi.Models.Validators;
using FluentValidation.TestHelper;

namespace CoindeskApi.Test.Models.Validators;

public class UpdateCurrencyDtoValidatorTest
{
    private UpdateCurrencyDtoValidator _validator = new UpdateCurrencyDtoValidator();

    [Fact(DisplayName = $"測試 {nameof(UpdateCurrencyDto.Code)} 為必填")]
    public void Code_Required_Test()
    {
        var request = new UpdateCurrencyDto()
        {
            Code = string.Empty,
            CurrencyName = "新台幣"
        };

        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Code).WithErrorMessage($"{nameof(UpdateCurrencyDto.Code)} 必填");
    }

    [Fact(DisplayName = $"測試 {nameof(UpdateCurrencyDto.CurrencyName)} 為必填")]
    public void CurrencyName_Required_Test()
    {
        var request = new UpdateCurrencyDto
        {
            Code = "TWD",
            CurrencyName = string.Empty
        };

        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.CurrencyName)
            .WithErrorMessage($"{nameof(UpdateCurrencyDto.CurrencyName)} 必填");
    }
}