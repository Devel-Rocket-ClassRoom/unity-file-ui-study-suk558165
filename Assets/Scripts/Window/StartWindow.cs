using UnityEngine;
using UnityEngine.UI;

public class StartWindow : GenericWindow // 시작 화면 창을 담당하는 클래스 (GenericWindow 상속)
{

    public Button continueButton; // 이어하기 버튼

    public Button startButton; // 새 게임 시작 버튼

    public Button optionButton; // 옵션 화면으로 이동 버튼

    public bool canContinue; // 이어하기 가능 여부 (저장 데이터 존재 시 true)
    private WindowManager windowManager; // 창 전환을 관리하는 WindowManager 참조



    private void Awake() // 오브젝트 초기화 시 버튼 이벤트 등록
    {
        continueButton.onClick.AddListener(OnContinue); // 이어하기 버튼 클릭 이벤트 등록
        startButton.onClick.AddListener(OnNewGame); // 새 게임 버튼 클릭 이벤트 등록
        optionButton.onClick.AddListener(OnOption); // 옵션 버튼 클릭 이벤트 등록
    }
    public override void Init(WindowManager mgr) // WindowManager 참조를 전달받아 초기화
    {
        windowManager = mgr; // 외부에서 전달받은 WindowManager를 멤버 변수에 저장
    }

    public override void Open() // 창을 열 때 이어하기 버튼 활성화 여부를 결정
    {
        base.Open(); // 부모 클래스의 Open 호출 (창 표시 및 포커스 설정)
        continueButton.gameObject.SetActive(canContinue); // 이어하기 가능 여부에 따라 버튼 표시/숨김
        if (!canContinue) // 이어하기가 불가능할 때
        {
            firstSelectrd = startButton.gameObject; // 포커스를 새 게임 버튼으로 설정
        }
    }
    public override void Close() // 창을 닫는 메서드
    {
        base.Close(); // 부모 클래스의 Close 호출 (창 숨김 처리)
    }

    public void OnContinue() // 이어하기 버튼 클릭 시 호출되는 메서드
    {
        windowManager.Open(1); // StartWindow id
        Debug.Log("버튼 누름"); // 디버그용 로그 출력

    }

    public void OnNewGame() // 새 게임 버튼 클릭 시 호출되는 메서드
    {
        Debug.Log("버튼 누름"); // 디버그용 로그 출력
    }

    public void OnOption() // 옵션 버튼 클릭 시 호출되는 메서드
    {
        Debug.Log("버튼 누름"); // 디버그용 로그 출력
    }
}
