using UnityEngine;

// AES 암호화/복호화 기능을 키 입력으로 테스트하는 MonoBehaviour 클래스
public class CryptoTest1 : MonoBehaviour
{
    private byte[] encrypted; // 암호화된 데이터를 임시 보관하는 바이트 배열 필드

    // 매 프레임마다 키 입력을 감지하여 암호화/복호화를 실행하는 메서드
    void Update()
    {
      if (Input.GetKeyUp(KeyCode.Alpha1)) // 숫자키 1을 뗐을 때 암호화 실행
        {
            string plainText = "Hello! AES!"; // 암호화할 평문 문자열 정의
            encrypted = CryptoUtil.Encrypt(plainText); // CryptoUtil을 통해 평문 암호화
            Debug.Log(System.BitConverter.ToString(encrypted)); // 암호화된 바이트 배열을 16진수 문자열로 콘솔에 출력
        }

        if (Input.GetKeyUp(KeyCode.Alpha2)) // 숫자키 2를 뗐을 때 복호화 실행
        {
            if (encrypted != null) // 암호화된 데이터가 존재하는지 확인
            {
                string plainText = CryptoUtil.Decrypt(encrypted); // 암호화된 데이터를 복호화하여 원본 문자열 복원
                Debug.Log(plainText); // 복호화된 원본 문자열을 콘솔에 출력
            }
        }
    }
}
