using CommunityToolkit.Mvvm.ComponentModel;

namespace SqlViewer.ViewModels;

public abstract class TreeItemViewModel : ObservableObject
{
    public string Name { get; set; }
}
