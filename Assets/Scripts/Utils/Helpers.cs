using System.Collections;
using UnityEngine;
using System;

public class Helpers
{
    public static bool isPaused()
    {
        return Time.timeScale == 0;
    }
    
    public static int GetRandomInt(int start, int end)
    {
        System.Random random = new System.Random();
        return random.Next(start, end);
    }
    public static float GetRandomFloat(float start, float end)
    {
        return UnityEngine.Random.Range(start, end);
    }


    public static Vector2 GetRandomDirection(float length=1)
    {
        float x = UnityEngine.Random.Range(-1f, 1f);
        float y = UnityEngine.Random.Range(-1f, 1f);
        Vector2 direction = new Vector3(x, y, 0f);
        direction = direction.normalized * length;
        return direction;
    }

    public static IEnumerator CallActionAfterSec(float seconds, Action callback)
    {
        yield return new WaitForSeconds(seconds);
        callback();
    }


    public static Color GetColorHex(string hex)
    {
        Color color;
        UnityEngine.ColorUtility.TryParseHtmlString(hex, out color);
        return color;
    }

    public static IEnumerator PushGameObject(GameObject gameObject, Vector2 position, float force, float duration)
    {
        while(duration > 0)
        {
            duration -= Time.deltaTime;
            gameObject.transform.position = Vector2.MoveTowards(
                gameObject.transform.position, 
                position, 
                force * Time.deltaTime);
            yield return null;
        }
    }

    public static IEnumerator ThrowGameObject(GameObject gameObject, Vector2 dropPosition, float yInclination, float force, float duration)
    {
        float startDuration = duration;
        while (duration > 0)
        {
            Vector2 _dropPosition = dropPosition;
            if (duration > startDuration * .6f)
            {
                _dropPosition.y += yInclination;
            }
            else if (duration < startDuration * .4f)
            {
                _dropPosition.y -= yInclination;
            }

            duration -= Time.deltaTime;
            gameObject.transform.position = Vector2.Lerp(
                gameObject.transform.position,
                _dropPosition,
                force * Time.deltaTime);
            yield return null;
        }
    }
}
