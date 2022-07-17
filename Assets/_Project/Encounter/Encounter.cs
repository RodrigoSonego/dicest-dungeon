using UnityEngine;

[CreateAssetMenu(fileName = "Encounter", menuName = "Encounter")]
public class Encounter : ScriptableObject
{
    [SerializeField] public Enemy[] enemies;
    [SerializeField] private int chipReward;
    [SerializeField] public Dice[] diceReward;

    public bool isCompleted;
}
