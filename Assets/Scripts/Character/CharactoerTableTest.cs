using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharactoerTableTest : MonoBehaviour // 캐릭터 테이블 데이터를 UI에 표시하는 테스트 컴포넌트
{
    public Image icon; // 캐릭터 아이콘 이미지 컴포넌트
    public LocallizationText textName; // 다국어 지원 캐릭터 이름 텍스트
    public LocallizationText textDesc; // 다국어 지원 캐릭터 직업 설명 텍스트
    public TextMeshProUGUI textStat; // 캐릭터 스탯(공격/방어/체력)을 표시할 TMP 텍스트

    private string[] ids = { "class1", "class2", "class3", "class4" }; // 캐릭터 ID 목록
    public Sprite[] icons; // 캐릭터별 아이콘 스프라이트 배열
    private int index = 0; // 현재 선택된 캐릭터 인덱스

    public void OnEnable() // 오브젝트가 활성화될 때 호출되는 메서드
    {
        SetItemData(ids[index]); // 현재 인덱스의 캐릭터 데이터를 UI에 표시
    }

    public void SetItemData(string itemId) // 캐릭터 ID를 받아 데이터를 로드하고 UI를 갱신하는 메서드
    {
        CharacterData data = DataTableManager.CharacterTable.Get(itemId); // 캐릭터 ID로 데이터 테이블에서 캐릭터 데이터 조회
        SetItemData(data); // 조회한 데이터를 UI에 적용
    }

    public void SetItemData(CharacterData data) // 캐릭터 데이터를 받아 UI 요소에 적용하는 메서드
    {
        textName.Id = data.Name; // 이름 텍스트의 로컬라이제이션 ID를 캐릭터 이름으로 설정
        textName.OnChangedId(); // 변경된 ID로 텍스트를 현재 언어에 맞게 갱신
        textDesc.Id = data.JOB; // 설명 텍스트의 로컬라이제이션 ID를 캐릭터 직업으로 설정
        textDesc.OnChangedId(); // 변경된 ID로 텍스트를 현재 언어에 맞게 갱신
        icon.sprite = icons[index]; // 현재 인덱스에 해당하는 캐릭터 아이콘 스프라이트 적용
        textStat.text = $"{DataTableManager.StringTable.Get("Stat_Attack")}: {data.Attack}\n{DataTableManager.StringTable.Get("Stat_Defend")}: {data.Dffend}\n{DataTableManager.StringTable.Get("Stat_Health")}: {data.Health}"; // 공격/방어/체력 스탯을 다국어 레이블과 함께 포맷하여 표시
    }

    public void OnClick() // 버튼 클릭 시 다음 캐릭터로 전환하는 메서드
    {
        index = (index + 1) % ids.Length; // 인덱스를 1 증가시키고 배열 길이로 나머지 연산하여 순환
        SetItemData(ids[index]); // 변경된 인덱스의 캐릭터 데이터를 UI에 표시
    }

    public void RefreshLanguage() // 언어 변경 시 현재 캐릭터 UI를 새 언어로 갱신하는 메서드
    {
        SetItemData(ids[index]); // 현재 인덱스의 캐릭터 데이터를 다시 불러와 언어를 반영
    }
}
