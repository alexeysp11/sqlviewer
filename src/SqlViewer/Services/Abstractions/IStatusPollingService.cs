using SqlViewer.ViewModels;
using System.Collections.ObjectModel;

namespace SqlViewer.Services.Abstractions;

public interface IStatusPollingService : IDisposable
{
    void StartPolling(ObservableCollection<TransferTaskViewModel> tasks, ObservableCollection<string> executionLogs);
    void StopPolling();
}
