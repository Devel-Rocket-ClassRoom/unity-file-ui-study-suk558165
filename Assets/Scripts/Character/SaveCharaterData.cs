
using System;
using Newtonsoft.Json;

[Serializable] // JSON 직렬화/역직렬화 가능하도록 표시
// 보유 캐릭터 목록에 저장되는 개별 캐릭터 인스턴스 데이터를 담는 클래스
public class SaveCharacterData
{
    public Guid instanceId; // 캐릭터 인스턴스 고유 식별자 (같은 종류여도 각 캐릭터를 구별)

    [JsonConverter(typeof(CharacterDataConverter))] // CharacterData를 JSON으로 변환할 때 사용할 커스텀 컨버터 지정

    public CharacterData characterData; // 캐릭터의 종류/스펙 정보를 담는 데이터 테이블 참조

    public DateTime creationTime; // 캐릭터를 획득한 시각

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] // null이면 JSON에서 해당 필드를 생략
    public SaveItemData weaponSlot; // 착용 중인 무기 슬롯 데이터 (미착용 시 null)

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] // null이면 JSON에서 해당 필드를 생략
    public SaveItemData equipSlot; // 착용 중인 장비 슬롯 데이터 (미착용 시 null)

    // 데이터 테이블에서 무작위 캐릭터를 선택하여 SaveCharacterData 인스턴스를 생성하는 정적 팩토리 메서드
    public static SaveCharacterData GetRandomCharacter()
    {
        SaveCharacterData newCharacter = new SaveCharacterData(); // 새 SaveCharacterData 인스턴스 생성 (생성자에서 instanceId, creationTime 자동 설정)
        newCharacter.characterData = DataTableManager.CharacterTable.GetRandom(); // 데이터 테이블에서 랜덤 캐릭터 데이터 할당
        return newCharacter; // 생성된 랜덤 캐릭터 반환
    }

    // 기본 생성자 — instanceId와 creationTime을 현재 시각 기준으로 자동 초기화
    public SaveCharacterData()
    {
        instanceId = Guid.NewGuid(); // 전 세계 고유한 새 GUID 생성
        creationTime = DateTime.Now; // 현재 로컬 시각을 획득 시각으로 기록
    }

    // 캐릭터 정보를 "instanceId\ncreationTime\ncharacterId" 형식의 문자열로 반환하는 메서드
    public override string ToString()
    {
        return $"{instanceId}\n{creationTime}\n{characterData.Id}"; // instanceId, 획득 시각, 캐릭터 ID를 줄바꿈으로 구분하여 반환
    }
}
