using BookStoreApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BookStoreApi.Services;

public sealed class BookService
{
    private readonly IMongoCollection<Book> _bookCollection;

    public BookService(IOptions<BookStoreDatabaseSettings> bookstoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(bookstoreDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(bookstoreDatabaseSettings.Value.DataBaseName);
        _bookCollection = mongoDatabase.GetCollection<Book>(
            bookstoreDatabaseSettings.Value.BookCollectionName
        );
    }

    public async Task<List<Book>> GetBooksAsync() => await _bookCollection.Find(_ => true).ToListAsync();

    public async Task<Book?> GetBookAsync(string id) =>
        await _bookCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateBookAsync(Book newBook) => await _bookCollection.InsertOneAsync(newBook);

    public async Task UpdateBookAsync(string id, Book updateBook) =>
        await _bookCollection.ReplaceOneAsync(x => x.Id == id, updateBook);

    public async Task RemoveBookAsync(string id) =>
        await _bookCollection.DeleteOneAsync(x => x.Id == id);
}
