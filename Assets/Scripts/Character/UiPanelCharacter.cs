using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 캐릭터 패널 전체를 총괄하는 클래스 — 정렬/필터 드롭다운, 슬롯 목록, 상세 정보, 장비 팝업을 연결하고 저장/불러오기를 처리
public class UiPanelCharacter : MonoBehaviour
{
    public TMP_Dropdown sorting;    // 캐릭터 목록 정렬 옵션을 선택하는 드롭다운 컴포넌트
    public TMP_Dropdown filterring; // 캐릭터 목록 필터링 옵션을 선택하는 드롭다운 컴포넌트

    public UiCharacterSlotList uiCharacterSlotList;    // 캐릭터 슬롯 목록 UI를 관리하는 컴포넌트
    public UiCharacterInfo uiCharacterInfo;            // 선택된 캐릭터의 상세 정보를 표시하는 컴포넌트
    public UiEquipSelectPopup weaponSelectPopup;       // 무기 슬롯 전용 아이템 선택 팝업
    public UiEquipSelectPopup equipSelectPopup;        // 장비 슬롯 전용 아이템 선택 팝업
    public Button openEquipButton;                     // 무기+장비 전체 선택 팝업을 여는 버튼 (Inspector에서 연결)
    public GameObject characterSelectText;             // 팝업이 열릴 때 함께 숨길 캐릭터 선택 안내 텍스트 오브젝트

    private SaveCharacterData currentCharacter; // 현재 선택된 캐릭터 데이터 (미선택 시 null)

    // 씬 시작 시 드롭다운·슬롯·장비 슬롯 이벤트 리스너를 등록하는 메서드
    private void Start()
    {
        sorting.onValueChanged.AddListener(OnchangeSorting);     // 정렬 드롭다운 값 변경 시 OnchangeSorting 호출
        filterring.onValueChanged.AddListener(OnchangeFiltering); // 필터 드롭다운 값 변경 시 OnchangeFiltering 호출

        uiCharacterSlotList.onSelectSlot.AddListener(OnSelectCharacter); // 캐릭터 슬롯 선택 시 OnSelectCharacter 호출
        uiCharacterSlotList.onUpdateSlots.AddListener(OnClearCharacter); // 슬롯 목록 갱신 시 OnClearCharacter 호출

        uiCharacterInfo.weaponEquipSlot.onClickSlot.AddListener(OnClickWeaponSlot); // 무기 슬롯 클릭 시 OnClickWeaponSlot 호출
        uiCharacterInfo.equipEquipSlot.onClickSlot.AddListener(OnClickEquipSlot);   // 장비 슬롯 클릭 시 OnClickEquipSlot 호출

        if (openEquipButton != null)
            openEquipButton.onClick.AddListener(OnClickOpenAllEquip); // 전체 장비 버튼 클릭 시 OnClickOpenAllEquip 호출
    }

    // 패널이 활성화될 때 데이터 로드·드롭다운 옵션 갱신·필터·정렬을 순서대로 적용하는 메서드
    private void OnEnable()
    {
        OnLoad();                              // 저장 파일에서 캐릭터·아이템 데이터를 불러와 슬롯에 적용
        RefreshDropdownOptions();              // 다국어 테이블에서 텍스트를 읽어 드롭다운 옵션 레이블을 갱신
        OnchangeFiltering(filterring.value);   // 현재 필터 드롭다운 값으로 필터링 옵션 즉시 적용
        OnchangeSorting(sorting.value);        // 현재 정렬 드롭다운 값으로 정렬 옵션 즉시 적용
    }

    // 패널이 비활성화될 때 등록한 이벤트 리스너를 모두 해제하는 메서드
    private void OnDisable()
    {
        sorting.onValueChanged.RemoveListener(OnchangeSorting);     // 정렬 드롭다운 이벤트 리스너 해제
        filterring.onValueChanged.RemoveListener(OnchangeFiltering); // 필터 드롭다운 이벤트 리스너 해제

        uiCharacterSlotList.onSelectSlot.RemoveListener(OnSelectCharacter); // 슬롯 선택 이벤트 리스너 해제
        uiCharacterSlotList.onUpdateSlots.RemoveListener(OnClearCharacter); // 슬롯 갱신 이벤트 리스너 해제
        uiCharacterInfo.weaponEquipSlot.onClickSlot.RemoveListener(OnClickWeaponSlot); // 무기 슬롯 클릭 이벤트 리스너 해제
        uiCharacterInfo.equipEquipSlot.onClickSlot.RemoveListener(OnClickEquipSlot);   // 장비 슬롯 클릭 이벤트 리스너 해제

        if (openEquipButton != null)
            openEquipButton.onClick.RemoveListener(OnClickOpenAllEquip); // 전체 장비 버튼 이벤트 리스너 해제
    }

    // 정렬 드롭다운 인덱스를 받아 슬롯 목록의 정렬 옵션을 변경하는 메서드
    public void OnchangeSorting(int index)
    {
        uiCharacterSlotList.Sorting = (UiCharacterSlotList.SortingOptions)index; // 인덱스를 SortingOptions 열거형으로 변환하여 슬롯 목록에 적용
    }

    // 필터 드롭다운 인덱스를 받아 슬롯 목록의 필터링 옵션을 변경하는 메서드
    public void OnchangeFiltering(int index)
    {
        uiCharacterSlotList.Filtering = (UiCharacterSlotList.FilteringOptions)index; // 인덱스를 FilteringOptions 열거형으로 변환하여 슬롯 목록에 적용
    }

    // 현재 캐릭터 목록·정렬·필터 상태를 파일에 저장하는 메서드
    public void OnSave()
    {
        SaveLoadManager.Data.characterList = uiCharacterSlotList.GetSaveCharacterDataList(); // 현재 슬롯 목록의 캐릭터 데이터를 세이브 데이터에 저장
        SaveLoadManager.Data.CharacterSortingOption = (int)uiCharacterSlotList.Sorting;     // 현재 정렬 옵션을 정수로 변환하여 세이브 데이터에 저장
        SaveLoadManager.Data.CharacterFilteringOption = (int)uiCharacterSlotList.Filtering; // 현재 필터 옵션을 정수로 변환하여 세이브 데이터에 저장
        SaveLoadManager.Save(); // 세이브 데이터를 파일에 기록
    }

    // 저장 파일에서 데이터를 불러와 슬롯·드롭다운 상태를 복원하는 메서드
    public void OnLoad()
    {
        SaveLoadManager.Load(); // 파일에서 세이브 데이터를 불러옴
        RelinkEquipment();      // JSON 역직렬화로 끊어진 장비 슬롯 참조를 instanceId로 재연결
        uiCharacterSlotList.SetSaveCharacterDataList(SaveLoadManager.Data.characterList); // 불러온 캐릭터 목록을 슬롯 UI에 적용

        uiCharacterSlotList.Sorting = (UiCharacterSlotList.SortingOptions)SaveLoadManager.Data.CharacterSortingOption;     // 저장된 정렬 옵션을 슬롯 목록에 복원
        uiCharacterSlotList.Filtering = (UiCharacterSlotList.FilteringOptions)SaveLoadManager.Data.CharacterFilteringOption; // 저장된 필터 옵션을 슬롯 목록에 복원

        sorting.SetValueWithoutNotify(SaveLoadManager.Data.CharacterSortingOption);     // 저장된 정렬 인덱스로 드롭다운 표시를 갱신 (이벤트 미발생)
        filterring.SetValueWithoutNotify(SaveLoadManager.Data.CharacterFilteringOption); // 저장된 필터 인덱스로 드롭다운 표시를 갱신 (이벤트 미발생)
    }

    // JSON 역직렬화 후 새 객체로 생성된 장비 슬롯 참조를 인벤토리의 동일 instanceId 객체로 다시 연결하는 메서드
    private void RelinkEquipment()
    {
        var items = SaveLoadManager.Data.itemList; // 인벤토리 아이템 목록 참조
        foreach (var character in SaveLoadManager.Data.characterList) // 전체 캐릭터를 순회
        {
            if (character.weaponSlot != null)
                character.weaponSlot = items.Find(x => x.instanceId == character.weaponSlot.instanceId); // 무기 슬롯의 instanceId와 일치하는 인벤토리 객체로 교체
            if (character.equipSlot != null)
                character.equipSlot = items.Find(x => x.instanceId == character.equipSlot.instanceId);   // 장비 슬롯의 instanceId와 일치하는 인벤토리 객체로 교체
        }
    }

    // 랜덤 캐릭터를 슬롯 목록에 추가하는 버튼 핸들러 메서드
    public void OnCreateCharacter()
    {
        uiCharacterSlotList.AddRandomCharacter(); // 슬롯 목록에 랜덤 캐릭터를 생성하여 추가
    }

    // 랜덤 캐릭터와 랜덤 아이템 2개를 동시에 추가하는 버튼 핸들러 메서드
    public void OnAddCharacter()
    {
        uiCharacterSlotList.AddRandomCharacter();                       // 슬롯 목록에 랜덤 캐릭터를 추가
        SaveLoadManager.Data.itemList.Add(SaveItemData.GetRandomItem()); // 인벤토리에 랜덤 아이템 1개 추가
        SaveLoadManager.Data.itemList.Add(SaveItemData.GetRandomItem()); // 인벤토리에 랜덤 아이템 1개 추가
    }

    // 현재 선택된 캐릭터를 슬롯 목록에서 제거하는 버튼 핸들러 메서드
    public void OnRemoveCharacter()
    {
        uiCharacterSlotList.RemoveCharacter(); // 슬롯 목록에서 선택된 캐릭터를 제거
    }

    // 다국어 테이블에서 텍스트를 읽어 정렬·필터 드롭다운 옵션 레이블을 갱신하는 메서드
    private void RefreshDropdownOptions()
    {
        int savedSorting = sorting.value; // 현재 선택된 정렬 인덱스를 저장 (옵션 초기화 후 복원용)
        sorting.ClearOptions(); // 기존 정렬 드롭다운 옵션을 모두 제거
        sorting.AddOptions(new System.Collections.Generic.List<string>
        {
            DataTableManager.StringTable.Get("SORT_CREATION_ASC"),  // 획득 시간 오름차순 레이블
            DataTableManager.StringTable.Get("SORT_CREATION_DESC"), // 획득 시간 내림차순 레이블
            DataTableManager.StringTable.Get("SORT_NAME_ASC"),      // 이름 오름차순 레이블
            DataTableManager.StringTable.Get("SORT_NAME_DESC"),     // 이름 내림차순 레이블
            DataTableManager.StringTable.Get("SORT_ATTACK_ASC"),    // 공격력 오름차순 레이블
            DataTableManager.StringTable.Get("SORT_ATTACK_DESC"),   // 공격력 내림차순 레이블
            DataTableManager.StringTable.Get("SORT_DEFEND_ASC"),    // 방어력 오름차순 레이블
            DataTableManager.StringTable.Get("SORT_DEFEND_DESC"),   // 방어력 내림차순 레이블
            DataTableManager.StringTable.Get("SORT_HEALTH_ASC"),    // 체력 오름차순 레이블
            DataTableManager.StringTable.Get("SORT_HEALTH_DESC"),   // 체력 내림차순 레이블
        });
        sorting.SetValueWithoutNotify(savedSorting); // 이전 선택 인덱스를 이벤트 없이 복원
        sorting.RefreshShownValue();                 // 드롭다운 표시 텍스트를 현재 값으로 갱신

        int savedFiltering = filterring.value; // 현재 선택된 필터 인덱스를 저장 (옵션 초기화 후 복원용)
        filterring.ClearOptions(); // 기존 필터 드롭다운 옵션을 모두 제거
        filterring.AddOptions(new System.Collections.Generic.List<string>
        {
            DataTableManager.StringTable.Get("FILTER_NONE"),         // 전체 표시 레이블
            DataTableManager.StringTable.Get("FILTER_HIGH_ATTACK"),  // 공격력 높음 필터 레이블
            DataTableManager.StringTable.Get("FILTER_LOW_ATTACK"),   // 공격력 낮음 필터 레이블
        });
        filterring.SetValueWithoutNotify(savedFiltering); // 이전 선택 인덱스를 이벤트 없이 복원
        filterring.RefreshShownValue();                   // 드롭다운 표시 텍스트를 현재 값으로 갱신
    }

    // 캐릭터 슬롯이 선택될 때 호출되어 currentCharacter를 업데이트하는 메서드
    private void OnSelectCharacter(SaveCharacterData data)
    {
        currentCharacter = data; // 선택된 캐릭터 데이터를 현재 캐릭터로 저장
    }

    // 슬롯 목록이 갱신될 때 호출되어 선택 상태와 상세 정보 UI를 초기화하는 메서드
    private void OnClearCharacter()
    {
        currentCharacter = null;      // 현재 선택 캐릭터를 초기화
        uiCharacterInfo.SetEmpty();   // 캐릭터 상세 정보 UI를 빈 상태로 초기화
    }

    // 아이템이 다른 캐릭터에 장착되어 있으면 해제하여 중복 장착을 방지하는 메서드
    private void TransferItem(SaveItemData item, SaveCharacterData targetCharacter)
    {
        if (item == null) return; // 아이템이 없으면 처리하지 않음
        foreach (var character in uiCharacterSlotList.GetSaveCharacterDataList()) // 전체 캐릭터를 순회
        {
            if (character == targetCharacter) continue;             // 장착 대상 캐릭터는 건너뜀
            if (character.weaponSlot == item) character.weaponSlot = null; // 다른 캐릭터의 무기 슬롯에 같은 아이템이 있으면 해제
            if (character.equipSlot  == item) character.equipSlot  = null; // 다른 캐릭터의 장비 슬롯에 같은 아이템이 있으면 해제
        }
    }

    // 캐릭터 슬롯 목록과 안내 텍스트의 표시 여부를 전환하는 메서드 (팝업 열릴 때 숨김, 닫힐 때 복원)
    private void ShowCharacterSelect(bool show)
    {
        uiCharacterSlotList.gameObject.SetActive(show); // 슬롯 목록 GameObject를 show 값에 따라 활성화/비활성화
        if (characterSelectText != null)
            characterSelectText.SetActive(show); // 안내 텍스트 오브젝트를 show 값에 따라 활성화/비활성화
    }

    // 전체 장비 버튼 클릭 시 무기+장비를 동시에 선택할 수 있는 팝업을 여는 메서드
    private void OnClickOpenAllEquip()
    {
        if (currentCharacter == null) return; // 선택된 캐릭터가 없으면 팝업을 열지 않음
        var character = currentCharacter;     // 팝업이 열린 후 currentCharacter가 바뀌어도 이 캐릭터에 적용되도록 로컬 변수로 캡처
        ShowCharacterSelect(false);           // 팝업이 열리는 동안 캐릭터 슬롯 목록을 숨김
        weaponSelectPopup.OpenAll(
            SaveLoadManager.Data.itemList,
            item => { TransferItem(item, character); character.weaponSlot = item; uiCharacterInfo.SetSaveCharacterData(character); }, // 무기 선택 시: 이전 장착자에서 해제 → 현재 캐릭터 무기 슬롯에 장착 → 상세 정보 갱신
            item => { TransferItem(item, character); character.equipSlot  = item; uiCharacterInfo.SetSaveCharacterData(character); }, // 장비 선택 시: 이전 장착자에서 해제 → 현재 캐릭터 장비 슬롯에 장착 → 상세 정보 갱신
            () => ShowCharacterSelect(true)  // 팝업 닫힐 때 캐릭터 슬롯 목록을 다시 표시
        );
    }

    // 무기 슬롯 클릭 시 무기 타입 아이템만 보여주는 팝업을 여는 메서드
    private void OnClickWeaponSlot()
    {
        if (currentCharacter == null) return; // 선택된 캐릭터가 없으면 팝업을 열지 않음
        var character = currentCharacter;     // 팝업이 열린 후 currentCharacter가 바뀌어도 이 캐릭터에 적용되도록 로컬 변수로 캡처
        ShowCharacterSelect(false);           // 팝업이 열리는 동안 캐릭터 슬롯 목록을 숨김
        weaponSelectPopup.Open(SaveLoadManager.Data.itemList, ItemTypes.Weapon, item =>
        {
            TransferItem(item, character);              // 이미 다른 캐릭터에 장착된 경우 해당 슬롯을 먼저 해제
            character.weaponSlot = item;               // 현재 캐릭터의 무기 슬롯에 선택한 아이템 장착
            uiCharacterInfo.SetSaveCharacterData(character); // 상세 정보 UI를 갱신하여 무기 슬롯 변경을 반영
        });
        weaponSelectPopup.SetCloseCallback(() =>
        {
            ShowCharacterSelect(true);                 // 팝업 닫힐 때 캐릭터 슬롯 목록을 다시 표시
            uiCharacterInfo.SetSaveCharacterData(character); // 팝업이 닫힐 때 상세 정보 UI를 최신 상태로 갱신
        });
    }

    // 장비 슬롯 클릭 시 장비 타입 아이템만 보여주는 팝업을 여는 메서드
    private void OnClickEquipSlot()
    {
        if (currentCharacter == null) return; // 선택된 캐릭터가 없으면 팝업을 열지 않음
        var character = currentCharacter;     // 팝업이 열린 후 currentCharacter가 바뀌어도 이 캐릭터에 적용되도록 로컬 변수로 캡처
        ShowCharacterSelect(false);           // 팝업이 열리는 동안 캐릭터 슬롯 목록을 숨김
        equipSelectPopup.Open(SaveLoadManager.Data.itemList, ItemTypes.Equip, item =>
        {
            TransferItem(item, character);              // 이미 다른 캐릭터에 장착된 경우 해당 슬롯을 먼저 해제
            character.equipSlot = item;                // 현재 캐릭터의 장비 슬롯에 선택한 아이템 장착
            uiCharacterInfo.SetSaveCharacterData(character); // 상세 정보 UI를 갱신하여 장비 슬롯 변경을 반영
        });
        equipSelectPopup.SetCloseCallback(() =>
        {
            ShowCharacterSelect(true);                 // 팝업 닫힐 때 캐릭터 슬롯 목록을 다시 표시
            uiCharacterInfo.SetSaveCharacterData(character); // 팝업이 닫힐 때 상세 정보 UI를 최신 상태로 갱신
        });
    }
}
