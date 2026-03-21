namespace SqlViewer.Shared.Dtos;

public record ProblemDetailsResponseDto
{
    public string? Title { get; set; }
    public string? Detail { get; set; }
    public int? Status { get; set; }
}
