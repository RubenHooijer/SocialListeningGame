using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// This class is based on the Easing Equations webpage by Robert Penner on 10-01-2014
/// Link to page http://www.gizma.com/easing
/// 
/// Authors: Joris van Leeuwen & Marijn Zwemmer
/// Date: 10-01-2014
/// Company: Little Chicken Game Company
/// </summary>

public enum EasingType {
    EaseLinear,
    EaseInQuad, EaseOutQuad, EaseInOutQuad,
    EaseInSine, EaseOutSine, EaseInOutSine,
    EaseInExpo, EaseOutExpo, EaseInOutExpo,
    EaseInCirc, EaseOutCirc, EaseInOutCirc,
    EaseInCubic, EaseOutCubic, EaseInOutCubic,
    EaseInQuart, EaseOutQuart, EaseInOutQuart,
    EaseInQuint, EaseOutQuint, EaseInOutQuint,
};

public static class EasingHelper {

    //---- Private Constants ----//
    const string customEasingsPath = "ScriptableObjects/CustomEasings/";
    const string fullCustomEasingsPath = "Assets/Resources/" + customEasingsPath;

    //---- Private Static Fields ----//
    static Dictionary<EasingType, Func<float, float, float, float, float>> easings = new Dictionary<EasingType, Func<float, float, float, float, float>>();
    static Dictionary<string, AnimationCurve> customEasings = new Dictionary<string, AnimationCurve>();

    //---- Public Static Methods ----//
    /// <summary>
    /// Ease the progress through the given type
    /// </summary>
    /// <param name="type">The EasingType to ease over</param>
    /// <param name="progress">The progress (0 - 1)</param>
    /// <returns></returns>
    public static float Ease(EasingType type, float progress) {
       return Ease(type, progress, 0.0f, 1.0f, 1.0f);
    }

    /// <summary>
    /// Ease the given values through the given type
    /// </summary>
    /// <param name="type">The EasingType to ease over</param>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns></returns>
    public static float Ease(EasingType type, float t, float b, float c, float d) {
        if (easings.Count == 0) {
            MapEaseMethods();
        }
        return easings[type](t, b, c, d);
    }

    #region Default Easings

    #region Linear Easing
    public static float EaseLinear(float t, float b, float c, float d) {
        return c * t / d + b;
    }
    #endregion



    #region Quadratic Easings
    /// <summary>
    /// Returns an eased value based on the input
    /// </summary>
    /// <param name="progress">Input factor with a region of 0.0f to 1.0f</param>
    public static float EaseInQuad(float progress) {
        return EaseInQuad(progress, 0.0f, 1.0f, 1.0f);
    }
    /// <summary>
    /// Returns an eased value based on the input
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    public static float EaseInQuad(float t, float b, float c, float d) {
        t /= d;
        return c * t * t + b;
    }

    /// <summary>
    /// Returns an eased value based on the input
    /// </summary>
    /// <param name="progress">Input factor with a region of 0.0f to 1.0f</param>
    public static float EaseOutQuad(float progress) {
        return EaseOutQuad(progress, 0.0f, 1.0f, 1.0f);
    }
    /// <summary>
    /// Returns an eased value based on the input
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    public static float EaseOutQuad(float t, float b, float c, float d) {
        t /= d;
        return -c * t * (t - 2) + b;
    }

    /// <summary>
    /// Returns an eased value based on the input
    /// </summary>
    /// <param name="progress">Input factor with a region of 0.0f to 1.0f</param>
    public static float EaseInOutQuad(float progress) {
        return EaseInOutQuad(progress, 0.0f, 1.0f, 1.0f);
    }
    /// <summary>
    /// Returns an eased value based on the input
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    public static float EaseInOutQuad(float t, float b, float c, float d) {
        t /= d / 2;
        if (t < 1) {
            return c / 2 * t * t + b;
        }
        t--;
        return -c / 2 * (t * (t - 2) - 1) + b;
    }
    #endregion



    #region Sinusoidal Easings
    /// <summary>
    /// Returns an eased value based on the input
    /// </summary>
    /// <param name="progress">Input factor with a region of 0.0f to 1.0f</param>
    public static float EaseInSine(float progress) {
        return EaseInSine(progress, 0.0f, 1.0f, 1.0f);
    }
    /// <summary>
    /// Returns an eased value based on the input
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    public static float EaseInSine(float t, float b, float c, float d) {
        return -c * Mathf.Cos(t / d * (Mathf.PI / 2)) + c + b;
    }

    /// <summary>
    /// Returns an eased value based on the input
    /// </summary>
    /// <param name="progress">Input factor with a region of 0.0f to 1.0f</param>
    public static float EaseOutSine(float progress) {
        return EaseOutSine(progress, 0.0f, 1.0f, 1.0f);
    }
    /// <summary>
    /// Returns an eased value based on the input
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    public static float EaseOutSine(float t, float b, float c, float d) {
        return c * Mathf.Sin(t / d * (Mathf.PI / 2)) + b;
    }

    /// <summary>
    /// Returns an eased value based on the input
    /// </summary>
    /// <param name="progress">Input factor with a region of 0.0f to 1.0f</param>
    public static float EaseInOutSine(float progress) {
        return EaseInOutSine(progress, 0.0f, 1.0f, 1.0f);
    }
    /// <summary>
    /// Returns an eased value based on the input
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    public static float EaseInOutSine(float t, float b, float c, float d) {
        return -c / 2 * (Mathf.Cos(Mathf.PI * t / d) - 1) + b;
    }
    #endregion



    #region Exponential Easings
    /// <summary>
    /// Returns an eased value based on the input
    /// </summary>
    /// <param name="progress">Input factor with a region of 0.0f to 1.0f</param>
    public static float EaseInExpo(float progress) {
        return EaseInExpo(progress, 0.0f, 1.0f, 1.0f);
    }
    /// <summary>
    /// Returns an eased value based on the input
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    public static float EaseInExpo(float t, float b, float c, float d) {
        return c * Mathf.Pow(2, 10 * (t / d - 1)) + b;
    }

    /// <summary>
    /// Returns an eased value based on the input
    /// </summary>
    /// <param name="progress">Input factor with a region of 0.0f to 1.0f</param>
    public static float EaseOutExpo(float progress) {
        return EaseOutExpo(progress, 0.0f, 1.0f, 1.0f);
    }
    /// <summary>
    /// Returns an eased value based on the input
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    public static float EaseOutExpo(float t, float b, float c, float d) {
        return c * (-Mathf.Pow(2, -10 * t / d) + 1) + b;
    }

    /// <summary>
    /// Returns an eased value based on the input
    /// </summary>
    /// <param name="progress">Input factor with a region of 0.0f to 1.0f</param>
    public static float EaseInOutExpo(float progress) {
        return EaseInOutExpo(progress, 0.0f, 1.0f, 1.0f);
    }
    /// <summary>
    /// Returns an eased value based on the input
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    public static float EaseInOutExpo(float t, float b, float c, float d) {
        t /= d / 2;
        if (t < 1)
            return c / 2 * Mathf.Pow(2, 10 * (t - 1)) + b;
        t--;
        return c / 2 * (-Mathf.Pow(2, -10 * t) + 2) + b;
    }
    #endregion



    #region Circular Easings
    /// <summary>
    /// Returns an eased value based on the input
    /// </summary>
    /// <param name="progress">Input factor with a region of 0.0f to 1.0f</param>
    public static float EaseInCirc(float progress) {
        return EaseInCirc(progress, 0.0f, 1.0f, 1.0f);
    }
    /// <summary>
    /// Returns an eased value based on the input
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    public static float EaseInCirc(float t, float b, float c, float d) {
        t /= d;
        return -c * (Mathf.Sqrt(1 - t * t) - 1) + b;
    }

    /// <summary>
    /// Returns an eased value based on the input
    /// </summary>
    /// <param name="progress">Input factor with a region of 0.0f to 1.0f</param>
    public static float EaseOutCirc(float progress) {
        return EaseOutCirc(progress, 0.0f, 1.0f, 1.0f);
    }
    /// <summary>
    /// Returns an eased value based on the input
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    public static float EaseOutCirc(float t, float b, float c, float d) {
        t /= d;
        t--;
        return c * Mathf.Sqrt(1 - t * t) + b;
    }

    /// <summary>
    /// Returns an eased value based on the input
    /// </summary>
    /// <param name="progress">Input factor with a region of 0.0f to 1.0f</param>
    public static float EaseInOutCirc(float progress) {
        return EaseInOutCirc(progress, 0.0f, 1.0f, 1.0f);
    }
    /// <summary>
    /// Returns an eased value based on the input
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    public static float EaseInOutCirc(float t, float b, float c, float d) {
        t /= d / 2;
        if (t < 1)
            return -c / 2 * (Mathf.Sqrt(1 - t * t) - 1) + b;
        t -= 2;
        return c / 2 * (Mathf.Sqrt(1 - t * t) + 1) + b;
    }
    #endregion



    #region Cubic Easings
    /// <summary>
    /// Returns an eased value based on the input
    /// </summary>
    /// <param name="progress">Input factor with a region of 0.0f to 1.0f</param>
    public static float EaseInCubic(float progress) {
        return EaseInCubic(progress, 0.0f, 1.0f, 1.0f);
    }
    /// <summary>
    /// Returns an eased value based on the input
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    public static float EaseInCubic(float t, float b, float c, float d) {
        t /= d;
        return c * t * t * t + b;
    }

    /// <summary>
    /// Returns an eased value based on the input
    /// </summary>
    /// <param name="progress">Input factor with a region of 0.0f to 1.0f</param>
    public static float EaseOutCubic(float progress) {
        return EaseOutCubic(progress, 0.0f, 1.0f, 1.0f);
    }
    /// <summary>
    /// Returns an eased value based on the input
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    public static float EaseOutCubic(float t, float b, float c, float d) {
        t /= d;
        t--;
        return c * (t * t * t + 1) + b;
    }

    /// <summary>
    /// Returns an eased value based on the input
    /// </summary>
    /// <param name="progress">Input factor with a region of 0.0f to 1.0f</param>
    public static float EaseInOutCubic(float progress) {
        return EaseInOutCubic(progress, 0.0f, 1.0f, 1.0f);
    }
    /// <summary>
    /// Returns an eased value based on the input
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    public static float EaseInOutCubic(float t, float b, float c, float d) {
        t /= d / 2;
        if (t < 1)
            return c / 2 * t * t * t + b;
        t -= 2;
        return c / 2 * (t * t * t + 2) + b;
    }
    #endregion



    #region Quartic Easings
    /// <summary>
    /// Returns an eased value based on the input
    /// </summary>
    /// <param name="progress">Input factor with a region of 0.0f to 1.0f</param>
    public static float EaseInQuart(float progress) {
        return EaseInQuart(progress, 0.0f, 1.0f, 1.0f);
    }
    /// <summary>
    /// Returns an eased value based on the input
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    public static float EaseInQuart(float t, float b, float c, float d) {
        t /= d;
        return c * t * t * t * t + b;
    }

    /// <summary>
    /// Returns an eased value based on the input
    /// </summary>
    /// <param name="progress">Input factor with a region of 0.0f to 1.0f</param>
    public static float EaseOutQuart(float progress) {
        return EaseOutQuart(progress, 0.0f, 1.0f, 1.0f);
    }
    /// <summary>
    /// Returns an eased value based on the input
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    public static float EaseOutQuart(float t, float b, float c, float d) {
        t /= d;
        t--;
        return -c * (t * t * t * t - 1) + b;
    }

    /// <summary>
    /// Returns an eased value based on the input
    /// </summary>
    /// <param name="progress">Input factor with a region of 0.0f to 1.0f</param>
    public static float EaseInOutQuart(float progress) {
        return EaseInOutQuart(progress, 0.0f, 1.0f, 1.0f);
    }
    /// <summary>
    /// Returns an eased value based on the input
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    public static float EaseInOutQuart(float t, float b, float c, float d) {
        t /= d / 2;
        if (t < 1)
            return c / 2 * t * t * t * t + b;
        t -= 2;
        return -c / 2 * (t * t * t * t - 2) + b;
    }
    #endregion



    #region Quintic Easings
    /// <summary>
    /// Returns an eased value based on the input
    /// </summary>
    /// <param name="progress">Input factor with a region of 0.0f to 1.0f</param>
    public static float EaseInQuint(float progress) {
        return EaseInQuint(progress, 0.0f, 1.0f, 1.0f);
    }
    /// <summary>
    /// Returns an eased value based on the input
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    public static float EaseInQuint(float t, float b, float c, float d) {
        t /= d;
        return c * t * t * t * t * t + b;
    }

    /// <summary>
    /// Returns an eased value based on the input
    /// </summary>
    /// <param name="progress">Input factor with a region of 0.0f to 1.0f</param>
    public static float EaseOutQuint(float progress) {
        return EaseOutQuint(progress, 0.0f, 1.0f, 1.0f);
    }
    /// <summary>
    /// Returns an eased value based on the input
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    public static float EaseOutQuint(float t, float b, float c, float d) {
        t /= d;
        t--;
        return c * (t * t * t * t * t + 1) + b;
    }

    /// <summary>
    /// Returns an eased value based on the input
    /// </summary>
    /// <param name="progress">Input factor with a region of 0.0f to 1.0f</param>
    public static float EaseInOutQuint(float progress) {
        return EaseInOutQuint(progress, 0.0f, 1.0f, 1.0f);
    }
    /// <summary>
    /// Returns an eased value based on the input
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    public static float EaseInOutQuint(float t, float b, float c, float d) {
        t /= d / 2;
        if (t < 1)
            return c / 2 * t * t * t * t * t + b;
        t -= 2;
        return c / 2 * (t * t * t * t * t + 2) + b;
    }
    #endregion

    #endregion


    //---- Private Static Methods ----//
    /// <summary> Connects all ease methods to enums in a dictionary </summary>
    static void MapEaseMethods() {
        IEnumerator values = Enum.GetValues(typeof(EasingType)).GetEnumerator();
        while (values.MoveNext()) {
            MethodInfo method = typeof(EasingHelper).GetMethod(values.Current.ToString(), new Type[] { typeof(float), typeof(float), typeof(float), typeof(float) });
            var func = (Func<float, float, float, float, float>)Delegate.CreateDelegate(typeof(Func<float, float, float, float, float>), method);
            easings.Add((EasingType)values.Current, func);
        }
    }

}
