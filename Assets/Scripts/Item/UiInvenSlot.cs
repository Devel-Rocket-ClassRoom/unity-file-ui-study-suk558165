using TMPro; // TextMeshPro 텍스트 컴포넌트 사용을 위한 네임스페이스
using UnityEngine; // Unity 엔진 기본 기능 사용을 위한 네임스페이스
using UnityEngine.UI; // Unity UI 컴포넌트(Image, Button 등) 사용을 위한 네임스페이스

// 인벤토리의 개별 슬롯 UI(아이콘, 이름, 버튼)를 관리하는 클래스
public class UiInvenSlot : MonoBehaviour
{
    public Image imageIcon; // 아이템 아이콘을 표시할 Image 컴포넌트
    public TextMeshProUGUI textName; // 아이템 이름을 표시할 TextMeshPro 텍스트 컴포넌트

    public SaveItemData SaveItemData { get; private set; } // 이 슬롯에 할당된 저장 아이템 데이터 (외부 읽기 가능, 내부에서만 쓰기 가능)
    public int slotIndex { get;  set; } // 슬롯 목록에서 이 슬롯의 인덱스

    public Button button; // 슬롯 클릭 이벤트를 처리할 버튼 컴포넌트

    // 슬롯을 빈 상태로 초기화하는 메서드
    public void SetEmpty()
    {
        imageIcon.sprite = null; // 아이콘 스프라이트를 null로 비움
        textName.text = string.Empty;  // 이름 텍스트를 빈 문자열로 초기화
        SaveItemData = null; // 저장 아이템 데이터 참조를 해제
    }

    // 저장 아이템 데이터를 받아 슬롯 UI(아이콘, 이름)를 설정하는 메서드
    public void SetItem(SaveItemData data)
    {
        SaveItemData = data; // 전달받은 저장 아이템 데이터를 필드에 저장
        imageIcon.sprite = SaveItemData.itemData.SpriteIcon; // 아이템의 스프라이트 아이콘을 Image에 설정
        textName.text = SaveItemData.itemData.StringName; // 다국어 처리된 아이템 이름을 텍스트에 설정
    }
}
