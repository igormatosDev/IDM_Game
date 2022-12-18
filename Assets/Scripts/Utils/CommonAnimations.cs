using Cinemachine.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;


public static class CommonAnimations
{
    #nullable enable

    public static IEnumerator FadeOut(SpriteRenderer renderer, float fadeOut, float duration, Action ?CallbackMethod)
    {

        Color minColor = new Color(255f / 255f, 255f / 255f, 255f / 255f);
        Color maxColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 0);
        Color endColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 0);
        float currentInterval = 0;
        while (duration > 0)
        {
            float tColor = currentInterval / fadeOut;
            renderer.color = Color.Lerp(minColor, maxColor, tColor);
            currentInterval += Time.deltaTime;
            duration -= Time.deltaTime;
            yield return null;
        }

        renderer.color = endColor;
        if(CallbackMethod != null)
            CallbackMethod();

    }

    public static IEnumerator FlashSprite(SpriteRenderer renderer, Color minColor,  Color maxColor,  Color endColor,  
                                          float interval, float duration)
    {
        float currentInterval = 0;
        while (duration > 0)
        {
            float tColor = currentInterval / interval;
            renderer.color = Color.Lerp(minColor, maxColor, tColor);
            currentInterval += Time.deltaTime;

            if (currentInterval >= interval)
            {
                Color temp = minColor;
                minColor = maxColor;
                maxColor = temp;
                currentInterval = currentInterval - interval;
            }
            duration -= Time.deltaTime;
            yield return null;
        }

        renderer.color = endColor;
    }

    public static IEnumerator PerformKnockback(Transform objectTransform, Vector3 direction, float KnockbackForce, float duration)
    {
        //while (duration > 0)
        //{
        //    objectTransform.position = Vector2.MoveTowards(objectTransform.position, objectTransform.position + direction, KnockbackForce * Time.deltaTime);
        //    duration -= Time.deltaTime;
        //    yield return null;
        //}

        objectTransform.position = Vector2.MoveTowards(objectTransform.position, objectTransform.position + direction, KnockbackForce * Time.deltaTime); ;
        yield return null;
    }
}
