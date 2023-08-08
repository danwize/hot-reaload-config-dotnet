using HotReloadableConfig.Books;
using HotReloadableConfig.Books.Pocos;

namespace HotReloadableConfig.GraphQl;

public class Mutate
{
    public Task<Book> AddBook([Service] IBooksRepository booksRepository, AddBookRequest addBookRequest)
    {
        return booksRepository.AddAsync(addBookRequest.ToBook());
    }

    public Task<Book> UpdateBook([Service] IBooksRepository booksRepository, Guid id, UpdateBookRequest updateRequest)=>booksRepository.UpdateAsync(updateRequest.ToBook(id));
}

public record AddBookRequest(string Title, Author Author) 
{
    public Book ToBook()
    {
        return new Book(Id: Guid.NewGuid(), Title: Title, Author: Author);
    }
}

public record UpdateBookRequest(string Title, Author Author) 
{
    public Book ToBook(Guid id)
    {
        return new Book(id, Title, Author);
    }
}

