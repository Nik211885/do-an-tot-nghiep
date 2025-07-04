    using Application.Interfaces.CQRS;
    using Application.Interfaces.UnitOfWork;
    using Core.Interfaces;
    using Core.Interfaces.Repositories;
    using Microsoft.Extensions.Logging;

    namespace Application.Behaviors;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="unitOfWork"></param>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class TransactionBehavior<TCommand, TResponse>(
            ILogger<TransactionBehavior<TCommand, TResponse>> logger,
            IUnitOfWorkFactory unitOfWorkFactory
        ) : IPipelineBehavior<TCommand, TResponse> 
        where TCommand : ICommand<TResponse>
    {
        private readonly ILogger<TransactionBehavior<TCommand, TResponse>> _logger = logger;
        private readonly IUnitOfWork _unitOfWork = unitOfWorkFactory.GetUnitOfWorkFor<TCommand>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="next"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<TResponse> Handle(TCommand request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var response = default(TResponse);
            var requestTypeName = typeof(TCommand).FullName;
            try
            {
                // If you open transaction in application in here it just execute
                // and don't open new transaction in behavior transaction 
                if (_unitOfWork.HasActiveTransaction)
                {
                    _logger.LogInformation("Transaction is already exiting {requestName}, {request}", requestTypeName, request);
                    return await next();
                }
                // When in commit transaction I have process errors I has roll back in commit 
                // So in here you don't need call roll back method
                await _unitOfWork.ExecutionStrategyRetry(async () =>
                {
                    
                    await _unitOfWork.BeginTransactionAsync(cancellationToken);
                    Guid? transactionId = _unitOfWork.CurrentTransactionId;
                    _logger.LogInformation("Beginning new transaction is [transactionId] :{transactionId} request: {requestName} ",
                        transactionId, request);
                    response = await next();
                    await _unitOfWork.CommitTransactionAsync(cancellationToken);
                    _logger.LogInformation("Completed process transactions is [transactionId] : {transactionId} and request :{requestName}",
                        transactionId,requestTypeName);
                });
                return response!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling transaction behavior for {commandName}, {command}", requestTypeName, request);
                throw;
            }
        }
    }
