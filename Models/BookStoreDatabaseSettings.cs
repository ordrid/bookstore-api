namespace BookStoreApi.Models;

public sealed class BookStoreDatabaseSettings
{
    public required string ConnectionString { get; set; }
    public required string DataBaseName { get; set; }
    public required string BookCollectionName { get; set; }
}