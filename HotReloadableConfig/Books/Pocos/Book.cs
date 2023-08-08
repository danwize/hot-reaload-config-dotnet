namespace HotReloadableConfig.Books.Pocos;

public record Book(Guid Id, string Title, Author Author, DateTime? LastModifiedDate = null);

public record Author(string Name);