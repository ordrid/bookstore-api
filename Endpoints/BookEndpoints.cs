using BookStoreApi.Models;
using BookStoreApi.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using static Microsoft.AspNetCore.Http.TypedResults;

namespace BookStoreApi.Endpoints;

public static class BookEndpoints
{
    public static void MapBookEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("api/books").WithOpenApi();

        group
            .MapGet("", GetBooksAsync)
            .WithName("Get Books")
            .WithDescription("Gets all books from the database.");

        group
            .MapGet("{id:length(24)}", GetBookAsync)
            .WithName("Get Book")
            .WithDescription("Gets a book by ID from the database.");

        group
            .MapPost("", CreateBookAsync)
            .WithName("Create New Book")
            .WithDescription("Inserts a new book");

        group
            .MapPut("{id:length(24)}", UpdateBookAsync)
            .WithName("Update Book")
            .WithDescription("Updates a book in the database.");

        group
            .MapDelete("{id:length(24)}", DeleteBookAsync)
            .WithName("Remove Book")
            .WithDescription("Deletes a book in the database.");
    }

    private static async Task<Results<NoContent, NotFound<string>>> DeleteBookAsync(
        BookService _bookService,
        string id
    )
    {
        var book = await _bookService.GetBookAsync(id);

        if (book is null)
        {
            return NotFound("No books found to delete.");
        }

        await _bookService.RemoveBookAsync(id);

        return NoContent();
    }

    private static async Task<Results<NoContent, NotFound<string>>> UpdateBookAsync(
        BookService _bookService,
        string id,
        Book updatedBook
    )
    {
        var book = await _bookService.GetBookAsync(id);

        if (book is null)
        {
            return NotFound("No books found to update.");
        }

        updatedBook.Id = book.Id;

        await _bookService.UpdateBookAsync(id, updatedBook);

        return NoContent();
    }

    private static async Task<CreatedAtRoute> CreateBookAsync(
        BookService _bookService,
        Book newBook
    )
    {
        await _bookService.CreateBookAsync(newBook);

        return CreatedAtRoute("GetBook", newBook);
    }

    private static async Task<Results<Ok<List<Book>>, NotFound<string>>> GetBooksAsync(
        BookService _bookService
    )
    {
        var books = await _bookService.GetBooksAsync();

        if (books is null || books.Count == 0)
        {
            return NotFound("No books found.");
        }

        return Ok(books);
    }

    private static async Task<Results<Ok<Book>, NotFound<string>>> GetBookAsync(
        BookService _bookService,
        string id
    )
    {
        var book = await _bookService.GetBookAsync(id);

        if (book is null)
        {
            return NotFound($"Book with ID {id} was not found.");
        }

        return Ok(book);
    }
}
