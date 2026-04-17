using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class GameOverWindow : GenericWindow
{
    public TextMeshProUGUI leftStatLabel;   // 왼쪽 스탯 이름 텍스트
    public TextMeshProUGUI leftStatValue;   // 왼쪽 스탯 값 텍스트
    public TextMeshProUGUI rightStatLabel;  // 오른쪽 스탯 이름 텍스트
    public TextMeshProUGUI rightStatValue;  // 오른쪽 스탯 값 텍스트
    public TextMeshProUGUI scoreValue;      // 토탈 스코어 텍스트

    public Button nextbutton;              // NEXT 버튼
    private WindowManager windowManager;  // 윈도우 전환 관리자

    private Coroutine routine;             // 현재 실행 중인 코루틴

    private const int totalStats = 6;      // 스탯 총 개수
    public float statsDelay = 1f;          // 스탯 한 줄 출력 간격
    public float scoreDuration = 1f;       // 토탈 스코어 카운트업 시간
    public float dureation = 2f;           // 미사용
    private int statsPerColum = totalStats / 2; // 열당 스탯 수 (3)

    private TextMeshProUGUI[] statsLabels; // 스탯 라벨 배열 (왼쪽, 오른쪽)
    private TextMeshProUGUI[] statsValue;  // 스탯 값 배열 (왼쪽, 오른쪽)
    public int[] statsRolls = new int[totalStats]; // 랜덤 스탯 값 저장

    private int finalScore;                // 최종 스코어

    private void Awake()
    {
        statsLabels = new TextMeshProUGUI[] { leftStatLabel, rightStatLabel }; // 라벨 배열 초기화
        statsValue = new TextMeshProUGUI[] { leftStatValue, rightStatValue };  // 값 배열 초기화
        nextbutton.onClick.AddListener(OnNext); // NEXT 버튼 클릭 시 OnNext 등록
    }

    public override void Open()
    {
        if (routine != null)
        {
            StopCoroutine(routine); // 이전 코루틴 중단
            routine = null;
        }
        base.Open();               // 오브젝트 활성화
        ResetStats();              // 스탯 초기화 및 랜덤 생성
        routine = StartCoroutine(CoPlayGameOverRoutine()); // 연출 코루틴 시작
    }

    // 이전 방식 - 스탯 순차 출력 코루틴
    //private IEnumerator ShowStats()
    //{
    //    int total = 0;                     // 최종 합산 점수
    //    int[] values = new int[6];         // 스탯 6개 값 저장 배열

    //    for (int i = 0; i < 6; i++)
    //        values[i] = Random.Range(0, 99999); // 랜덤 생성 범위 설정

    //    leftStatLabel.text = "";   // 왼쪽 라벨 초기화
    //    leftStatValue.text = "";   // 왼쪽 값 초기화
    //    rightStatLabel.text = ""; // 오른쪽 라벨 초기화
    //    rightStatValue.text = "";  // 오른쪽 값 초기화

    //    for (int i = 0; i < 3; i++)       // 왼쪽 3줄 순차 출력
    //    {
    //        leftStatLabel.text += (i == 0 ? "" : "\n") + "STATS"; // 줄바꿈 후 라벨 추가
    //        leftStatValue.text += (i == 0 ? "" : "\n") + values[i].ToString("0000"); // 줄바꿈 후 값 추가
    //        total += values[i];            // 왼쪽 값 합산
    //        yield return new WaitForSeconds(1f);
    //    }

    //    for (int i = 0; i < 3; i++)       // 오른쪽 3줄 순차 출력
    //    {
    //        rightStatLabel.text += (i == 0 ? "" : "\n") + "STATS"; // 줄바꿈 후 라벨 추가
    //        rightStatValue.text += (i == 0 ? "" : "\n") + values[i + 3].ToString("0000"); // 줄바꿈 후 값 추가
    //        total += values[i + 3];        // 오른쪽 값 합산
    //        yield return new WaitForSeconds(1f);
    //    }
    //    yield return StartCoroutine(CountUpScore(total)); // 토탈 스코어 연출 시작
    //}

    // 이전 방식 - 토탈 스코어 카운트업 코루틴
    //private IEnumerator CountUpScore(int target)
    //{
    //    int current = 0;                   // 현재 표시 점수
    //    float duration = 1.5f;            // 카운트업 총 시간
    //    float elapsed = 0f;               // 경과 시간

    //    while (elapsed < duration)        // 경과 시간이 duration 미만일 동안 반복
    //    {
    //        elapsed += Time.deltaTime;    // 프레임마다 경과 시간 누적
    //        current = Mathf.RoundToInt(Mathf.Lerp(0, target, elapsed / duration)); // 0에서 target까지 보간
    //        scoreValue.text = current.ToString("0000000000"); // 점수 텍스트 갱신
    //        yield return null;            // 다음 프레임까지 대기
    //    }
    //    scoreValue.text = target.ToString("0000000000"); // 최종값 확정 출력
    //}

    private void ResetStats()
    {
        for (int i = 0; i < statsRolls.Length; ++i)
            statsRolls[i] = Random.Range(0, 1000); // 스탯 0~999 랜덤 생성

        finalScore = Random.Range(0, 10000000);    // 최종 스코어 랜덤 생성

        for (int i = 0; i < statsLabels.Length; ++i)
        {
            statsLabels[i].text = string.Empty;    // 라벨 초기화
            statsValue[i].text = string.Empty;     // 값 초기화
        }

        scoreValue.text = $"{0:D9}";               // 스코어 0으로 초기화
    }

    private IEnumerator CoPlayGameOverRoutine()
    {
        for (int i = 0; i < totalStats; i++)
        {
            yield return new WaitForSeconds(statsDelay); // 딜레이 대기

            int column = i / statsPerColum;              // 현재 열 인덱스 (0=왼쪽, 1=오른쪽)
            var labelText = statsLabels[column];          // 해당 열 라벨
            var valueText = statsValue[column];           // 해당 열 값
            string newLine = (i % statsPerColum == 0) ? string.Empty : "\n"; // 첫 줄이면 줄바꿈 없음
            labelText.text = $"{labelText.text}{newLine}Stat{i}";            // 라벨 추가
            valueText.text = $"{valueText.text}{newLine}{statsRolls[i]:D4}"; // 값 추가
        }

        float t = 0f;
        while (t < scoreDuration)
        {
            t += Time.deltaTime / scoreDuration;                              // 경과 비율 누적
            int current = Mathf.FloorToInt(Mathf.Lerp(0, finalScore, t));    // 현재 표시 점수 보간
            scoreValue.text = $"{current:D9}";                                // 점수 갱신
            yield return null;                                                // 다음 프레임 대기
        }

        scoreValue.text = $"{finalScore:D9}"; // 최종값 확정 출력
        routine = null;
    }

    public override void Close()
    {
        if (routine != null)
        {
            StopCoroutine(routine); // 코루틴 중단
            routine = null;
        }
        base.Close(); // 오브젝트 비활성화
    }

    public void OnNext()
    {
        windowManager.Open(2); 
    }

    public override void Init(WindowManager mgr)
    {
        windowManager = mgr; // WindowManager 참조 저장
    }
}