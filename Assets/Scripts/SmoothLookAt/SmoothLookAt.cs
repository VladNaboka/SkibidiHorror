using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothLookAt : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    private Transform _target;

    private Coroutine _lookCoroutine;
    private Coroutine _cameraLookCoroutine;

    public void StartRotating(Transform target)
    {
        if (_lookCoroutine != null)
        {
            StopCoroutine(_lookCoroutine);
        }

        _lookCoroutine = StartCoroutine(LookAt(target));
    }

    public void StartCameraRotationg(Transform cameraTransform, Transform target)
    {
        if (_cameraLookCoroutine != null)
        {
            StopCoroutine(_cameraLookCoroutine);
        }

        _cameraLookCoroutine = StartCoroutine(CameraLookAt(cameraTransform, target));
    }

    private IEnumerator LookAt(Transform target)
    {
        _target = target;
        Quaternion lookRotation = Quaternion.LookRotation(_target.position - transform.position);

        float time = 0;

        Quaternion initialRotation = transform.rotation;
        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(initialRotation, lookRotation, time);

            time += Time.deltaTime * _speed;

            yield return null;
        }
    }

    private IEnumerator CameraLookAt(Transform cameraTransform, Transform target)
    {
        _target = target;
        Quaternion lookRotation = Quaternion.LookRotation(_target.position - cameraTransform.position);

        float time = 0;

        Quaternion initialRotation = cameraTransform.rotation;
        while (time < 1)
        {
            cameraTransform.rotation = Quaternion.Slerp(initialRotation, lookRotation, time);

            time += Time.deltaTime * _speed;

            yield return null;
        }
    }
}
