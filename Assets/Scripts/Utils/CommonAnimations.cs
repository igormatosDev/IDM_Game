using Cinemachine.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
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


    public static IEnumerator BlinkSprite(SpriteRenderer renderer, Color blinkColor, float interval, float duration)
    {
        Shader shaderGUItext = Shader.Find("GUI/Text Shader");
        Shader shaderDefault = renderer.material.shader;
        Color defaultColor = renderer.color;

        float currentInterval = 0;
        float lastSplit = 0f;
        bool split = true;

        while (duration > 0)
        {
            currentInterval += Time.deltaTime;

            if (split)
            {
                lastSplit = currentInterval;
                split = false;
                
                if (renderer.material.shader == shaderDefault)
                {
                    renderer.material.shader = shaderGUItext;
                    renderer.color = blinkColor;
                }
                else
                {
                    renderer.material.shader = shaderDefault;
                    renderer.color = defaultColor;
                }
            }
            if (currentInterval - lastSplit > interval)
            {
                // if the time that passes is bigger then split time
                split = true;
            }
            duration -= Time.deltaTime;
            yield return null;
        }

        renderer.color = defaultColor;
        renderer.material.shader = shaderDefault;
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
