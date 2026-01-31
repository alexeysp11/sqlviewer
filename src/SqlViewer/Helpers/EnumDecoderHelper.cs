using SqlViewer.Enums.Editor;

namespace SqlViewer.Helpers;

public static class EnumDecoderHelper
{
    public static string GetFontSizeName(FontSizeEnum fontSize) => fontSize switch
    {
        FontSizeEnum.FontSize8 => "8",
        FontSizeEnum.FontSize9 => "9",
        FontSizeEnum.FontSize10 => "10",
        FontSizeEnum.FontSize11 => "11",
        FontSizeEnum.FontSize12 => "12",
        FontSizeEnum.FontSize14 => "14",
        FontSizeEnum.FontSize16 => "16",
        FontSizeEnum.FontSize18 => "18",
        FontSizeEnum.FontSize20 => "20",
        _ => throw new Exception("Unable to find font size enum")
    };

    public static string GetTabSizeName(TabSizeEnum tabSize) => tabSize switch
    {
        TabSizeEnum.TabSize1 => "1",
        TabSizeEnum.TabSize2 => "2",
        TabSizeEnum.TabSize3 => "3",
        TabSizeEnum.TabSize4 => "4",
        TabSizeEnum.TabSize5 => "5",
        TabSizeEnum.TabSize6 => "6",
        TabSizeEnum.TabSize7 => "7",
        TabSizeEnum.TabSize8 => "8",
        _ => throw new Exception("Unable to find tab size enum")
    };
}
