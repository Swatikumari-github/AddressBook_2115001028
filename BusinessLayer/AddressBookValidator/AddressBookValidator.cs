using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using ModelLayer.DTO;

namespace BusinessLayer.AddressBookValidator
{
    public class AddressBookValidator : AbstractValidator<AddressBookDTO>
    {
        public AddressBookValidator()
        {
            RuleFor(contact => contact.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(contact => contact.Email).EmailAddress().WithMessage("Invalid Email");
            RuleFor(contact => contact.Phone).Length(10).WithMessage("Phone number must be 10 digits");
        }

    }
}
