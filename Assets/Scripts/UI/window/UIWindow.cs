using UnityEngine;

public abstract class UIWindow : MonoBehaviour
{
    public virtual void Show() => gameObject.SetActive(true);
    public virtual void Hide() => gameObject.SetActive(false);

    public void CloseSelf()
    {
        UIManager.instance.CloseWindow(this);
    }
}
