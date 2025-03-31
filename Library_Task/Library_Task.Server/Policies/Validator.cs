using FluentValidation;
using Library_Task.Server.DTO;

namespace Library_Task.Server.Policies
{
    public class Validator : AbstractValidator<BusinessEntity>
    {
        public Validator()
        {
            try
            {
                RuleFor(x => x.GetType().GetProperty("ISBN").GetValue(x).ToString()).Length(13, 13).WithErrorCode("400").WithMessage("ISBN should be exactly 13 symbols long");
            }
            catch (Exception ex)
            {
            }
        }
    }
}