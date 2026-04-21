//using UnityEngine; // Unity의 핵심 기능을 위한 라이브러리 (현재 비활성화)

//public class CubeController : MonoBehaviour // 큐브 오브젝트의 저장/로드를 담당하는 MonoBehaviour 클래스
//{
//    public JsonTest2 jsonTest2; // JSON 저장/로드 담당 컴포넌트 참조
//    public Renderer myrenderer; // 큐브의 렌더러 컴포넌트 참조 (색상 접근용)

//    public void Save() // 현재 큐브의 위치와 색상을 JSON으로 저장하는 메서드
//    {
//        SomeClass obj = new SomeClass(); // 저장 데이터 객체 생성
//        obj.pos = transform.position; // 현재 위치를 저장 데이터에 설정
//        obj.color = myrenderer.material.color; // 현재 색상을 저장 데이터에 설정

//        SomeClass some = obj; // 저장 데이터 참조 복사 (미사용)
//        jsonTest2.Save(obj); // JSON 파일로 저장 실행
//        Debug.Log($"Save 완료: pos={obj.pos}, color={obj.color}"); // 저장 완료 로그 출력

//    }


//    public void Load() // JSON에서 큐브의 위치와 색상을 불러오는 메서드
//    {
//        SomeClass obj = jsonTest2.Load(); // JSON 파일에서 데이터 로드
//        transform.position = obj.pos; // 로드된 위치를 큐브에 적용
//        myrenderer.material.color = obj.color; // 로드된 색상을 큐브에 적용
//        Debug.Log($"Load 완료: pos={obj.pos}, color={obj.color}"); // 로드 완료 로그 출력
//    }

//}
