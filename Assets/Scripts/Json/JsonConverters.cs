using UnityEngine;
using Newtonsoft.Json;
using System;
using Newtonsoft.Json.Linq;

// CharacterData를 JSON에서 ID 문자열로 읽고 쓰는 커스텀 JsonConverter 클래스
public class CharacterDataConverter : JsonConverter<CharacterData>
{
    // JSON에서 CharacterData를 읽을 때 ID 문자열을 통해 캐릭터 데이터를 조회하는 메서드
    public override CharacterData ReadJson(JsonReader reader, Type objectType, CharacterData existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var id = reader.Value as string; // JSON 리더에서 현재 값을 문자열 ID로 가져오기
        return DataTableManager.CharacterTable.Get(id); // ID로 캐릭터 테이블에서 CharacterData를 조회하여 반환
    }

    // CharacterData를 JSON에 쓸 때 ID 문자열 값만 저장하는 메서드
    public override void WriteJson(JsonWriter writer, CharacterData value, JsonSerializer serializer)
    {
        writer.WriteValue(value.Id); // CharacterData의 ID 값만 JSON에 문자열로 기록
    }
}

// ItemData를 JSON에서 ID 문자열로 읽고 쓰는 커스텀 JsonConverter 클래스
public class ItemDataConverter : JsonConverter<ItemData>
{
    // JSON에서 ItemData를 읽을 때 ID 문자열을 통해 아이템 데이터를 조회하는 메서드
    public override ItemData ReadJson(JsonReader reader, Type objectType, ItemData existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var id = reader.Value as string; // JSON 리더에서 현재 값을 문자열 ID로 가져오기
        return DataTableManager.ItemTable.Get(id); // ID로 아이템 테이블에서 ItemData를 조회하여 반환
    }

    // ItemData를 JSON에 쓸 때 ID 문자열 값만 저장하는 메서드
    public override void WriteJson(JsonWriter writer, ItemData value, JsonSerializer serializer)
    {
        writer.WriteValue(value.Id); // ItemData의 ID 값만 JSON에 문자열로 기록
    }
}

// Unity의 Vector3 타입을 JSON으로 직렬화/역직렬화하는 커스텀 JsonConverter 클래스
public class Vector3Converter : JsonConverter<Vector3>
{
    // JSON에서 X, Y, Z 필드를 읽어 Vector3를 복원하는 메서드
    public override Vector3 ReadJson(JsonReader reader, Type objectType, Vector3 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        Vector3 v = Vector3.zero; // 기본값 (0, 0, 0)으로 Vector3 초기화
        JObject jObj = JObject.Load(reader); // JSON 리더에서 JSON 오브젝트를 파싱
        v.x = (float)jObj["X"]; // JSON의 "X" 필드 값을 float으로 변환하여 x 성분에 할당
        v.y = (float)jObj["Y"]; // JSON의 "Y" 필드 값을 float으로 변환하여 y 성분에 할당
        v.z = (float)jObj["Z"]; // JSON의 "Z" 필드 값을 float으로 변환하여 z 성분에 할당
        return v; // 복원된 Vector3 반환
    }

    // Vector3를 X, Y, Z 필드를 가진 JSON 오브젝트로 직렬화하는 메서드
    public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer)
    {
        writer.WriteStartObject(); // JSON 오브젝트 시작 ({)
        writer.WritePropertyName("X"); // "X" 프로퍼티 이름 기록
        writer.WriteValue(value.x); // x 성분 값 기록
        writer.WritePropertyName("Y"); // "Y" 프로퍼티 이름 기록
        writer.WriteValue(value.y); // y 성분 값 기록
        writer.WritePropertyName("Z"); // "Z" 프로퍼티 이름 기록
        writer.WriteValue(value.z); // z 성분 값 기록
        writer.WriteEndObject(); // JSON 오브젝트 끝 (})
    }
}

// Unity의 Quaternion 타입을 JSON으로 직렬화/역직렬화하는 커스텀 JsonConverter 클래스
public class QuaternionConverter : JsonConverter<Quaternion>
{
    // JSON에서 X, Y, Z, W 필드를 읽어 Quaternion을 복원하는 메서드
    public override Quaternion ReadJson(JsonReader reader, Type objectType, Quaternion existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        Quaternion q = Quaternion.identity; // 기본값 (항등 회전)으로 Quaternion 초기화
        JObject jObj = JObject.Load(reader); // JSON 리더에서 JSON 오브젝트를 파싱
        q.x = (float)jObj["X"]; // JSON의 "X" 필드 값을 float으로 변환하여 x 성분에 할당
        q.y = (float)jObj["Y"]; // JSON의 "Y" 필드 값을 float으로 변환하여 y 성분에 할당
        q.z = (float)jObj["Z"]; // JSON의 "Z" 필드 값을 float으로 변환하여 z 성분에 할당
        q.w = (float)jObj["W"]; // JSON의 "W" 필드 값을 float으로 변환하여 w 성분에 할당
        return q; // 복원된 Quaternion 반환
    }

    // Quaternion을 X, Y, Z, W 필드를 가진 JSON 오브젝트로 직렬화하는 메서드
    public override void WriteJson(JsonWriter writer, Quaternion value, JsonSerializer serializer)
    {
        writer.WriteStartObject(); // JSON 오브젝트 시작 ({)
        writer.WritePropertyName("X"); // "X" 프로퍼티 이름 기록
        writer.WriteValue(value.x); // x 성분 값 기록
        writer.WritePropertyName("Y"); // "Y" 프로퍼티 이름 기록
        writer.WriteValue(value.y); // y 성분 값 기록
        writer.WritePropertyName("Z"); // "Z" 프로퍼티 이름 기록
        writer.WriteValue(value.z); // z 성분 값 기록
        writer.WritePropertyName("W"); // "W" 프로퍼티 이름 기록
        writer.WriteValue(value.w); // w 성분 값 기록
        writer.WriteEndObject(); // JSON 오브젝트 끝 (})
    }
}

// Unity의 Color 타입을 JSON으로 직렬화/역직렬화하는 커스텀 JsonConverter 클래스
public class ColorConverter : JsonConverter<Color>
{
    // JSON에서 R, G, B, A 필드를 읽어 Color를 복원하는 메서드
    public override Color ReadJson(JsonReader reader, Type objectType, Color existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        Color c = Color.black; // 기본값 (검정, alpha=1)으로 Color 초기화
        JObject jObj = JObject.Load(reader); // JSON 리더에서 JSON 오브젝트를 파싱
        c.r = (float)jObj["R"]; // JSON의 "R" 필드 값을 float으로 변환하여 적색 채널에 할당
        c.g = (float)jObj["G"]; // JSON의 "G" 필드 값을 float으로 변환하여 녹색 채널에 할당
        c.b = (float)jObj["B"]; // JSON의 "B" 필드 값을 float으로 변환하여 청색 채널에 할당
        c.a = (float)jObj["A"]; // JSON의 "A" 필드 값을 float으로 변환하여 투명도 채널에 할당
        return c; // 복원된 Color 반환
    }

    // Color를 R, G, B, A 필드를 가진 JSON 오브젝트로 직렬화하는 메서드
    public override void WriteJson(JsonWriter writer, Color value, JsonSerializer serializer)
    {
        writer.WriteStartObject(); // JSON 오브젝트 시작 ({)
        writer.WritePropertyName("R"); // "R" 프로퍼티 이름 기록
        writer.WriteValue(value.r); // 적색 채널 값 기록
        writer.WritePropertyName("G"); // "G" 프로퍼티 이름 기록
        writer.WriteValue(value.g); // 녹색 채널 값 기록
        writer.WritePropertyName("B"); // "B" 프로퍼티 이름 기록
        writer.WriteValue(value.b); // 청색 채널 값 기록
        writer.WritePropertyName("A"); // "A" 프로퍼티 이름 기록
        writer.WriteValue(value.a); // 투명도 채널 값 기록
        writer.WriteEndObject(); // JSON 오브젝트 끝 (})
    }
}
