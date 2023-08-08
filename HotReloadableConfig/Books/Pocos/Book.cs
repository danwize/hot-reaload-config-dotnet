namespace HotReloadableConfig.Books.Pocos;

public record Book(Guid Id, string Title, Author Author, int NumberOfCopiesSold = 0, DateTime? LastModifiedDate = null);

public record Author(string Name);
