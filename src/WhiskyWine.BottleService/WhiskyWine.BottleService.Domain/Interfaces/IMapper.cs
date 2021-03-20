using System.Collections.Generic;

namespace WhiskyWine.BottleService.Domain.Interfaces
{
    /// <summary>
    /// Interface that specifies the contract that must be met by classes acting as mappers/ adapters between models in different project layers.
    /// Generic parameter S specifies the type to map from.
    /// Generic parameter T specifies the type to map to.
    /// </summary>
    public interface IMapper<S, T>
    {
        /// <summary>
        /// Maps an object of type S to an object of type T.
        /// </summary>
        /// <param name="from">The object of type S to map from.</param>
        /// <returns>Object of type T resulting from mapping.</returns>
        T MapOne(S from);

        IEnumerable<T> MapMany(IEnumerable<S> from);
    }
}
