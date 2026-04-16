using UnityEngine;

public class CryptoTest1 : MonoBehaviour
{
    private byte[] encrypted;

    void Update()
    {
      if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            string plainText = "Hello! AES!";
            encrypted = CryptoUtil.Encrypt(plainText);
            Debug.Log(System.BitConverter.ToString(encrypted));
        }

        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            if (encrypted != null)
            {
                string plainText = CryptoUtil.Decrypt(encrypted);
                Debug.Log(plainText);
            }
        }
    }
}
