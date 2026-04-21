using UnityEngine;
using TMPro; // TextMeshPro 텍스트 컴포넌트 사용을 위한 네임스페이스
using UnityEngine.UI; // Unity UI 컴포넌트(Image, Button 등) 사용을 위한 네임스페이스

// 캐릭터 목록의 개별 슬롯 UI(아이콘, 이름, 버튼)를 관리하는 클래스
public class UiCharacterSlot : MonoBehaviour
{
    public Image imageIcon;
    public TextMeshProUGUI textName;

    public SaveCharacterData SaveCharacterData { get; private set; }

    public int slotIndex { get; set; } // 슬롯 목록에서 이 슬롯의 인덱스

    public Button button;

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

