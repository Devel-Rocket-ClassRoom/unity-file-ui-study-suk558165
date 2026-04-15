using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
public class Vector3Converter : JsonConverter<Vector3>
{
    public override Vector3 ReadJson(JsonReader reader, Type objectType, Vector3 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        Vector3 v = Vector3.zero;

        JObject jObj = JObject.Load(reader);
        v.x = (float)jObj["X"];
        v.y = (float)jObj["Y"];
        v.z = (float)jObj["Z"];
        return v;
    }

    public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("X");
        writer.WriteValue(value.x);
        writer.WritePropertyName("Y");
        writer.WriteValue(value.y);
        writer.WritePropertyName("Z");
        writer.WriteValue(value.z);
        writer.WriteEndObject();
    }
}

    class QuaternionConverter : JsonConverter<Quaternion>
    {
        public override Quaternion ReadJson(JsonReader reader, Type objectType, Quaternion existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject jObj = JObject.Load(reader);
            return new Quaternion(
                (float)jObj["X"],
                (float)jObj["Y"],
                (float)jObj["Z"],
                (float)jObj["W"]
            );
        }

        public override void WriteJson(JsonWriter writer, Quaternion value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("X"); writer.WriteValue(value.x);
            writer.WritePropertyName("Y"); writer.WriteValue(value.y);
            writer.WritePropertyName("Z"); writer.WriteValue(value.z);
            writer.WritePropertyName("W"); writer.WriteValue(value.w);
            writer.WriteEndObject();
        }
    }

     class ColorConverter : JsonConverter<Color>
    {
        public override Color ReadJson(JsonReader reader, Type objectType, Color existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject jObj = JObject.Load(reader);
            return new Color(
                (float)jObj["R"],
                (float)jObj["G"],
                (float)jObj["B"],
                (float)jObj["A"]
            );
        }

        public override void WriteJson(JsonWriter writer, Color value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("R"); writer.WriteValue(value.r);
            writer.WritePropertyName("G"); writer.WriteValue(value.g);
            writer.WritePropertyName("B"); writer.WriteValue(value.b);
            writer.WritePropertyName("A"); writer.WriteValue(value.a);
            writer.WriteEndObject();
        }
    }

