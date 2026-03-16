namespace SqlViewer.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;

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
}
