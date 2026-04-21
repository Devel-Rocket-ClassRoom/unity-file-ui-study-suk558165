using System; // 기본 .NET 시스템 라이브러리 (Convert 등)
using System.IO; // 파일/스트림 입출력 라이브러리
using System.Security.Cryptography; // AES 등 암호화 알고리즘 라이브러리
using System.Text; // 문자열 인코딩 라이브러리 (UTF-8 등)

// AES-256 CBC 방식으로 문자열을 암호화/복호화하는 유틸리티 정적 클래스
public static class CryptoUtil
{
    // AES-256 키 (32바이트 = 16진수 64자). 수업용 하드코딩 — 실제 프로덕션에서는 안전한 키 관리 필요.
    private const string KeyHex = "536176654C6F616453747564795F4145533235365F4B65795F32303235212100";

    private static readonly byte[] Key = HexToBytes(KeyHex); // 16진수 문자열 키를 바이트 배열로 변환하여 저장

    // 16진수 문자열을 바이트 배열로 변환하는 헬퍼 메서드
    private static byte[] HexToBytes(string hex)
    {
        int len = hex.Length / 2; // 16진수 2자리가 1바이트이므로 절반이 실제 바이트 수
        byte[] bytes = new byte[len]; // 결과 바이트 배열 할당
        for (int i = 0; i < len; i++) // 각 바이트 위치 순회
        {
            bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16); // 2자리 16진수를 1바이트로 변환
        }
        return bytes; // 변환된 바이트 배열 반환
    }

    // 암호화: plainText → [IV 16바이트][AES-CBC 암호문]
    public static byte[] Encrypt(string plainText) // 평문 문자열을 받아 암호화된 바이트 배열을 반환하는 메서드
    {
        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText); // 평문 문자열을 UTF-8 바이트 배열로 변환

        using (Aes aes = Aes.Create()) // AES 암호화 객체 생성 (using으로 자동 해제)
        {
            aes.Key = Key; // 미리 준비한 32바이트 키 설정
            aes.Mode = CipherMode.CBC; // CBC 블록 암호화 모드 설정
            aes.Padding = PaddingMode.PKCS7; // PKCS7 패딩 방식 설정
            aes.GenerateIV(); // 매 암호화마다 새로운 랜덤 IV 생성

            using (ICryptoTransform encryptor = aes.CreateEncryptor()) // 암호화 변환기 생성
            using (MemoryStream ms = new MemoryStream()) // 암호화 결과를 담을 메모리 스트림 생성
            {
                ms.Write(aes.IV, 0, aes.IV.Length); // 스트림 앞부분에 IV(16바이트)를 먼저 기록

                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write)) // AES 암호화 스트림 생성
                {
                    cs.Write(plainBytes, 0, plainBytes.Length); // 평문 바이트를 암호화 스트림에 기록
                    cs.FlushFinalBlock(); // 마지막 블록(패딩 포함) 처리 완료
                }

                return ms.ToArray(); // [IV + 암호문] 전체 바이트 배열 반환
            }
        }
    }

    // 복호화: [IV 16바이트][AES-CBC 암호문] → 원본 문자열
    public static string Decrypt(byte[] encryptedData) // 암호화된 바이트 배열을 받아 원본 문자열을 반환하는 메서드
    {
        const int ivSize = 16; // AES CBC IV 크기는 항상 16바이트

        byte[] iv = new byte[ivSize]; // IV를 저장할 바이트 배열 할당
        Buffer.BlockCopy(encryptedData, 0, iv, 0, ivSize); // 암호화 데이터 앞 16바이트를 IV로 복사

        byte[] cipherBytes = new byte[encryptedData.Length - ivSize]; // 나머지 부분을 암호문으로 분리
        Buffer.BlockCopy(encryptedData, ivSize, cipherBytes, 0, cipherBytes.Length); // 암호문 바이트 복사

        using (Aes aes = Aes.Create()) // AES 복호화 객체 생성
        {
            aes.Key = Key; // 동일한 키 설정
            aes.IV = iv; // 암호화 시 사용한 IV 설정
            aes.Mode = CipherMode.CBC; // CBC 모드 설정
            aes.Padding = PaddingMode.PKCS7; // PKCS7 패딩 설정

            using (ICryptoTransform decryptor = aes.CreateDecryptor()) // 복호화 변환기 생성
            using (MemoryStream ms = new MemoryStream(cipherBytes)) // 암호문을 읽을 메모리 스트림 생성
            using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read)) // AES 복호화 스트림 생성
            using (StreamReader reader = new StreamReader(cs, Encoding.UTF8)) // 복호화 스트림을 UTF-8 텍스트로 읽는 리더 생성
            {
                return reader.ReadToEnd(); // 복호화된 원본 문자열 전체를 읽어 반환
            }
        }
    }
}
