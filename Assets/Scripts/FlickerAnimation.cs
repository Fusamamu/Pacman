using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerAnimation : MonoBehaviour
{
    public bool isAnimated = false;

    public Color OriginColor;

    public float current_R;
    public float current_G;
    public float current_B;
    public float currentAlpha;

    public float increment = 0.1f;

    public float FLICKER_DURATION = 4f;

    private void Start()
    {
        OriginColor = GetComponent<SpriteRenderer>().color;
        currentAlpha = OriginColor.a;
    }

    private void Update()
    {
        if (isAnimated && FLICKER_DURATION > 0)
        {

            SetColorBackNForth(ref current_R);
            SetColorBackNForth(ref current_G);
            SetColorBackNForth(ref current_B);
            SetAlpahBackNForth();

            Color newColor = new Color(current_R, current_G, current_B, currentAlpha);

            GetComponent<SpriteRenderer>().color = newColor;
            GetComponent<Collider2D>().enabled = false;

            FLICKER_DURATION -= Time.deltaTime;
        }
        else
        {
            isAnimated = false;
            FLICKER_DURATION = 4f;

            GetComponent<SpriteRenderer>().color = OriginColor;
            GetComponent<Collider2D>().enabled = true;
        }
    }

    private void SetAlpahBackNForth()
    {
        if (currentAlpha >= 1.0f)
            increment = -0.05f;

        if (currentAlpha <= 0.5f)
            increment = 0.05f;

        currentAlpha += increment;
    }

    private void SetColorBackNForth(ref float _color)
    {
        if (_color >= 1.0f)
            increment = -0.05f;

        if (_color <= 0.5f)
            increment = 0.05f;

        _color += increment;
    }

    public void SetAnimation(bool On)
    {
        isAnimated = On;
    }
}
