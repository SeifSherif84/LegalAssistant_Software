using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.ChatBot.Conan.Responses
{
    public record ConanParseResponse(
        string Text,
        string Filename,
        string ContentType,
        int CharCount,
        List<string> Warnings);
}
