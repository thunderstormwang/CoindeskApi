using FluentValidation;

namespace CoindeskApi.Models.Validators;

public class DeleteCurrencyDtoValidator : AbstractValidator<DeleteCurrencyDto>
{
    public DeleteCurrencyDtoValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(request => request.Code).NotNull().WithMessage(r => $"{nameof(r.Code)} 必填")
            .NotEmpty().WithMessage(r => $"{nameof(r.Code)} 必填");
    }
}