using System.Collections.Generic; // 제네릭 컬렉션(Dictionary, List 등) 라이브러리
using UnityEngine; // Unity 핵심 엔진 라이브러리 (Sprite, Debug 등)

public class CharacterData // 캐릭터 한 명의 데이터를 담는 데이터 클래스
{
    public string Id { get; set; } // 캐릭터 고유 ID (CSV 키 컬럼)
    public string Name { get; set; } // 캐릭터 이름의 로컬라이제이션 키
    public string JOB { get; set; } // 캐릭터 직업의 로컬라이제이션 키
    public int Attack { get; set; } // 캐릭터 공격력 수치
    public int Dffend { get; set; } // 캐릭터 방어력 수치
    public int Health { get; set; } // 캐릭터 체력 수치
    public string Icon { get; set; } // Resources/Icon 폴더에서 로드할 아이콘 파일명

    public string StringName => DataTableManager.StringTable.Get(Name); // 현재 언어로 번역된 캐릭터 이름 반환
    public string StringJob => DataTableManager.StringTable.Get(JOB); // 현재 언어로 번역된 캐릭터 직업명 반환
    public Sprite SpriteIcon => Resources.Load<Sprite>($"Icon/{Icon}"); // Resources/Icon 경로에서 아이콘 스프라이트 로드
}

public class CharacterTable : DataTable // CSV 파일에서 캐릭터 데이터를 로드하고 ID로 조회하는 테이블 클래스
{
    private readonly Dictionary<string, CharacterData> table = new Dictionary<string, CharacterData>(); // 캐릭터 ID를 키로 데이터를 저장하는 딕셔너리
    private List<string> keyList; // 랜덤 캐릭터 선택에 사용할 키 목록

    public override void Load(string filename) // CSV 파일명을 받아 캐릭터 데이터를 로드하는 메서드
    {
        table.Clear(); // 기존 테이블 데이터를 모두 삭제
        string path = string.Format(FormatPath, filename); // 리소스 폴더 내 파일 경로 포맷 생성
        TextAsset textAsset = Resources.Load<TextAsset>(path); // 지정 경로에서 텍스트 에셋(CSV 파일) 로드
        List<CharacterData> list = LoadCSV<CharacterData>(textAsset.text); // CSV 텍스트를 CharacterData 목록으로 파싱
        foreach (var item in list) // 파싱된 캐릭터 목록을 순회
        {
            if (!table.ContainsKey(item.Id)) // 동일한 ID가 테이블에 없는 경우
                table.Add(item.Id, item); // 새 캐릭터 데이터를 딕셔너리에 추가
            else
                Debug.LogError("캐릭터 아이디 중복"); // 중복 ID가 발견된 경우 에러 로그 출력
        }
        keyList = new List<string>(table.Keys); // 랜덤 선택을 위해 딕셔너리 키 목록을 리스트로 저장
    }

    public CharacterData Get(string id) // 캐릭터 ID로 해당 캐릭터 데이터를 반환하는 메서드
    {
        if (!table.ContainsKey(id)) // 요청한 ID가 테이블에 없는 경우
        {
            Debug.LogError("캐릭터 아이디 없음"); // ID 없음 에러 로그 출력
            return null; // null 반환
        }
        return table[id]; // 해당 ID의 캐릭터 데이터 반환
    }

    public CharacterData GetRandom() // 테이블에서 무작위 캐릭터 데이터를 반환하는 메서드
    {
        return Get(keyList[Random.Range(0, keyList.Count)]); // 키 목록에서 무작위 인덱스를 선택하여 해당 캐릭터 반환
    }
}
