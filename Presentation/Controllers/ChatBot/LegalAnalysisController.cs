using Domain.Entities;
using Domain.Entities.ChatBotAIEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions.ChatBot;
using Shared.Dtos.ChatBot.Conan;
using Shared.Dtos.ChatBot.Conan.Responses;
using System.Security.Claims;
using static Shared.Dtos.ChatBot.Conan.Requests;

namespace Presentation.Controllers;

[ApiController]
[Route("api/legal")]
[Authorize]
public class LegalAnalysisController(ILegalAnalysisService _legalService,IChatBotService _chatBotService) : ControllerBase
{
    private string LawyerId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    // ── Health ────────────────────────────────────────────────────────────────

    [HttpGet("health")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ConanHealthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GetHealth(CancellationToken ct)
    {
        var result = await _legalService.GetHealthAsync(ct);
        return result is null ? StatusCode(503) : Ok(result);
    }
    
    // ── Weakness ──────────────────────────────────────────────────────────────

    [HttpPost("weakness")]
    [ProducesResponseType(typeof(ConanAnswerEnvelope), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> AnalyzeWeakness(
        [FromBody] ConanWeaknessRequest request, CancellationToken ct)
        => await ExecuteAsync(() => _legalService.AnalyzeWeaknessAsync(request, ct));

    [HttpPost("weakness/upload")]
    [ProducesResponseType(typeof(ConanAnswerEnvelope), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> AnalyzeWeaknessFromFile(
        IFormFile file,
        [FromForm] string? evidence = null,
        [FromForm] string? defendantStatement = null,
        CancellationToken ct = default)
        => await ExecuteAsync(
            () => _legalService.AnalyzeWeaknessFromFileAsync(file, evidence, defendantStatement, ct));

    //---Updated using history-------------------

    [HttpPost("{chatSessionId:int}/weakness/upload")]
    [ProducesResponseType(typeof(ConanAnswerEnvelope), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> AnalyzeWeaknessFromFile(
    IFormFile file,
    [FromRoute] int chatSessionId,
    [FromForm] string? evidence = null,
    [FromForm] string? defendantStatement = null,
    CancellationToken ct = default)
    => await ExecuteAsync(() => _chatBotService.AnalyzeWeaknessAndSaveAsync(
        LawyerId, chatSessionId, file, evidence, defendantStatement, ct));


    // ── Defense ───────────────────────────────────────────────────────────────

    [HttpPost("defense")]
    [ProducesResponseType(typeof(ConanAnswerEnvelope), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GenerateDefenseMemo(
        [FromBody] ConanDefenseRequest request, CancellationToken ct)
        => await ExecuteAsync(() => _legalService.GenerateDefenseMemoAsync(request, ct));

    [HttpPost("defense/upload")]
    [ProducesResponseType(typeof(ConanAnswerEnvelope), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GenerateDefenseMemoFromFile(
        IFormFile file,
        [FromForm] string? weaknesses = null,
        [FromForm] string? evidence = null,
        [FromForm] string? defendantStatement = null,
        CancellationToken ct = default)
        => await ExecuteAsync(
            () => _legalService.GenerateDefenseMemoFromFileAsync(file, weaknesses, evidence, defendantStatement, ct));

    //----Updated Defence using history-----------------------

    [HttpPost("{chatSessionId:int}/defense/upload")]
    [ProducesResponseType(typeof(ConanAnswerEnvelope), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GenerateDefenseMemoFromFile(
        IFormFile file,
        [FromRoute] int chatSessionId,
        [FromForm] string? weaknesses = null,
        [FromForm] string? evidence = null,
        [FromForm] string? defendantStatement = null,
        CancellationToken ct = default)
        => await ExecuteAsync(() => _chatBotService.GenerateDefenseMemoAndSaveAsync(
        LawyerId, chatSessionId, file, weaknesses, evidence, defendantStatement, ct));


    // ── Summarize ─────────────────────────────────────────────────────────────

    [HttpPost("summarize")]
    [ProducesResponseType(typeof(ConanSummarizeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> Summarize(
        [FromBody] ConanSummarizeRequest request, CancellationToken ct)
        => await ExecuteAsync(() => _legalService.SummarizeAsync(request, ct));

    [HttpPost("summarize/upload")]
    [ProducesResponseType(typeof(ConanSummarizeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> SummarizeFromFile(
        IFormFile file, CancellationToken ct)
        => await ExecuteAsync(() => _legalService.SummarizeFromFileAsync(file, ct));

    //----Updated Summarize using history-----------------------

    [HttpPost("{chatSessionId:int}/summarize/upload")]
    [ProducesResponseType(typeof(ConanSummarizeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> SummarizeFromFile(
       IFormFile file, [FromRoute] int chatSessionId, CancellationToken ct)
       => await ExecuteAsync(() => _chatBotService.SummarizeFromFileAsyncAndSaveAsync(
        LawyerId, chatSessionId, file,ct));

    // ── Helper ────────────────────────────────────────────────────────────────

    private async Task<IActionResult> ExecuteAsync<T>(Func<Task<ConanApiResult<T>>> call)
    {
        var result = await call();
        return result.Status switch
        {
            ConanApiResultStatus.Success =>
                Ok(result.Data),
            ConanApiResultStatus.ServiceBusy =>
                StatusCode(StatusCodes.Status503ServiceUnavailable,
                    new { message = result.ErrorDetail ?? "الخدمة مشغولة حالياً. يرجى المحاولة مرة أخرى." }),
            ConanApiResultStatus.BadRequest =>
                BadRequest(new { message = result.ErrorDetail ?? "طلب غير صالح." }),
            _ =>
                StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "حدث خطأ غير متوقع. يرجى المحاولة لاحقاً." })
        };
    }
}