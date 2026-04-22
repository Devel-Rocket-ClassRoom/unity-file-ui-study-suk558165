using System.Collections.Generic;
using UnityEngine;

public static class DataTableManager // 모든 데이터 테이블을 초기화하고 관리하는 전역 정적 클래스
{
    private static readonly Dictionary<string, DataTable> tables = new Dictionary<string, DataTable>(); // 테이블 ID를 키로 DataTable 인스턴스를 저장하는 딕셔너리

    public static StringTable StringTable => Get<StringTable>(DatableIds.String); // 현재 언어에 해당하는 스트링 테이블 반환
    public static ItemTable ItemTable => Get<ItemTable>(DatableIds.Item); // 아이템 테이블 반환

    public static CharacterTable CharacterTable => Get<CharacterTable>(DatableIds.Character); // 캐릭터 테이블 반환
#if UNITY_EDITOR
    public static StringTable GetStringTable(Languages lang) // 에디터 전용: 지정 언어의 스트링 테이블을 반환하는 메서드
    {
        return Get<StringTable>(DatableIds.StringTableIds[(int)lang]); // 언어 인덱스로 해당 스트링 테이블 조회 후 반환
    }
#endif

    static DataTableManager() // 정적 생성자 - 클래스 최초 접근 시 자동 호출
    {
        Init(); // 모든 데이터 테이블 초기화
    }

    private static void Init() // 스트링/아이템/캐릭터 테이블을 로드하여 딕셔너리에 등록하는 초기화 메서드
    {
#if UNITY_EDITOR
        foreach (var id in DatableIds.StringTableIds) // 에디터에서는 모든 언어의 스트링 테이블을 미리 로드
        {
            var stringTable = new StringTable(); // 새 스트링 테이블 인스턴스 생성
            stringTable.Load(id); // 해당 ID의 CSV 파일 로드
            tables.Add(id, stringTable); // 테이블 딕셔너리에 추가
        }
#else
        var stringTable = new StringTable(); // 빌드에서는 현재 언어의 스트링 테이블만 로드
        stringTable.Load(DatableIds.String); // 현재 언어에 해당하는 CSV 파일 로드
        tables.Add(DatableIds.String, stringTable); // 테이블 딕셔너리에 추가
#endif
        var itemTable = new ItemTable(); // 아이템 테이블 인스턴스 생성
        itemTable.Load(DatableIds.Item); // 아이템 CSV 파일 로드
        tables.Add(DatableIds.Item, itemTable); // 아이템 테이블을 딕셔너리에 추가

        var characterTable = new CharacterTable(); // 캐릭터 테이블 인스턴스 생성
        characterTable.Load(DatableIds.Character); // 캐릭터 CSV 파일 로드
        tables.Add(DatableIds.Character, characterTable); // 캐릭터 테이블을 딕셔너리에 추가
    }

    public static T Get<T>(string Id) where T : DataTable // 지정 ID의 테이블을 T 타입으로 캐스팅하여 반환하는 제네릭 메서드
    {
        if (!tables.ContainsKey(Id)) // 요청한 ID의 테이블이 없는 경우
        {
            Debug.Log("테이블 없음"); // 테이블 없음 로그 출력
            return null; // null 반환
        }
        return tables[Id] as T; // 해당 테이블을 T 타입으로 캐스팅하여 반환
    }

    public static void ChangeLanguage(Languages lang) // 언어를 변경하고 화면의 모든 다국어 UI를 갱신하는 메서드
    {
        Variables.language = lang; // 전역 언어 변수를 새 언어로 설정
        string id = DatableIds.String; // 새 언어에 해당하는 스트링 테이블 ID 가져오기

#if !UNITY_EDITOR
        if (tables.ContainsKey(id)) // 빌드에서는 기존 스트링 테이블이 있으면 제거 후 재로드
            tables.Remove(id); // 기존 스트링 테이블 딕셔너리에서 제거
        var stringTable = new StringTable(); // 새 언어용 스트링 테이블 인스턴스 생성
        stringTable.Load(id); // 새 언어 CSV 파일 로드
        tables.Add(id, stringTable); // 새 스트링 테이블을 딕셔너리에 추가
#endif

        foreach (var t in Object.FindObjectsByType<LocallizationText>(FindObjectsSortMode.None)) // 씬의 모든 LocallizationText 컴포넌트 순회
            if (t != null) t.OnChangedId(); // null이 아닌 컴포넌트의 텍스트를 새 언어로 갱신

        Variables.InvokeLanguageChanged(); // LocalizationDropDown 등 이벤트 구독자에게 언어 변경 알림
    }
}
