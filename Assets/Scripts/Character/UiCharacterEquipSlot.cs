using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// 캐릭터 정보창의 무기/장비 슬롯 UI(아이콘, 버튼)를 관리하는 클래스
public class UiCharacterEquipSlot : MonoBehaviour
{
    public Image imageIcon;        // 착용 아이템 아이콘을 표시할 Image 컴포넌트
    public Button button;          // 슬롯 클릭 이벤트를 처리할 버튼 컴포넌트
    public UnityEvent onClickSlot; // 슬롯 버튼 클릭 시 발생하는 이벤트

    private void Awake()
    {
        button.onClick.AddListener(() => onClickSlot.Invoke()); // 버튼 클릭을 onClickSlot 이벤트로 전달
    }

    // 아이템 데이터를 받아 슬롯 아이콘을 설정하는 메서드 (null이면 빈 슬롯 표시)
    public void Set(SaveItemData data)
    {
        imageIcon.sprite = data?.itemData.SpriteIcon;
    }

    // 슬롯을 빈 상태로 초기화하는 메서드
    public void SetEmpty()
    {
        imageIcon.sprite = null;
    }
}
