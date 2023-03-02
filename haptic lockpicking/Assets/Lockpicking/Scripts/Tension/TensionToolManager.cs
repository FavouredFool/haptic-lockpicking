using UnityEngine;

public class TensionToolManager : MonoBehaviour
{
    [SerializeField]
    LockController _lock;

    public Transform GetTensionTool()
    {
        return _lock.GetCoreController().GetTensionTool();
    }

    public void SetTensionToolActive(bool active)
    {
        _lock.GetCoreController().GetTensionTool().gameObject.SetActive(active);
    }
}
