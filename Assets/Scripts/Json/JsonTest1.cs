using Newtonsoft.Json; // JSON 직렬화/역직렬화를 위한 Newtonsoft.Json 라이브러리
using System; // Serializable 등 기본 시스템 기능을 위한 라이브러리
using System.IO; // 파일 입출력(읽기/쓰기/경로) 기능을 위한 라이브러리
using Unity.VisualScripting; // Unity Visual Scripting 기능을 위한 라이브러리
using UnityEngine; // Unity의 핵심 기능(MonoBehaviour, Vector3 등)을 위한 라이브러리

[Serializable] // 이 클래스를 직렬화 가능하도록 표시하는 어트리뷰트
// 플레이어의 스탯(능력치) 정보를 담는 데이터 클래스
public class PlayerStat
{
    public string playerName; // 플레이어 이름

    public int lives; // 남은 목숨 수

    public float health; // 현재 체력

    public Vector3 position; // 플레이어 위치


    // 객체를 문자열로 표현하는 메서드 (이름/목숨/체력을 슬래시로 구분하여 반환)
    public override string ToString()
    {
        return $"{playerName} / {lives} / {health}"; // 이름, 목숨, 체력을 포맷하여 문자열로 반환
    }
}

// Newtonsoft.Json을 이용한 JSON 저장/로드 테스트 MonoBehaviour 클래스
public class JsonTest1 : MonoBehaviour
{
    private JsonSerializerSettings jsonSettings; // JSON 직렬화 설정 객체 (포맷, 컨버터 등)

    // 초기화 시 JSON 직렬화 설정을 구성하는 메서드
    private void Awake()
    {
        jsonSettings = new JsonSerializerSettings(); // JSON 직렬화 설정 객체 생성
        jsonSettings.Formatting = Formatting.Indented; // JSON을 들여쓰기된 보기 좋은 형식으로 설정
        jsonSettings.Converters.Add(new Vector3Converter()); // Vector3 타입 변환을 위한 커스텀 컨버터 추가
    }
    // 매 프레임 키 입력을 감지하여 저장/로드 동작을 처리하는 메서드
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) // 숫자 1 키를 누르면 저장 실행
        {
            // Save
            // 테스트용 PlayerStat 객체 생성 및 초기값 설정
            PlayerStat obj = new PlayerStat
            {
                playerName = "ABC", // 플레이어 이름 설정
                lives = 10, // 목숨 수 설정
                health = 10.999f, // 체력 설정
            };
            // JSON 파일을 저장할 폴더 경로 조합 (persistentDataPath/JsonTest)
            string pathFolder = Path.Combine(
                Application.persistentDataPath,
                "JsonTest"
                );

            if (!Directory.Exists(pathFolder)) // 폴더가 존재하지 않으면
            {
                Directory.CreateDirectory(pathFolder); // 폴더 생성
            }
            // 저장할 JSON 파일의 전체 경로 조합
            string path = Path.Combine(
                pathFolder,
                "player2.json");

            string json = JsonConvert.SerializeObject(obj, jsonSettings); // 객체를 JSON 문자열로 직렬화
            File.WriteAllText(path, json); // JSON 문자열을 파일에 저장

            Debug.Log(path); // 저장 경로 콘솔 출력
            Debug.Log(json); // 저장된 JSON 내용 콘솔 출력
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) // 숫자 2 키를 누르면 로드 실행
        {
            // 로드할 JSON 파일의 전체 경로 조합
            string path = Path.Combine(
                Application.persistentDataPath,
                "JsonTest",
                "player2.Json"
                );

            string json = File.ReadAllText(path); // 파일에서 JSON 문자열 읽기
             PlayerStat obj = JsonConvert.DeserializeObject<PlayerStat>( // JSON 문자열을 PlayerStat 객체로 역직렬화
            json, jsonSettings);

            Debug.Log(json); // 읽어온 JSON 문자열 콘솔 출력
            Debug.Log(obj); // 역직렬화된 PlayerStat 객체 콘솔 출력 (ToString 호출)

        }
    }
}
