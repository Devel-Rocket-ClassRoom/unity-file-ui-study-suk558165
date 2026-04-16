using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;
using SaveDataVC = SaveDataV3;

public class SaveLoadManager
{
    public static SaveMode mode { get; set; } = SaveMode.Encrypted;

    public static bool Save(int slot = 0) => Save(slot, mode);
    public static int SaveDataVersion { get; } = 3;

    public enum SaveMode
    {
        Text,
        Encrypted,
    };

    private static readonly string SaveDirectory = $"{Application.persistentDataPath}/Save";

    private static readonly string[] SaveFileNames =
        { "SaveAuto",
          "Save1",
          "Save2",
          "Save3",
        };

    public static SaveDataV3 Data { get; set; } = new SaveDataVC();

    public static string GetSaveFilePath(int slot)
    {
        return GetSaveFilePath(slot, mode);
    }

    public static string GetSaveFilePath(int slot, SaveMode mode)
    {
        string ext = mode == SaveMode.Text ? ".json" : ".dat";
        return Path.Combine(SaveDirectory, $"{SaveFileNames[slot]}{ext}");
    }

    private static JsonSerializerSettings settings = new JsonSerializerSettings()
    {
        Formatting = Formatting.Indented,
        TypeNameHandling = TypeNameHandling.All,
    };

    public static bool Save(int slot, SaveMode mode)
    {
        if (Data == null || slot < 0 || slot >= SaveFileNames.Length)
        {
            return false;
        }
        try
        {
            if (!Directory.Exists(SaveDirectory))
            {
                Directory.CreateDirectory(SaveDirectory);
            }
            var json = JsonConvert.SerializeObject(Data, settings);
            string path = GetSaveFilePath(0, mode);
            switch (mode)
            {
                case SaveMode.Text:
                    File.WriteAllText(path, json);
                    break;
                case SaveMode.Encrypted:
                    File.WriteAllBytes(path, CryptoUtil.Encrypt(json));
                    break;
            }
            return true;
        }
        catch
        {
            Debug.LogError("Save 예외");
            return false;
        }
    }

    public static bool Load(int slot = 0)
    {
        if (slot < 0 || slot >= SaveFileNames.Length)
        {
            return false;
        }
        string path = GetSaveFilePath(0, mode);
        if (!File.Exists(path))
        {
            return false;
        }

        try
        {
            string json = string.Empty;
            switch (mode)
            {
                case SaveMode.Text:
                    json = File.ReadAllText(path);
                    break;
                case SaveMode.Encrypted:
                    json = CryptoUtil.Decrypt(File.ReadAllBytes(path));
                    break;
            }
            var saveData = JsonConvert.DeserializeObject<SaveData>(json, settings);
            while (saveData.Version < SaveDataVersion)
            {
                saveData = saveData.VersionUp();
            }
            Data = saveData as SaveDataVC;
            return true;
        }
        catch
        {
            Debug.LogError("Load 예외");
            return false;
        }
    }
}