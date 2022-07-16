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

    [SerializeField] private Enemy[] enemyUnits;
    [SerializeField] private Player playerUnit;

    public static BattleSystem instance { get; private set; }

    void Start()
    {
        instance = this;

        StartCoroutine(StartBattle());
    }

    IEnumerator StartBattle()
    {
        currentState = BattleState.Start;

        BattleUI.instance.SetDiceButtons(playerUnit.dices);

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
        int damage = playerUnit.RollDice(dice);

        enemyUnits[0].TakeDamage(damage);

        print("player atacou o inimigo com um "+ dice.diceName + " e deu " + damage + " de dano");
        print(enemyUnits[0].isDead ? "matou o bicho zé" : "n matou");

        yield return new WaitForSeconds(1.5f);

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
        foreach (Enemy enemy in enemyUnits)
        {
            print("turno do inimigo " + enemy.name);
            int damage = enemy.RollDice();

            playerUnit.TakeDamage(damage);

            print("atacou o player e deu " + damage + " de dano");

            yield return new WaitForSeconds(1.5f);

            if(playerUnit.isDead)
            {
                currentState = BattleState.Lost;
                EndBattle();

                yield break;
            }

        }
        
        currentState = BattleState.PlayerTurn;
        StartPlayerTurn();

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

    public void OnDiceClicked(Dice dice)
    {
        if(currentState != BattleState.PlayerTurn) { return; }

        StartCoroutine(PlayerAttack(dice));
    }
}
