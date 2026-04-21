//using Newtonsoft.Json; // JSON 직렬화/역직렬화를 위한 Newtonsoft.Json 라이브러리 (현재 비활성화)
//using System.Collections.Generic; // List 등 제네릭 컬렉션을 위한 라이브러리 (현재 비활성화)
//using UnityEngine; // Unity의 핵심 기능을 위한 라이브러리 (현재 비활성화)

//public class SpawnManager : MonoBehaviour // 오브젝트 생성/저장/로드를 관리하는 MonoBehaviour 클래스
//{
//    public GameObject[] spawnPrefab;        // 스폰할 프리팹 배열
//    public JsonTest2 jsonTest2;             // JSON 저장/로드 담당 컴포넌트 참조
//    private List<GameObject> spawnedObjects = new List<GameObject>();   // 생성된 오브젝트 리스트
//    private List<int> spawnedIndexes = new List<int>();                 // 생성된 오브젝트의 프리팹 인덱스 리스트

//    public void Create() // 랜덤 오브젝트 3개를 씬에 생성하는 메서드
//    {
//        for (int i = 0; i < 3; i++) // 3회 반복하여 오브젝트 생성
//        {
//            int index = Random.Range(0, spawnPrefab.Length);            // 랜덤 프리팹 인덱스 선택
//            Vector3 pos = new Vector3(Random.Range(-8f, 8f), Random.Range(-8f, 8f), Random.Range(-8f, 8f));              // 랜덤 위치 생성
//            GameObject obj = Instantiate(spawnPrefab[index], pos, Random.rotation); // 랜덤 회전으로 오브젝트 생성
//            Color color = new Color(Random.value, Random.value, Random.value);      // 랜덤 색상 생성
//            obj.GetComponent<Renderer>().material.color = color;        // 색상 적용
//            spawnedObjects.Add(obj);                                    // 오브젝트 리스트에 추가
//            spawnedIndexes.Add(index);                                  // 인덱스 리스트에 추가
//        }
//    }

//    public void Save() // 현재 씬의 오브젝트 데이터를 JSON으로 저장하는 메서드
//    {
//        List<SomeClass> list = new List<SomeClass>();                   // 저장할 데이터 리스트 생성
//        for (int i = 0; i < spawnedObjects.Count; i++) // 생성된 오브젝트 수만큼 반복
//        {
//            SomeClass data = new SomeClass();                           // 데이터 인스턴스 생성
//            data.prefabIndex = spawnedIndexes[i];                       // 프리팹 인덱스 저장
//            data.pos = spawnedObjects[i].transform.position;            // 위치 저장
//            data.rot = spawnedObjects[i].transform.rotation;            // 회전 저장
//            data.scale = spawnedObjects[i].transform.localScale;        // 크기 저장
//            data.color = spawnedObjects[i].GetComponent<Renderer>().material.color; // 색상 저장
//            list.Add(data);                                             // 리스트에 추가
//        }
//        jsonTest2.Save(list);                                           // JSON 파일로 저장
//    }

//    public void Load() // JSON에서 오브젝트 데이터를 불러와 씬에 재생성하는 메서드
//    {
//        foreach (var obj in spawnedObjects) Destroy(obj);              // 기존 오브젝트 전부 삭제
//        spawnedObjects.Clear();                                         // 오브젝트 리스트 초기화
//        spawnedIndexes.Clear();                                         // 인덱스 리스트 초기화

//        List<SomeClass> list = jsonTest2.Load();                        // JSON 파일에서 데이터 로드
//        foreach (var data in list) // 로드된 데이터 각각에 대해 반복
//        {
//            GameObject obj = Instantiate(spawnPrefab[data.prefabIndex], data.pos, data.rot); // 저장된 프리팹/위치/회전으로 생성
//            obj.transform.localScale = data.scale;                      // 크기 적용
//            obj.GetComponent<Renderer>().material.color = data.color;   // 색상 적용
//            spawnedObjects.Add(obj);                                    // 오브젝트 리스트에 추가
//            spawnedIndexes.Add(data.prefabIndex);                       // 인덱스 리스트에 추가
//        }
//    }
//}
