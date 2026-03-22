using FluentAssertions;
using SqlViewer.Shared.Helpers.DataTransfer;

namespace SqlViewer.Shared.Tests.Helpers.DataTransfer;

public class DataTransferCursorHelperTests
{
    [Fact]
    public void EncodeAndDecode_ShouldReturnOriginalValues()
    {
        // Arrange
        DateTime originalDate = new(2023, 10, 27, 10, 0, 0, DateTimeKind.Utc);
        long originalId = 12345;

        // Act
        string token = DataTransferCursorHelper.EncodeCursor(originalDate, originalId);
        (DateTime CreatedAt, long Id)? result = DataTransferCursorHelper.DecodeCursor(token);

        // Assert
        result.Should().NotBeNull();
        result!.Value.CreatedAt.Should().Be(originalDate);
        result!.Value.Id.Should().Be(originalId);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("invalid-base64")]
    public void DecodeCursor_WithInvalidInput_ShouldReturnNull(string? input)
    {
        // Act
        (DateTime CreatedAt, long Id)? result = DataTransferCursorHelper.DecodeCursor(input);

        // Assert
        result.Should().BeNull();
    }
}
