using TestAppSmartWay.Domain.Enum;

namespace TestAppSmartWay.Application.Requests;

public record CreatePassportRequest(PassportType Type, string Number);