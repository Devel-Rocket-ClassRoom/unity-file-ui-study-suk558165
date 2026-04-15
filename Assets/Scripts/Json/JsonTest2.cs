using Newtonsoft.Json;                  
using System.Collections.Generic;      
using System.IO;                        
using UnityEngine;
public class SomeClass
{
    public int prefabIndex;     // 어떤 프리팹인지 인덱스
    public Vector3 pos;         // 위치
    public Quaternion rot;      // 회전
    public Vector3 scale;       // 크기
    public Color color;         // 색상
}

public class JsonTest2 : MonoBehaviour
{
    public string fileName = "test.json";   // 저장 파일명

    // 저장 경로: Application.persistentDataPath/JsonTest/test.json
    public string FileFullPath => Path.Combine(Application.persistentDataPath, "JsonTest", fileName);

    private JsonSerializerSettings jsonSettings;    // 직렬화 설정

    private void Awake()
    {
        jsonSettings = new JsonSerializerSettings();                    // 설정 인스턴스 생성
        jsonSettings.Formatting = Formatting.Indented;                  // JSON 들여쓰기 포맷
        jsonSettings.Converters.Add(new Vector3Converter());            // Vector3 커스텀 컨버터 등록
        jsonSettings.Converters.Add(new QuaternionConverter());         // Quaternion 커스텀 컨버터 등록
        jsonSettings.Converters.Add(new ColorConverter());              // Color 커스텀 컨버터 등록
    }

    // SomeClass 리스트를 JSON으로 직렬화 후 파일 저장
    public void Save(List<SomeClass> ob)
    {
        string dir = Path.GetDirectoryName(FileFullPath);               // 폴더 경로 추출
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);    // 폴더 없으면 생성
        var json = JsonConvert.SerializeObject(ob, jsonSettings);       // 리스트를 JSON 문자열로 변환
        File.WriteAllText(FileFullPath, json);                          // 파일에 JSON 저장
    }

    // 파일에서 JSON 읽어 SomeClass 리스트로 역직렬화 후 반환
    public List<SomeClass> Load()
    {
        string json = File.ReadAllText(FileFullPath);                           // 파일에서 JSON 문자열 읽기
        return JsonConvert.DeserializeObject<List<SomeClass>>(json, jsonSettings); // JSON을 리스트로 변환 후 반환
    }
}