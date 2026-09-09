namespace PeakSet.Application.Exceptions;

public sealed class InvalidCredentialsException : Exception
{
	public InvalidCredentialsException()
		: base("Email o contraseña incorrectos.")
	{
	}
}