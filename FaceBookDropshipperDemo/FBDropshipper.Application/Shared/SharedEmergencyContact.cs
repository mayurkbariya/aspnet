using FBDropshipper.Application.Extensions;
using FluentValidation;

namespace FBDropshipper.Application.Shared
{
    public class SharedEmergencyContact
    {
        public string Email { get; set; }
        public string Relation { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
    }

    public class SharedEmergencyContactValidator : AbstractValidator<SharedEmergencyContact>
    {
        public SharedEmergencyContactValidator()
        {
            RuleFor(p => p.Name).Required().Max(50);
            RuleFor(p => p.Email).EmailAddress().Max(50);
            RuleFor(p => p.PhoneNumber).Phone();
            RuleFor(p => p.Relation).Required().Max(50);
        }
    }
}