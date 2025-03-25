using Application.Interfaces.CQRS;

namespace Application.Services.Test;

public class CreateTestCommandHandler : ICommandHandler<CreateTestCommand, Guid>
{
    public async Task<Guid> Handle(CreateTestCommand request, CancellationToken cancellationToken)
    {
        var guid = Guid.CreateVersion7();
        await Task.Delay(1000, cancellationToken);
        return guid;
    }
}
