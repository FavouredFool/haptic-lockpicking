using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    [SerializeField]
    Camera _camera;

    public Camera GetCamera()
    {
        return _camera;
    }
}
