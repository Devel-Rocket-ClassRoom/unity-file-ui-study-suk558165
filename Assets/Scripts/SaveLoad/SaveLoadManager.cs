using Newtonsoft.Json; // Json.NET 라이브러리 — JSON 직렬화/역직렬화 지원
using System.IO; // 파일 및 디렉토리 입출력 라이브러리
using UnityEngine; // Unity 엔진 핵심 라이브러리 (Application, Debug 등)
using SaveDataVC = SaveDataV6; // 현재 최신 세이브 데이터 버전을 SaveDataVC라는 별칭으로 사용

// 세이브 파일 저장 형식을 결정하는 열거형
public enum SaveMode
{
    Text,       // JSON 텍스트 (.json) — 개발용
    Encrypted,  // AES 암호화 바이너리 (.dat) — 릴리즈용
}

// 게임 데이터의 저장/불러오기를 총괄하는 정적 관리 클래스
public class SaveLoadManager
{
    public static int SaveDataVersion { get; } = 6; // 현재 지원하는 최신 세이브 데이터 버전 번호

    public static SaveMode Mode { get; set; } = SaveMode.Text; // 현재 세이브 모드 (기본값: 텍스트 JSON)

    public static SaveDataVC Data { get; set; } = new SaveDataVC(); // 현재 메모리에 올라와 있는 세이브 데이터

    // 정적 생성자 — 클래스 최초 접근 시 자동으로 실행되어 세이브 파일 로드 시도
    static SaveLoadManager()
    {
<<<<<<< HEAD
        var path = GetSaveFilePath(0);
        if (File.Exists(path) && !Load()) // 파일이 존재하는데 로드에 실패한 경우만 에러
        {
            Debug.LogError("세이브 파일 로드 실패!");
=======
        if(!Load()) // 세이브 파일 로드 실패 시
        {
            Debug.LogError("세이브 파일 로드 실패!"); // 에러 메시지를 Unity 콘솔에 출력
>>>>>>> ac335ae9d04564330e8933454b073a144641c6c6
        }
    }
    // 슬롯별 세이브 파일 이름 목록 (인덱스 0 = 자동저장, 1~3 = 수동 슬롯)
    private static readonly string[] SaveFileName =
    {
        "SaveAuto", // 슬롯 0: 자동 저장 파일
        "Save1", // 슬롯 1: 수동 저장 슬롯 1
        "Save2", // 슬롯 2: 수동 저장 슬롯 2
        "Save3", // 슬롯 3: 수동 저장 슬롯 3
    };

    public static string SaveDirectory => $"{Application.persistentDataPath}/Save"; // 플랫폼별 영구 저장 경로 아래 Save 폴더를 세이브 디렉토리로 사용

    // 슬롯 번호를 받아 해당 슬롯의 세이브 파일 전체 경로를 반환하는 메서드
    public static string GetSaveFilePath(int slot)
    {
        if (slot < 0 || slot >= SaveFileName.Length) // 슬롯 번호가 유효 범위를 벗어난 경우
        {
            throw new System.ArgumentOutOfRangeException(nameof(slot), $"slot은 0 이상 {SaveFileName.Length - 1} 이하여야 합니다."); // 범위 초과 예외를 던짐
        }

        var ext = Mode == SaveMode.Text ? ".json" : ".dat"; // 텍스트 모드면 .json, 암호화 모드면 .dat 확장자 선택
        return Path.Combine(SaveDirectory, SaveFileName[slot] + ext); // 디렉토리 경로와 파일명을 합쳐 전체 경로 반환
    }

    // JSON 직렬화/역직렬화 설정 객체
    private static JsonSerializerSettings settings = new JsonSerializerSettings()
    {
        Formatting = Formatting.Indented, // JSON 출력을 들여쓰기 적용하여 사람이 읽기 쉽게 포맷팅
        // TypeNameHandling.All: JSON에 $type 필드를 기록/복원.
        // DeserializeObject<SaveData>로 부모 타입을 요청해도 $type을 보고 실제 타입(V1/V2/V3)으로 복원되어
        // 구버전 세이브도 VersionUp() 마이그레이션 체인을 탈 수 있다.
        TypeNameHandling = TypeNameHandling.All, // 모든 타입 정보를 JSON에 포함하여 다형성 역직렬화 지원
    };

    // 현재 Data를 지정 슬롯에 저장하는 메서드 (기본 슬롯: 0)
    public static bool Save(int slot = 0)
    {
        if (Data == null || slot < 0 || slot >= SaveFileName.Length) // 데이터가 없거나 슬롯이 유효하지 않으면
        {
            return false; // 저장 실패를 false로 반환
        }

        try
        {
            if (!Directory.Exists(SaveDirectory)) // 세이브 디렉토리가 존재하지 않으면
            {
                Directory.CreateDirectory(SaveDirectory); // 디렉토리 새로 생성
            }

            var path = GetSaveFilePath(slot); // 해당 슬롯의 파일 경로 조회
            var json = JsonConvert.SerializeObject(Data, settings); // 세이브 데이터를 JSON 문자열로 직렬화

            if (Mode == SaveMode.Text) // 텍스트 모드일 때
            {
                File.WriteAllText(path, json); // JSON 문자열을 텍스트 파일로 저장
            }
            else // 암호화 모드일 때
            {
                File.WriteAllBytes(path, CryptoUtil.Encrypt(json)); // JSON 문자열을 AES 암호화 후 바이너리 파일로 저장
            }

            return true; // 저장 성공을 true로 반환
        }
        catch
        {
            Debug.LogError("Save 예외 발생"); // 예외 발생 시 에러 메시지 출력
            return false; // 저장 실패를 false로 반환
        }
    }

    // 지정 슬롯의 세이브 파일을 불러와 Data에 적용하는 메서드 (기본 슬롯: 0)
    public static bool Load(int slot = 0)
    {
        if (slot < 0 || slot >= SaveFileName.Length) // 슬롯이 유효 범위를 벗어난 경우
        {
            return false; // 로드 실패를 false로 반환
        }

        var path = GetSaveFilePath(slot); // 해당 슬롯의 파일 경로 조회
        if (!File.Exists(path)) // 파일이 존재하지 않으면
        {
            return false; // 로드 실패를 false로 반환
        }

        try
        {
            string json; // 역직렬화에 사용할 JSON 문자열 변수
            if (Mode == SaveMode.Text) // 텍스트 모드일 때
            {
                json = File.ReadAllText(path); // 텍스트 파일에서 JSON 문자열 읽기
            }
            else // 암호화 모드일 때
            {
                json = CryptoUtil.Decrypt(File.ReadAllBytes(path)); // 바이너리 파일을 AES 복호화하여 JSON 문자열 복원
            }

            var dataSave = JsonConvert.DeserializeObject<SaveData>(json, settings); // JSON을 SaveData 부모 타입으로 역직렬화 ($type 필드로 실제 버전 클래스 복원)
            // 구버전 세이브면 최신 버전까지 한 단계씩 끌어올린다.
            while (dataSave.Version < SaveDataVersion) // 현재 버전이 최신 버전보다 낮으면 마이그레이션 반복
            {
                var prevVersion = dataSave.Version; // 마이그레이션 전 버전 번호 저장 (로그 출력용)
                dataSave = dataSave.VersionUp(); // 한 단계 위 버전으로 데이터 변환
                Debug.Log($"[SaveLoad] 마이그레이션 V{prevVersion} → V{dataSave.Version}"); // 마이그레이션 진행 상황을 콘솔에 출력
            }
            Data = dataSave as SaveDataVC; // 최종적으로 최신 버전 타입으로 캐스팅하여 Data에 저장
            return true; // 로드 성공을 true로 반환
        }
        catch
        {
            Debug.LogError("Load 예외 발생"); // 예외 발생 시 에러 메시지 출력
            return false; // 로드 실패를 false로 반환
        }
    }
}
