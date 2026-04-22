using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KetboradWindow2 : GenericWindow // 커스텀 키보드 입력 창을 담당하는 클래스 (GenericWindow 상속)
{
    private readonly StringBuilder sb = new StringBuilder(); // 현재 입력 문자열을 효율적으로 관리하는 StringBuilder

    public TextMeshProUGUI inputFiled; // 입력 내용을 표시하는 TextMeshPro 텍스트 필드

    public GameObject rootKeyborad; // 키보드 버튼들이 모여 있는 부모 오브젝트
    public Button deleteButton; // 마지막 입력 문자를 삭제하는 버튼
    public Button cancelButton; // 입력 내용을 전부 지우는 취소 버튼
    public Button acceptButton; // 입력을 확정하고 다음 창으로 넘어가는 확인 버튼
    public int maxChatacters = 7; // 최대 입력 가능 문자 수

    private float timer = 0f; // 커서 깜빡임 타이머 누적값

    private float cursorDelay = 0.5f; // 커서가 깜빡이는 시간 간격 (초)

    private bool blink; // 커서 표시 여부를 토글하는 플래그
    private List<Button> keys; // 키보드의 모든 알파벳 키 버튼 목록
    private WindowManager windowManager; // 창 전환을 관리하는 WindowManager 참조


    private void Awake() // 오브젝트 초기화 시 버튼 이벤트 등록
    {
        keys = new List<Button>(rootKeyborad.GetComponentsInChildren<Button>()); // 키보드 부모에서 모든 버튼을 수집하여 리스트에 저장
        foreach (var key in keys) // 각 키 버튼에 클릭 이벤트 등록
        {
            var text = key.GetComponentInChildren<TextMeshProUGUI>(); // 버튼 안에 있는 텍스트 컴포넌트를 가져옴
            key.onClick.AddListener(() => OnKey(text.text)); // 버튼 클릭 시 해당 텍스트를 입력 처리로 전달
        }
        deleteButton.onClick.AddListener(OnDelete); // 삭제 버튼 클릭 이벤트 등록
        cancelButton.onClick.AddListener(OnCancel); // 취소 버튼 클릭 이벤트 등록
        acceptButton.onClick.AddListener(OnAccept); // 확인 버튼 클릭 이벤트 등록
    }


    public override void Init(WindowManager mgr) // WindowManager 참조를 전달받아 초기화
    {
        windowManager = mgr; // 외부에서 전달받은 WindowManager를 멤버 변수에 저장
    }

    public override void Open() // 창을 열 때 입력 상태를 초기화하는 메서드
    {
        sb.Clear(); // 이전 입력 문자열 초기화
            timer = 0f; // 커서 타이머 초기화
        blink = false; // 커서 표시 상태 초기화
        base.Open(); // 부모 클래스의 Open 호출 (창 표시 처리)
        UpdateInputFiled(); // 초기 상태로 입력 필드 갱신
    }

    public override void Close() // 창을 닫는 메서드
    {
        base.Close(); // 부모 클래스의 Close 호출 (창 숨김 처리)
    }


    public void Update() // 매 프레임 커서 깜빡임 타이머를 처리하는 메서드
    {
        timer += Time.deltaTime; // 프레임 경과 시간 누적
        if  (timer > cursorDelay) // 깜빡임 간격을 초과하면 커서 상태 전환
        {
            timer = 0f; // 타이머 초기화
            blink = !blink; // 커서 표시/숨김 상태 반전
            UpdateInputFiled(); // 커서 상태 변경 후 입력 필드 갱신
        }
    }
    public void OnKey(string key) // 알파벳 키 버튼 클릭 시 문자를 입력에 추가하는 메서드
    {
        sb.Append(key); // 눌린 키 문자를 입력 문자열에 추가
        UpdateInputFiled(); // 입력 필드 갱신

    }

    private void UpdateInputFiled() // 입력 문자열과 커서를 화면에 반영하는 메서드
    {
        bool showCursor = sb.Length < maxChatacters && !blink; // 최대 글자 미만이고 blink가 false일 때 커서 표시
        if (showCursor)
        {
            sb.Append('_'); // 커서 문자를 임시로 추가
        }
        inputFiled.SetText(sb); // StringBuilder 내용을 텍스트 필드에 반영
        if (showCursor)
        {
            sb.Length -= 1; // 커서 문자를 다시 제거하여 실제 입력 문자열 유지
        }
        Debug.Log(inputFiled.text); // 현재 텍스트 필드 내용을 콘솔에 출력

    }

    public void OnDelete() // 삭제 버튼 클릭 시 마지막 문자를 제거하는 메서드
    {
        if (sb.Length > 0) // 입력 문자열이 있을 때만 삭제
        {
            sb.Length -= 1; // StringBuilder 마지막 문자 제거
            UpdateInputFiled(); // 입력 필드 갱신
        }
    }

    public void OnCancel() // 취소 버튼 클릭 시 입력 내용을 전부 지우는 메서드
    {
        sb.Clear(); // 입력 문자열 전체 초기화
        UpdateInputFiled(); // 빈 상태로 입력 필드 갱신
    }

    public void OnAccept() // 확인 버튼 클릭 시 다음 창으로 전환하는 메서드
    {
        windowManager.Open(2); // GameOverWindow로 전환
    }
}


