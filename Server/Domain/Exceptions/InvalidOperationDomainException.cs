namespace Server.Domain.Exceptions;

public class InvalidOperationDomainException(string message) : Exception(message);