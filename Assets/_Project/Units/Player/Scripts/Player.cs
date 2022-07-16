public class Player : Unit
{
    public int RollDice(Dice dice)
    {
        if(dices.Contains(dice) == false) { print("não tem o dado"); return -1; }

        int diceValue = dice.Roll();
        dices.Remove(dice);

        return diceValue;
    }
}
