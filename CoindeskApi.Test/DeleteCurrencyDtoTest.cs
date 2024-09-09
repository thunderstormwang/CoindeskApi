using CoindeskApi.Models;
using CoindeskApi.Models.Validators;
using FluentValidation.TestHelper;

namespace CoindeskApi.Test;

public class DeleteCurrencyDtoTest
{
    private DeleteCurrencyDtoValidator _validator = new DeleteCurrencyDtoValidator();

    [Fact(DisplayName = $"測試 {nameof(DeleteCurrencyDto.Code)} 為必填")]
    public void Code_Required_Test()
    {
        var request = new DeleteCurrencyDto()
        {
            Code = string.Empty
        };

        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Code).WithErrorMessage($"{nameof(DeleteCurrencyDto.Code)} 必填");
    }
}