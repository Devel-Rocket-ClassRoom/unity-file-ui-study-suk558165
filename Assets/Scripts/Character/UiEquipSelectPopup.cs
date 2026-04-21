using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// 장착 슬롯 클릭 시 나타나는 아이템 선택 팝업 UI를 관리하는 클래스
public class UiEquipSelectPopup : MonoBehaviour
{
    public UiInvenSlot prefab;    // 아이템 슬롯 생성에 사용할 UiInvenSlot 프리팹
    public ScrollRect scrollRect; // 아이템 슬롯 목록을 스크롤할 ScrollRect 컴포넌트
    public Button unequipButton;  // 착용 해제 버튼
    public Button closeButton;    // 팝업 닫기 버튼

    private List<UiInvenSlot> slots = new List<UiInvenSlot>(); // 생성된 슬롯 오브젝트 목록
    private Action<SaveItemData> onSelect; // 선택 시 호출할 콜백 (null 전달 = 해제)

    private void Awake()
    {
        unequipButton.onClick.AddListener(OnClickUnequip); // 착용 해제 버튼 클릭 이벤트 등록
        closeButton.onClick.AddListener(Close);            // 닫기 버튼 클릭 이벤트 등록
    }

    // 아이템 목록·타입 필터·선택 콜백을 받아 팝업을 여는 메서드
    public void Open(List<SaveItemData> items, ItemTypes type, Action<SaveItemData> callback)
    {
        onSelect = callback;
        gameObject.SetActive(true);
        RefreshSlots(items.Where(x => x.itemData.Type == type).ToList()); // 타입으로 필터링 후 슬롯 갱신
    }

    // 팝업을 닫는 메서드
    public void Close()
    {
        gameObject.SetActive(false);
    }

    // 착용 해제 버튼 클릭 핸들러 — null을 콜백으로 전달하여 해제 의미로 사용
    private void OnClickUnequip()
    {
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
                onSelect?.Invoke(newSlot.SaveItemData); // 클릭된 슬롯의 아이템 데이터를 콜백으로 전달
                Close();
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
