using UnityEngine;
using TMPro;
using UnityEngine.UI;

// 캐릭터 목록의 개별 슬롯 UI(아이콘, 이름, 버튼)를 관리하는 클래스
public class UiCharacterSlot : MonoBehaviour
{
    public Image imageIcon;           // 캐릭터 아이콘을 표시할 Image 컴포넌트
    public TextMeshProUGUI textName;  // 캐릭터 이름을 표시할 TextMeshPro 텍스트 컴포넌트

    public SaveCharacterData SaveCharacterData { get; private set; } // 이 슬롯에 할당된 캐릭터 저장 데이터 (외부 읽기 가능, 내부에서만 쓰기 가능)

    public int slotIndex { get; set; } // 슬롯 목록에서 이 슬롯의 인덱스

    public Button button; // 슬롯 클릭 이벤트를 처리할 버튼 컴포넌트

    // 슬롯을 빈 상태로 초기화하는 메서드
    public void SetEmpty()
    {
        imageIcon.sprite = null;
        textName.text = string.Empty;
        SaveCharacterData = null;
    }

    // 캐릭터 데이터를 받아 슬롯 UI(아이콘, 이름)를 설정하는 메서드
    public void SetCharacter(SaveCharacterData data)
    {
        SaveCharacterData = data;
        imageIcon.sprite = SaveCharacterData.characterData.SpriteIcon;
        textName.text = SaveCharacterData.characterData.StringName;
    }
}

