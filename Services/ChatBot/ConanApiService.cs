using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Services.Abstractions.ChatBot;
using Shared.Dtos.ChatBot.Conan;
using Shared.Dtos.ChatBot.Conan.Responses;
using System.Net;
using System.Net.Http.Json;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using static Shared.Dtos.ChatBot.Conan.Requests;

namespace Services.ChatBot;
public class ConanApiService(
    IHttpClientFactory _httpClientFactory,
    ILogger<ConanApiService> _logger) : IConanApiService
{
    // ── JSON options shared across all calls ──────────────────────────────────
    private static readonly JsonSerializerOptions _jsonOpts = new()
    {
        PropertyNameCaseInsensitive = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };

    private HttpClient CreateClient() => _httpClientFactory.CreateClient("ConanServiceClient");


    // ── Health ────────────────────────────────────────────────────────────────

    public async Task<ConanHealthResponse?> GetHealthAsync(CancellationToken ct = default)
    {
        try
        {
            var client = CreateClient();
            var response = await client.GetAsync("health", ct);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ConanHealthResponse>(_jsonOpts, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[Conan] Health check failed");
            return null;
        }
    }


    // ── Q&A ───────────────────────────────────────────────────────────────────

    public async Task<ConanApiResult<ConanAnswerEnvelope>> AskQuestionAsync(
        ConanQaRequest request,
        CancellationToken ct = default)
    {
        return await PostJsonAsync<ConanAnswerEnvelope>("qa", request, ct);
    }

    public async Task<ConanApiResult<ConanAnswerEnvelope>> AskQuestionWithFileAsync(
        string question,
        IFormFile file,
        int k = 7,
        CancellationToken ct = default)
    {
        using var form = BuildMultipartForm(file);
        form.Add(new StringContent(question), "question");
        form.Add(new StringContent(k.ToString()), "k");

        return await PostMultipartAsync<ConanAnswerEnvelope>("qa/upload", form, ct);
    }


    // ── Multi-turn chat ───────────────────────────────────────────────────────

    public async Task<ConanApiResult<ConanAnswerEnvelope>> SendChatMessageAsync(
        ConanChatRequest request,
        CancellationToken ct = default)
    {
        return await PostJsonAsync<ConanAnswerEnvelope>("chat", request, ct);
    }


    // ── Session attachments ───────────────────────────────────────────────────

    public async Task<ConanApiResult<ConanAttachResponse>> AttachDocumentToSessionAsync(
        string sessionId,
        IFormFile file,
        CancellationToken ct = default)
    {
        using var form = BuildMultipartForm(file);
        form.Add(new StringContent(sessionId), "session_id");

        return await PostMultipartAsync<ConanAttachResponse>("chat/attach", form, ct);
    }

    public async Task<ConanAttachmentsListResponse?> GetSessionAttachmentsAsync(
        string sessionId,
        CancellationToken ct = default)
    {
        try
        {
            var client = CreateClient();
            var response = await client.GetAsync($"chat/{sessionId}/attachments", ct);
            // Contract: unknown sessions return 200 + empty list, never 404.
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ConanAttachmentsListResponse>(_jsonOpts, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[Conan] GetSessionAttachments failed for session {SessionId}", sessionId);
            return null;
        }
    }

    public async Task<bool> RemoveSessionAttachmentAsync(
        string sessionId,
        string docId,
        CancellationToken ct = default)
    {
        try
        {
            var client = CreateClient();
            var response = await client.DeleteAsync($"chat/{sessionId}/attachments/{docId}", ct);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[Conan] RemoveAttachment failed — session {SessionId}, doc {DocId}", sessionId, docId);
            return false;
        }
    }

    public async Task<bool> ClearSessionAttachmentsAsync(
        string sessionId,
        CancellationToken ct = default)
    {
        try
        {
            var client = CreateClient();
            var response = await client.DeleteAsync($"chat/{sessionId}/attachments", ct);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[Conan] ClearAttachments failed — session {SessionId}", sessionId);
            return false;
        }
    }


    // ── Legal analysis ────────────────────────────────────────────────────────

    public async Task<ConanApiResult<ConanAnswerEnvelope>> AnalyzeWeaknessAsync(
        ConanWeaknessRequest request,
        CancellationToken ct = default)
    {
        return await PostJsonAsync<ConanAnswerEnvelope>("weakness", request, ct);
    }

    public async Task<ConanApiResult<ConanAnswerEnvelope>> GenerateDefenseMemoAsync(
        ConanDefenseRequest request,
        CancellationToken ct = default)
    {
        return await PostJsonAsync<ConanAnswerEnvelope>("defense", request, ct);
    }

    public async Task<ConanApiResult<ConanAnswerEnvelope>> AnalyzeForensicConsistencyAsync(
        ConanForensicRequest request,
        CancellationToken ct = default)
    {
        return await PostJsonAsync<ConanAnswerEnvelope>("forensic", request, ct);
    }


    // ── Utilities ─────────────────────────────────────────────────────────────

    public async Task<ConanApiResult<ConanSummarizeResponse>> SummarizeTextAsync(
        ConanSummarizeRequest request,
        CancellationToken ct = default)
    {
        return await PostJsonAsync<ConanSummarizeResponse>("summarize", request, ct);
    }

    public async Task<ConanApiResult<ConanParseResponse>> ParseDocumentAsync(
        IFormFile file,
        CancellationToken ct = default)
    {
        using var form = BuildMultipartForm(file);
        return await PostMultipartAsync<ConanParseResponse>("parse", form, ct);
    }

    public async Task<ConanApiResult<ConanParseResponse>> ParseTextAsync(
        string text,
        CancellationToken ct = default)
    {
        using var form = new MultipartFormDataContent();
        form.Add(new StringContent(text), "text");
        return await PostMultipartAsync<ConanParseResponse>("parse", form, ct);
    }


    // ── Private helpers ───────────────────────────────────────────────────────

    /// <summary>POST with JSON body → typed result.</summary>
    private async Task<ConanApiResult<T>> PostJsonAsync<T>(
        string endpoint,
        object body,
        CancellationToken ct)
    {
        try
        {
            var client = CreateClient();
            var response = await client.PostAsJsonAsync(endpoint, body, _jsonOpts, ct);
            return await MapResponseAsync<T>(response, endpoint, ct);
        }
        catch (TaskCanceledException ex) when (!ct.IsCancellationRequested)
        {
            _logger.LogError(ex, "[Conan] Timeout on POST {Endpoint} — check HttpClient timeout (must be ≥ 120 s)", endpoint);
            return ConanApiResult<T>.Failure("انتهت مهلة الاتصال. يرجى المحاولة مجدداً.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[Conan] Unexpected error on POST {Endpoint}", endpoint);
            return ConanApiResult<T>.Failure();
        }
    }

    /// <summary>POST with multipart/form-data body → typed result.</summary>
    private async Task<ConanApiResult<T>> PostMultipartAsync<T>(
        string endpoint,
        MultipartFormDataContent form,
        CancellationToken ct)
    {
        try
        {
            var client = CreateClient();
            var response = await client.PostAsync(endpoint, form, ct);
            return await MapResponseAsync<T>(response, endpoint, ct);
        }
        catch (TaskCanceledException ex) when (!ct.IsCancellationRequested)
        {
            _logger.LogError(ex, "[Conan] Timeout on multipart POST {Endpoint}", endpoint);
            return ConanApiResult<T>.Failure("انتهت مهلة الاتصال. يرجى المحاولة مجدداً.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[Conan] Unexpected error on multipart POST {Endpoint}", endpoint);
            return ConanApiResult<T>.Failure();
        }
    }
    private async Task<ConanApiResult<T>> MapResponseAsync<T>(
        HttpResponseMessage response,
        string endpoint,
        CancellationToken ct)
    {
        switch ((int)response.StatusCode)
        {
            case 200:
                var data = await response.Content.ReadFromJsonAsync<T>(_jsonOpts, ct);
                if (data is null)
                {
                    _logger.LogWarning("[Conan] {Endpoint} returned 200 but deserialization produced null", endpoint);
                    return ConanApiResult<T>.Failure();
                }
                return ConanApiResult<T>.Success(data);

            case 503:
                // LLM provider budget exhausted — expected, not a crash. Show retry to user.
                var detail503 = await ReadDetailAsync(response, ct);
                _logger.LogWarning("[Conan] {Endpoint} → 503 (LLM busy). Detail: {Detail}", endpoint, detail503);
                return ConanApiResult<T>.ServiceBusy(detail503);

            case 400:
            case 404:
            case 422:
                var detail4xx = await ReadDetailAsync(response, ct);
                _logger.LogWarning("[Conan] {Endpoint} → {Status}. Detail: {Detail}", endpoint, (int)response.StatusCode, detail4xx);
                return ConanApiResult<T>.BadRequest(detail4xx);

            default:
                _logger.LogError("[Conan] {Endpoint} → unexpected {Status}", endpoint, (int)response.StatusCode);
                return ConanApiResult<T>.Failure();
        }
    }

    /// <summary>Reads the Arabic "detail" field from a FastAPI error response.</summary>
    private static async Task<string?> ReadDetailAsync(HttpResponseMessage response, CancellationToken ct)
    {
        try
        {
            var err = await response.Content.ReadFromJsonAsync<FastApiError>(_jsonOpts, ct);
            return err?.Detail;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>Wraps an IFormFile into a multipart stream without loading it all into memory.</summary>
    private static MultipartFormDataContent BuildMultipartForm(IFormFile file)
    {
        var form = new MultipartFormDataContent();
        var fileContent = new StreamContent(file.OpenReadStream());
        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(
            file.ContentType ?? "application/octet-stream");
        form.Add(fileContent, "file", file.FileName);
        return form;
    }

    private record FastApiError(string? Detail);
}
