namespace SqlViewerConfig
{
    public sealed class Settings
    {
        public int KeyOne { get; set; }
        public bool KeyTwo { get; set; }
        public NestedSettings KeyThree { get; set; } = null!;

        public string[] Languages { get; set; }
        public string SelectedLanguage { get; set; }
    }

    public sealed class NestedSettings
    {
        public string Message { get; set; } = null!;
    }
}