using MediatR;

namespace Router.Models.Command;

public record DeviceCommandRequest(string DeviceId, string Command) : IRequest;
