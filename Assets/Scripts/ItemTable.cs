using System.Collections.Generic;
using UnityEngine;

public class ItemData
{
    public string Id { get; set; }
    public ItemTytpes Type { get; set; }
    public string Name { get; set; }
    public string Desc { get; set; }

    public int Value { get; set; }
    public int Cost { get; set; }
    public string Icon { get; set; }

    public string SringName => DataTableManager.StringTable.Get(Name);
    public string SringDesc => DataTableManager.StringTable.Get(Desc);

    public Sprite SpriteIcon => Resources.Load<Sprite>($"Icon/{Icon}");

    public override string ToString()
    {
        return $"{Id}/ {Type}/{Name}/{Desc}/{Value}/{Cost}/{Icon}";
    }
}

public class ItemTable : DataTable
{

    private readonly Dictionary<string, ItemData> table = new Dictionary<string, ItemData>();
    public override void Load(string filename)
    {
        table .Clear();

        string path = string.Format(FormatPath, filename);
        TextAsset textAsset = Resources.Load<TextAsset>(path);
        List<ItemData> list = LoadCSV<ItemData>(textAsset.text);

        foreach( var item in list )
        {
            if ( !table.ContainsKey(item.Id) )
            {
                table.Add(item.Id, item);
            }
            else
            {
                Debug.LogError("아이템 아이디 중복");
            }
        }

    }
      public ItemData Get(string id)
    {
        if (!table.ContainsKey(id))
        {
            Debug.LogError("아이템 아이디 없음");
            return null;
        }
        return table[id];
    }
}
