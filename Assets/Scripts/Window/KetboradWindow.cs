//using System.Collections;
//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;

////커서 깜빡임
////버튼누르면 그 해당하는 텍스트에 맞춰 커서칸에 텍스트입력
////최대칸제한
////다쓰면 커서 사라지게 만들기
////딜레트 누르면 오른쪽부터 한개 삭제 제한 풀리면 다시 커서생성 깜빡임
////캔슬버튼은 텍스트 전체 삭제
////억셉트는 스타트 윈도우 띄우게 만들기

//public class KetboradWindow : GenericWindow
//{
//    public TextMeshProUGUI inputText;      // 입력 표시 텍스트
//    public int maxLength = 6;              // 최대 입력 길이
//    private string inputString = "";       // 현재 입력 문자열
//    private Coroutine cursorRoutine;       // 커서 깜빡임 코루틴
//    private WindowManager windowManager;  // 윈도우 매니저
//    public GameObject keyboardParent;      // 키 버튼들의 부모 오브젝트

//    public Button[] buttons;               // 알파벳 키 버튼 배열
//    public Button deleteButon;             // 딜리트 버튼
//    public Button cancelButon;             // 캔슬 버튼
//    public Button acceptButon;             // 억셉트 버튼

//    public void Awake()
//    {
//        buttons = keyboardParent.GetComponentsInChildren<Button>(); // 부모에서 버튼 배열 자동 수집

//        string keys = "QWERTYUIOPASDFGHJKLZXCVBNM";
//        for (int i = 0; i < buttons.Length; i++)
//        {
//            string key = keys[i].ToString();
//            buttons[i].onClick.AddListener(() => OnKeyPressed(key)); // 각 버튼에 키 입력 이벤트 등록
//        }

//        deleteButon.onClick.AddListener(OnDelete);     // 딜리트 이벤트 등록
//        cancelButon.onClick.AddListener(OnCancel);     // 캔슬 이벤트 등록
//        acceptButon.onClick.AddListener(OnAccept);     // 억셉트 이벤트 등록
//        cursorRoutine = StartCoroutine(CursorBlink()); // 커서 깜빡임 시작
//    }

//    public override void Open()
//    {
//        if (cursorRoutine != null)
//        {
//            StopCoroutine(cursorRoutine); // 이전 코루틴 중단
//            cursorRoutine = null;
//        }
//        base.Open();                                   // 오브젝트 활성화
//        cursorRoutine = StartCoroutine(CursorBlink()); // 커서 깜빡임 재시작
//    }

//    public override void Close()
//    {
//        if (cursorRoutine != null)
//        {
//            StopCoroutine(cursorRoutine); // 코루틴 중단
//            cursorRoutine = null;
//        }
//        base.Close(); // 오브젝트 비활성화
//    }

//    public void OnKeyPressed(string key)
//    {
//        if (inputString.Length < maxLength) // 최대 길이 미만이면 입력
//        {
//            inputString += key;
//        }
//        else // 최대 길이 도달 시 커서 제거
//        {
//            if (cursorRoutine != null)
//            {
//                StopCoroutine(cursorRoutine);
//                cursorRoutine = null;
//            }
//            inputText.text = inputString; // 커서 없이 확정 출력
//        }
//    }

//    public void OnDelete()
//    {
//        if (inputString.Length > 0)
//        {
//            inputString = inputString.Remove(inputString.Length - 1);
//            if (cursorRoutine == null)
//                inputText.text = inputString; // 코루틴 없을 때 직접 갱신
//        }
//    }

//    public void OnCancel()
//    {
//        inputString = string.Empty;                                           // 입력 문자열 초기화
//        if (cursorRoutine != null)
//        {
//            StopCoroutine(cursorRoutine);                                     // 기존 코루틴 중단
//            cursorRoutine = null;
//        }
//        cursorRoutine = StartCoroutine(CursorBlink());                        // 커서 깜빡임 재시작
//    }

//    public void OnAccept()
//    {
//        windowManager.Open(2); // 2번 윈도우(GameOverWindow)로 전환
//    }

//    public override void Init(WindowManager mgr)
//    {
//        windowManager = mgr; // WindowManager 참조 저장
//    }

//    public IEnumerator CursorBlink()
//    {
//        while (true)
//        {
//            inputText.text = inputString + "_"; // 커서 표시
//            yield return new WaitForSeconds(0.5f);
//            inputText.text = inputString;        // 커서 숨김
//            yield return new WaitForSeconds(0.5f);
//        }
//    }
//}
