using Application.Interfaces.CQRS;
using Core.Interfaces;

namespace Application.Interfaces.UnitOfWork;

public interface IUnitOfWorkFactory
{
    IUnitOfWork GetUnitOfWorkFor<TCommand>(); 
}
