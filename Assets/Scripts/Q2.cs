using System;
using System.IO;
using UnityEngine;

public class EncryptionAndDecryption : MonoBehaviour
{
    private const byte XOR_KEY = 0xAB;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            string saveDir = Application.persistentDataPath + "/SaveData";
            if (!Directory.Exists(saveDir))
                Directory.CreateDirectory(saveDir);

            string secretPath = saveDir + "/secret.txt";
            string encryptedPath = saveDir + "/encrypted.dat";
            string decryptedPath = saveDir + "/decrypted.txt";

            // 1. 원본 파일 생성
            File.WriteAllText(secretPath, "Hello Unity World");
            Debug.Log("원본: " + File.ReadAllText(secretPath));

            // 2. 암호화
            using (FileStream fsIn = new FileStream(secretPath, FileMode.Open, FileAccess.Read))
            using (FileStream fsOut = new FileStream(encryptedPath, FileMode.Create, FileAccess.Write))
            {
                int b;
                while ((b = fsIn.ReadByte()) != -1)
                    fsOut.WriteByte((byte)(b ^ XOR_KEY));
            }
            Debug.Log($"암호화 완료 (파일 크기: {new FileInfo(encryptedPath).Length} bytes)");

            // 3. 복호화
            using (FileStream fsIn = new FileStream(encryptedPath, FileMode.Open, FileAccess.Read))
            using (FileStream fsOut = new FileStream(decryptedPath, FileMode.Create, FileAccess.Write))
            {
                int b;
                while ((b = fsIn.ReadByte()) != -1)
                    fsOut.WriteByte((byte)(b ^ XOR_KEY));
            }
            Debug.Log("복호화 완료");

            // 4. 결과 출력
            string original = File.ReadAllText(secretPath);
            string decrypted = File.ReadAllText(decryptedPath);
            Debug.Log("복호화 결과: " + decrypted);
            Debug.Log("원본과 일치: " + (original == decrypted));
        }
    }
}