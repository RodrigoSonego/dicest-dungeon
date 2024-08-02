using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Enemy : Unit, IPointerClickHandler
{
    public bool isSelectable = false;

    private EnemySprite sprite;

    public Action<Enemy> onEnemyClicked;

    [SerializeField] private Slider healthBar;
    [SerializeField] private Animator animator;

    void OnEnable()
    {
        sprite = GetComponentInChildren<EnemySprite>();
        healthBar.maxValue = MaxHp;
        healthBar.value = MaxHp;
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        healthBar.value = currentHp;
        
        if (isDead)
        {
            FadeOut();
        }
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

    public (int, Dice) RollDice()
    {
        int randomIndex = UnityEngine.Random.Range(0, dices.Count - 1);
        Dice dice = dices[randomIndex];
		
        int diceValue = dice.Roll();

        animator.Play("attack");

        return (diceValue, dice);
    }

    public void FadeOut()
    {
        sprite.FadeOut();
        FadeHealthBar();

        sprite.DisableOutline();
        Destroy(gameObject, 2f);
    }


    private void FadeHealthBar()
    {
        foreach (var graphic in healthBar.GetComponentsInChildren<Graphic>())
        {
            StartCoroutine(LerpMovement.LerpOpacity(graphic, 0, 1.5f));
        }
    }
}
