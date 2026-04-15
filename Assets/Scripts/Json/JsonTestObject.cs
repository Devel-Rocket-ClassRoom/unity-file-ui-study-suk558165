using UnityEngine;

public class JsonTestObject : MonoBehaviour
{
    public string prefabName;
    private Material mat;

    private void Awake()
    {
        mat = GetComponent<Renderer>().material;
    }

    public void Set(ObjectSaveData data)
    {
        transform.position = data.pos;
        transform.rotation = data.rot;
        transform.localScale = data.scale;
        mat.color = data.color;
    }

    public ObjectSaveData GetSaveData()
    {
        return new ObjectSaveData
        {
            prefabName = prefabName,
            pos = transform.position,
            rot = transform.rotation,
            scale = transform.localScale,
            color = mat.color
        };
    }
}