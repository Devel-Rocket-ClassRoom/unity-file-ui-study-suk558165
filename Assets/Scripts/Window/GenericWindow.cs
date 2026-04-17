using UnityEngine;
using UnityEngine.EventSystems;

public class GenericWindow : MonoBehaviour
{
    public GameObject firstSelectrd;

    public virtual void Init(WindowManager mgr) { }
    public virtual void Open()
    {
        gameObject.SetActive(true);
        if (firstSelectrd != null)
            EventSystem.current.SetSelectedGameObject(firstSelectrd);
    }
    public virtual void Close()
    {
        gameObject.SetActive(false);
    }
}
