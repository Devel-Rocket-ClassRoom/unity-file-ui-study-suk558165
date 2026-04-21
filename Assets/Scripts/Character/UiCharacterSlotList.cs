using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// 캐릭터 슬롯 목록을 생성·정렬·필터링하고 선택 이벤트를 관리하는 클래스
public class UiCharacterSlotList : MonoBehaviour
{
    // 슬롯 목록의 정렬 방식을 나타내는 열거형
    public enum SortingOptions
    {
        CreationTimeAsscending,  // 획득 시간 오름차순
        CreationTimeDeaccending, // 획득 시간 내림차순
        NameAsscending,          // 이름 오름차순
        NameDeaccending,         // 이름 내림차순
        AttackAsscending,        // 공격력 오름차순
        AttackDeaccending,       // 공격력 내림차순
        DefendAsscending,        // 방어력 오름차순
        DefendDeaccending,       // 방어력 내림차순
        HealthAsscending,        // 체력 오름차순
        HealthDeaccending,       // 체력 내림차순
    }

    // 슬롯 목록의 필터링 방식을 나타내는 열거형
    public enum FilteringOptions
    {
        None,        // 전체 표시
        HighAttack,  // 공격력 15 이상
        LowAttack,   // 공격력 15 미만
    }

    // 각 정렬 옵션에 대응하는 비교 함수 배열 (SortingOptions 열거형 순서와 동일)
    public readonly Comparison<SaveCharacterData>[] comparisons =
    {
        (lhs, rhs) => lhs.creationTime.CompareTo(rhs.creationTime),                          // 획득 시간 오름차순 비교
        (lhs, rhs) => rhs.creationTime.CompareTo(lhs.creationTime),                          // 획득 시간 내림차순 비교
        (lhs, rhs) => lhs.characterData.StringName.CompareTo(rhs.characterData.StringName),  // 이름 오름차순 비교
        (lhs, rhs) => rhs.characterData.StringName.CompareTo(lhs.characterData.StringName),  // 이름 내림차순 비교
        (lhs, rhs) => lhs.characterData.Attack.CompareTo(rhs.characterData.Attack),          // 공격력 오름차순 비교
        (lhs, rhs) => rhs.characterData.Attack.CompareTo(lhs.characterData.Attack),          // 공격력 내림차순 비교
        (lhs, rhs) => lhs.characterData.Dffend.CompareTo(rhs.characterData.Dffend),          // 방어력 오름차순 비교
        (lhs, rhs) => rhs.characterData.Dffend.CompareTo(lhs.characterData.Dffend),          // 방어력 내림차순 비교
        (lhs, rhs) => lhs.characterData.Health.CompareTo(rhs.characterData.Health),          // 체력 오름차순 비교
        (lhs, rhs) => rhs.characterData.Health.CompareTo(lhs.characterData.Health),          // 체력 내림차순 비교
    };

    // 각 필터링 옵션에 대응하는 조건 함수 배열 (FilteringOptions 열거형 순서와 동일)
    public readonly Func<SaveCharacterData, bool>[] filterings =
    {
        (x) => true,                           // 전체 표시 (필터 없음)
        (x) => x.characterData.Attack >= 15,   // 공격력 15 이상만 통과
        (x) => x.characterData.Attack < 15,    // 공격력 15 미만만 통과
    };

    public UiCharacterSlot prefab;       // 슬롯 생성에 사용할 UiCharacterSlot 프리팹
    public ScrollRect scrollRect;        // 슬롯 목록을 스크롤할 ScrollRect 컴포넌트
    public UiCharacterInfo uiCharacterInfo; // 선택된 캐릭터의 상세 정보를 표시할 UiCharacterInfo 참조

    private List<UiCharacterSlot> uiSlotList = new List<UiCharacterSlot>();             // 현재 생성된 슬롯 오브젝트 목록
    private List<SaveCharacterData> saveCharacterDataList = new List<SaveCharacterData>(); // 보유 캐릭터 데이터 목록

    private SortingOptions sorting = SortingOptions.CreationTimeAsscending;  // 현재 적용 중인 정렬 옵션 (기본값: 획득 시간 오름차순)
    private FilteringOptions filtering = FilteringOptions.None;              // 현재 적용 중인 필터링 옵션 (기본값: 필터 없음)

    private int selectedSlotIndex = -1; // 현재 선택된 슬롯의 인덱스 (-1이면 미선택 상태)

    public UnityEvent onUpdateSlots;                          // 슬롯 목록이 갱신될 때 발생하는 이벤트
    public UnityEvent<SaveCharacterData> onSelectSlot;        // 슬롯이 선택될 때 캐릭터 데이터를 전달하는 이벤트

    // 씬 시작 시 uiCharacterInfo와의 이벤트 연결을 설정하는 메서드
    private void Start()
    {
        if (uiCharacterInfo == null) return;
        onSelectSlot.AddListener(uiCharacterInfo.SetSaveCharacterData); // 슬롯 선택 이벤트에 상세 정보 표시 메서드를 연결
        onUpdateSlots.AddListener(uiCharacterInfo.SetEmpty);            // 슬롯 목록 갱신 이벤트에 상세 정보 초기화 메서드를 연결
    }

    // 외부에서 캐릭터 데이터 목록을 설정하고 슬롯을 갱신하는 메서드
    public void SetSaveCharacterDataList(List<SaveCharacterData> source)
    {
        saveCharacterDataList = source.ToList(); // 전달받은 리스트를 복사하여 내부 리스트에 저장
        UpdateSlots(); // 변경된 데이터로 슬롯 UI를 갱신
    }

    // 현재 보유 캐릭터 데이터 목록의 복사본을 반환하는 메서드
    public List<SaveCharacterData> GetSaveCharacterDataList()
    {
        return saveCharacterDataList.ToList(); // 내부 리스트의 복사본을 반환
    }

    // 오브젝트가 비활성화될 때 캐릭터 데이터 목록을 해제하는 메서드
    private void OnDisable()
    {
        saveCharacterDataList = null; // 비활성화 시 캐릭터 데이터 목록 참조를 해제
    }

    // 현재 정렬 옵션을 가져오거나 변경 시 슬롯을 자동 갱신하는 프로퍼티
    public SortingOptions Sorting
    {
        get => sorting;
        set
        {
            if (sorting != value) // 새 값이 기존 값과 다른 경우에만 처리
            {
                sorting = value;   // 정렬 옵션을 새 값으로 업데이트
                UpdateSlots();     // 변경된 정렬 옵션으로 슬롯 UI를 갱신
            }
        }
    }

    // 현재 필터링 옵션을 가져오거나 변경 시 슬롯을 자동 갱신하는 프로퍼티
    public FilteringOptions Filtering
    {
        get => filtering;
        set
        {
            if (filtering != value) // 새 값이 기존 값과 다른 경우에만 처리
            {
                filtering = value;  // 필터링 옵션을 새 값으로 업데이트
                UpdateSlots();      // 변경된 필터링 옵션으로 슬롯 UI를 갱신
            }
        }
    }

    // 현재 필터링·정렬 옵션을 적용하여 슬롯 목록 UI를 갱신하는 메서드
    private void UpdateSlots()
    {
        var list = saveCharacterDataList.Where(filterings[(int)filtering]).ToList(); // 현재 필터링 조건에 맞는 캐릭터만 추출
        list.Sort(comparisons[(int)sorting]); // 현재 정렬 비교 함수로 리스트를 정렬

        if (uiSlotList.Count < list.Count) // 기존 슬롯 수가 표시할 캐릭터 수보다 적으면 슬롯 추가 생성
        {
            for (int i = 0; uiSlotList.Count < list.Count; i++) // 필요한 수만큼 슬롯을 생성
            {
                var newSlot = Instantiate(prefab, scrollRect.content); // 프리팹으로 새 슬롯을 ScrollRect의 content에 생성
                newSlot.slotIndex = i;            // 생성된 슬롯에 인덱스를 할당
                newSlot.SetEmpty();               // 슬롯을 빈 상태로 초기화
                newSlot.gameObject.SetActive(false); // 초기에는 비활성 상태로 설정

                newSlot.button.onClick.AddListener(() => // 슬롯 버튼 클릭 이벤트에 람다 함수를 등록
                {
                    selectedSlotIndex = newSlot.slotIndex;             // 클릭된 슬롯의 인덱스를 선택 인덱스로 저장
                    onSelectSlot.Invoke(newSlot.SaveCharacterData);    // 슬롯 선택 이벤트를 발생시켜 캐릭터 데이터를 전달
                });
                uiSlotList.Add(newSlot); // 생성된 슬롯을 목록에 추가
            }
        }

        for (int i = 0; i < uiSlotList.Count; ++i) // 전체 슬롯을 순회하며 데이터 할당 또는 비활성화
        {
            if (i < list.Count) // 표시할 캐릭터가 있는 슬롯
            {
                uiSlotList[i].slotIndex = i;              // 슬롯에 현재 인덱스를 업데이트
                uiSlotList[i].gameObject.SetActive(true); // 슬롯을 활성화하여 화면에 표시
                uiSlotList[i].SetCharacter(list[i]);      // 해당 인덱스의 캐릭터 데이터를 슬롯에 설정
            }
            else
            {
                uiSlotList[i].gameObject.SetActive(false); // 표시할 캐릭터가 없는 슬롯은 비활성화
                uiSlotList[i].SetEmpty();                  // 슬롯을 빈 상태로 초기화
            }
        }

        selectedSlotIndex = -1; // 슬롯 갱신 후 선택 상태를 초기화
        onUpdateSlots.Invoke(); // 슬롯 갱신 완료 이벤트를 발생
    }

    // 랜덤 캐릭터를 획득하여 목록에 추가하고 슬롯을 갱신하는 메서드
    public void AddRandomCharacter()
    {
        saveCharacterDataList.Add(SaveCharacterData.GetRandomCharacter()); // 랜덤 캐릭터 데이터를 생성하여 목록에 추가
        UpdateSlots(); // 추가된 캐릭터를 반영하여 슬롯 UI를 갱신
    }

    // 현재 선택된 슬롯의 캐릭터를 목록에서 제거하는 메서드
    public void RemoveCharacter()
    {
        if (selectedSlotIndex == -1) return; // 선택된 슬롯이 없으면 처리하지 않음
        saveCharacterDataList.Remove(uiSlotList[selectedSlotIndex].SaveCharacterData); // 선택된 슬롯의 캐릭터 데이터를 목록에서 제거
        UpdateSlots(); // 제거 후 슬롯 UI를 갱신
    }
}
