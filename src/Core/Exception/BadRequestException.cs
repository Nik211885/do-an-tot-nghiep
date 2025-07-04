namespace Core.Exception;

public class BadRequestException(string mess) : System.Exception(mess);
