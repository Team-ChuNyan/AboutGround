using Cinemachine;
using UnityEngine;

public class VirtualCameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void SetFollowTarget(Transform transform)
    {
        _virtualCamera.Follow = transform;
    }

    public void SetLookAtTarget(Transform transform)
    {
        _virtualCamera.LookAt = transform;
    }
}
