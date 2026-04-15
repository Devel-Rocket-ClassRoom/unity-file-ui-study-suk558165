using System.Collections.Generic;
using System.IO;
using UnityEditor.SceneManagement;
using UnityEngine;

public class PlayerInfo
{
    public string playerName;
    public int lives;

    public float health;

    public Vector3 position;

    public Dictionary<string, int> scores = new Dictionary<string, int>
    {
        {"stage1", 100},
        {"stage2", 200 },
    };

}
public class JsonUtilityTest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Save
            PlayerInfo obj = new PlayerInfo
            {
                playerName = "ABC",
                lives = 10,
                health = 10.999f,
                position = new Vector3(1f,2f,3f)
            };
            string pathFolder = Path.Combine(
                Application.persistentDataPath,
                "JsonTest"
                );

            if (!Directory.Exists(pathFolder) )
            {
                Directory.CreateDirectory(pathFolder);
            }
            string path = Path.Combine(
                pathFolder,
                "player.json");

            string json = JsonUtility.ToJson(obj, prettyPrint: true);
            File.WriteAllText(path, json );

            Debug.Log(path);
            Debug.Log(json);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            string path = Path.Combine(
                Application.persistentDataPath,
                "JsonTest",
                "player.Json"
                );

            string json = File.ReadAllText(path);
            //PlyaerInfo obj =  JsonUtility.FromJson<PlyaerInfo>(json);
            PlayerInfo obj = new PlayerInfo();
            JsonUtility.FromJsonOverwrite(json,obj);
            Debug.Log(json);
            Debug.Log($"{obj.playerName}/ {obj.health}");

        }
    }
}
