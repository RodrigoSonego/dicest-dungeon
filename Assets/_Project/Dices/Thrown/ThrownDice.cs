using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownDice : MonoBehaviour
{
    private SpriteRenderer sprite;

	private void OnEnable()
	{
		sprite = GetComponent<SpriteRenderer>();
	}

    public IEnumerator LerpDiceTo(Transform origin, Transform target, float duration)
    {
        gameObject.SetActive(true);

        transform.position = origin.position;
        yield return StartCoroutine(LerpMovement.LerpToPosition(transform, target.position, duration));

        gameObject.SetActive(false);
    }

    public void FlipSprite(bool flip)
    {
        sprite.flipX = flip;
    }

    public void ChangeColor(Color color)
    {
        sprite.color = color;
    }
}
