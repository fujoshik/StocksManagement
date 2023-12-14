using Analyzer.API.Analyzer.Domain.DTOs;

namespace Analyzer.Domain.DTOs
{

    public class UserDataDto
    {
        public CalculationDTOs CalculationDTO { get; set; }
        public decimal CurrentBalance { get; set; }
    }
}
