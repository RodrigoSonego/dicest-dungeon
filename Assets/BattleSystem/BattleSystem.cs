using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState
{
    Start,
    PlayerTurn,
    EnemyTurn,
    Won,
    Lost
}

public class BattleSystem : MonoBehaviour
{
    // serializing for debugging only
    [SerializeField] private BattleState currentState;

    [SerializeField] private Dice debugDiceLmao;

    [SerializeField] private Unit[] enemyUnits;
    [SerializeField] private Unit playerUnit;

    void Start()
    {
        StartCoroutine(StartBattle());
    }

    IEnumerator StartBattle()
    {
        currentState = BattleState.Start;

        yield return new WaitForSeconds(2);

        currentState = BattleState.PlayerTurn;
        StartPlayerTurn();
    }

    void StartPlayerTurn()
    {
        print("turno do player");
    }

    IEnumerator PlayerAttack(Dice dice)
    {
        int damage = dice.Roll();

        enemyUnits[0].TakeDamage(damage);

        print("player atacou o inimigo e deu " + damage + " de dano");
        print(enemyUnits[0].isDead ? "matou o bicho zé" : "n matou");

        yield return new WaitForSeconds(2);

       if(enemyUnits[0].isDead && HaveAllEnemiesDied())
       {
            currentState = BattleState.Won;
            EndBattle();
       } 
       else
       {
            currentState = BattleState.EnemyTurn;
            StartCoroutine(EnemyTurn());
       }
    }

    private IEnumerator EnemyTurn()
    {
        print("turno do inimigo");

        int randomIndex = Random.Range(0, enemyUnits[0].dices.Count);
        int damage = enemyUnits[0].dices[randomIndex].Roll();

        playerUnit.TakeDamage(damage);

        print("atacou o player e deu " + damage + " de dano");

        yield return new WaitForSeconds(2);

        if(playerUnit.isDead)
        {
            currentState = BattleState.Lost;
            EndBattle();
        }
        else
        {
            currentState = BattleState.PlayerTurn;
            StartPlayerTurn();
        }

    }

    private void EndBattle()
    {
        if(currentState == BattleState.Won)
        {
            print("gañaste");
        }
        else if (currentState == BattleState.Lost)
        {
            print("Congratulations! You're a failure");
        }
    }

    bool HaveAllEnemiesDied()
    {
        bool areAllDead = true;

        foreach (Unit enemy in enemyUnits)
        {
            areAllDead = areAllDead && enemy.isDead;
        }

        return areAllDead;
    }

    public void OnDiceClicked()
    {
        if(currentState != BattleState.PlayerTurn) { return; }

        StartCoroutine(PlayerAttack(debugDiceLmao));
    }
}
