using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public string unitName;

    [SerializeField] public int MaxHp { get { return maxHp; } }
    [SerializeField] protected int maxHp;

    [SerializeField] public int CurrentHp { get { return currentHp; } }
    [SerializeField] protected int currentHp;
    public bool isDead { get; private set; }

    public float attackAnimationWindUpTime;
    public Transform diceSpawnPosition;
    [SerializeField] public List<Dice> dices; 

    public virtual void TakeDamage(int damage)
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
