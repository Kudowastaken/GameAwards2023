using Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }
    private CinemachineBasicMultiChannelPerlin _cinemachineShake;

    private float shakeTimer;
    private float startingIntensity;
    private float shakeTimeTotal;

    private void OnEnable()
    {
        Instance = this;
        _cinemachineShake = FindObjectOfType<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCamera(float intensity, float time)
    {
        Debug.Log("ShakeCamera got called");
        _cinemachineShake.m_AmplitudeGain = intensity;

        startingIntensity = intensity;
        shakeTimer = time;
        shakeTimeTotal = time;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            _cinemachineShake.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, 1 - (shakeTimer / shakeTimeTotal));
        }
    }
}
