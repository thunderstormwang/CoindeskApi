using FluentValidation;

namespace CoindeskApi.Models.Validators;

public class UpdateCurrencyDtoValidator : AbstractValidator<UpdateCurrencyDto>
{
    public UpdateCurrencyDtoValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(request => request.Code).NotNull().WithMessage(r => $"{nameof(r.Code)} 必填")
            .NotEmpty().WithMessage(r => $"{nameof(r.Code)} 必填");

        RuleFor(request => request.CurrencyName).NotNull().WithMessage(r => $"{nameof(r.CurrencyName)} 必填")
            .NotEmpty().WithMessage(r => $"{nameof(r.CurrencyName)} 必填");
    }
}