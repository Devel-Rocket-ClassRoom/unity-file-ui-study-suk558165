using TMPro; 
using UnityEngine; 
using UnityEngine.UI; 

public class UiItemInfo : MonoBehaviour
{
    public static readonly string FormatCommon = "{0}: {1}"; // "레이블: 값" 형식으로 텍스트를 구성할 때 사용하는 공통 포맷 문자열

    public Image imageIcon;                  // 아이템 아이콘을 표시할 Image 컴포넌트
    public TextMeshProUGUI textName;         // 아이템 이름을 표시할 TextMeshPro 텍스트 컴포넌트
    public TextMeshProUGUI textDescription;  // 아이템 설명을 표시할 TextMeshPro 텍스트 컴포넌트
    public TextMeshProUGUI textType;         // 아이템 타입을 표시할 TextMeshPro 텍스트 컴포넌트
    public TextMeshProUGUI textValue;        // 아이템 수치를 표시할 TextMeshPro 텍스트 컴포넌트
    public TextMeshProUGUI textCost;         // 아이템 비용을 표시할 TextMeshPro 텍스트 컴포넌트

    // 모든 UI 요소를 null로 초기화하여 빈 상태로 만드는 메서드
    public void SetEmpty()
    {
        imageIcon.sprite = null;       // 아이콘 스프라이트를 null로 비움
        textName.text = null;          // 이름 텍스트를 null로 비움
        textDescription.text = null;   // 설명 텍스트를 null로 비움
        textType.text = null;          // 타입 텍스트를 null로 비움
        textValue.text = null;         // 수치 텍스트를 null로 비움
        textCost.text = null;          // 비용 텍스트를 null로 비움
    }

    // 저장 아이템 데이터를 받아 상세 정보 UI를 모두 설정하는 메서드
    public void SetSaveItemData(SaveItemData saveItemData)
    {
        ItemData data = saveItemData.itemData; // 저장 데이터에서 ItemData를 추출

        imageIcon.sprite = data.SpriteIcon; // 아이템 아이콘 스프라이트를 Image에 설정
        textName.text = string.Format(FormatCommon,
            DataTableManager.StringTable.Get("NAME"), data.StringName); // "이름: 아이템명" 형식으로 이름 텍스트를 설정
        textDescription.text = string.Format(FormatCommon,
            DataTableManager.StringTable.Get("DESC"), data.StringDesc); // "설명: 아이템설명" 형식으로 설명 텍스트를 설정
        string id = data.Type.ToString().ToUpper(); // 아이템 타입을 대문자 문자열로 변환하여 다국어 키로 사용
        textType.text = string.Format(FormatCommon,
            DataTableManager.StringTable.Get("TYPE"),
            DataTableManager.StringTable.Get(id)); // "타입: 무기/장비/소비" 형식으로 타입 텍스트를 설정
        textValue.text = string.Format(FormatCommon,
            DataTableManager.StringTable.Get("VALUE"), data.Value); // "수치: 값" 형식으로 수치 텍스트를 설정
        textCost.text = string.Format(FormatCommon,
            DataTableManager.StringTable.Get("COST"), data.Cost); // "비용: 값" 형식으로 비용 텍스트를 설정
    }

    // 매 프레임마다 키 입력을 감지하여 테스트 동작을 수행하는 메서드
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) // 숫자 1 키를 누르면
        {
            SetEmpty(); // UI를 빈 상태로 초기화
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) // 숫자 2 키를 누르면
        {
            SetSaveItemData(SaveItemData.GetRandomItem()); // 랜덤 아이템 데이터로 UI를 설정
        }
    }
}
