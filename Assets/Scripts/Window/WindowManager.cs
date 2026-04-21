using UnityEngine; // Unity 엔진 기본 기능 사용

public class WindowManager : MonoBehaviour // 여러 창(Window)의 열기/닫기를 관리하는 매니저 클래스
{
    public GenericWindow[] windows;  // 관리할 모든 창 배열

    public int currentWindowId;      // 현재 열려 있는 창의 인덱스
    public int defaultWindowId;      // 시작 시 기본으로 열 창의 인덱스
    public WindowManager windowManager;  // 외부 WindowManager 참조 (필요 시 사용)

    private void Awake() // 씬 시작 시 모든 창을 초기화하고 기본 창을 여는 메서드
    {
        foreach (var window in windows)
        {
            window.gameObject.SetActive(false);  // 모든 창을 비활성화 상태로 초기화
            window.Init(this);                   // 각 창에 이 WindowManager 참조 전달
        }
        currentWindowId = defaultWindowId;           // 현재 창을 기본 창으로 설정
        windows[currentWindowId].Open();             // 기본 창 열기
    }

    public GenericWindow Open(int id) // 지정한 인덱스의 창을 열고 현재 창을 닫는 메서드
    {
        windows[currentWindowId].Close();  // 현재 열려 있는 창 닫기
        currentWindowId = id;              // 현재 창 인덱스를 새 id로 변경
        windows[currentWindowId].Open();   // 새 창 열기

        return windows[currentWindowId];   // 열린 창 반환
    }
}
