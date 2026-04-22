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
    public Button unequipWeaponButton;  // 무기 해제 버튼 (OpenAll 전용)
    public Button unequipEquipButton;   // 장비 해제 버튼
    public Button closeButton;          // 팝업 닫기 버튼

    private List<UiInvenSlot> slots = new List<UiInvenSlot>(); // 생성된 슬롯 오브젝트 목록
    private Action<SaveItemData> onSelect;        // 단일 타입 선택 콜백
    private Action<SaveItemData> onSelectWeapon;  // 무기 선택 콜백 (OpenAll 전용)
    private Action<SaveItemData> onSelectEquip;   // 장비 선택 콜백 (OpenAll 전용)
    private Action onClose;                       // 팝업 닫힐 때 호출할 콜백
    private bool isAllMode = false;               // OpenAll 모드 여부

    private void Awake()
    {
        unequipWeaponButton.onClick.AddListener(OnClickUnequipWeapon);
        unequipEquipButton.onClick.AddListener(OnClickUnequipEquip);
        closeButton.onClick.AddListener(Close);
    }

    // 아이템 목록·타입 필터·선택 콜백을 받아 팝업을 여는 메서드
    public void Open(List<SaveItemData> items, ItemTypes type, Action<SaveItemData> callback)
    {
        isAllMode = false;
        onSelect = callback;
        onClose = null;
        gameObject.SetActive(true);
        RefreshSlots(items.Where(x => x.itemData.Type == type).ToList()); // 타입으로 필터링 후 슬롯 갱신
    }

    // 무기+장비 전체 아이템을 보여주고 타입에 따라 자동으로 슬롯에 배분하는 메서드
    public void OpenAll(List<SaveItemData> items, Action<SaveItemData> weaponCallback, Action<SaveItemData> equipCallback, Action closeCallback = null)
    {
        isAllMode = true;
        onSelectWeapon = weaponCallback;
        onSelectEquip = equipCallback;
        onClose = closeCallback;
        gameObject.SetActive(true);
        var filtered = items.Where(x => x.itemData.Type == ItemTypes.Weapon || x.itemData.Type == ItemTypes.Equip).ToList();
        RefreshSlots(filtered);
    }

    public void SetCloseCallback(Action callback)
    {
        onClose = callback;
    }

    // 팝업을 닫는 메서드
    public void Close()
    {
        gameObject.SetActive(false);
        onClose?.Invoke();
    }

    private void OnClickUnequipWeapon()
    {
        if (isAllMode)
            onSelectWeapon?.Invoke(null);
        else
            onSelect?.Invoke(null);
        Close();
    }

    private void OnClickUnequipEquip()
    {
        if (isAllMode)
            onSelectEquip?.Invoke(null);
        else
            onSelect?.Invoke(null);
        Close();
    }

    // 필터링된 아이템 목록으로 슬롯 UI를 갱신하는 메서드
    private void RefreshSlots(List<SaveItemData> items)
    {
        while (slots.Count < items.Count) // 슬롯이 부족하면 추가 생성
        {
            var newSlot = Instantiate(prefab, scrollRect.content);
            newSlot.SetEmpty();
            newSlot.button.onClick.AddListener(() =>
            {
                if (isAllMode)
                {
                    // 아이템 타입에 따라 무기/장비 슬롯으로 자동 분배
                    if (newSlot.SaveItemData?.itemData.Type == ItemTypes.Weapon)
                        onSelectWeapon?.Invoke(newSlot.SaveItemData);
                    else if (newSlot.SaveItemData?.itemData.Type == ItemTypes.Equip)
                        onSelectEquip?.Invoke(newSlot.SaveItemData);
                }
                else
                {
                    onSelect?.Invoke(newSlot.SaveItemData);
                }
            });
            slots.Add(newSlot);
        }

        for (int i = 0; i < slots.Count; i++)
        {
            if (i < items.Count)
            {
                slots[i].gameObject.SetActive(true);
                slots[i].SetItem(items[i]);
            }
            else
            {
                slots[i].gameObject.SetActive(false);
                slots[i].SetEmpty();
            }
        }
    }
}
