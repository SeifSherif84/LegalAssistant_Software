using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions.ChatBot
{
    public record ConanApiResult<T>
    {
        public bool IsSuccess { get; init; }
        public T? Data { get; init; }
        public ConanApiResultStatus Status { get; init; }

        /// <summary>Arabic detail string straight from the API — safe to surface to the user.</summary>
        public string? ErrorDetail { get; init; }

        public static ConanApiResult<T> Success(T data) =>
            new() { IsSuccess = true, Data = data, Status = ConanApiResultStatus.Success };

        public static ConanApiResult<T> ServiceBusy(string? detail = null) =>
            new() { IsSuccess = false, Status = ConanApiResultStatus.ServiceBusy, ErrorDetail = detail };

        public static ConanApiResult<T> BadRequest(string? detail = null) =>
            new() { IsSuccess = false, Status = ConanApiResultStatus.BadRequest, ErrorDetail = detail };

        public static ConanApiResult<T> Failure(string? detail = null) =>
            new() { IsSuccess = false, Status = ConanApiResultStatus.Failure, ErrorDetail = detail };
    }

    public enum ConanApiResultStatus
    {
        Success,
        ServiceBusy,    // 503 — LLM quota exhausted; safe to retry
        BadRequest,     // 400 / 404 / 422 — client error
        Failure         // network / unexpected server error
    }
}
