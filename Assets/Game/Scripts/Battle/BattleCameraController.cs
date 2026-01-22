using System;
using System.Collections;
using UnityEngine;

public class BattleCameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BattleController _battleController;

    [Header("UI References")]
    [SerializeField] private Transform _camera;

    [Header("Animation Parameters")]
    [SerializeField] private float _cameraMoveDuration = 1f;

    private IEnumerator AnimateCameraMovementCoroutine(Transform target, float duration)
    {
        var accumTime = 0f;

        var startPosition = _camera.position;
        var startRotation = _camera.rotation;

        while (accumTime < duration)
        {
            accumTime += Time.deltaTime;

            var t = accumTime / duration;

            _camera.position = Vector3.Lerp(startPosition, target.position, t);
            _camera.rotation = Quaternion.Slerp(startRotation, target.rotation, t);

            yield return null;
        }
    }

    public void MoveCameraTo(Transform target)
    {
        StartCoroutine(AnimateCameraMovementCoroutine(target, _cameraMoveDuration));
    }
}
