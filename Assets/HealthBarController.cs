using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    public Transform healthBarFg;
    private SpriteRenderer healthBarFgSpriteRenderer;
    private Color fullHealthColor, halfHealthColor, lowHealthColor;

    private void Start()
    {
        healthBarFgSpriteRenderer = healthBarFg.GetComponent<SpriteRenderer>();
        lowHealthColor = Helpers.GetColorHex("#FF5862");
        halfHealthColor = Helpers.GetColorHex("#FFE058");
        fullHealthColor = Helpers.GetColorHex("#58FF99");
    }


    public void manageHealthBar(int health, int maxHealth)
    {
        if (health == maxHealth || health <= 0)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(true);

            float lifePercentage = (float)health / (float)maxHealth;
            lifePercentage = lifePercentage > 0 ? lifePercentage : 0;

            Color color;
            if(lifePercentage < 0.33f)
            {
                color = lowHealthColor;
            }else if(lifePercentage < 0.66f)
            {
                color = halfHealthColor;
            }
            else
            {
                color = fullHealthColor;
            }

            if(healthBarFgSpriteRenderer != null)
            {
                healthBarFgSpriteRenderer.color = color;
            }
            Vector3 newScale = new Vector3(lifePercentage, healthBarFg.transform.localScale.y, healthBarFg.transform.localScale.z);
            healthBarFg.transform.localScale = Vector3.Lerp(healthBarFg.localScale, newScale, .06f);
        }
    }
}
