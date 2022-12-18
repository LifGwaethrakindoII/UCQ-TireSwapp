using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ulyssess
{
public static class Extensions
{
    public static Coroutine StartCoroutine(this MonoBehaviour _mono, IEnumerator _enumerator, ref Coroutine _coroutine)
    {
        if (_coroutine != null)
        {
            _mono.StopCoroutine(_coroutine);
            _coroutine = null;
        }
        return _coroutine = _mono.StartCoroutine(_enumerator);
    }

    public static void DispatchCoroutine(this MonoBehaviour _mono, ref Coroutine _coroutine)
    {
        if (_coroutine != null)
        {
            _mono.StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }

    /// low2 + (value - low1) * (high2 - low2) / (high1 - low1)
    /// <summary>Remaps Value from a range into a target range.</summary>
    /// <param name="x">Value to remap.</param>
    /// <param name="aMin">Value's range minimum value.</param>
    /// <param name="aMax">Value's range maximum value.</param>
    /// <param name="bMin">Target's range minimum value.</param>
    /// <param name="bMax">Target's range maximum value.</param>
    /// <returns>Remapped Value.</returns>
    public static float Remap(float x, float aMin, float aMax, float bMin, float bMax)
    {
        return (((x - aMin) * (bMax - bMin)) / (aMax - aMin)) + bMin;
    }

    public static int Log(int x, int b = 2)
    {
        int count = 0;

        while(x > (b - 1))
        {
            x /= b;
            count++;
        }

        return count;
    }

    public static bool IsOnLayerMask(int layer, LayerMask mask)
    {
        return Log(mask.value) == layer;
    }
}
}