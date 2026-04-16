using UnityEngine;
using UnityEngine.UI;

public class StartWindow : GenericWindow
{

    public Button continueButton;

    public Button startButton;

    public Button optionButton;

    public bool canContinue;
    private WindowManager windowManager;

    private void Awake()
    {
        continueButton.onClick.AddListener(OnContinue);
        startButton.onClick.AddListener(OnNewGame);
        optionButton.onClick.AddListener(OnOption);
    }
    public void Init(WindowManager mgr)
    {
        windowManager = mgr;
    }

    public override void Open()
    {
        base.Open();
        continueButton.gameObject.SetActive(canContinue);
        if (!canContinue)
        {
            firstSelectrd = startButton.gameObject;
        }
    }
    public override void Close()
    {
        base.Close();
    }

    public void OnContinue()
    {
        Debug.Log("버튼 누름");

    }

    public void OnNewGame()
    {
        Debug.Log("버튼 누름");
    }

    public void OnOption()
    {
        Debug.Log("버튼 누름");
    }
}
