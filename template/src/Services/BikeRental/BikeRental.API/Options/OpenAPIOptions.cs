namespace BikeRental.API.Options;
public record OpenApiOptions
{
    public required EndpointOptions Endpoint { get; init; }
    public required List<DocumentOptions> Documents { get; init; }
}

public record EndpointOptions
{
    public required string Name { get; init; }
}

public record DocumentOptions
{
    public required string Area { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Version { get; init; }
}
