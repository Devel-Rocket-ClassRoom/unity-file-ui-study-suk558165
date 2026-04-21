using System; // 기본 .NET 시스템 라이브러리 (Serializable 어트리뷰트 등)
using System.Collections.Generic; // 제네릭 컬렉션 라이브러리 (List<T> 등)

[System.Serializable] // JSON 직렬화/역직렬화 가능하도록 표시
// 모든 세이브 데이터 버전의 공통 기반이 되는 추상 클래스
public abstract class SaveData
{
    public int Version { get; protected set; } // 세이브 데이터 버전 번호 (자식 클래스에서만 설정 가능)
    public abstract SaveData VersionUp(); // 현재 버전 데이터를 다음 버전으로 변환하는 추상 메서드
}

[System.Serializable] // JSON 직렬화/역직렬화 가능하도록 표시
// 버전 1 세이브 데이터 클래스 — 플레이어 이름만 저장
public class SaveDataV1 : SaveData
{
    public string PlayerName { get; set; } = string.Empty; // 플레이어 이름 필드 (기본값: 빈 문자열)

    public SaveDataV1() { Version = 1; } // 생성자에서 버전을 1로 설정

    // V1 데이터를 V2로 마이그레이션 — PlayerName을 Name으로 이전
    public override SaveData VersionUp()
    {
        return new SaveDataV2() // V2 데이터 객체 생성
        {
            Name = PlayerName, // V1의 PlayerName을 V2의 Name으로 복사
        };
    }
}

[System.Serializable] // JSON 직렬화/역직렬화 가능하도록 표시
// 버전 2 세이브 데이터 클래스 — 이름과 골드 저장
public class SaveDataV2 : SaveData
{
    public string Name { get; set; } = string.Empty; // 플레이어 이름 필드 (기본값: 빈 문자열)
    public int Gold { get; set; } = 0; // 소지 골드 필드 (기본값: 0)

    public SaveDataV2() { Version = 2; } // 생성자에서 버전을 2로 설정

    // V2 데이터를 V3으로 마이그레이션 — 이름과 골드를 이전
    public override SaveData VersionUp()
    {
        return new SaveDataV3() // V3 데이터 객체 생성
        {
            Name = Name, // 이름 복사
            Gold = Gold, // 골드 복사
        };
    }
}

[System.Serializable] // JSON 직렬화/역직렬화 가능하도록 표시
// 버전 3 세이브 데이터 클래스 — 이름, 골드, 난이도, 아이템 ID 목록 저장
public class SaveDataV3 : SaveData
{
    public string Name { get; set; } = string.Empty; // 플레이어 이름 필드
    public int Gold { get; set; } = 0; // 소지 골드 필드
    public int Difficulty { get; set; } = 0; // 난이도 설정값 필드
    public List<string> itemList { get; set; } = new List<string>(); // 아이템 ID 문자열 목록 (V3에서는 ID만 저장)

    public SaveDataV3() { Version = 3; } // 생성자에서 버전을 3으로 설정

    // V3 데이터를 V4로 마이그레이션 — 아이템 ID 목록을 SaveItemData 객체 목록으로 변환
    public override SaveData VersionUp()
    {
        var data = new SaveDataV4() // V4 데이터 객체 생성
        {
            Name = Name, // 이름 복사
            Gold = Gold, // 골드 복사
            Difficulty = Difficulty, // 난이도 복사
        };
        foreach (string id in itemList) // 기존 아이템 ID 목록 순회
        {
            data.itemList.Add(new SaveItemData() { itemData = DataTableManager.ItemTable.Get(id) }); // ID로 아이템 데이터를 조회하여 SaveItemData 객체로 변환 후 추가
        }
        return data; // 마이그레이션된 V4 데이터 반환
    }
}

[System.Serializable] // JSON 직렬화/역직렬화 가능하도록 표시
// 버전 4 세이브 데이터 클래스 — 이름, 골드, 난이도, SaveItemData 목록 저장
public class SaveDataV4 : SaveData
{
    public string Name { get; set; } = string.Empty; // 플레이어 이름 필드
    public int Gold { get; set; } = 0; // 소지 골드 필드
    public int Difficulty { get; set; } = 0; // 난이도 설정값 필드
    public List<SaveItemData> itemList { get; set; } = new List<SaveItemData>(); // 상세 아이템 데이터 목록 (V4부터 SaveItemData 객체 저장)

    public SaveDataV4() { Version = 4; } // 생성자에서 버전을 4로 설정

    // V4 데이터를 V5로 마이그레이션 — 정렬/필터링 옵션 기본값 추가
    public override SaveData VersionUp() => new SaveDataV5
    {
        Name = Name, // 이름 복사
        Gold = Gold, // 골드 복사
        Difficulty = Difficulty, // 난이도 복사
        itemList = itemList, // 아이템 목록 복사
        SortingOption = 0, // 정렬 옵션 기본값 0으로 초기화
        FilteringOption = 0, // 필터링 옵션 기본값 0으로 초기화
    };
}

[Serializable] // JSON 직렬화/역직렬화 가능하도록 표시
// 버전 5 세이브 데이터 클래스 — 현재 최신 버전, 정렬/필터링 옵션 추가
public class SaveDataV5 : SaveData
{
    public string Name { get; set; } = string.Empty; // 플레이어 이름 필드
    public int Gold { get; set; } = 0; // 소지 골드 필드
    public int Difficulty { get; set; } = 0; // 난이도 설정값 필드
    public List<SaveItemData> itemList { get; set; } = new List<SaveItemData>(); // 상세 아이템 데이터 목록
    public int SortingOption { get; set; } = 0; // 인벤토리 정렬 옵션 (0 = 기본값)
    public int FilteringOption { get; set; } = 0; // 인벤토리 필터링 옵션 (0 = 기본값)

    public SaveDataV5() { Version = 5; } // 생성자에서 버전을 5로 설정

    // V5 데이터를 V6으로 마이그레이션 — 캐릭터 목록 및 정렬/필터 옵션 기본값 추가
    public override SaveData VersionUp() => new SaveDataV6
    {
        Name = Name,
        Gold = Gold,
        Difficulty = Difficulty,
        itemList = itemList,
        SortingOption = SortingOption,
        FilteringOption = FilteringOption,
        characterList = new List<SaveCharacterData>(),
        CharacterSortingOption = 0,
        CharacterFilteringOption = 0,
    };
}

[Serializable] // JSON 직렬화/역직렬화 가능하도록 표시
// 버전 6 세이브 데이터 클래스 — 캐릭터 목록 및 캐릭터 정렬/필터 옵션 추가
public class SaveDataV6 : SaveData
{
    public string Name { get; set; } = string.Empty;                          // 플레이어 이름 필드
    public int Gold { get; set; } = 0;                                        // 소지 골드 필드
    public int Difficulty { get; set; } = 0;                                  // 난이도 설정값 필드
    public List<SaveItemData> itemList { get; set; } = new List<SaveItemData>();             // 아이템 데이터 목록
    public int SortingOption { get; set; } = 0;                               // 인벤토리 정렬 옵션
    public int FilteringOption { get; set; } = 0;                             // 인벤토리 필터링 옵션
    public List<SaveCharacterData> characterList { get; set; } = new List<SaveCharacterData>(); // 보유 캐릭터 데이터 목록
    public int CharacterSortingOption { get; set; } = 0;                      // 캐릭터 목록 정렬 옵션 (0 = 기본값)
    public int CharacterFilteringOption { get; set; } = 0;                    // 캐릭터 목록 필터링 옵션 (0 = 기본값)

    public SaveDataV6() { Version = 6; } // 생성자에서 버전을 6으로 설정

    public override SaveData VersionUp() => this; // 최신 버전이므로 자기 자신을 그대로 반환
}
