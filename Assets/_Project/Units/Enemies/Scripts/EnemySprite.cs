using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemySprite : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private const string outlineEnableProperty = "_OutlineEnabled";

    void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.material.SetFloat(outlineEnableProperty, 0);
    }

    public void DisableOutline()
    {
        spriteRenderer.material.SetFloat(outlineEnableProperty, 0);
    }

    public void EnableOutline()
    {
        spriteRenderer.material.SetFloat(outlineEnableProperty, 1);
    }

    public void FadeOut()
    {
        StartCoroutine(LerpMovement.LerpOpacity(spriteRenderer, 0, 1.5f));
        Destroy(gameObject, 1.5f);
    }
}
