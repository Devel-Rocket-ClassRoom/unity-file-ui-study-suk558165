using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SomeClass
{
    public Vector3 pos;
    public Quaternion rot;
    public Vector3 scale;
    public Color color;
}

[System.Serializable]
public class ObjectSaveData
{
    public string prefabName;
    public Vector3 pos;
    public Quaternion rot;
    public Vector3 scale;
    public Color color;
}


public class JsonTest2 : MonoBehaviour
{
    public string fileName = "test.json";
    public string FullFilePath => Path.Combine(Application.persistentDataPath, "JsonTest", fileName);

    public string[] prefabNames =
    {
        "Cube",
        "Sphere",
        "Capsule",
        "Cylinder",
    };

    private JsonSerializerSettings jsonSettings;

    private void Awake()
    {
        jsonSettings = new JsonSerializerSettings();
        jsonSettings.Formatting = Formatting.Indented;
        jsonSettings.Converters.Add(new Vector3Converter());
        jsonSettings.Converters.Add(new QuaternionConverter());
        jsonSettings.Converters.Add(new ColorConverter());
    }

    private void CreateRandomObject()
    {
        var prefabName = prefabNames[Random.Range(0, prefabNames.Length)];
        var prefab = Resources.Load<JsonTestObject>(prefabName);
        var obj = Instantiate(prefab);
        obj.prefabName = prefabName;
        obj.transform.position = Random.insideUnitSphere * 10f;
        obj.transform.rotation = Random.rotation;
        obj.transform.localScale = Vector3.one * Random.Range(0.5f, 3f);
        obj.GetComponent<Renderer>().material.color = Random.ColorHSV();
    }

    public void OnCreate()
    {
        for (int i = 0; i < 10; ++i)
        {
            CreateRandomObject();
            Debug.Log(FullFilePath);
        }

    }

    public void OnClear()
    {
        var objs = GameObject.FindGameObjectsWithTag("TestObject");
        foreach (var obj in objs)
        {
            Destroy(obj);
        }
    }

    public void OnSave()
    {
        var saveList = new List<ObjectSaveData>();
        var objs = GameObject.FindGameObjectsWithTag("TestObject");
        foreach (var obj in objs)
        {
            var jsonTestObj = obj.GetComponent<JsonTestObject>();
            saveList.Add(jsonTestObj.GetSaveData());
        }
        var json = JsonConvert.SerializeObject(saveList, jsonSettings);
        File.WriteAllText(FullFilePath, json);
    }

    public void OnLoad()
    {
        OnClear();

        var json = File.ReadAllText(FullFilePath);
        var saveList = JsonConvert.DeserializeObject<List<ObjectSaveData>>(json, jsonSettings);

        foreach (var saveData in saveList)
        {
            var prefab = Resources.Load<JsonTestObject>(saveData.prefabName);
            var jsonTestObj = Instantiate(prefab);
            jsonTestObj.Set(saveData);
            Debug.Log(saveData.prefabName);
        }
       
    }
}
