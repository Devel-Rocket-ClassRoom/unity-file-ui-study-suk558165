//using Newtonsoft.Json;
//using System.Collections.Generic;
//using UnityEngine;

//public class SpawnManager : MonoBehaviour
//{
//    public GameObject[] spawnPrefab;        // 스폰할 프리팹 배열
//    public JsonTest2 jsonTest2;             // JSON 저장/로드 담당 컴포넌트 참조
//    private List<GameObject> spawnedObjects = new List<GameObject>();   // 생성된 오브젝트 리스트
//    private List<int> spawnedIndexes = new List<int>();                 // 생성된 오브젝트의 프리팹 인덱스 리스트

//    public void Create()
//    {
//        for (int i = 0; i < 3; i++)
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

//    public void Save()
//    {
//        List<SomeClass> list = new List<SomeClass>();                   // 저장할 데이터 리스트 생성
//        for (int i = 0; i < spawnedObjects.Count; i++)
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

//    public void Load()
//    {
//        foreach (var obj in spawnedObjects) Destroy(obj);              // 기존 오브젝트 전부 삭제
//        spawnedObjects.Clear();                                         // 오브젝트 리스트 초기화
//        spawnedIndexes.Clear();                                         // 인덱스 리스트 초기화

//        List<SomeClass> list = jsonTest2.Load();                        // JSON 파일에서 데이터 로드
//        foreach (var data in list)
//        {
//            GameObject obj = Instantiate(spawnPrefab[data.prefabIndex], data.pos, data.rot); // 저장된 프리팹/위치/회전으로 생성
//            obj.transform.localScale = data.scale;                      // 크기 적용
//            obj.GetComponent<Renderer>().material.color = data.color;   // 색상 적용
//            spawnedObjects.Add(obj);                                    // 오브젝트 리스트에 추가
//            spawnedIndexes.Add(data.prefabIndex);                       // 인덱스 리스트에 추가
//        }
//    }
//}