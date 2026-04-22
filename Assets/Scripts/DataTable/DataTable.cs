using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

public abstract class DataTable // 모든 데이터 테이블의 공통 기반이 되는 추상 클래스
{
    public static readonly string FormatPath = "DataTables/{0}"; // Resources 폴더 내 데이터 테이블 파일 경로 포맷 문자열

    public abstract void Load(string filename); // 파생 클래스에서 반드시 구현해야 할 CSV 로드 추상 메서드

    public static List<T> LoadCSV<T>(string csvText, string delimiter = ",") // CSV 텍스트를 T 타입 객체 목록으로 파싱하는 정적 제네릭 메서드
    {
        csvText = csvText.TrimStart('﻿', '​'); // BOM(UTF-8 바이트 순서 표시) 및 제로폭 공백 문자 제거

        var config = new CsvConfiguration(CultureInfo.InvariantCulture) // CSV 파싱 설정 생성 (문화권 독립적)
        {
            Delimiter = delimiter, // 구분자를 매개변수로 받은 값(기본값 쉼표)으로 설정
        };
        using (var reader = new StringReader(csvText)) // CSV 텍스트를 읽기 위한 StringReader 생성
        using (var csvReader = new CsvReader(reader, config)) // 설정을 적용한 CsvReader 생성
        {
            return csvReader.GetRecords<T>().ToList(); // CSV의 모든 행을 T 타입으로 역직렬화하여 리스트로 반환
        }
    }
}
