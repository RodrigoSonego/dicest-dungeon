using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LerpMovement : MonoBehaviour
{
    public static IEnumerator LerpToPosition(Transform current, Vector3 targetPosition, float time)
    {
        float elapsedTime = 0;
        var currentPos = current.position;

        while (elapsedTime < time)
        {
            current.position = Vector3.Lerp(currentPos, targetPosition, (elapsedTime / time));
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        yield return null;
    }

    public static IEnumerator LerpOpacity(Graphic graphic, float opacity, float time)
    {
        float elapsedTime = 0;
        var currentColor = graphic.color;
        var targetColor = new Color(currentColor.r, currentColor.g, currentColor.b, opacity);

        while (elapsedTime < time)
        {
            graphic.color = Color.Lerp(currentColor, targetColor, elapsedTime / time);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        yield return null;
    }

    public static IEnumerator LerpOpacity(SpriteRenderer spriteRenderer, float opacity, float time)
    {
        float elapsedTime = 0;
        var currentColor = spriteRenderer.color;
        var targetColor = new Color(currentColor.r, currentColor.g, currentColor.b, opacity);

        while (elapsedTime < time)
        {
            spriteRenderer.color = Color.Lerp(currentColor, targetColor, elapsedTime / time);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        yield return null;
    }
}
