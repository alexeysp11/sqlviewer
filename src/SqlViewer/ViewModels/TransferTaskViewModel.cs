namespace SqlViewer.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using SqlViewer.Shared.Dtos.Etl;

public partial class TransferTaskViewModel : ObservableObject
{
    public Guid CorrelationId { get; set; }
    public string TableName { get; set; }

    [ObservableProperty]
    private double _progress;

    [ObservableProperty]
    private string _status;

    [ObservableProperty]
    private bool _isCompleted;

    /// <summary>
    /// Updates the view model state from the DTO.
    /// </summary>
    public void ApplyChanges(TransferStatusResponseDto dto)
    {
        Progress = dto.Progress;
        Status = dto.StatusMessage;
        IsCompleted = dto.IsFinalState;
    }
}
