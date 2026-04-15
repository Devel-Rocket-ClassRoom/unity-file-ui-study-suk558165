using Newtonsoft.Json;
using System;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class PlayerStat
{
    public string playerName;

    public int lives;

    public float health;

    public Vector3 position;


    public override string ToString()
    {
        return $"{playerName} / {lives} / {health}";
    }
}

public class JsonTest1 : MonoBehaviour
{
    private JsonSerializerSettings jsonSettings;

    private void Awake() 
    {
        jsonSettings = new JsonSerializerSettings(); 
        jsonSettings.Formatting = Formatting.Indented;
        jsonSettings.Converters.Add(new Vector3Converter());
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Save
            PlayerStat obj = new PlayerStat
            {
                playerName = "ABC",
                lives = 10,
                health = 10.999f,
            };
            string pathFolder = Path.Combine(
                Application.persistentDataPath,
                "JsonTest"
                );

            if (!Directory.Exists(pathFolder))
            {
                Directory.CreateDirectory(pathFolder);
            }
            string path = Path.Combine(
                pathFolder,
                "player2.json");

            string json = JsonConvert.SerializeObject(obj, jsonSettings);
            File.WriteAllText(path, json);

            Debug.Log(path);
            Debug.Log(json);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            string path = Path.Combine(
                Application.persistentDataPath,
                "JsonTest",
                "player2.Json"
                );

            string json = File.ReadAllText(path);
             PlayerStat obj = JsonConvert.DeserializeObject<PlayerStat>(
            json, jsonSettings);
            
            Debug.Log(json);
            Debug.Log(obj);

        }
    }
}
