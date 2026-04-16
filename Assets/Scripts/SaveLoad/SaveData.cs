using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.LowLevelPhysics2D.PhysicsLayers;

[System.Serializable]
public abstract class SaveData
{
    public int Version { get; protected set; } // 세이브 데이터 버전 번호
    public abstract SaveData VersionUp(); // 다음 버전으로 마이그레이션
}

[System.Serializable]
public class SaveDataV1 : SaveData
{
    public string PlayerName { get; set; } = string.Empty; // 플레이어 이름 (V1 필드명)

    public SaveDataV1()
    {
        Version = 1; // V1으로 초기화
    }

    public override SaveData VersionUp()
    {
        var saveData = new SaveDataV2();
        saveData.Name = PlayerName; // PlayerName → Name으로 필드명 변경 마이그레이션
        return saveData;
    }
}

[System.Serializable]
public class SaveDataV2 : SaveData
{
    public string Name { get; set; } = string.Empty; // 플레이어 이름
    public int Gold { get; set; } = 0; // 골드 (V2 신규 필드)

    public SaveDataV2()
    {
        Version = 2; // V2로 초기화
    }

    public override SaveData VersionUp()
    {
        var saveData = new SaveDataV3();
        saveData.Name = Name;
        saveData.Gold = Gold; // Gold 필드 유지
        return saveData;
    }
}

[System.Serializable]
public class SaveDataV3 : SaveDataV2
{
    public List<string> itemList = new List<string>();
    public new string Name { get; set; } = string.Empty; // 플레이어 이름
    public new int Gold { get; set; } = 0; // 골드 (V2 신규 필드)

    public SaveDataV3()
    {
        Version = 3; // V2로 초기화
    }

    public override SaveData VersionUp()
    {
        var saveData = new SaveDataV3();
        return saveData;
    }


}

//[System.Serializable]
//public class SaveDataV3 : SaveData
//{
//    public List<string> items { get; set; } = new List<string>(); // 아이템 목록 (V3 신규 필드)
//    public string Name { get; set; } = string.Empty; // 플레이어 이름
//    public int Gold { get; set; } = 0; // 골드

//    public SaveDataV3()
//    {
//        Version = 3; // V3로 초기화
//    }

//    public override SaveData VersionUp()
//    {
//        // 현재 최신 버전이므로 자기 자신을 복사해서 반환
//        var saveData = new SaveDataV3();
//        saveData.Name = Name;
//        saveData.Gold = Gold;
//        return saveData;
//    }
//}