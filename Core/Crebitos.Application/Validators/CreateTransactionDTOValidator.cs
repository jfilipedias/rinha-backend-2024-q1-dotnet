using FluentValidation;

namespace Crebitos.Application;

public class CreateTransactionDTOValidator : AbstractValidator<CreateTransactionDTO>
{
    public CreateTransactionDTOValidator()
    {
        RuleFor(x => x.Description).NotEmpty().MaximumLength(10);
        RuleFor(x => x.Type).NotEmpty().MaximumLength(1).Matches("/[cd]/");
        RuleFor(x => x.Value).GreaterThan(0);
    }
}
