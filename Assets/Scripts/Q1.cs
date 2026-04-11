using System.IO;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            string saveDir = Application.persistentDataPath + "/SaveData"; // 저장할 디렉토리 경로
            string saveFilePath = saveDir + "/save1.txt"; // 저장할 파일 경로
            string saveFilePath2 = saveDir + "/save2.txt"; // 저장할 파일 경로
            string saveFilePath3 = saveDir + "/save3.txt"; // 저장할 파일 경로

            if (!Directory.Exists(saveDir)) // 디렉토리가 존재하지 않으면 생성
            {
                Directory.CreateDirectory(saveDir); // 디렉토리 생성
            }
            File.WriteAllText(saveFilePath, "이건 첫번째 레슨."); // 파일 생성 및 내용 작성
            File.WriteAllText(saveFilePath2, "이건 두번째 레슨."); // 파일 생성 및 내용 작성
            File.WriteAllText(saveFilePath3, "이건 세번째 레슨"); // 파일 생성 및 내용 작성
            if (File.Exists(saveDir + "/save1_backup.txt"))
            {
                File.Delete(saveDir + "/save1_backup.txt");
            }
            Debug.Log("=== 세이브 파일 목록 ===");
            foreach (string file in Directory.GetFiles(saveDir)) // 디렉토리 내의 모든 파일을 가져와서 출력
            {
                Debug.Log($" - {Path.GetFileName(file)} ({Path.GetExtension(file)})"); // 파일 이름과 확장자 출력
            }
            File.Copy(saveDir + "/save1.txt", saveDir + "/save1_backup.txt", true); // 파일 복사
            Debug.Log("save1.txt → save1_backup.txt 복사 완료");
            File.Delete(saveFilePath3); // 파일 삭제
            Debug.Log("save3.txt 삭제 완료");

            Debug.Log("=== 작업 후 파일 목록 ===");
            foreach (string file in Directory.GetFiles(saveDir))
            {
                Debug.Log($" - {Path.GetFileName(file)} ({Path.GetExtension(file)})");
            }
        }
    }
 }

