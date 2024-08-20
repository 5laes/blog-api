using blogg_api.Models.DTOs;
using FluentValidation;

namespace blogg_api.Models.Validators
{
    public class BlogContentCreateValidator : AbstractValidator<BlogContentCreateDTO>
    {
        public BlogContentCreateValidator()
        {
            RuleFor(model => model.Title).NotEmpty();
            RuleFor(model => model.Content).NotEmpty();
        }
    }
}
