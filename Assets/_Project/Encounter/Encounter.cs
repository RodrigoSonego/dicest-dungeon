using UnityEngine;

[CreateAssetMenu(fileName = "Encounter", menuName = "Encounter")]
public class Encounter : ScriptableObject
{
    [SerializeField] public Enemy[] enemies;
    [SerializeField] private int chipReward;
    [SerializeField] private Dice[] diceReward;

    public bool isCompleted;
}
