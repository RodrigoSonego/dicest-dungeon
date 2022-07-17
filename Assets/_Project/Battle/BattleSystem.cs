using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState
{
    Start,
    PlayerTurn,
    EnemyTurn,
    Won,
    Lost,
    None
}

public class BattleSystem : MonoBehaviour
{
    // serializing for debugging only
    [SerializeField] private BattleState currentState;

    [SerializeField] private Dice debugDiceLmao;

    [SerializeField] private Enemy[] enemyUnits;
    [SerializeField] private Player playerUnit;

    private Enemy selectedEnemy = null;
    
    private Dice[] diceReward = null;

    public static BattleSystem instance { get; private set; }

    void Start()
    {
       instance = this;

       //StartCoroutine(StartBattle());
    }

    public IEnumerator StartBattle(Enemy[] enemies, Dice[] diceReward)
    {
        currentState = BattleState.Start;
        enemyUnits = enemies;
        this.diceReward = diceReward;

        PlayerCamera.instance.willFollow = false;

        BattleUI.instance.OnBattleStateChanged(currentState);
        BattleUI.instance.SetDiceButtons(playerUnit.dices);
        BattleUI.instance.OnBattleStart();

        yield return new WaitForSeconds(2);

        SetEnemyClickListenersIfNeeded();

        currentState = BattleState.PlayerTurn;
        StartPlayerTurn();
    }

    private void SetEnemyClickListenersIfNeeded()
    {
        if(enemyUnits.Length > 1)
        {
            foreach (var enemy in enemyUnits)
            {
                enemy.isSelectable = true;
                enemy.onEnemyClicked += SelectEnemy;
            }
        }

        SelectEnemy(enemyUnits[0]);
    }

    void StartPlayerTurn()
    {
        BattleUI.instance.OnBattleStateChanged(currentState);
        BattleUI.instance.EnableDiceButtons();
        BattleUI.instance.OnAttackerChanged(playerUnit.transform.position);
    }

    IEnumerator PlayerAttack(Dice dice)
    {
        if(enemyUnits.Length > 1 && selectedEnemy == null)
        {
            print("no enemy selected");
            yield break;
        }

        int damage = playerUnit.RollDice(dice);

        selectedEnemy.TakeDamage(damage);
        BattleUI.instance.OnDamageDealt(damage, selectedEnemy.transform.position);

        BattleUI.instance.DisableDiceButtons();
        yield return new WaitForSeconds(1.5f);

        if(selectedEnemy.isDead)
        {

            print(HaveAllEnemiesDied() ? "todo mundo morreu": "n morreu todo mundo");
            if (HaveAllEnemiesDied())
            {
                currentState = BattleState.Won;
                selectedEnemy.DisableOutline();

                EndBattle();
                yield break;
            }

            selectedEnemy.isSelectable = false;
            SelectNextEnemy();
        }

        currentState = BattleState.EnemyTurn;
        StartCoroutine(EnemyTurn());     
    }

    private IEnumerator EnemyTurn()
    {
        BattleUI.instance.OnBattleStateChanged(currentState);

        foreach (Enemy enemy in enemyUnits)
        {
            if(enemy.isDead) { continue; }

            BattleUI.instance.OnAttackerChanged(enemy.transform.position);

            int damage = enemy.RollDice();

            playerUnit.TakeDamage(damage);
            BattleUI.instance.OnDamageDealt(damage, playerUnit.transform.position);
            yield return new WaitForSeconds(enemy.attackAnimationWindUpTime);

            yield return new WaitForSeconds(2f);

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
            BattleUI.instance.OnBattleStateChanged(currentState);

            ClearScreen();
            StartCoroutine(ReturnFromBattle());
        }
        else if (currentState == BattleState.Lost)
        {
            print("Congratulations! You're a failure");
        }
    }

    private void ClearScreen()
    {
        BattleUI.instance.OnBattleEnd();
        ClearEnemies();
    }

    private void ClearEnemies()
    {
        foreach (var enemy in enemyUnits)
        {
            enemy.FadeOut();
        }

        enemyUnits = null;
    }

    IEnumerator ReturnFromBattle()
    {
        yield return new WaitForSeconds(1.5f);

        PlayerCamera.instance.willFollow = true;
        playerUnit.EnableCollisionAndMovement();
        playerUnit.dices.AddRange(diceReward);
    }

    public void SelectEnemy(Enemy enemy)
    {
        if(selectedEnemy != null)
        {
            selectedEnemy.DisableOutline();
        }

        selectedEnemy = enemy;
        selectedEnemy.EnableOutline();
    }

    private void SelectNextEnemy()
    {
        foreach (var enemy in enemyUnits)
        {
            if (enemy.isDead) { continue; }

            SelectEnemy(enemy);
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
