using UnityEngine; // Unity 엔진 핵심 라이브러리 (MonoBehaviour, Input, Debug 등)

// 키 입력으로 저장/불러오기를 테스트하는 MonoBehaviour 클래스
public class SaveLoadTest1 : MonoBehaviour
{
    // 매 프레임마다 키 입력을 감지하여 저장 또는 로드를 실행하는 메서드
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) // 숫자키 1을 눌렀을 때 저장 테스트 실행
        {
            SaveLoadManager.Data = new SaveDataV6(); // 새 V5 세이브 데이터 객체 생성 및 할당
            SaveLoadManager.Data.Name = "TEST1234"; // 플레이어 이름을 테스트 값으로 설정
            SaveLoadManager.Data.Gold = 4321; // 골드를 테스트 값으로 설정
            SaveLoadManager.Data.itemList.Add(new SaveItemData() { itemData = DataTableManager.ItemTable.Get("Item1") }); // 아이템 테이블에서 Item1을 조회하여 인벤토리에 추가
            SaveLoadManager.Data.itemList.Add(new SaveItemData() { itemData = DataTableManager.ItemTable.Get("Item2") }); // 아이템 테이블에서 Item2를 조회하여 인벤토리에 추가

            bool result = SaveLoadManager.Save(); // 현재 데이터를 기본 슬롯(0)에 저장하고 성공 여부 반환
            Debug.Log($"[Save] 결과: {result}"); // 저장 성공/실패 결과 출력
            Debug.Log($"[Save] Name: {SaveLoadManager.Data.Name}"); // 저장된 플레이어 이름 출력
            Debug.Log($"[Save] Gold: {SaveLoadManager.Data.Gold}"); // 저장된 골드 출력
            Debug.Log($"[Save] Version: {SaveLoadManager.Data.Version}"); // 저장된 세이브 데이터 버전 출력
            Debug.Log($"[Save] ItemList 수: {SaveLoadManager.Data.itemList.Count}"); // 저장된 아이템 개수 출력
            foreach (var item in SaveLoadManager.Data.itemList) // 저장된 아이템 목록 순회
            {
                Debug.Log($"[Save] Item: {item.itemData.Name}"); // 각 아이템의 이름 출력
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) // 숫자키 2를 눌렀을 때 로드 테스트 실행
        {
            bool result = SaveLoadManager.Load(); // 기본 슬롯(0)에서 데이터를 불러오고 성공 여부 반환
            Debug.Log($"[Load] 결과: {result}"); // 로드 성공/실패 결과 출력
            if (result) // 로드가 성공했을 때만 데이터 출력
            {
                Debug.Log($"[Load] Name: {SaveLoadManager.Data.Name}"); // 불러온 플레이어 이름 출력
                Debug.Log($"[Load] Gold: {SaveLoadManager.Data.Gold}"); // 불러온 골드 출력
                Debug.Log($"[Load] Version: {SaveLoadManager.Data.Version}"); // 불러온 세이브 데이터 버전 출력
                Debug.Log($"[Load] ItemList 수: {SaveLoadManager.Data.itemList.Count}"); // 불러온 아이템 개수 출력
                foreach (var item in SaveLoadManager.Data.itemList) // 불러온 아이템 목록 순회
                {
                    Debug.Log($"[Load] Item: {item.itemData.Name}"); // 각 아이템의 이름 출력
                }
            }
        }
    }
}
