using UnityEngine; // Unity 핵심 엔진 라이브러리
using UnityEngine.UI; // Unity UI 컴포넌트 라이브러리 (Image, Button 등)
using TMPro; // TextMeshPro UI 텍스트 컴포넌트 라이브러리

public class CharactoerTableTest2 : MonoBehaviour // 여러 캐릭터 버튼과 상세 정보 패널을 함께 관리하는 테스트 컴포넌트
{
    [Header("버튼 목록")]
    public string[] characterIds = { "class1", "class2", "class3", "class4" }; // 버튼에 매핑된 캐릭터 ID 배열
    public Image[] buttonIcons; // 각 버튼에 표시할 캐릭터 아이콘 이미지 배열
    public LocallizationText[] buttonNames; // 각 버튼에 표시할 캐릭터 이름 다국어 텍스트 배열
    public LocallizationText[] buttonJobs; // 각 버튼에 표시할 캐릭터 직업 다국어 텍스트 배열

    [Header("상세 정보 (가운데)")]
    public Image icon; // 선택된 캐릭터의 상세 아이콘 이미지
    public LocallizationText textName; // 선택된 캐릭터의 이름 다국어 텍스트
    public LocallizationText textDesc; // 선택된 캐릭터의 직업 설명 다국어 텍스트
    public TextMeshProUGUI textStat; // 선택된 캐릭터의 스탯을 표시할 TMP 텍스트

    public void OnEnable() // 오브젝트가 활성화될 때 호출되는 메서드
    {
        SetEmpty(); // 상세 정보 영역을 초기화(빈 상태로)
        for (int i = 0; i < characterIds.Length; i++) // 모든 캐릭터 ID 개수만큼 반복
            SetButtonData(i); // 각 인덱스의 버튼 데이터를 캐릭터 테이블에서 로드하여 적용
    }

    private void SetButtonData(int index) // 지정된 인덱스의 버튼 UI에 캐릭터 데이터를 적용하는 메서드
    {
        CharacterData data = DataTableManager.CharacterTable.Get(characterIds[index]); // 해당 인덱스의 캐릭터 ID로 데이터 조회

        if (buttonIcons != null && index < buttonIcons.Length) // 아이콘 배열이 유효하고 인덱스 범위 내인지 확인
            buttonIcons[index].sprite = data.SpriteIcon; // 버튼 아이콘에 캐릭터 스프라이트 적용

        if (buttonNames != null && index < buttonNames.Length) // 이름 텍스트 배열이 유효하고 인덱스 범위 내인지 확인
        {
            buttonNames[index].Id = data.Name; // 버튼 이름 텍스트의 로컬라이제이션 ID 설정
            buttonNames[index].OnChangedId(); // 변경된 ID로 텍스트를 현재 언어에 맞게 갱신
        }

        if (buttonJobs != null && index < buttonJobs.Length) // 직업 텍스트 배열이 유효하고 인덱스 범위 내인지 확인
        {
            buttonJobs[index].Id = data.JOB; // 버튼 직업 텍스트의 로컬라이제이션 ID 설정
            buttonJobs[index].OnChangedId(); // 변경된 ID로 텍스트를 현재 언어에 맞게 갱신
        }
    }

    public void SetEmpty() // 상세 정보 영역의 UI를 빈 상태로 초기화하는 메서드
    {
        icon.sprite = null; // 상세 아이콘 이미지를 비움
        textName.Id = string.Empty; // 이름 텍스트의 로컬라이제이션 ID를 빈 문자열로 설정
        textDesc.Id = string.Empty; // 설명 텍스트의 로컬라이제이션 ID를 빈 문자열로 설정
        textName.OnChangedId(); // 빈 ID로 이름 텍스트 갱신
        textDesc.OnChangedId(); // 빈 ID로 설명 텍스트 갱신
        if (textStat != null) textStat.text = string.Empty; // 스탯 텍스트가 있으면 빈 문자열로 초기화
    }

    public void OnClickButton(int index) // 캐릭터 버튼 클릭 시 해당 캐릭터의 상세 정보를 표시하는 메서드
    {
        CharacterData data = DataTableManager.CharacterTable.Get(characterIds[index]); // 클릭된 버튼의 캐릭터 ID로 데이터 조회

        icon.sprite = data.SpriteIcon; // 상세 아이콘에 캐릭터 스프라이트 적용
        textName.Id = data.Name; // 상세 이름 텍스트의 로컬라이제이션 ID 설정
        textDesc.Id = data.JOB; // 상세 직업 텍스트의 로컬라이제이션 ID 설정
        textName.OnChangedId(); // 변경된 ID로 이름 텍스트를 현재 언어로 갱신
        textDesc.OnChangedId(); // 변경된 ID로 직업 텍스트를 현재 언어로 갱신

        if (textStat != null) // 스탯 텍스트 컴포넌트가 존재하는지 확인
            textStat.text = $"{DataTableManager.StringTable.Get("Stat_Attack")}: {data.Attack}\n" + // 공격력 스탯을 다국어 레이블과 함께 표시
                            $"{DataTableManager.StringTable.Get("Stat_Defend")}: {data.Dffend}\n" + // 방어력 스탯을 다국어 레이블과 함께 표시
                            $"{DataTableManager.StringTable.Get("Stat_Health")}: {data.Health}"; // 체력 스탯을 다국어 레이블과 함께 표시
    }

    public void RefreshLanguage() // 언어 변경 시 모든 버튼 UI를 새 언어로 갱신하는 메서드
    {
        for (int i = 0; i < characterIds.Length; i++) // 모든 캐릭터 ID 개수만큼 반복
            SetButtonData(i); // 각 버튼 데이터를 다시 로드하여 새 언어 반영
    }
}
