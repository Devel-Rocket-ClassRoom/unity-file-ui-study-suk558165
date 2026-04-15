//using UnityEngine;

//public class CubeController : MonoBehaviour
//{
//    public JsonTest2 jsonTest2;
//    public Renderer myrenderer;

//    public void Save()
//    {
//        SomeClass obj = new SomeClass();
//        obj.pos = transform.position;
//        obj.color = myrenderer.material.color;

//        SomeClass some = obj;
//        jsonTest2.Save(obj);
//        Debug.Log($"Save 완료: pos={obj.pos}, color={obj.color}");

//    }


//    public void Load()
//    {
//        SomeClass obj = jsonTest2.Load();
//        transform.position = obj.pos;
//        myrenderer.material.color = obj.color;
//        Debug.Log($"Load 완료: pos={obj.pos}, color={obj.color}");
//    }

//}
