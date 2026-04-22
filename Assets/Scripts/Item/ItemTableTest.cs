using UnityEngine;

// 아이템 테이블 데이터를 디버그 출력으로 확인하는 테스트 클래스
public class ItemTableTest : MonoBehaviour
{
    // 매 프레임마다 호출되는 업데이트 메서드
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape)) // ESC 키를 떼는 순간 감지
        {
            Debug.Log(DataTableManager.ItemTable); // 아이템 테이블 전체 내용을 콘솔에 출력
        }
    }
}
