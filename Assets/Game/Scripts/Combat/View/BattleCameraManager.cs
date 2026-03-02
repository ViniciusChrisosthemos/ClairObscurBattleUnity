using UnityEngine;

using System;
using System.Collections;

public class BattleCameraManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform m_camera;
    [SerializeField] private CameraBlurController m_cameraBlurController;

    [Header("Animation Parameters")]
    [SerializeField] private float _cameraMoveDuration = 1f;
    
    private Transform m_cameraTarget;
    private bool m_isFollowing = false;

    private void Awake()
    {
        m_cameraTarget = m_camera;
    }

    private void Update()
    {
        if (m_isFollowing)
        {
            m_camera.position = m_cameraTarget.position;
            m_camera.rotation = m_cameraTarget.rotation;
        }
    }

    public IEnumerator AnimateCameraMovementCoroutine(Transform target, float duration)
    {
        var accumTime = 0f;

        var startPosition = m_camera.position;
        var startRotation = m_camera.rotation;

        while (accumTime < duration)
        {
            accumTime += Time.deltaTime;

            var t = accumTime / duration;

            m_camera.position = Vector3.Lerp(startPosition, target.position, t);
            m_camera.rotation = Quaternion.Slerp(startRotation, target.rotation, t);

            yield return null;
        }
    }

    public void MoveCameraTo(Transform target)
    {
        StopFollow();
        StartCoroutine(AnimateCameraMovementCoroutine(target, _cameraMoveDuration));
    }
    
    public void FollowTarget(Transform newParent)
    {
        m_cameraTarget = newParent;
        m_isFollowing = true;
    }

    public void StopFollow()
    {
        m_isFollowing = false;
    }

    public Transform GetCameraTransform()
    {
        return m_camera;
    }

    public void SetBlur(bool isActive)
    {
        m_cameraBlurController.SetBlur(isActive);
    }
}
