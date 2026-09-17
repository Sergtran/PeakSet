namespace Liftraza.Application.Exceptions;

public sealed class InvalidCredentialsException : Exception
{
	public InvalidCredentialsException()
		: base("Incorrect email or password.")
	{
	}
}