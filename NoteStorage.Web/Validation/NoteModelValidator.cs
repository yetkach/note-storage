using FluentValidation;
using NoteStorage.Web.Models;

namespace NoteStorage.Web.Validation
{
    public class NoteModelValidator : AbstractValidator<NoteModel>
    {
        public NoteModelValidator()
        {
            RuleFor(n => n.Text)
                .NotEmpty()
                    .WithMessage("Text cannot be null or empty")
                .MinimumLength(1)
                     .WithMessage("Minimum text length 1 character")
                .MaximumLength(200)
                    .WithMessage("Maximum text length 200 characters");
        }
    }
}
