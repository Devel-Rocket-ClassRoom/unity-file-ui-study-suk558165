using System.Collections.Generic;
using UnityEngine;

public static class DataTableManager
{
    private static readonly Dictionary<string, DataTable> tables = new Dictionary<string, DataTable>();

    public static StringTable StringTable => Get<StringTable>(DatableIds.String);
    public static ItemTable ItemTable => Get<ItemTable>(DatableIds.Item);

    public static CharacterTable CharacterTable => Get<CharacterTable>(DatableIds.Character);
#if UNITY_EDITOR
    public static StringTable GetStringTable(Languages lang)
    {
        return Get<StringTable>(DatableIds.StringTableIds[(int)lang]);
    }
#endif

    static DataTableManager()
    {
        Init();
    }

    private static void Init()
    {
#if UNITY_EDITOR
        foreach (var id in DatableIds.StringTableIds)
        {
            var stringTable = new StringTable();
            stringTable.Load(id);
            tables.Add(id, stringTable);
        }
#else
        var stringTable = new StringTable();
        stringTable.Load(DatableIds.String);
        tables.Add(DatableIds.String, stringTable);
#endif
        var itemTable = new ItemTable();
        itemTable.Load(DatableIds.Item);
        tables.Add(DatableIds.Item, itemTable);

        var characterTable = new CharacterTable();
        characterTable.Load(DatableIds.Character);
        tables.Add(DatableIds.Character, characterTable);
    }

    public static T Get<T>(string Id) where T : DataTable
    {
        if (!tables.ContainsKey(Id))
        {
            Debug.Log("테이블 없음");
            return null;
        }
        return tables[Id] as T;
    }

    public static void ChangeLanguage(Languages lang)
    {
        Varuables.language = lang;
        string id = DatableIds.String;

#if !UNITY_EDITOR
        if (tables.ContainsKey(id))
            tables.Remove(id);
        var stringTable = new StringTable();
        stringTable.Load(id);
        tables.Add(id, stringTable);
#endif

        foreach (var t in Object.FindObjectsByType<LocallizationText>(FindObjectsSortMode.None))
            t.OnChangedId();
    }
}