public enum Languages
{
    Korean,
    English,
    Japanese,
}
public enum ItemTytpes
{
    Weapon,
    Equip,
    Consumable,
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

    public static  string Item => "ItemTable";

    public const string Character = "CharacterTable";
}