using UnityEngine;

// 씬에 배치된 테스트 오브젝트의 저장/복원 기능을 담당하는 MonoBehaviour 클래스
public class JsonTestObject : MonoBehaviour
{
    public string prefabName; // 이 오브젝트의 프리팹 이름 (로드 시 어떤 프리팹을 생성할지 식별에 사용)
    private Material mat; // 오브젝트의 머티리얼 참조 (색상 접근용, 캐싱)

    // 오브젝트 초기화 시 머티리얼을 캐싱하는 메서드
    private void Awake()
    {
        mat = GetComponent<Renderer>().material; // 렌더러 컴포넌트에서 머티리얼을 가져와 캐싱
    }

    // 저장 데이터를 받아 오브젝트의 위치/회전/크기/색상을 복원하는 메서드
    public void Set(ObjectSaveData data)
    {
        transform.position = data.pos; // 저장된 위치를 오브젝트에 적용
        transform.rotation = data.rot; // 저장된 회전을 오브젝트에 적용
        transform.localScale = data.scale; // 저장된 크기를 오브젝트에 적용
        mat.color = data.color; // 저장된 색상을 오브젝트 머티리얼에 적용
    }

    // 현재 오브젝트의 상태를 ObjectSaveData로 반환하는 메서드
    public ObjectSaveData GetSaveData()
    {
        return new ObjectSaveData // 현재 상태를 담은 ObjectSaveData 객체를 생성하여 반환
        {
            prefabName = prefabName, // 프리팹 이름 저장
            pos = transform.position, // 현재 위치 저장
            rot = transform.rotation, // 현재 회전 저장
            scale = transform.localScale, // 현재 크기 저장
            color = mat.color // 현재 머티리얼 색상 저장
        };
    }
}
