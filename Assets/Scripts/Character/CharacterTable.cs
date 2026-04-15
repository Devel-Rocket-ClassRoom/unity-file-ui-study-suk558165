using System.Collections.Generic;
using UnityEngine;

public class CharacterData
{
    internal Sprite Icon;
    internal Sprite SpriteIcon;

    public string Id { get; set; }
    public string Name { get; set; }
    public string JOB { get; set; }
    public int Attack { get; set; }
    public int Dffend { get; set; }
    public int Health { get; set; }

    public string StringName => DataTableManager.StringTable.Get(Name);
    public string StringJob => DataTableManager.StringTable.Get(JOB);
}

public class CharacterTable : DataTable
{
    private readonly Dictionary<string, CharacterData> table = new Dictionary<string, CharacterData>();

    public override void Load(string filename)
    {
        table.Clear();
        string path = string.Format(FormatPath, filename);
        TextAsset textAsset = Resources.Load<TextAsset>(path);
        List<CharacterData> list = LoadCSV<CharacterData>(textAsset.text);
        foreach (var item in list)
        {
            if (!table.ContainsKey(item.Id))
                table.Add(item.Id, item);
            else
                Debug.LogError("캐릭터 아이디 중복");
        }
    }

    public CharacterData Get(string id)
    {
        if (!table.ContainsKey(id))
        {
            Debug.LogError("캐릭터 아이디 없음");
            return null;
        }
        return table[id];
    }
}