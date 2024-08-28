using blogg_api.Models.DTOs;
using FluentValidation;

namespace blogg_api.Models.Validators
{
    public class BlogContentUpdateValidator : AbstractValidator<BlogContentUpdateDTO>
    {
        public BlogContentUpdateValidator()
        {
            RuleFor(model => model.Id).NotEmpty();
            RuleFor(model => model.Title).NotEmpty();
            RuleFor(model => model.Content).NotEmpty();
            RuleFor(model => model.TagId).NotEmpty();
        }
    }
}
