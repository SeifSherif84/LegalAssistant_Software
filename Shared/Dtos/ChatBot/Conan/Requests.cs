using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.ChatBot.Conan
{
    public class Requests
    {
        public record ConanChatRequest(
            string Message,
            string? SessionId = null,
            int K = 7);

        public record ConanQaRequest(
            string Question,
            int K = 7,
            string PromptStyle = "restrictive");

        public record ConanWeaknessRequest(
            string CaseFacts,
            string? Evidence = null,
            string? DefendantStatement = null);

        public record ConanDefenseRequest(
            string CaseFacts,
            string? Weaknesses = null,
            string? Evidence = null,
            string? DefendantStatement = null);

        public record ConanForensicRequest(
            string CaseFacts,
            string? Evidence = null);

        public record ConanSummarizeRequest(string Text);
    }
}
