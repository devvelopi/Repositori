namespace Repositori.Core.Model
{
    /// <summary>
    /// Tag used on data objects to use within a repository
    /// </summary>
    /// <typeparam name="TIdentifier">The identifier type</typeparam>
    public interface IIdentifiable<TIdentifier>
    {
        /// <summary>
        /// Unique identifier of the data object
        /// </summary>
        TIdentifier Id { get; set; }
    }
}