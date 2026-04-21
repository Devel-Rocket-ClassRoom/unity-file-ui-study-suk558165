using TMPro; // TextMeshPro 텍스트 컴포넌트 사용을 위한 네임스페이스
using UnityEngine; // Unity 엔진 기본 기능 사용을 위한 네임스페이스
using UnityEngine.UI; // Unity UI 컴포넌트(Image 등) 사용을 위한 네임스페이스
using Image = UnityEngine.UI.Image; // TMPro와 UnityEngine.UI 간 Image 타입 충돌을 방지하기 위한 별칭 선언

// 아이템 목록과 선택된 아이템의 상세 정보를 화면에 표시하는 클래스
public class Itemscreen : MonoBehaviour
{
    public ItemInpo[] itemInpos; // 화면에 표시할 아이템 슬롯 배열
    public Image detailIcon; // 선택된 아이템의 아이콘을 표시할 Image 컴포넌트
    public TextMeshProUGUI detailName; // 선택된 아이템의 이름을 표시할 텍스트
    public TextMeshProUGUI detailInfo; // 선택된 아이템의 상세 정보를 표시할 텍스트
    // 선택된 아이템 데이터를 받아 상세 정보 UI를 업데이트하는 메서드

    public UiItemInfo uiItemInfo;
    public void ShowDetail(ItemData data)
    {
        var saveItem = new SaveItemData {itemData = data};
        uiItemInfo.SetSaveItemData(saveItem);
    }
    // 씬 시작 시 아이템 슬롯 배열을 초기화하는 메서드
      public void Start()
      {
            for (int i = 0; i < itemInpos.Length; i++) // 슬롯 배열 전체를 순회
            {
                var data = DataTableManager.ItemTable.Get(itemInpos[i].id); // 슬롯의 ID로 아이템 데이터를 테이블에서 조회
                itemInpos[i].SetData(data, this); // 조회한 데이터와 화면 참조를 슬롯에 설정
            }
        }

}
