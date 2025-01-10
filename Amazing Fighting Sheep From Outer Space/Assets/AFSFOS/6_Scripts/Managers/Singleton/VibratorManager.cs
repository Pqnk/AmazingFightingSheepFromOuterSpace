using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VibratorManager : MonoBehaviour
{
    private Gamepad pad;

    private Coroutine stopRumbleAdterTimeCoroutine;

    public void RumblePulse(float lowFrequency, float highFrequency, float duration)
    {
        pad = Gamepad.current;

        if (pad != null)
        {
            pad.SetMotorSpeeds(lowFrequency, highFrequency);
            stopRumbleAdterTimeCoroutine = StartCoroutine(StopRumble(duration, pad));
        }
    }


    private IEnumerator StopRumble(float duration, Gamepad pad)
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        pad.SetMotorSpeeds(0.0f, 0.0f);
    }
}
