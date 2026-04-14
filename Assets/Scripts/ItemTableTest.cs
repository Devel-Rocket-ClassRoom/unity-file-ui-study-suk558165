using UnityEngine;

public class ItemTableTest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Debug.Log(DataTableManager.ItemTable);
        }
    }
}
