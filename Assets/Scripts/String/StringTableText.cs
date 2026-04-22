using UnityEngine;
using TMPro;
public class StringTableText : MonoBehaviour // 문자열 테이블에서 텍스트를 가져와 표시하는 컴포넌트
{
    public string Id; // 문자열 테이블에서 참조할 키 ID
    public TextMeshProUGUI text; // 텍스트를 출력할 TextMeshPro UI 컴포넌트

    private void Start() // 오브젝트가 활성화될 때 최초 한 번 호출
    {
        OnChangedId(); // ID에 해당하는 텍스트로 초기화
    }

    private void OnChangedId() // ID가 변경되었을 때 텍스트를 갱신하는 메서드
    {
        text.text = DataTableManager.StringTable.Get(Id); // 문자열 테이블에서 ID에 해당하는 텍스트를 가져와 설정
    }
}
