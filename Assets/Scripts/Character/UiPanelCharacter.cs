using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiPanelCharacter : MonoBehaviour
{
    public TMP_Dropdown sorting;
    public TMP_Dropdown filterring;

    public UiCharacterSlotList uiCharacterSlotList;
    public UiCharacterInfo uiCharacterInfo;
    public UiEquipSelectPopup weaponSelectPopup; // 무기 슬롯 전용 팝업
    public UiEquipSelectPopup equipSelectPopup;  // 장비 슬롯 전용 팝업
    public Button openEquipButton;               // 장비 전체 선택 팝업을 여는 버튼 (Inspector에서 연결)
    public GameObject characterSelectText;       // 팝업 열릴 때 함께 숨길 캐릭터 선택 텍스트

    private SaveCharacterData currentCharacter;

    private void Start()
    {
        sorting.onValueChanged.AddListener(OnchangeSorting);
        filterring.onValueChanged.AddListener(OnchangeFiltering);

        uiCharacterSlotList.onSelectSlot.AddListener(OnSelectCharacter);
        uiCharacterSlotList.onUpdateSlots.AddListener(OnClearCharacter);

        uiCharacterInfo.weaponEquipSlot.onClickSlot.AddListener(OnClickWeaponSlot);
        uiCharacterInfo.equipEquipSlot.onClickSlot.AddListener(OnClickEquipSlot);

        if (openEquipButton != null)
            openEquipButton.onClick.AddListener(OnClickOpenAllEquip);
    }

    private void OnEnable()
    {
        OnLoad();
        RefreshDropdownOptions();
        OnchangeFiltering(filterring.value);
        OnchangeSorting(sorting.value);
    }

    private void OnDisable()
    {
        sorting.onValueChanged.RemoveListener(OnchangeSorting);
        filterring.onValueChanged.RemoveListener(OnchangeFiltering);

        uiCharacterSlotList.onSelectSlot.RemoveListener(OnSelectCharacter);
        uiCharacterSlotList.onUpdateSlots.RemoveListener(OnClearCharacter);
        uiCharacterInfo.weaponEquipSlot.onClickSlot.RemoveListener(OnClickWeaponSlot);
        uiCharacterInfo.equipEquipSlot.onClickSlot.RemoveListener(OnClickEquipSlot);

        if (openEquipButton != null)
            openEquipButton.onClick.RemoveListener(OnClickOpenAllEquip);
    }

    public void OnchangeSorting(int index)
    {
        uiCharacterSlotList.Sorting = (UiCharacterSlotList.SortingOptions)index;
    }

    public void OnchangeFiltering(int index)
    {
        uiCharacterSlotList.Filtering = (UiCharacterSlotList.FilteringOptions)index;
    }

    public void OnSave()
    {
        SaveLoadManager.Data.characterList = uiCharacterSlotList.GetSaveCharacterDataList();
        SaveLoadManager.Data.CharacterSortingOption = (int)uiCharacterSlotList.Sorting;
        SaveLoadManager.Data.CharacterFilteringOption = (int)uiCharacterSlotList.Filtering;
        SaveLoadManager.Save();
    }

    public void OnLoad()
    {
        SaveLoadManager.Load();
        uiCharacterSlotList.SetSaveCharacterDataList(SaveLoadManager.Data.characterList);

        uiCharacterSlotList.Sorting = (UiCharacterSlotList.SortingOptions)SaveLoadManager.Data.CharacterSortingOption;
        uiCharacterSlotList.Filtering = (UiCharacterSlotList.FilteringOptions)SaveLoadManager.Data.CharacterFilteringOption;

        sorting.SetValueWithoutNotify(SaveLoadManager.Data.CharacterSortingOption);
        filterring.SetValueWithoutNotify(SaveLoadManager.Data.CharacterFilteringOption);
    }

    public void OnCreateCharacter()
    {
        uiCharacterSlotList.AddRandomCharacter();
    }

    public void OnAddCharacter()
    {
        uiCharacterSlotList.AddRandomCharacter();
        SaveLoadManager.Data.itemList.Add(SaveItemData.GetRandomItem());
        SaveLoadManager.Data.itemList.Add(SaveItemData.GetRandomItem());
    }

    public void OnRemoveCharacter()
    {
        uiCharacterSlotList.RemoveCharacter();
    }

    private void RefreshDropdownOptions()
    {
        int savedSorting = sorting.value;
        sorting.ClearOptions();
        sorting.AddOptions(new System.Collections.Generic.List<string>
        {
            DataTableManager.StringTable.Get("SORT_CREATION_ASC"),
            DataTableManager.StringTable.Get("SORT_CREATION_DESC"),
            DataTableManager.StringTable.Get("SORT_NAME_ASC"),
            DataTableManager.StringTable.Get("SORT_NAME_DESC"),
            DataTableManager.StringTable.Get("SORT_ATTACK_ASC"),
            DataTableManager.StringTable.Get("SORT_ATTACK_DESC"),
            DataTableManager.StringTable.Get("SORT_DEFEND_ASC"),
            DataTableManager.StringTable.Get("SORT_DEFEND_DESC"),
            DataTableManager.StringTable.Get("SORT_HEALTH_ASC"),
            DataTableManager.StringTable.Get("SORT_HEALTH_DESC"),
        });
        sorting.SetValueWithoutNotify(savedSorting);
        sorting.RefreshShownValue();

        int savedFiltering = filterring.value;
        filterring.ClearOptions();
        filterring.AddOptions(new System.Collections.Generic.List<string>
        {
            DataTableManager.StringTable.Get("FILTER_NONE"),
            DataTableManager.StringTable.Get("FILTER_HIGH_ATTACK"),
            DataTableManager.StringTable.Get("FILTER_LOW_ATTACK"),
        });
        filterring.SetValueWithoutNotify(savedFiltering);
        filterring.RefreshShownValue();
    }

    private void OnSelectCharacter(SaveCharacterData data)
    {
        currentCharacter = data;
    }

    private void OnClearCharacter()
    {
        currentCharacter = null;
        uiCharacterInfo.SetEmpty();
    }

    private void ShowCharacterSelect(bool show)
    {
        uiCharacterSlotList.gameObject.SetActive(show);
        if (characterSelectText != null)
            characterSelectText.SetActive(show);
    }

    private void OnClickOpenAllEquip()
    {
        if (currentCharacter == null) return;
        var character = currentCharacter;
        ShowCharacterSelect(false);
        weaponSelectPopup.OpenAll(
            SaveLoadManager.Data.itemList,
            item => { character.weaponSlot = item; uiCharacterInfo.SetSaveCharacterData(character); },
            item => { character.equipSlot  = item; uiCharacterInfo.SetSaveCharacterData(character); },
            () => ShowCharacterSelect(true)
        );
    }

    private void OnClickWeaponSlot()
    {
        if (currentCharacter == null) return;
        var character = currentCharacter;
        weaponSelectPopup.Open(SaveLoadManager.Data.itemList, ItemTypes.Weapon, item =>
        {
            character.weaponSlot = item;
            uiCharacterInfo.SetSaveCharacterData(character);
        });
        weaponSelectPopup.SetCloseCallback(() => uiCharacterInfo.SetSaveCharacterData(character));
    }

    private void OnClickEquipSlot()
    {
        if (currentCharacter == null) return;
        var character = currentCharacter;
        equipSelectPopup.Open(SaveLoadManager.Data.itemList, ItemTypes.Equip, item =>
        {
            character.equipSlot = item;
            uiCharacterInfo.SetSaveCharacterData(character);
        });
        equipSelectPopup.SetCloseCallback(() => uiCharacterInfo.SetSaveCharacterData(character));
    }
}
