using System.Collections.Generic;
using UnityEngine;

// 아이템 종류를 나타내는 열거형
public enum ItemTypes  // ItemTytpes 오타 수정
{
    Weapon,     // 무기 타입
    Equip,      // 장비 타입
    Consumable, // 소비 아이템 타입
}

// 아이템 하나의 데이터를 담는 데이터 클래스
public class ItemData
{
    public string Id { get; set; }         // 아이템 고유 식별자
    public ItemTypes Type { get; set; }    // 아이템 종류 (ItemTytpes → ItemTypes)
    public string Name { get; set; }       // 다국어 테이블에서 이름을 조회할 때 사용하는 키
    public string Desc { get; set; }       // 다국어 테이블에서 설명을 조회할 때 사용하는 키
    public int Value { get; set; }         // 아이템 수치 (공격력, 방어력 등)
    public int Cost { get; set; }          // 아이템 비용(가격)
    public string Icon { get; set; }       // Resources/Icon 폴더에서 스프라이트를 로드할 때 사용하는 파일명

    public string StringName => DataTableManager.StringTable.Get(Name); // 다국어 테이블에서 현재 언어의 이름 문자열을 반환하는 읽기 전용 프로퍼티
    public string StringDesc => DataTableManager.StringTable.Get(Desc); // 다국어 테이블에서 현재 언어의 설명 문자열을 반환하는 읽기 전용 프로퍼티
    public Sprite SpriteIcon => Resources.Load<Sprite>($"Icon/{Icon}"); // Resources/Icon 경로에서 아이콘 스프라이트를 로드하여 반환하는 읽기 전용 프로퍼티

    // 아이템 데이터를 슬래시로 구분된 문자열로 반환하는 메서드
    public override string ToString()
    {
        return $"{Id}/{Type}/{Name}/{Desc}/{Value}/{Cost}/{Icon}"; // 모든 필드를 슬래시로 연결하여 반환
    }
}

// CSV 파일을 읽어 아이템 데이터를 딕셔너리로 관리하는 테이블 클래스
public class ItemTable : DataTable
{
    private readonly Dictionary<string, ItemData> table = new Dictionary<string, ItemData>(); // 아이템 ID를 키로 ItemData를 저장하는 딕셔너리
    private List<string> keyList;  // 랜덤 아이템 선택에 사용할 키 목록 (오타 수정: ketList → keyList)

    // 파일명을 받아 CSV를 파싱하여 테이블을 구성하는 메서드
    public override void Load(string filename)
    {
        table.Clear(); // 기존 테이블 데이터를 모두 제거

        string path = string.Format(FormatPath, filename); // 포맷 경로에 파일명을 적용하여 최종 경로 생성
        TextAsset textAsset = Resources.Load<TextAsset>(path); // Resources 폴더에서 텍스트 에셋을 로드
        List<ItemData> list = LoadCSV<ItemData>(textAsset.text, "\t"); // 탭 구분자로 CSV 텍스트를 파싱하여 ItemData 리스트 생성

        foreach (var item in list) // 파싱된 아이템 리스트를 순회
        {
            if (!table.ContainsKey(item.Id)) // 중복 ID가 없는 경우에만 추가
            {
                table.Add(item.Id, item); // 아이템 ID를 키로 딕셔너리에 추가
            }
            else
            {
                Debug.LogError("아이템 아이디 중복"); // 중복 ID 발견 시 에러 로그 출력
            }
        }

        keyList = new List<string>(table.Keys);  // 랜덤 선택을 위해 딕셔너리 키 목록을 리스트로 저장
    }

    // ID 문자열로 아이템 데이터를 조회하여 반환하는 메서드
    public ItemData Get(string id)
    {
        if (!table.ContainsKey(id)) // 해당 ID가 테이블에 없는 경우
        {
            Debug.LogError("아이템 아이디 없음"); // ID 없음 에러 로그 출력
            return null; // null 반환
        }
        return table[id]; // 해당 ID의 ItemData를 반환
    }

    // 테이블에서 무작위 아이템 데이터를 반환하는 메서드
    public ItemData GetRandom()
    {
        return Get(keyList[Random.Range(0, keyList.Count)]);  // 키 목록에서 무작위 인덱스를 선택하여 해당 아이템 반환
    }
}
