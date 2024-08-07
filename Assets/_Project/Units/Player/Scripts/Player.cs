using System.Collections;
using UnityEngine;

public class Player : Unit
{
    private PlayerMovement movementController;
    [SerializeField] private Animator animator;
    [SerializeField] private float fadeOutTime;

    public static Player instance;

    void OnEnable()
    {
        instance = this;   
        movementController = GetComponent<PlayerMovement>();
    }

    public int RollDice(Dice dice)
    {
        if(dices.Contains(dice) == false) { print("n�o tem o dado"); return -1; }

        int diceValue = dice.Roll();

        if (diceValue == 1)
        {
            diceValue = dice.Roll();
        }

        dices.Remove(dice);

        animator.Play("attack");

        return diceValue;
    }

    public void DisableCollisionAndMovement()
    {
        movementController.DisableCollisionAndMovement();
    }

    public void EnableCollisionAndMovement()
    {
        movementController.EnableCollisionAndMovement();
    }

    public void FullyHeal()
    {
        currentHp = MaxHp;
    }

    public void SetAnimatorWalk(bool isWalking)
    {
        animator.SetBool("walking", isWalking);
    }

    public IEnumerator FadeOut()
    {
        SpriteRenderer sprite = animator.GetComponent<SpriteRenderer>();

        yield return StartCoroutine(LerpMovement.LerpOpacity(sprite, 0, fadeOutTime));
    }
}
