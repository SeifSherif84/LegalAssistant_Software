using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.ChatBot.Conan.Responses
{
    public record ConanAttachedDoc(
        string DocId,
        string Filename,
        string ContentType,
        int CharCount,
        double AttachedAt);

    public record ConanAttachResponse(
        string SessionId,
        ConanAttachedDoc Attached,
        int AttachmentsTotal,
        int TotalChars,
        List<string> Warnings);

    public record ConanAttachmentsListResponse(
        string SessionId,
        List<ConanAttachedDoc> Attachments,
        int TotalChars);
}
