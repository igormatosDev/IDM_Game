using Cinemachine.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;


public static class CommonAnimations
{

    public static IEnumerator FadeOut(SpriteRenderer renderer, float fadeSpeed, float duration)
    {
        Color minColor = new Color(255f / 255f, 255f / 255f, 255f / 255f);
        Color maxColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 0);
        Color endColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 0);
        float currentInterval = 0;
        while (duration > 0)
        {
            float tColor = currentInterval / fadeSpeed;
            renderer.color = Color.Lerp(minColor, maxColor, tColor);
            currentInterval += Time.deltaTime;
            duration -= Time.deltaTime;
            yield return null;
        }

        renderer.color = endColor;
    }
}
