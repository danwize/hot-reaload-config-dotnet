using HotReloadableConfig.Books;
using HotReloadableConfig.Books.Pocos;

namespace HotReloadableConfig.GraphQl;

public class Mutate
{
    public Task<Book> AddBook([Service] IBooksRepository booksRepository, BookToAdd bookToAdd)
    {
        return booksRepository.AddAsync(bookToAdd.ToBook());
    }
}

public record BookToAdd(string Title, Author Author) 
{
    


    public Book ToBook()
    {
        return new Book(Id: Guid.NewGuid(), Title: this.Title, Author: this.Author);
    }
}