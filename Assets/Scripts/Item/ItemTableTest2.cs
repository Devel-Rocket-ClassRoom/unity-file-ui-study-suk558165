using UnityEngine; // Unity 엔진 기본 기능 사용을 위한 네임스페이스
using UnityEngine.UI; // Unity UI 컴포넌트(Image 등) 사용을 위한 네임스페이스

// 아이템 상세 정보(아이콘, 이름, 설명)를 UI에 표시하는 테스트 클래스
public class ItemTableTest2 : MonoBehaviour
{
    public Image icon; // 아이템 아이콘을 표시할 Image 컴포넌트
    public LocallizationText textName; // 아이템 이름을 표시할 다국어 텍스트 컴포넌트
    public LocallizationText textDesc; // 아이템 설명을 표시할 다국어 텍스트 컴포넌트

    // 오브젝트가 활성화될 때 호출되는 메서드
    public void OnEnable()
    {
        SetEmpty(); // 활성화 시 UI를 빈 상태로 초기화
    }
    // UI의 아이콘, 이름, 설명을 모두 비우는 초기화 메서드
    public void SetEmpty()
    {
        icon.sprite = null; // 아이콘 스프라이트를 null로 비움
        textName.Id = string.Empty; // 이름 텍스트 ID를 빈 문자열로 초기화
        textDesc.Id = string.Empty; // 설명 텍스트 ID를 빈 문자열로 초기화
        textName.OnChangedId(); // 이름 다국어 텍스트를 갱신하여 화면에 반영
        textDesc.OnChangedId(); // 설명 다국어 텍스트를 갱신하여 화면에 반영
    }
    // 아이템 ID 문자열을 받아 테이블에서 데이터를 조회 후 UI를 설정하는 메서드
    public void SetItemData(string  itemId)
    {
        ItemData data = DataTableManager.ItemTable.Get(itemId); // 아이템 테이블에서 해당 ID의 데이터를 가져옴
        SetItemData(data); // 조회한 데이터로 UI를 설정
    }
    // ItemData 객체를 직접 받아 아이콘, 이름, 설명 UI를 설정하는 메서드
    public void SetItemData(ItemData data)
    {
        icon.sprite = data.SpriteIcon; // 아이템 스프라이트 아이콘을 Image에 설정
        textName.Id = data.Name; // 아이템 이름 키를 다국어 텍스트에 설정
        textDesc.Id = data.Desc; // 아이템 설명 키를 다국어 텍스트에 설정

        textName.OnChangedId(); // 이름 다국어 텍스트를 갱신하여 화면에 반영
        textDesc.OnChangedId(); // 설명 다국어 텍스트를 갱신하여 화면에 반영
    }
}
