
namespace WhiskyWine.BottleService.Domain.Interfaces
{
    public interface IMapper<S, T>
    {
        T Map(S fromType);
    }
}
