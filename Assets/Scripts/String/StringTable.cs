using System.Collections.Generic;
using UnityEngine;

// CSV 파일에서 로컬라이제이션 문자열을 로드하고 키로 조회하는 테이블 클래스
public class StringTable : DataTable
{
    // CSV 한 행(Id, String)을 담는 내부 데이터 클래스
    public class Data
    {
        public string Id { get; set; }     // 문자열 조회 키 (예: "NAME", "JOB", "Stat_Attack")
        public string String { get; set; } // 해당 키에 대응하는 현재 언어 문자열
    }

    public static readonly string Unknown = "Unknown"; // 키가 테이블에 없을 때 반환하는 기본값 문자열

    private readonly Dictionary<string, string> table = new Dictionary<string, string>(); // 키-문자열 쌍을 저장하는 딕셔너리

    // 키에 해당하는 현재 언어 문자열을 반환하는 메서드 (없으면 "Unknown" 반환)
    public string Get(string key)
    {
        if (!table.ContainsKey(key)) // 요청한 키가 테이블에 없는 경우
            return Unknown; // 기본값 "Unknown" 반환
        return table[key]; // 해당 키의 문자열 반환
    }

    // CSV 파일명을 받아 문자열 테이블을 로드하는 메서드
    public override void Load(string filename)
    {
        table.Clear(); // 기존 테이블 데이터를 모두 삭제
        string path = string.Format(FormatPath, filename); // 리소스 폴더 내 파일 경로 포맷 생성
        TextAsset textAsset = Resources.Load<TextAsset>(path); // 지정 경로에서 텍스트 에셋(CSV 파일) 로드
        var list = LoadCSV<Data>(textAsset.text); // CSV 텍스트를 Data 목록으로 파싱
        foreach (Data data in list) // 파싱된 데이터 목록을 순회
        {
            if (!table.ContainsKey(data.Id)) // 동일한 키가 없는 경우에만 추가
                table.Add(data.Id, data.String); // 키-문자열 쌍을 딕셔너리에 추가
            else
                Debug.LogError($"키 중복: {data.Id}"); // 중복 키 발견 시 에러 로그 출력
        }
    }
}
