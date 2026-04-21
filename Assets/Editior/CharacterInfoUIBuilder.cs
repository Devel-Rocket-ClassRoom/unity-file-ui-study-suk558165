// Assets/Editor/CharacterInfoUIBuilder.cs
// 기존 프리팹을 로드해서 CharacterPanel 안에 조립합니다.
#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

public class CharacterInfoUIBuilder : EditorWindow
{
    // ── 프리팹 경로 (Assets/Prefabs/ 기준, 실제 경로에 맞게 수정) ──
    const string P_SAVE       = "Assets/Prefabs/SAVE Button.prefab";
    const string P_LOAD       = "Assets/Prefabs/Load Button.prefab";
    const string P_ADD        = "Assets/Prefabs/ADD Button .prefab";
    const string P_DELETE     = "Assets/Prefabs/Delete Button.prefab";
    const string P_SORTING    = "Assets/Prefabs/Sorting.prefab";
    const string P_FILTERING  = "Assets/Prefabs/Filtering.prefab";
    const string P_CHAR_INFO  = "Assets/Prefabs/CharaterInfo.prefab";
    const string P_CHAR_SLOT  = "Assets/Prefabs/CharaterSlot 1.prefab";
    const string P_CHAR_LIST  = "Assets/Prefabs/ChrarcterList.prefab";

    [MenuItem("Tools/Build Character Info UI")]
    public static void Build()
    {
        // 기존 패널 제거
        var existing = GameObject.Find("CharacterPanel");
        if (existing != null)
        {
            if (!EditorUtility.DisplayDialog("확인", "기존 CharacterPanel을 교체할까요?", "예", "아니오"))
                return;
            Object.DestroyImmediate(existing);
        }

        // Canvas 찾기
        Canvas canvas = Object.FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            var cGO = new GameObject("Canvas");
            canvas = cGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var cs = cGO.AddComponent<CanvasScaler>();
            cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            cs.referenceResolution = new Vector2(1920, 1080);
            cGO.AddComponent<GraphicRaycaster>();
            Undo.RegisterCreatedObjectUndo(cGO, "Create Canvas");
        }

        // ── 루트 패널 ──
        var root = new GameObject("CharacterPanel", typeof(RectTransform), typeof(Image));
        root.transform.SetParent(canvas.transform, false);
        Undo.RegisterCreatedObjectUndo(root, "Create CharacterPanel");

        var rootRT = root.GetComponent<RectTransform>();
        // 우측 전체 높이 고정 패널
        rootRT.anchorMin = new Vector2(1f, 0f);
        rootRT.anchorMax = new Vector2(1f, 1f);
        rootRT.pivot     = new Vector2(1f, 1f);
        rootRT.offsetMin = new Vector2(-600f, 0f);
        rootRT.offsetMax = Vector2.zero;
        root.GetComponent<Image>().color = Hex("#2B2B2B");

        var rootVLG = root.AddComponent<VerticalLayoutGroup>();
        rootVLG.padding = new RectOffset(12, 12, 12, 12);
        rootVLG.spacing = 8;
        rootVLG.childForceExpandWidth  = true;
        rootVLG.childForceExpandHeight = false;
        rootVLG.childAlignment = TextAnchor.UpperLeft;

        // ── 1. 드롭다운 바 (Sorting + Filtering 프리팹) ──
        var dropBar = new GameObject("DropdownBar", typeof(RectTransform));
        dropBar.transform.SetParent(root.transform, false);
        var dropBarLE = dropBar.AddComponent<LayoutElement>();
        dropBarLE.preferredHeight = 40; dropBarLE.flexibleWidth = 1;
        var dropHLG = dropBar.AddComponent<HorizontalLayoutGroup>();
        dropHLG.spacing = 8;
        dropHLG.childForceExpandWidth  = false;
        dropHLG.childForceExpandHeight = true;
        dropHLG.childAlignment = TextAnchor.MiddleLeft;

        InstantiatePrefabInto(P_SORTING,   dropBar.transform, 180, 40);
        InstantiatePrefabInto(P_FILTERING, dropBar.transform, 180, 40);

        // ── 2. CharaterInfo 프리팹 ──
        var infoGO = InstantiatePrefabInto(P_CHAR_INFO, root.transform, -1, -1);
        if (infoGO != null)
        {
            var infoLE = infoGO.GetComponent<LayoutElement>() ?? infoGO.AddComponent<LayoutElement>();
            infoLE.flexibleWidth = 1;
        }

        // ── 3. 캐릭터 목록 (ChrarcterList 프리팹) ──
        var listGO = InstantiatePrefabInto(P_CHAR_LIST, root.transform, -1, 400);
        if (listGO != null)
        {
            var listLE = listGO.GetComponent<LayoutElement>() ?? listGO.AddComponent<LayoutElement>();
            listLE.flexibleWidth = 1;
            listLE.preferredHeight = 400;
        }

        // ── 4. 버튼 바 (SAVE / Load / ADD / Delete 프리팹) ──
        var btnBar = new GameObject("ButtonBar", typeof(RectTransform));
        btnBar.transform.SetParent(root.transform, false);
        var btnBarLE = btnBar.AddComponent<LayoutElement>();
        btnBarLE.preferredHeight = 44; btnBarLE.flexibleWidth = 1;
        var btnHLG = btnBar.AddComponent<HorizontalLayoutGroup>();
        btnHLG.spacing = 6;
        btnHLG.childForceExpandWidth  = true;
        btnHLG.childForceExpandHeight = true;
        btnHLG.childAlignment = TextAnchor.MiddleLeft;

        InstantiatePrefabInto(P_SAVE,   btnBar.transform, -1, 44);
        InstantiatePrefabInto(P_LOAD,   btnBar.transform, -1, 44);
        InstantiatePrefabInto(P_ADD,    btnBar.transform, -1, 44);
        InstantiatePrefabInto(P_DELETE, btnBar.transform, -1, 44);

        Selection.activeGameObject = root;
        EditorUtility.SetDirty(root);
        Debug.Log("[CharacterInfoUIBuilder] 완료 — 프리팹 경로가 다르면 스크립트 상단 const 경로를 수정하세요.");
    }

    // ── 프리팹 로드 후 자식으로 배치 ──────────────────────────────
    static GameObject InstantiatePrefabInto(string path, Transform parent, float prefW, float prefH)
    {
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        if (prefab == null)
        {
            Debug.LogWarning($"[CharacterInfoUIBuilder] 프리팹을 찾을 수 없음: {path}");
            return null;
        }

        var go = (GameObject)PrefabUtility.InstantiatePrefab(prefab, parent);
        Undo.RegisterCreatedObjectUndo(go, "Instantiate " + prefab.name);

        // LayoutElement 설정
        var le = go.GetComponent<LayoutElement>() ?? go.AddComponent<LayoutElement>();
        if (prefW > 0) le.preferredWidth  = prefW;
        else           le.flexibleWidth   = 1;
        if (prefH > 0) le.preferredHeight = prefH;

        return go;
    }

    static Color Hex(string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out Color c);
        return c;
    }
}
#endif
