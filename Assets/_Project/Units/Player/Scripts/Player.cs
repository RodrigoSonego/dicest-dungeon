using UnityEngine;

public class Player : Unit
{
    private PlayerMovement movementController;
    [SerializeField] private Animator animator;

    public static Player instance;

    void OnEnable()
    {
        instance = this;   
        movementController = GetComponent<PlayerMovement>();
    }

    public int RollDice(Dice dice)
    {
        if(dices.Contains(dice) == false) { print("não tem o dado"); return -1; }

        int diceValue = dice.Roll();
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
}
