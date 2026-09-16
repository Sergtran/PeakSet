using System.Security.Claims;
using PeakSet.Application.Abstractions;
using PeakSet.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PeakSet.Api.Controllers;

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
	[ProducesResponseType(typeof(PeakSetExportDto), StatusCodes.Status200OK)]
	public async Task<ActionResult<PeakSetExportDto>> Export(CancellationToken ct)
		=> Ok(await _backupService.ExportAsync(UserId, ct));

	[HttpPost("import")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> Import(PeakSetExportDto snapshot, CancellationToken ct)
	{
		await _backupService.ImportAsync(UserId, snapshot, ct);
		return NoContent();
	}
}