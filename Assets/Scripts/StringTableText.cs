using UnityEngine;
using TMPro;
public class StringTableText : MonoBehaviour
{
    public string Id;
    public TextMeshProUGUI text;

    private void Start()
    {
        OnChangedId();
    }

    private void OnChangedId()
    {
        text.text = DataTableManager.StringTable.Get(Id);
    }
}
