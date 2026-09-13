namespace Server.Domain.Exceptions;

public class NetworkCommunicationException(string message, Exception? innerException = null) 
    : Exception(message, innerException);