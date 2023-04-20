using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AnimationEvents : MonoBehaviour
{
    [SerializeField] float intensity;
    [SerializeField] float time;

    public void CameraShakeAnimEvent()
    {
        CameraShake.Instance.ShakeCamera(intensity, time);
    }
}