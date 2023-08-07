namespace HotReloadableConfig.Books.Pocos;

public record Book(Guid Id, string Title, Author Author);

public record Author(string Name);