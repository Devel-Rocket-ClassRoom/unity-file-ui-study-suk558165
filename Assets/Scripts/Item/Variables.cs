using System;

// 게임 전역에서 공유하는 런타임 변수와 이벤트를 관리하는 정적 클래스
public static class Variables
{
    public static Languages language; // 현재 선택된 언어 (DatableIds.String 경로 계산에 사용)

    public static event Action OnLanguageChanged; // 언어가 변경될 때 발생하는 이벤트 (LocalizationDropDown 등 구독자에게 알림)

    // OnLanguageChanged 이벤트를 발생시켜 모든 구독자에게 언어 변경을 알리는 메서드
    public static void InvokeLanguageChanged()
    {
        OnLanguageChanged?.Invoke(); // 구독자가 있는 경우에만 이벤트 호출
    }
}
