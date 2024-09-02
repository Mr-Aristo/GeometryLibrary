using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using GeometryLibrary.Concretes;

namespace GeometryLibrary.Validations
{
    public class CircleValidator : AbstractValidator<Circle>
    {
        public CircleValidator()
        {
            RuleFor(c => c.Radius)
                .GreaterThan(0).WithMessage("Radius must be greater than zero.");
        }
    }
}
}
