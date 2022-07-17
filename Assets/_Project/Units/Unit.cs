using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public string unitName;

    [SerializeField] private int maxHp;
    [SerializeField] private int currentHp;
    public bool isDead { get; private set; }

    public float attackAnimationWindUpTime;
    public Transform diceSpawnPosition;
    [SerializeField] public List<Dice> dices; 

    public void TakeDamage(int damage)
    {
        currentHp -= damage;

        if (currentHp <= 0)
        {
            Die();
        }

    }

    private void Die()
    {
        isDead = true;
        // Call animations and whatnot
    }

}
