using System;
using System.IO;
using UnityEngine;

public class EncryptionAndDecryption : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            string saveDir = Application.persistentDataPath + "/SaveData"; // 저장할 디렉토리 경로
            string saveFilePath = saveDir + "/save1.txt"; // 저장할 파일 경로
            if (!Directory.Exists(saveDir)) // 디렉토리가 존재하지 않으면 생성
            {
                Directory.CreateDirectory(saveDir); // 디렉토리 생성
            }
            File.WriteAllText(saveFilePath, "Hello Unity World");
            Debug.Log("원본:" + File.ReadAllText(saveFilePath));
        }

    }
}
