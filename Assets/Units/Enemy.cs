using UnityEngine;

public class Enemy : Unit
{
    public int RollDice()
    {
        int randomIndex = Random.Range(0, dices.Count - 1);
        int diceValue = dices[randomIndex].Roll();

        dices.RemoveAt(randomIndex);

        return diceValue;
    }   
}
