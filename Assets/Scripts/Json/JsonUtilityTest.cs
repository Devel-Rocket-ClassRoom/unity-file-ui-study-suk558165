using System.Collections.Generic;
using System.IO;
using UnityEditor.SceneManagement;
using UnityEngine;

// 플레이어 정보를 담는 데이터 클래스
public class PlayerInfo
{
    public string playerName; // 플레이어 이름
    public int lives; // 남은 목숨 수

    public float health; // 현재 체력

    public Vector3 position; // 플레이어 위치

    // 스테이지별 점수를 저장하는 딕셔너리 (초기값 포함)
    public Dictionary<string, int> scores = new Dictionary<string, int>
    {
        {"stage1", 100}, // 스테이지1 초기 점수
        {"stage2", 200 }, // 스테이지2 초기 점수
    };

}
// JsonUtility를 이용한 JSON 저장/로드 테스트 MonoBehaviour 클래스
public class JsonUtilityTest : MonoBehaviour
{
    // 매 프레임 키 입력을 감지하여 저장/로드 동작을 처리하는 메서드
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) // 숫자 1 키를 누르면 저장 실행
        {
            // Save
            // 테스트용 PlayerInfo 객체 생성 및 초기값 설정
            PlayerInfo obj = new PlayerInfo
            {
                playerName = "ABC", // 플레이어 이름 설정
                lives = 10, // 목숨 수 설정
                health = 10.999f, // 체력 설정
                position = new Vector3(1f,2f,3f) // 위치 설정
            };
            // JSON 파일을 저장할 폴더 경로 조합 (persistentDataPath/JsonTest)
            string pathFolder = Path.Combine(
                Application.persistentDataPath,
                "JsonTest"
                );

            if (!Directory.Exists(pathFolder) ) // 폴더가 존재하지 않으면
            {
                Directory.CreateDirectory(pathFolder); // 폴더 생성
            }
            // 저장할 JSON 파일의 전체 경로 조합
            string path = Path.Combine(
                pathFolder,
                "player.json");

            string json = JsonUtility.ToJson(obj, prettyPrint: true); // 객체를 보기 좋게 정렬된 JSON 문자열로 변환
            File.WriteAllText(path, json ); // JSON 문자열을 파일에 저장

            Debug.Log(path); // 저장 경로 콘솔 출력
            Debug.Log(json); // 저장된 JSON 내용 콘솔 출력
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) // 숫자 2 키를 누르면 로드 실행
        {
            // 로드할 JSON 파일의 전체 경로 조합
            string path = Path.Combine(
                Application.persistentDataPath,
                "JsonTest",
                "player.Json"
                );

            string json = File.ReadAllText(path); // 파일에서 JSON 문자열 읽기
            //PlyaerInfo obj =  JsonUtility.FromJson<PlyaerInfo>(json);
            PlayerInfo obj = new PlayerInfo(); // 덮어쓰기용 PlayerInfo 객체 생성
            JsonUtility.FromJsonOverwrite(json,obj); // JSON 데이터를 기존 객체에 덮어써서 역직렬화
            Debug.Log(json); // 읽어온 JSON 문자열 콘솔 출력
            Debug.Log($"{obj.playerName}/ {obj.health}"); // 로드된 플레이어 이름과 체력 콘솔 출력

        }
    }
}
