using Unity.VisualScripting; // Unity 비주얼 스크립팅 연동 기능 사용을 위한 네임스페이스
using UnityEngine; // Unity 엔진 기본 기능 사용을 위한 네임스페이스
using UnityEngine.UI; // Unity UI 컴포넌트(Image, Button 등) 사용을 위한 네임스페이스

// 아이템 ID를 기반으로 아이콘과 이름을 UI에 표시하는 테스트 클래스
public class ItemTableTest1 : MonoBehaviour
{
    public string itemId; // 표시할 아이템의 고유 ID
    public Image icon; // 아이템 아이콘을 표시할 Image 컴포넌트
    public LocallizationText textName; // 아이템 이름을 표시할 다국어 텍스트 컴포넌트

    public ItemTableTest2 itemInfo; // 아이템 상세 정보를 표시할 ItemTableTest2 참조
    // 오브젝트가 활성화될 때 호출되는 메서드
    private void OnEnable()
    {
        OnChangeItemId(); // 아이템 ID에 맞게 UI를 갱신
    }

    // 인스펙터에서 값이 변경될 때 호출되는 메서드 (에디터 전용)
    private void OnValidate()
    {
        textName.OnChangedId(); // 텍스트 ID가 바뀌었을 때 다국어 텍스트를 갱신
    }
    // 아이템 ID가 변경되었을 때 아이콘과 이름을 갱신하는 메서드
    public void OnChangeItemId()
    {
        if (itemId != null) // itemId가 null이 아닌 경우에만 처리
        {
            ItemData data = DataTableManager.ItemTable.Get(itemId); // 아이템 테이블에서 해당 ID의 데이터를 가져옴
            icon.sprite = data.SpriteIcon; // 가져온 데이터의 스프라이트 아이콘을 Image에 설정
            textName.Id = data.Name; // 아이템 이름 키를 다국어 텍스트에 설정
            textName.OnChangedId(); // 다국어 텍스트를 갱신하여 화면에 반영
        }
    }

    // 슬롯 클릭 시 상세 정보 패널에 아이템 정보를 표시하는 메서드
    public void Onclick()
    {
        itemInfo.SetItemData(itemId); // itemId를 전달하여 상세 정보 UI를 갱신
    }
}
