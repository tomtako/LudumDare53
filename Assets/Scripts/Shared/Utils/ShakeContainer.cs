using Sirenix.OdinInspector;
using UnityEngine;

public class ShakeContainer : MonoBehaviour
{
    public bool shakePosition = true;
    [ShowIf("shakePosition")] public bool shakeX = true;
    [ShowIf("shakePosition")] public bool shakeY = true;
    public bool shakeRotation;
    [ShowIf("shakeRotation")] public Vector3 rotationShakeMultiplier = new Vector3(0, 0, 1);

    private Vector3 originPosition;
    private float _shakeDuration;
    private float _shakeTime;
    private float _shakeRemain;
    private int _shakeRotateSide;

    private Vector3 position
    {
        get
        {
            return this.RectTransform() != null ? (Vector3)this.RectTransform().anchoredPosition : transform.position;
        }

        set
        {
            if (this.RectTransform() != null) this.RectTransform().anchoredPosition = value;
            else transform.position = value;
        }
    }

    private void Awake()
    {
        originPosition = position;
    }

    private void Update()
    {
        if (_shakeRemain > 0)
        {
            if (shakePosition)
            {
                position = new Vector3(
                    originPosition.x + (shakeX ? Random.Range(-_shakeRemain, _shakeRemain) : 0),
                    originPosition.y + (shakeY ? Random.Range(-_shakeRemain, _shakeRemain) : 0),
                    originPosition.z);
            }

            if (shakeRotation)
            {
                transform.localEulerAngles = rotationShakeMultiplier * (_shakeRemain * _shakeRotateSide);
                _shakeRotateSide *= -1;
            }

            _shakeTime = _shakeTime + Time.deltaTime;
            _shakeRemain = Mathf.Lerp(_shakeRemain, 0, _shakeTime / _shakeDuration);

            if (_shakeRemain <= float.Epsilon)
            {
                _shakeRemain = 0;
                position = originPosition;
                transform.localEulerAngles = Vector3.zero;
            }
        }
    }

    public void Shake(float magnitude, float duration)
    {
        if (_shakeRemain > 0) position = originPosition;

        originPosition = position;

        _shakeDuration = duration;
        _shakeRemain = magnitude;
        _shakeTime = 0;

        if (_shakeRotateSide == 0) _shakeRotateSide = Random.value > 0.5f ? -1 : 1;
    }
}
