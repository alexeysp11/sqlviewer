using FluentValidation;
using SqlViewer.Common.Dtos.SqlQueries;

namespace SqlViewer.ApiGateway.Dtos.FluentValidation;

// TODO: Should be moved into metadata and query execution service.
public sealed class CreateTableRequestValidator : AbstractValidator<CreateTableRequestDto>
{
    public CreateTableRequestValidator()
    {
        RuleFor(x => x.TableName)
            .NotEmpty().WithMessage("Table name is required")
            .MaximumLength(128).WithMessage("Table name is too long");

        RuleFor(x => x.Columns)
            .NotEmpty().WithMessage("Table must have at least one column");

        RuleForEach(x => x.Columns).ChildRules(column =>
        {
            column.RuleFor(c => c.ColumnName).NotEmpty();
            column.RuleFor(c => c.ColumnType).IsInEnum();
        });
    }
}
