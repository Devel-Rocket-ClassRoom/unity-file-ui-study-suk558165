using TMPro;
using UnityEngine;
using UnityEngine.UI;

<<<<<<< HEAD
// 선택된 캐릭터의 상세 정보(아이콘, 이름, 직업, 스탯, 장착 슬롯)를 표시하는 클래스
=======
// 선택된 캐릭터의 상세 정보(아이콘, 이름, 직업, 스탯)를 표시하는 클래스
>>>>>>> ac335ae9d04564330e8933454b073a144641c6c6
public class UiCharacterInfo : MonoBehaviour
{
    public static readonly string FormatCommon = "{0}: {1}"; // "레이블: 값" 형식으로 텍스트를 구성할 때 사용하는 공통 포맷 문자열

    public Image imageIcon;               // 캐릭터 아이콘을 표시할 Image 컴포넌트
    public TextMeshProUGUI textName;      // 캐릭터 이름을 표시할 TextMeshPro 텍스트 컴포넌트
    public TextMeshProUGUI textJob;       // 캐릭터 직업을 표시할 TextMeshPro 텍스트 컴포넌트
    public TextMeshProUGUI textAttack;    // 캐릭터 공격력을 표시할 TextMeshPro 텍스트 컴포넌트
    public TextMeshProUGUI textDefend;    // 캐릭터 방어력을 표시할 TextMeshPro 텍스트 컴포넌트
    public TextMeshProUGUI textHealth;    // 캐릭터 체력을 표시할 TextMeshPro 텍스트 컴포넌트

<<<<<<< HEAD
    public UiCharacterEquipSlot weaponEquipSlot; // 무기 장착 슬롯 UI 컴포넌트
    public UiCharacterEquipSlot equipEquipSlot;  // 장비 장착 슬롯 UI 컴포넌트

=======
>>>>>>> ac335ae9d04564330e8933454b073a144641c6c6
    // 모든 UI 요소를 null로 초기화하여 빈 상태로 만드는 메서드
    public void SetEmpty()
    {
        imageIcon.sprite = null;
        textName.text = null;
        textJob.text = null;
        textAttack.text = null;
        textDefend.text = null;
        textHealth.text = null;
<<<<<<< HEAD
        weaponEquipSlot.SetEmpty(); // 무기 슬롯 초기화
        equipEquipSlot.SetEmpty();  // 장비 슬롯 초기화
    }

    // 저장 캐릭터 데이터를 받아 상세 정보 UI를 모두 설정하는 메서드 — 외부에서 주입하는 유일한 갱신 경로
    public void SetSaveCharacterData(SaveCharacterData saveCharacterData)
    {
        CharacterData data = saveCharacterData.characterData;

        // 착용 아이템의 능력치 보너스 계산 (무기 → 공격력, 장비 → 방어력)
        int weaponBonus = saveCharacterData.weaponSlot?.itemData.Value ?? 0;
        int equipBonus  = saveCharacterData.equipSlot?.itemData.Value ?? 0;

        imageIcon.sprite = data.SpriteIcon;
        textName.text = string.Format(FormatCommon,
            DataTableManager.StringTable.Get("NAME"), data.StringName);
        textJob.text = string.Format(FormatCommon,
            DataTableManager.StringTable.Get("JOB"), data.StringJob);
        textAttack.text = string.Format(FormatCommon,
            DataTableManager.StringTable.Get("Stat_Attack"), data.Attack + weaponBonus); // 무기 보너스 합산
        textDefend.text = string.Format(FormatCommon,
            DataTableManager.StringTable.Get("Stat_Defend"), data.Dffend + equipBonus);  // 장비 보너스 합산
        textHealth.text = string.Format(FormatCommon,
            DataTableManager.StringTable.Get("Stat_Health"), data.Health);

        weaponEquipSlot.Set(saveCharacterData.weaponSlot); // 무기 슬롯 아이콘 설정 (null이면 빈 슬롯)
        equipEquipSlot.Set(saveCharacterData.equipSlot);   // 장비 슬롯 아이콘 설정 (null이면 빈 슬롯)
=======
    }

    // 저장 캐릭터 데이터를 받아 상세 정보 UI를 모두 설정하는 메서드
    public void SetSaveCharacterData(SaveCharacterData saveCharacterData)
    {
        CharacterData data = saveCharacterData.characterData; // 저장 데이터에서 CharacterData를 추출

        imageIcon.sprite = data.SpriteIcon; // 캐릭터 아이콘 스프라이트를 Image에 설정
        textName.text = string.Format(FormatCommon,
            DataTableManager.StringTable.Get("NAME"), data.StringName); // "이름: 캐릭터명" 형식으로 이름 텍스트를 설정
        textJob.text = string.Format(FormatCommon,
            DataTableManager.StringTable.Get("JOB"), data.StringJob);  // "직업: 직업명" 형식으로 직업 텍스트를 설정
        textAttack.text = string.Format(FormatCommon,
            DataTableManager.StringTable.Get("Stat_Attack"), data.Attack); // "공격력: 값" 형식으로 공격력 텍스트를 설정
        textDefend.text = string.Format(FormatCommon,
            DataTableManager.StringTable.Get("Stat_Defend"), data.Dffend); // "방어력: 값" 형식으로 방어력 텍스트를 설정
        textHealth.text = string.Format(FormatCommon,
            DataTableManager.StringTable.Get("Stat_Health"), data.Health); // "체력: 값" 형식으로 체력 텍스트를 설정
>>>>>>> ac335ae9d04564330e8933454b073a144641c6c6
    }
}
