using UnityEngine; // Unity 엔진 기본 기능 사용
using UnityEngine.EventSystems; // UI 이벤트 시스템 기능 사용 (포커스 설정 등)

public class GenericWindow : MonoBehaviour // 모든 창(Window)의 기반이 되는 추상 클래스
{
    public GameObject firstSelectrd; // 창이 열릴 때 처음으로 포커스를 받을 UI 오브젝트

    public virtual void Init(WindowManager mgr) { } // WindowManager 참조를 전달받아 초기화하는 가상 메서드 (하위 클래스에서 재정의)
    public virtual void Open() // 창을 여는 가상 메서드 (하위 클래스에서 재정의 가능)
    {
        gameObject.SetActive(true); // 창 오브젝트를 활성화
        if (firstSelectrd != null)
            EventSystem.current.SetSelectedGameObject(firstSelectrd); // 지정한 UI 요소에 포커스 설정
    }
    public virtual void Close() // 창을 닫는 가상 메서드 (하위 클래스에서 재정의 가능)
    {
        gameObject.SetActive(false); // 창 오브젝트를 비활성화
    }
}
