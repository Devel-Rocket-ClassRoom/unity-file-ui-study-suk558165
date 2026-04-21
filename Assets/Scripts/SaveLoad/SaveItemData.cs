using System; // 기본 .NET 시스템 라이브러리 (Guid, DateTime 등)
using Newtonsoft.Json; // Json.NET 라이브러리 — JSON 직렬화/역직렬화 및 커스텀 컨버터 지원

[Serializable] // JSON 직렬화/역직렬화 가능하도록 표시
// 인벤토리에 저장되는 개별 아이템 인스턴스 데이터를 담는 클래스
public class SaveItemData
{
    public Guid instanceId; // 아이템 인스턴스 고유 식별자 (같은 종류여도 각 아이템을 구별)

    [JsonConverter(typeof(ItemDataConverter))] // ItemData를 JSON으로 변환할 때 사용할 커스텀 컨버터 지정

    public ItemData itemData; // 아이템의 종류/스펙 정보를 담는 데이터 테이블 참조

    public DateTime creationTime; // 아이템이 인벤토리에 추가된 시각

    // 데이터 테이블에서 무작위 아이템을 선택하여 SaveItemData 인스턴스를 생성하는 정적 팩토리 메서드
    public static SaveItemData GetRandomItem()
    {
        SaveItemData newItem = new SaveItemData(); // 새 SaveItemData 인스턴스 생성 (생성자에서 instanceId, creationTime 자동 설정)
        newItem.itemData = DataTableManager.ItemTable.GetRandom(); // 데이터 테이블에서 랜덤 아이템 데이터 할당
        return newItem; // 생성된 랜덤 아이템 반환
    }

    // 기본 생성자 — instanceId와 creationTime을 현재 시각 기준으로 자동 초기화
    public SaveItemData()
    {
        instanceId = Guid.NewGuid(); // 전 세계 고유한 새 GUID 생성
        creationTime = DateTime.Now; // 현재 로컬 시각을 생성 시각으로 기록
    }

    // 아이템 정보를 "instanceId\ncreationTime\nitemId" 형식의 문자열로 반환하는 메서드
    public override string ToString()
    {
        return $"{instanceId}\n{creationTime}\n{itemData.Id}"; // instanceId, 생성 시각, 아이템 ID를 줄바꿈으로 구분하여 반환
    }
}
