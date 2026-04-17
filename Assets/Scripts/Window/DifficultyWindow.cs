using UnityEngine;
using UnityEngine.UI;

public class DifficultyWindow : GenericWindow
{
    public Toggle[] toggles;

    public int selected;

    public WindowManager windowManager;

    public Button cancelButton;
    public Button applyButton;

    private void Awake()
    {
        toggles[0].onValueChanged.AddListener(OnEasy);
        toggles[1].onValueChanged.AddListener(OnNormal);
        toggles[2].onValueChanged.AddListener(OnHard);
        cancelButton.onClick.AddListener(OnCancel);
        applyButton.onClick.AddListener(OnApply);
    }

    public override void Open()
    {
        base.Open();
        selected = SaveLoadManager.Data.Difficulty; // saveloadmanager 참고 
        toggles[selected].isOn = true;
        
    }
    public override void Close()
    {
        base.Close();
    }

    public void OnEasy(bool active)
    {
        if (active)
        {
            selected = 0;
            Debug.Log("OnEasy");
        }
    }

    public void OnNormal(bool active)
    {
        if (active)
        {

            selected = 1;
            Debug.Log("OnNormal");
        }
    }
    public void OnHard(bool active)
    {
        if (active)
        {
            selected = 2;
            Debug.Log("OnHard");
        }
    }

    public void OnApply()
    {
        SaveLoadManager.Data.Difficulty = selected; // 선택한걸 데이터에 저장
        SaveLoadManager.Save(); // 세이브 호출
        windowManager.Open(0); // 시작화면으로 돌리기
    }

    public void OnCancel()
    {
        windowManager.Open(0); // 시작하면 으로 돌리기
    }

    public override void Init(WindowManager mgr)
    {
        windowManager = mgr; // WindowManager 참조 저장
    }
}
