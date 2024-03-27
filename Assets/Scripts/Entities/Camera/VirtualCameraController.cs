using Cinemachine;
using UnityEngine;

public class VirtualCameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    private CinemachineTransposer _composer;

    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _composer = _virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
    }

    public void SetFollowTarget(Transform transform)
    {
        _virtualCamera.Follow = transform;
    }

    public void SetLookAtTarget(Transform transform)
    {
        _virtualCamera.LookAt = transform;
    }

    public void SetBodyFollowVertical(float value)
    {
        _composer.m_FollowOffset.y = value;
    }

    public Vector3 GetBodyFollowOffset()
    {
        return _composer.m_FollowOffset;
    }

    public float GetVerticalFOV()
    {
        return _virtualCamera.m_Lens.FieldOfView;
    }

    public void SetFollowOffset(Vector3 vector3)
    {
        _composer.m_FollowOffset = vector3;
    }

    public void SetFollowOffset(float x, float y, float z)
    {
        _composer.m_FollowOffset = new Vector3(x, y, z);
    }

    public void SetVerticalFOV(float value)
    {
        _virtualCamera.m_Lens.FieldOfView = value;
    }
}
