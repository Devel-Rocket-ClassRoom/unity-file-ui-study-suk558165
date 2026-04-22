using UnityEngine;
using CsvHelper;
using System.IO;
using System.Globalization;


public class CSVData // CSV 한 행의 데이터를 담는 클래스
{
    public string Id { get; set; } // CSV의 Id 열 값을 저장하는 프로퍼티
    public string String { get; set; } // CSV의 String 열 값을 저장하는 프로퍼티
}


public class CSVTest1 : MonoBehaviour // CSV 로드 및 파싱 테스트용 컴포넌트
{
    //public TextAsset textAsset;

    private void Update() // 매 프레임 호출되어 스페이스 키 입력을 감지
    {
        if (Input.GetKeyDown(KeyCode.Space)) // 스페이스 키가 눌렸을 때 CSV 로드 실행
        {
            TextAsset textAsset =  Resources.Load<TextAsset>("DataTables/StringTableKr"); // Resources 폴더에서 한국어 CSV 파일 로드
            string csv = textAsset.text; // TextAsset에서 CSV 문자열 추출
            using (var reader = new StringReader(csv)) // CSV 문자열을 읽기 위한 StringReader 생성
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture)) // 문화권 독립적인 CsvReader 생성
            {
                var records = csvReader.GetRecords<CSVData>(); // CSV 전체 행을 CSVData 리스트로 변환
                foreach (var record in records) // 각 행을 순서대로 처리
                {
                    Debug.Log($"{record.Id} : {record.String}"); // ID와 문자열을 콘솔에 출력
                }
            }
        }

    }
}
