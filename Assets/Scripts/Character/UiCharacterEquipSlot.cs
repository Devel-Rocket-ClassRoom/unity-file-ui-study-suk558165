using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

// 캐릭터 정보창의 무기/장비 슬롯 UI(아이콘, 버튼)를 관리하는 클래스
public class UiCharacterEquipSlot : MonoBehaviour
{
    public Image imageIcon;           // 착용 아이템 아이콘을 표시할 Image 컴포넌트
    public TextMeshProUGUI textName;  // 착용 아이템 이름을 표시할 텍스트 컴포넌트
    public Button button;             // 슬롯 클릭 이벤트를 처리할 버튼 컴포넌트
    public UnityEvent onClickSlot;    // 슬롯 버튼 클릭 시 발생하는 이벤트
    public Sprite emptySprite;        // 아이템 미착용 시 표시할 기본 스프라이트
    public string emptyText;          // 아이템 미착용 시 표시할 기본 텍스트 (예: "무기", "장비")

    private void Awake()
    {
        button.onClick.AddListener(() => onClickSlot.Invoke());
    }

    // 아이템 데이터를 받아 슬롯 아이콘과 이름을 설정하는 메서드 (null이면 기본 스프라이트 표시)
    public void Set(SaveItemData data)
    {
        imageIcon.sprite = data?.itemData.SpriteIcon ?? emptySprite;
        if (textName != null)
            textName.text = data?.itemData.StringName ?? string.Empty;
    }

    // 슬롯을 빈 상태로 초기화하는 메서드
    public void SetEmpty()
    {
        imageIcon.sprite = emptySprite;
        if (textName != null)
            textName.text = emptyText;
    }
}
