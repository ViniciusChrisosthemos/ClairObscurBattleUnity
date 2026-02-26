using UnityEngine;

using System;
using System.Collections;

public class BattleCameraManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform m_camera;

    [Header("Animation Parameters")]
    [SerializeField] private float _cameraMoveDuration = 1f;

    private Transform m_prevCameraParent;

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
        StartCoroutine(AnimateCameraMovementCoroutine(target, _cameraMoveDuration));
    }

    public void SetParent(Transform newParent)
    {
        m_prevCameraParent = m_camera.parent;

        m_camera.SetParent(newParent, false);
        m_camera.localPosition = Vector3.zero;
        m_camera.localRotation = Quaternion.identity;
    }

    public void Reset()
    {
        m_camera.SetParent(m_prevCameraParent, false);
        m_camera.localPosition = Vector3.zero;
        m_camera.localRotation = Quaternion.identity;
    }

    public Transform GetCameraTransform()
    {
        return m_camera;
    }
}
