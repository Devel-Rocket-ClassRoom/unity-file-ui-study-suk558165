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

    [ContextMenu("한국어로 전체 변경")]
    private void SetAllKorean() => SetAllLanguage(Languages.Korean);

    [ContextMenu("영어로 전체 변경")]
    private void SetAllEnglish() => SetAllLanguage(Languages.English);

    [ContextMenu("일본어로 전체 변경")]
    private void SetAllJapanese() => SetAllLanguage(Languages.Japanese);

    private void SetAllLanguage(Languages lang)
    {
        editorLang = lang;
        foreach (var t in FindObjectsByType<LocallizationText>(FindObjectsSortMode.None))
        {
            t.editorLang = lang;
            t.OnChangeLanguage(lang);
        }
    }
#endif
}