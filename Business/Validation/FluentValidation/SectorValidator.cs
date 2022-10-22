using DataAccess.Entities.Section;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Validation.FluentValidation
{
    public class SectorValidator : AbstractValidator<Section>
    {
        public SectorValidator()
        {
            RuleFor(sector => sector.Name).Length(2,30);
            RuleFor(sector => sector.SectionNo).GreaterThan(300);
        }
    }
}
