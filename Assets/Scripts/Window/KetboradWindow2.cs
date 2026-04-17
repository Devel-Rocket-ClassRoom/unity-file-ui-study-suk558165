using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KetboradWindow2 : GenericWindow
{
    private readonly StringBuilder sb = new StringBuilder();

    public TextMeshProUGUI inputFiled;

    public GameObject rootKeyborad;

    public int maxChatacters = 7;

    private float timer = 0f;

    private float cursorDelay = 0.5f;

    private bool blink;
    private List<Button> keys;


    private void Awake()
    {
        keys = new List<Button>(rootKeyborad.GetComponentsInChildren<Button>());
        foreach (var key in keys)
        {
            var text = key.GetComponentInChildren<TextMeshProUGUI>();
            key.onClick.AddListener(() => OnKey(text.text));
        }
    }
    public override void Open()
    {
        sb.Clear();
            timer = 0f;
        blink = false;
        base.Open();
        UpdateInputFiled();
    }

    public override void Close()
    {
        base.Close();
    }


    public void Update()
    {
        timer += Time.deltaTime;
        if  (timer > cursorDelay)
        {
            timer = 0f;
            blink = !blink;
            UpdateInputFiled();
        }
    }
    public void OnKey(string key)
    {
        sb.Append(key);
        UpdateInputFiled();

    }

    private void UpdateInputFiled()
    {
        bool showCursor = sb.Length < maxChatacters && !blink;
        if (showCursor)
        {
            sb.Append('_');
        }
        inputFiled.SetText(sb);
        if (showCursor)
        {
            sb.Length -= 1;
        }
        
    }

    public void OnDelete()
    {
        if (sb.Length > 0)
        {
            sb.Length -= 1;
            UpdateInputFiled();
        }
    }

    public void OnCancel()
    {
        sb.Clear();
        UpdateInputFiled();
    }

    public void OnAccept()
    {

    }
}


