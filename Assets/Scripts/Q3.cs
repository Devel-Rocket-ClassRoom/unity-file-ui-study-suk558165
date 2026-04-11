using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class Q3 : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            string saveDir = Application.persistentDataPath + "/SaveData";
            if (!Directory.Exists(saveDir))
                Directory.CreateDirectory(saveDir);

            string settingsPath = saveDir + "/settings.cfg";

            // 1. 설정 파일 생성
            string defaultSettings =
                "master_volume=80\n" +
                "bgm_volume=70\n" +
                "sfx_volume=90\n" +
                "language=kr\n" +
                "show_damage=true";
            File.WriteAllText(settingsPath, defaultSettings);

            // 2. StreamReader로 파싱
            var config = new Dictionary<string, string>();
            using (StreamReader sr = new StreamReader(settingsPath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    int idx = line.IndexOf('=');
                    if (idx < 0) continue;
                    string key = line.Substring(0, idx);
                    string value = line.Substring(idx + 1);
                    config[key] = value;
                }
            }
            Debug.Log($"설정 로드 완료 (항목 {config.Count}개)");

            // 3. 변경 전 출력
            Debug.Log("--- 변경 전 ---");
            Debug.Log("bgm_volume = " + config["bgm_volume"]);
            Debug.Log("language = " + config["language"]);

            // 4. 값 변경
            config["bgm_volume"] = "50";
            config["language"] = "en";

            // 5. StreamWriter로 덮어쓰기
            using (StreamWriter sw = new StreamWriter(settingsPath, append: false))
            {
                foreach (var kv in config)
                    sw.WriteLine(kv.Key + "=" + kv.Value);
            }

            Debug.Log("--- 변경 후 저장 ---");
            Debug.Log("bgm_volume = " + config["bgm_volume"]);
            Debug.Log("language = " + config["language"]);

            // 6. 최종 파일 내용 출력
            Debug.Log("--- 최종 파일 내용 ---");
            Debug.Log(File.ReadAllText(settingsPath));
        }
    }
}