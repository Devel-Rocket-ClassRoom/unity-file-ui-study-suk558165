public enum Languages
{
    Korean,
    English,
    Japanese,
}

public static class Varuables
{
    public static Languages language = Languages.Korean;
}

public static class DatableIds
{
    public static readonly string[] StringTableIds =
    {
        "StringTableKr",
        "StringTableEn",
        "StringTableJp",
    };

    public static string String => StringTableIds[(int)Varuables.language];
}