using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class LocallizationText : MonoBehaviour
{
#if UNITY_EDITOR
    public Languages editorLang;
#endif

    public string Id;
    public TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void OnEnable()
    {
        if (Application.isPlaying)
        {
            OnChangedId();
        }
#if UNITY_EDITOR
        else
        {
            OnChangeLanguage(editorLang);
        }
#endif
    }

    private void OnValidate()
    {
        if (text == null)
            text = GetComponent<TextMeshProUGUI>();
#if UNITY_EDITOR
        OnChangeLanguage(editorLang);
#endif
    }

    public void OnChangedId()
    {
        if (text == null) return;
        text.text = DataTableManager.StringTable.Get(Id);
    }

#if UNITY_EDITOR
    private void OnChangeLanguage(Languages lang)
    {
        if (text == null) return;
        DataTableManager.ChangeLanguage(lang);
        text.text = DataTableManager.StringTable.Get(Id);
    }

    [ContextMenu]("ChangeLanguage")
#endif
}