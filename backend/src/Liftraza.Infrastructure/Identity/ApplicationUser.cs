using Microsoft.AspNetCore.Identity;

namespace Liftraza.Infrastructure.Identity;

public sealed class ApplicationUser : IdentityUser
{
	public string? DisplayName { get; set; }
}