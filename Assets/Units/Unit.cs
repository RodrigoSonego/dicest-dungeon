using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;

    [SerializeField] private int maxHp;
    [SerializeField] private int currentHp;

    [SerializeField] private Dice[] dices;
}
