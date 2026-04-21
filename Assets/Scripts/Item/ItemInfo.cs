using TMPro; // TextMeshPro 텍스트 컴포넌트 사용을 위한 네임스페이스
using UnityEngine; // Unity 엔진 기본 기능 사용을 위한 네임스페이스
using UnityEngine.UI; // Unity UI 컴포넌트(Image 등) 사용을 위한 네임스페이스

// 개별 아이템 슬롯의 UI(이름, 아이콘)를 관리하고 클릭 이벤트를 처리하는 클래스
public class ItemInpo : MonoBehaviour
{
    public TextMeshProUGUI text; // 아이템 이름을 표시할 TextMeshPro 텍스트 컴포넌트
    public Image sprite; // 아이템 아이콘을 표시할 Image 컴포넌트
    public ItemData itemData; // 이 슬롯에 할당된 아이템 데이터
    public Itemscreen itemscreen; // 상세 정보를 표시할 Itemscreen 참조
    public string id; // 이 슬롯에 표시할 아이템의 고유 ID

    // 아이템 데이터와 화면 참조를 받아 슬롯 UI를 설정하는 메서드
    public void SetData(ItemData data, Itemscreen screen)
    {
       itemData = data; // 전달받은 아이템 데이터를 필드에 저장
       itemscreen = screen; // 전달받은 화면 참조를 필드에 저장

        text.text = itemData.StringName; // 다국어 처리된 아이템 이름을 텍스트에 설정
        sprite.sprite = itemData.SpriteIcon; // 아이템 아이콘 스프라이트를 Image에 설정
    }
    // 슬롯 클릭 시 호출되어 상세 정보 화면에 이 아이템의 데이터를 전달하는 메서드
    public void OnClick()
    {
        itemscreen.ShowDetail(itemData); // 저장된 아이템 데이터를 Itemscreen에 전달하여 상세 정보를 표시
    }
}
