using System.IO;
using UnityEngine;

public class Q2: MonoBehaviour
{
    private const byte XOR_KEY = 0xAB; // 간단한 XOR 키 (0xAB)

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) 
        {
            string saveDir = Application.persistentDataPath + "/SaveData"; // 저장할 디렉토리 경로
            if (!Directory.Exists(saveDir)) // 디렉토리가 존재하지 않으면 생성
                Directory.CreateDirectory(saveDir); // 디렉토리 생성

            string secretPath = saveDir + "/secret.txt"; // 원본 파일 경로
            string encryptedPath = saveDir + "/encrypted.dat"; // 암호화된 파일 경로
            string decryptedPath = saveDir + "/decrypted.txt"; // 복호화된 파일 경로

            // 1. 원본 파일 생성
            File.WriteAllText(secretPath, "Hello Unity World"); // 원본 파일 생성
            Debug.Log("원본: " + File.ReadAllText(secretPath)); // 원본 내용 출력

            // 2. 암호화
            using (FileStream fsIn = new FileStream(secretPath, FileMode.Open, FileAccess.Read)) // 원본 파일 읽기
            using (FileStream fsOut = new FileStream(encryptedPath, FileMode.Create, FileAccess.Write)) // 암호화된 파일 쓰기
            {
                int b;
                while ((b = fsIn.ReadByte()) != -1) // 파일 끝까지 읽기
                    fsOut.WriteByte((byte)(b ^ XOR_KEY)); // XOR 연산으로 암호화하여 쓰기
            }
            Debug.Log($"암호화 완료 (파일 크기: {new FileInfo(encryptedPath).Length} bytes)"); // 암호화된 파일 크기 출력

            // 3. 복호화
            using (FileStream fsIn = new FileStream(encryptedPath, FileMode.Open, FileAccess.Read)) // 암호화된 파일 읽기
            using (FileStream fsOut = new FileStream(decryptedPath, FileMode.Create, FileAccess.Write)) // 복호화된 파일 쓰기
            {
                int b;
                while ((b = fsIn.ReadByte()) != -1) // 파일 끝까지 읽기
                    fsOut.WriteByte((byte)(b ^ XOR_KEY)); // XOR 연산으로 복호화하여 쓰기 (암호화와 동일한 연산)
            }
            Debug.Log("복호화 완료"); 

            // 4. 결과 출력
            string original = File.ReadAllText(secretPath); // 원본 내용 읽기
            string decrypted = File.ReadAllText(decryptedPath); // 복호화된 내용 읽기
            Debug.Log("복호화 결과: " + decrypted);
            Debug.Log("원본과 일치: " + (original == decrypted));
        }
    }
}