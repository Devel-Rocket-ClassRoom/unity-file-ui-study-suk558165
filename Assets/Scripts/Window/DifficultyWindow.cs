using UnityEngine; // Unity 엔진 기본 기능 사용
using UnityEngine.UI; // Unity 기본 UI 컴포넌트 (Toggle, Button 등) 사용

public class DifficultyWindow : GenericWindow // 난이도 선택 창을 담당하는 클래스 (GenericWindow 상속)
{
    public Toggle[] toggles;        // 난이도 선택 토글 배열 (인덱스 0: Easy, 1: Normal, 2: Hard)
    public int selected;            // 현재 선택된 난이도 인덱스

    public WindowManager windowManager;  // 창 전환을 관리하는 WindowManager 참조

    public Button cancelButton;     // 변경 취소 버튼
    public Button applyButton;      // 변경 적용 버튼

    private void Awake() // 오브젝트 초기화 시 토글 및 버튼 이벤트 등록
    {
        // 각 토글의 값 변경 이벤트에 난이도 선택 메서드 등록
        toggles[0].onValueChanged.AddListener(OnEasy); // Easy 토글 이벤트 등록
        toggles[1].onValueChanged.AddListener(OnNormal); // Normal 토글 이벤트 등록
        toggles[2].onValueChanged.AddListener(OnHard); // Hard 토글 이벤트 등록
        // 버튼 클릭 이벤트 등록
        cancelButton.onClick.AddListener(OnCancel); // 취소 버튼 클릭 이벤트 등록
        applyButton.onClick.AddListener(OnApply); // 적용 버튼 클릭 이벤트 등록
    }

    public override void Open() // 창을 열 때 저장된 난이도 값을 불러와 적용하는 메서드
    {
        base.Open();                                        // 부모 클래스의 Open 호출 (창 표시 처리)
        selected = SaveLoadManager.Data.Difficulty;         // 저장된 난이도 값을 불러와 selected에 설정
        toggles[selected].isOn = true;                      // 저장된 난이도에 해당하는 토글을 활성화
    }

    public override void Close() // 창을 닫는 메서드
    {
        base.Close();  // 부모 클래스의 Close 호출 (창 숨김 처리)
    }

    public void OnEasy(bool active) // Easy 토글 값 변경 시 호출되는 메서드
    {
        if (active) selected = 0;  // Easy 토글이 켜졌을 때 selected를 0으로 설정
    }

    public void OnNormal(bool active) // Normal 토글 값 변경 시 호출되는 메서드
    {
        if (active) selected = 1;  // Normal 토글이 켜졌을 때 selected를 1로 설정
    }

    public void OnHard(bool active) // Hard 토글 값 변경 시 호출되는 메서드
    {
        if (active) selected = 2;  // Hard 토글이 켜졌을 때 selected를 2로 설정
    }

    public void OnApply() // 적용 버튼 클릭 시 난이도를 저장하고 시작 화면으로 전환하는 메서드
    {
        SaveLoadManager.Data.Difficulty = selected;  // 현재 selected 값을 데이터에 반영
        SaveLoadManager.Save();                      // 변경된 데이터를 파일에 저장
        windowManager.Open(0);                       // 시작 화면(인덱스 0)으로 전환
    }

    public void OnCancel() // 취소 버튼 클릭 시 저장하지 않고 시작 화면으로 전환하는 메서드
    {
        windowManager.Open(0);  // 변경 사항을 저장하지 않고 시작 화면으로 전환
    }

    public override void Init(WindowManager mgr) // WindowManager 참조를 전달받아 초기화
    {
        windowManager = mgr;  // 외부에서 전달받은 WindowManager를 멤버 변수에 저장
    }
}
