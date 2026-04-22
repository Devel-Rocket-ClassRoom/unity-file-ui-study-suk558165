using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable] // 이 클래스를 직렬화 가능하도록 표시하는 어트리뷰트
// 씬 오브젝트의 트랜스폼과 색상 데이터를 저장하는 데이터 클래스
public class SomeClass
{
    public Vector3 pos; // 오브젝트 위치
    public Quaternion rot; // 오브젝트 회전
    public Vector3 scale; // 오브젝트 크기
    public Color color; // 오브젝트 머티리얼 색상
}

[System.Serializable] // 이 클래스를 직렬화 가능하도록 표시하는 어트리뷰트
// 프리팹 이름 포함 씬 오브젝트의 저장 데이터 클래스 (로드 시 어떤 프리팹을 생성할지 식별)
public class ObjectSaveData
{
    public string prefabName; // 저장 시의 프리팹 이름
    public Vector3 pos; // 오브젝트 위치
    public Quaternion rot; // 오브젝트 회전
    public Vector3 scale; // 오브젝트 크기
    public Color color; // 오브젝트 머티리얼 색상
}


// JSON 파일을 이용한 씬 오브젝트 저장/로드 기능을 담당하는 MonoBehaviour 클래스
public class JsonTest2 : MonoBehaviour
{
    public string fileName = "test.json"; // 저장/로드에 사용할 JSON 파일 이름
    public string FullFilePath => Path.Combine(Application.persistentDataPath, "JsonTest", fileName); // persistentDataPath/JsonTest/fileName으로 전체 파일 경로를 반환하는 프로퍼티

    // 씬에 생성할 수 있는 프리팹 이름 목록
    public string[] prefabNames =
    {
        "Cube", // 큐브 프리팹 이름
        "Sphere", // 구체 프리팹 이름
        "Capsule", // 캡슐 프리팹 이름
        "Cylinder", // 실린더 프리팹 이름
    };

    private JsonSerializerSettings jsonSettings; // JSON 직렬화 설정 객체 (포맷, 컨버터 등)

    // 초기화 시 JSON 직렬화 설정과 커스텀 컨버터를 구성하는 메서드
    private void Awake()
    {
        jsonSettings = new JsonSerializerSettings(); // JSON 직렬화 설정 객체 생성
        jsonSettings.Formatting = Formatting.Indented; // JSON을 들여쓰기된 보기 좋은 형식으로 설정
        jsonSettings.Converters.Add(new Vector3Converter()); // Vector3 타입 변환을 위한 커스텀 컨버터 추가
        jsonSettings.Converters.Add(new QuaternionConverter()); // Quaternion 타입 변환을 위한 커스텀 컨버터 추가
        jsonSettings.Converters.Add(new ColorConverter()); // Color 타입 변환을 위한 커스텀 컨버터 추가
    }

    // 랜덤 위치/회전/크기/색상으로 프리팹 오브젝트 하나를 씬에 생성하는 내부 메서드
    private void CreateRandomObject()
    {
        var prefabName = prefabNames[Random.Range(0, prefabNames.Length)]; // 배열에서 랜덤 프리팹 이름 선택
        var prefab = Resources.Load<JsonTestObject>(prefabName); // 선택된 이름의 프리팹을 Resources에서 로드
        var obj = Instantiate(prefab); // 프리팹을 씬에 인스턴스화
        obj.prefabName = prefabName; // 생성된 오브젝트에 프리팹 이름 설정
        obj.transform.position = Random.insideUnitSphere * 10f; // 구 범위 내 랜덤 위치 설정
        obj.transform.rotation = Random.rotation; // 완전 랜덤 회전 설정
        obj.transform.localScale = Vector3.one * Random.Range(0.5f, 3f); // 0.5~3 사이 랜덤 균일 크기 설정
        obj.GetComponent<Renderer>().material.color = Random.ColorHSV(); // HSV 기반 랜덤 색상 적용
    }

    // 버튼 클릭 시 랜덤 오브젝트 10개를 씬에 생성하는 메서드
    public void OnCreate()
    {
        for (int i = 0; i < 10; ++i) // 10회 반복하여 랜덤 오브젝트 생성
        {
            CreateRandomObject(); // 랜덤 오브젝트 1개 생성
            Debug.Log(FullFilePath); // 현재 저장 경로를 콘솔에 출력
        }

    }

    // 버튼 클릭 시 "TestObject" 태그를 가진 씬의 모든 오브젝트를 삭제하는 메서드
    public void OnClear()
    {
        var objs = GameObject.FindGameObjectsWithTag("TestObject"); // "TestObject" 태그로 씬의 모든 오브젝트 검색
        foreach (var obj in objs) // 검색된 오브젝트 각각에 대해 반복
        {
            Destroy(obj); // 오브젝트를 씬에서 삭제
        }
    }

    // 버튼 클릭 시 씬의 모든 TestObject 데이터를 JSON 파일로 저장하는 메서드
    public void OnSave()
    {
        var saveList = new List<ObjectSaveData>(); // 저장할 데이터 리스트 생성
        var objs = GameObject.FindGameObjectsWithTag("TestObject"); // "TestObject" 태그로 씬의 모든 오브젝트 검색
        foreach (var obj in objs) // 검색된 오브젝트 각각에 대해 반복
        {
            var jsonTestObj = obj.GetComponent<JsonTestObject>(); // 오브젝트에서 JsonTestObject 컴포넌트 가져오기
            saveList.Add(jsonTestObj.GetSaveData()); // 저장 데이터를 추출하여 리스트에 추가
        }
        var json = JsonConvert.SerializeObject(saveList, jsonSettings); // 데이터 리스트를 JSON 문자열로 직렬화
        File.WriteAllText(FullFilePath, json); // JSON 문자열을 파일에 저장
    }

    // 버튼 클릭 시 JSON 파일에서 데이터를 로드하여 오브젝트를 복원하는 메서드
    public void OnLoad()
    {
        OnClear(); // 기존 씬의 TestObject를 모두 삭제

        var json = File.ReadAllText(FullFilePath); // JSON 파일에서 문자열 읽기
        var saveList = JsonConvert.DeserializeObject<List<ObjectSaveData>>(json, jsonSettings); // JSON 문자열을 ObjectSaveData 리스트로 역직렬화

        foreach (var saveData in saveList) // 역직렬화된 데이터 각각에 대해 반복
        {
            var prefab = Resources.Load<JsonTestObject>(saveData.prefabName); // 저장된 프리팹 이름으로 Resources에서 프리팹 로드
            var jsonTestObj = Instantiate(prefab); // 프리팹을 씬에 인스턴스화
            jsonTestObj.Set(saveData); // 저장 데이터를 오브젝트에 적용(위치/회전/크기/색상 복원)
            Debug.Log(saveData.prefabName); // 복원된 프리팹 이름 콘솔 출력
        }

    }
}
