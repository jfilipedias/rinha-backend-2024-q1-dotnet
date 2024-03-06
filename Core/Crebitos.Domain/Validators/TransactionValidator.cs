using FluentValidation;

namespace Crebitos.Domain;

public class TransactionValidator : AbstractValidator<Transaction>
{
    public TransactionValidator()
    {
        RuleFor(x => x.CustomerId).GreaterThan(0);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(10);
        RuleFor(x => x.Type).NotEmpty().MaximumLength(1).Matches("/[cd]/");
        RuleFor(x => x.Value).GreaterThan(0);
        RuleFor(x => x.CreatedAt).LessThanOrEqualTo(DateTime.Now);
    }
}
