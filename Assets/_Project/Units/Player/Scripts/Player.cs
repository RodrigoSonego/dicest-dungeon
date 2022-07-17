public class Player : Unit
{
    private PlayerMovement movementController;

    void Start()
    {
        movementController = GetComponent<PlayerMovement>();
    }

    public int RollDice(Dice dice)
    {
        if(dices.Contains(dice) == false) { print("não tem o dado"); return -1; }

        int diceValue = dice.Roll();
        dices.Remove(dice);

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
}
