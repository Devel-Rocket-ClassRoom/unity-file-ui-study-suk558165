using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

// 아이템 목록과 선택된 아이템의 상세 정보를 화면에 표시하는 클래스
public class Itemscreen : MonoBehaviour
{
    public ItemInpo[] itemInpos;            // 화면에 표시할 아이템 슬롯 배열
    public Image detailIcon;               // 선택된 아이템의 아이콘을 표시할 Image 컴포넌트
    public TextMeshProUGUI detailName;     // 선택된 아이템의 이름을 표시할 텍스트
    public TextMeshProUGUI detailInfo;     // 선택된 아이템의 상세 정보를 표시할 텍스트
    public UiItemInfo uiItemInfo;          // 아이템 상세 정보를 표시하는 UiItemInfo 컴포넌트

    // 슬롯에서 아이템이 클릭되면 해당 데이터를 UiItemInfo에 전달하여 상세 정보를 표시하는 메서드
    public void ShowDetail(ItemData data)
    {
        var saveItem = new SaveItemData { itemData = data }; // ItemData를 SaveItemData로 래핑하여 UiItemInfo에 전달 가능하게 변환
        uiItemInfo.SetSaveItemData(saveItem);               // 변환된 데이터를 UiItemInfo에 전달하여 상세 정보 UI 갱신
    }

    // 씬 시작 시 아이템 슬롯 배열을 초기화하는 메서드
    public void Start()
    {
        for (int i = 0; i < itemInpos.Length; i++) // 슬롯 배열 전체를 순회
        {
            var data = DataTableManager.ItemTable.Get(itemInpos[i].id); // 슬롯의 ID로 아이템 데이터를 테이블에서 조회
            itemInpos[i].SetData(data, this);                           // 조회한 데이터와 화면 참조를 슬롯에 설정
        }
    }
}
