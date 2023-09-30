using Batch_Manager.Models;
using FluentValidation;
using FluentValidation.Results;

namespace Batch_Manager.Validators
{
    public interface IBatchValidator
    {
        ValidationResult Validate(Batch batch);
    }

    public class BatchValidator : AbstractValidator<Batch>, IBatchValidator
    {
        private readonly IConfiguration _configuration;
        public BatchValidator(IConfiguration configuration)
        {
            _configuration = configuration;
            RuleFor(u => u.BusinessUnit).NotNull().NotEmpty().WithMessage("BusinessUnit cannot be empty.");
            RuleFor(u => u.BusinessUnit).Must(BeAValidBusinessUnit).WithMessage("Business Unit must be BU1 Or BU2");
            RuleFor(u => u.Files).NotNull().NotEmpty().WithMessage("Atleast one file should be exists in batch.");
            RuleFor(u => u.Attributes).Must(BeAValidKeyValueBatchAttributePair).WithMessage("Attribute Key and Value should be exists in batch.");
            RuleFor(u => u.ExpiryDate).Must(BeAValidDateNotInPast).WithMessage("Expiry date should not be past date.");
        }
        ValidationResult IBatchValidator.Validate(Batch batch)
        {
            return Validate(batch);
        }
        private bool BeAValidBusinessUnit(string unit)
        {
            var units = _configuration.GetValue<string>("BusinessUnit");
            var unitArray = units.Split(",");
            return unitArray.Contains(unit);
        }

        private bool BeAValidKeyValueBatchAttributePair(ICollection<BatchAttribute> attributes)
        {
            foreach (var item in attributes)
            {
                if (string.IsNullOrEmpty(item.Key) || string.IsNullOrEmpty(item.Value))
                    return false;
            }
            return true;
        }

        private bool BeAValidDateNotInPast(string date)
        {
            DateTime expiryDate = new DateTime();
            bool result = DateTime.TryParse(date, out expiryDate);
            if (expiryDate.Year < DateTime.Now.Year ||
                expiryDate.Month < DateTime.Now.Month ||
                expiryDate.Day < DateTime.Now.Day)
                return false;
            else
                return true;
        }
    }
}
