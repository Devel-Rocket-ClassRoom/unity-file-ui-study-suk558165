using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class SaveLoadTest1 : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        { 
            SaveLoadManager.Data = new SaveDataV3();
            SaveLoadManager.Data.Name = "TEST1234";
            SaveLoadManager.Data.Gold = 4321;
            SaveLoadManager.Data.itemList.Add("Item1");
            SaveLoadManager.Data.itemList.Add("Item2");
            SaveLoadManager.Data.itemList.Add("Item3");
            SaveLoadManager.Data.itemList.Add("Item4");
            SaveLoadManager.Save();
   
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (SaveLoadManager.Load())
            {
                //foreach (var item in SaveLoadManager.Data.items)
                //{
                //    Debug.Log(DataTableManager.ItemTable.Get(item).Name);
                //}
                Debug.Log(SaveLoadManager.Data.Name);
                Debug.Log(SaveLoadManager.Data.Gold);
                Debug.Log(SaveLoadManager.Data.Version);
                foreach (var item in SaveLoadManager.Data.itemList)
                {
                    Debug.Log(DataTableManager.ItemTable.Get(item).Name);
                }

            }
            else
            {
                Debug.Log("세이브 파일 없음");
            }
        }
    }
}
