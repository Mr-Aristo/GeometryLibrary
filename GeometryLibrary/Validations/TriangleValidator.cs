using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using GeometryLibrary.Concretes;

namespace GeometryLibrary.Validations
{
    public class TriangleValidator : AbstractValidator<Triangle>
    {
        public TriangleValidator()
        {
            RuleFor(t => t.SideA)
                .GreaterThan(0).WithMessage("SideA must be greater than zero.");
            RuleFor(t => t.SideB)
                .GreaterThan(0).WithMessage("SideB must be greater than zero.");
            RuleFor(t => t.SideC)
                .GreaterThan(0).WithMessage("SideC must be greater than zero.");
            RuleFor(t => new { t.SideA, t.SideB, t.SideC })
                .Must(s => s.SideA + s.SideB > s.SideC && s.SideA + s.SideC > s.SideB && s.SideB + s.SideC > s.SideA)
                .WithMessage("The sides provided do not form a valid triangle.");
        }
    }
}
