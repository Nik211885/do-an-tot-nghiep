namespace Application.Interfaces.Elastic;

public interface IElasticFactory
{
    IElasticServices<TType> GetInstance<TType>() where TType : class;   
}
