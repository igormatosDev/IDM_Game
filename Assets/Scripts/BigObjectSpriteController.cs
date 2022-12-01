using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BigObjectSpriteController : MonoBehaviour
{
    public SpriteRenderer PlayerSpriteRenderer;
    public float animationDuration;
    public Color startColor = new Color(255f / 255f, 255f / 255f, 255f / 255f);
    public Color endColor = new Color(100f / 255f, 100f / 255f, 100f / 255f, 250f / 255f);

    private SpriteRenderer ObjectSpriteRenderer;

    private void Start()
    {
        ObjectSpriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "SpriteController")
        {
            StartCoroutine(ToggleSprite(ObjectSpriteRenderer, startColor, endColor, animationDuration));
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "SpriteController")
        {
            StartCoroutine(ToggleSprite(ObjectSpriteRenderer, ObjectSpriteRenderer.color, startColor, animationDuration));
        }
    }

    public static IEnumerator ToggleSprite(SpriteRenderer renderer, Color startColor, Color endColor, float duration)
    {
        float currentInterval = 0;

        while (duration > 0)
        {
            renderer.color = Color.Lerp(startColor, endColor, currentInterval / duration);
            currentInterval += Time.deltaTime;
            duration -= Time.deltaTime;
            yield return null;
        }
    }
}
