using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// 장착 슬롯 클릭 시 나타나는 아이템 선택 팝업 UI를 관리하는 클래스
public class UiEquipSelectPopup : MonoBehaviour
{
    public UiInvenSlot prefab;          // 아이템 슬롯 생성에 사용할 UiInvenSlot 프리팹
    public ScrollRect scrollRect;       // 아이템 슬롯 목록을 스크롤할 ScrollRect 컴포넌트
    public Button unequipWeaponButton;  // 무기 해제 버튼 (OpenAll 모드 전용)
    public Button unequipEquipButton;   // 장비 해제 버튼 (단일·OpenAll 모드 공용)
    public Button closeButton;          // 팝업 닫기 버튼

    private List<UiInvenSlot> slots = new List<UiInvenSlot>(); // 재사용을 위해 풀링하는 슬롯 오브젝트 목록
    private Action<SaveItemData> onSelect;        // Open() 호출 시 등록되는 단일 타입 선택 콜백 (선택한 아이템을 전달)
    private Action<SaveItemData> onSelectWeapon;  // OpenAll() 호출 시 등록되는 무기 선택 콜백
    private Action<SaveItemData> onSelectEquip;   // OpenAll() 호출 시 등록되는 장비 선택 콜백
    private Action onClose;                       // 팝업이 닫힐 때 호출할 콜백 (캐릭터 슬롯 복원 등)
    private bool isAllMode = false;               // true면 OpenAll 모드 (무기+장비 동시 표시), false면 단일 타입 모드

    // 씬 로드 시 각 버튼의 클릭 이벤트에 핸들러를 등록하는 메서드
    private void Awake()
    {
        unequipWeaponButton.onClick.AddListener(OnClickUnequipWeapon); // 무기 해제 버튼 클릭 → OnClickUnequipWeapon 호출
        unequipEquipButton.onClick.AddListener(OnClickUnequipEquip);   // 장비 해제 버튼 클릭 → OnClickUnequipEquip 호출
        closeButton.onClick.AddListener(Close);                        // 닫기 버튼 클릭 → Close 호출
    }

    // 지정 타입의 아이템만 필터링하여 팝업을 여는 메서드 (무기 슬롯 또는 장비 슬롯 단독 클릭 시 사용)
    public void Open(List<SaveItemData> items, ItemTypes type, Action<SaveItemData> callback)
    {
        isAllMode = false;    // 단일 타입 모드로 설정
        onSelect = callback;  // 아이템 선택 시 호출할 콜백 등록
        onClose = null;       // 닫기 콜백 초기화 (SetCloseCallback으로 나중에 등록)
        gameObject.SetActive(true); // 팝업 GameObject를 활성화하여 화면에 표시
        RefreshSlots(items.Where(x => x.itemData.Type == type).ToList()); // 지정 타입으로 필터링 후 슬롯 갱신
    }

    // 무기+장비 전체 아이템을 한 화면에 표시하고 타입별 콜백으로 자동 분배하는 메서드 (전체 장비 버튼 클릭 시 사용)
    public void OpenAll(List<SaveItemData> items, Action<SaveItemData> weaponCallback, Action<SaveItemData> equipCallback, Action closeCallback = null)
    {
        isAllMode = true;               // OpenAll 모드로 설정
        onSelectWeapon = weaponCallback; // 무기 아이템 선택 시 호출할 콜백 등록
        onSelectEquip = equipCallback;   // 장비 아이템 선택 시 호출할 콜백 등록
        onClose = closeCallback;         // 팝업 닫힐 때 호출할 콜백 등록
        gameObject.SetActive(true);      // 팝업 GameObject를 활성화하여 화면에 표시
        var filtered = items.Where(x => x.itemData.Type == ItemTypes.Weapon || x.itemData.Type == ItemTypes.Equip).ToList(); // 무기·장비 타입만 필터링
        RefreshSlots(filtered); // 필터링된 아이템 목록으로 슬롯 갱신
    }

    // 팝업이 닫힐 때 실행할 콜백을 외부에서 설정하는 메서드 (Open 호출 후 별도 등록 가능)
    public void SetCloseCallback(Action callback)
    {
        onClose = callback; // 닫기 콜백을 전달받은 함수로 교체
    }

    // 팝업을 비활성화하고 닫기 콜백을 호출하는 메서드
    public void Close()
    {
        gameObject.SetActive(false); // 팝업 GameObject를 비활성화하여 화면에서 숨김
        onClose?.Invoke();           // 닫기 콜백이 등록된 경우에만 호출 (캐릭터 슬롯 복원 등)
    }

    // 무기 해제 버튼 클릭 시 호출되는 핸들러 — null을 콜백에 전달하여 무기 슬롯을 비움
    private void OnClickUnequipWeapon()
    {
        if (isAllMode)
            onSelectWeapon?.Invoke(null); // OpenAll 모드: 무기 콜백에 null 전달 → 무기 슬롯 해제
        else
            onSelect?.Invoke(null);       // 단일 모드: 선택 콜백에 null 전달 → 슬롯 해제
        Close(); // 해제 후 팝업 닫기
    }

    // 장비 해제 버튼 클릭 시 호출되는 핸들러 — null을 콜백에 전달하여 장비 슬롯을 비움
    private void OnClickUnequipEquip()
    {
        if (isAllMode)
            onSelectEquip?.Invoke(null); // OpenAll 모드: 장비 콜백에 null 전달 → 장비 슬롯 해제
        else
            onSelect?.Invoke(null);      // 단일 모드: 선택 콜백에 null 전달 → 슬롯 해제
        Close(); // 해제 후 팝업 닫기
    }

    // 전달받은 아이템 목록을 슬롯에 배분하고 초과 슬롯은 비활성화하는 메서드
    private void RefreshSlots(List<SaveItemData> items)
    {
        while (slots.Count < items.Count) // 슬롯 풀이 부족하면 새 슬롯을 생성하여 보충
        {
            var newSlot = Instantiate(prefab, scrollRect.content); // 프리팹을 ScrollRect content 하위에 생성
            newSlot.SetEmpty(); // 생성 직후 빈 상태로 초기화
            newSlot.button.onClick.AddListener(() => // 슬롯 버튼 클릭 시 아이템 타입에 맞는 콜백 호출
            {
                if (isAllMode)
                {
                    if (newSlot.SaveItemData?.itemData.Type == ItemTypes.Weapon)
                        onSelectWeapon?.Invoke(newSlot.SaveItemData); // 무기 타입이면 무기 콜백 호출
                    else if (newSlot.SaveItemData?.itemData.Type == ItemTypes.Equip)
                        onSelectEquip?.Invoke(newSlot.SaveItemData);  // 장비 타입이면 장비 콜백 호출
                }
                else
                {
                    onSelect?.Invoke(newSlot.SaveItemData); // 단일 모드: 선택 콜백에 아이템 데이터 전달
                }
            });
            slots.Add(newSlot); // 생성된 슬롯을 풀 목록에 추가
        }

        for (int i = 0; i < slots.Count; i++) // 슬롯 풀 전체를 순회하며 데이터 할당 또는 비활성화
        {
            if (i < items.Count) // 표시할 아이템이 있는 슬롯
            {
                slots[i].gameObject.SetActive(true); // 슬롯을 활성화하여 화면에 표시
                slots[i].SetItem(items[i]);          // 해당 인덱스의 아이템 데이터를 슬롯에 설정
            }
            else // 표시할 아이템이 없는 초과 슬롯
            {
                slots[i].gameObject.SetActive(false); // 슬롯을 비활성화하여 화면에서 숨김
                slots[i].SetEmpty();                  // 슬롯을 빈 상태로 초기화
            }
        }
    }
}
