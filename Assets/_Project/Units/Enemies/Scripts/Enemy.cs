using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy : Unit, IPointerClickHandler
{
    public bool isSelectable = false;

    private EnemySprite sprite;

    public Action<Enemy> onEnemyClicked;

    void OnEnable()
    {
        sprite = GetComponentInChildren<EnemySprite>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isSelectable == false) { return; }

        onEnemyClicked(this);
    }

    public void DisableOutline()
    {
        sprite.DisableOutline();
    }

    public void EnableOutline()
    {
        sprite.EnableOutline();
    }

    public int RollDice()
    {
        int randomIndex = UnityEngine.Random.Range(0, dices.Count - 1);
        int diceValue = dices[randomIndex].Roll();

        dices.RemoveAt(randomIndex);

        return diceValue;
    }

    public void FadeOut()
    {
        sprite.FadeOut();
    }
}
