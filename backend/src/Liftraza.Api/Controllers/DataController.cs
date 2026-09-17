using System.Security.Claims;
using Liftraza.Application.Abstractions;
using Liftraza.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Liftraza.Api.Controllers;

[ApiController]
[Route("api/data")]
[Authorize]
public class DataController : ControllerBase
{
	private readonly IBackupService _backupService;

	public DataController(IBackupService backupService)
		=> _backupService = backupService;

	private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)
		?? throw new InvalidOperationException("User id claim not found.");

	[HttpGet("export")]
	[ProducesResponseType(typeof(LiftrazaExportDto), StatusCodes.Status200OK)]
	public async Task<ActionResult<LiftrazaExportDto>> Export(CancellationToken ct)
		=> Ok(await _backupService.ExportAsync(UserId, ct));

	[HttpPost("import")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> Import(LiftrazaExportDto snapshot, CancellationToken ct)
	{
		await _backupService.ImportAsync(UserId, snapshot, ct);
		return NoContent();
	}
}