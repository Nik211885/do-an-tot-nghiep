namespace Application.Exceptions;

public class NotFoundException(string entityName, Dictionary<string, string>? identifier)
    : Exception($"Không tìm thấy {entityName} {(identifier is not null ? "Có" : string.Empty)} {identifier?.Keys} là {identifier?.Values}");
