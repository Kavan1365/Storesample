using BaseCore.Configuration;
using Core.Entities.AAA;
using Resources;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ViewModels.AAA
{
    public class RegisterViewModel : BaseDto<RegisterViewModel, User>, IValidatableObject
    {
        public string FristName { get; set; }
        public string Password { get; set; }
        public string AgentTitle { get; set; }
        public string LastName { get; set; }
        public int? OfficeId { get; set; }
        public int? CityId { get; set; }
        public int? ProvinceId { get; set; }
        public string OfficeTitle { get; set; }
        public string NationalCode { get; set; }
        public string UserName { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            string strRegex = @"^[0][9][0-9][0-9]{8,8}$";
            Regex re = new Regex(strRegex);
            if (!re.IsMatch(UserName))
                yield return new ValidationResult(ErrorMessages.Usernamemobilenumber, new[] { nameof(UserName) });

        }
    }
}
