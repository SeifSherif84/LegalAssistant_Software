using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.ChatBot.Conan.Responses
{
    public record ConanSummarizeResponse(
        string Summary,
        int InputLength,
        double LatencyMs,
        string Model
    );
    
}
