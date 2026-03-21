using System.Collections.ObjectModel;
using SqlViewer.Shared.Dtos.Metadata;

namespace SqlViewer.ViewModels;

public sealed class TableViewModel : TreeItemViewModel
{
    private bool _isExpanded;
    public TableViewModel()
    {
        Columns = [];
    }

    public ObservableCollection<ColumnViewModel> Columns { get; }

    public bool IsExpanded
    {
        get => _isExpanded;
        set
        {
            _isExpanded = value;
            OnPropertyChanged();
            if (_isExpanded && Columns.Count == 0) LoadColumns();
        }
    }

    public Func<string, Task<IEnumerable<ColumnInfoResponseDto>>> LoadColumnsFunc { get; set; }

    private async void LoadColumns()
    {
        IEnumerable<ColumnInfoResponseDto> cols = await LoadColumnsFunc(Name);
        foreach (ColumnInfoResponseDto c in cols)
        {
            Columns.Add(new ColumnViewModel { Name = $"{c.ColumnName} ({c.NativeColumnType}, {c.ColumnType})" });
        }
    }
}
