using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

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

    [SerializeField] private ThrownDice thrownDice;
    [SerializeField] private float diceThrowLength;

    [SerializeField] private Enemy[] enemyUnits;
    [SerializeField] private Player playerUnit;

    private Enemy selectedEnemy = null;
    
    private Dice[] diceReward = null;
    private List<Dice> lastUsedDices = null;

    public static BattleSystem instance { get; private set; }

    void Start()
    {
       instance = this;

        thrownDice.gameObject.SetActive(false);
    }

    public IEnumerator StartBattle(Enemy[] enemies, Dice[] diceReward)
    {
        currentState = BattleState.Start;
        enemyUnits = enemies;
        this.diceReward = diceReward;

        lastUsedDices = new List<Dice>();

        PlayerCamera.instance.willFollow = false;

        BattleUI.instance.OnBattleStateChanged(currentState);
        BattleUI.instance.SetDiceButtons(playerUnit.dices);
        BattleUI.instance.OnBattleStart();
        BattleUI.instance.SetPlayerHealthBarValue(playerUnit.CurrentHp);

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
        selectedEnemy.EnableOutline();
    }

    IEnumerator PlayerAttack(Dice dice)
    {
        if(enemyUnits.Length > 1 && selectedEnemy == null)
        {
            print("no enemy selected");
            yield break;
        }

        int damage = playerUnit.RollDice(dice);
        lastUsedDices.Add(dice);

        yield return StartCoroutine(ThrowDie(playerUnit.diceSpawnPosition, selectedEnemy.transform, dice.color, willFlip: false, playerUnit.attackAnimationWindUpTime));

		selectedEnemy.TakeDamage(damage);
        BattleUI.instance.OnDamageDealt(damage, selectedEnemy.transform.position);

        BattleUI.instance.DisableDiceButtons();
        yield return new WaitForSeconds(1.5f);

        if(selectedEnemy.isDead)
        {
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
        selectedEnemy.DisableOutline();

        foreach (Enemy enemy in enemyUnits)
        {
            if(enemy.isDead) { continue; }

            BattleUI.instance.OnAttackerChanged(enemy.transform.position);

            (int damage, Dice dice) = enemy.RollDice();

            yield return StartCoroutine(ThrowDie(enemy.diceSpawnPosition, playerUnit.transform, dice.color, willFlip: true, enemy.attackAnimationWindUpTime));

            playerUnit.TakeDamage(damage);
            BattleUI.instance.OnDamageDealt(damage, playerUnit.transform.position);
            BattleUI.instance.SetPlayerHealthBarValue(playerUnit.CurrentHp);

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
        BattleUI.instance.OnBattleStateChanged(currentState);
        ClearScreen();
        
        if(currentState == BattleState.Won)
        {
            playerUnit.dices.AddRange(diceReward);
            StartCoroutine(ReturnFromBattle());
        }
        else if (currentState == BattleState.Lost)
        {
            BattleUI.instance.HideUI();
            RestoreUsedDices();
        }
    }

    private void ClearScreen()
    {
        BattleUI.instance.OnBattleEnd();
        ClearEnemies();
    }

    private void ClearEnemies()
    {
        enemyUnits = null;
    }

    private void RestoreUsedDices()
    {
        playerUnit.dices.AddRange(lastUsedDices);
        lastUsedDices = null;
    }

    IEnumerator ReturnFromBattle()
    {
        yield return new WaitForSeconds(1.5f);

        PlayerCamera.instance.willFollow = true;
        playerUnit.EnableCollisionAndMovement();
        playerUnit.FullyHeal();
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

    private IEnumerator ThrowDie(Transform origin, Transform target, Color color, bool willFlip, float animWindUp)
    {
		yield return new WaitForSeconds(animWindUp);
		thrownDice.ChangeColor(color);
		thrownDice.FlipSprite(true);
		yield return StartCoroutine(thrownDice.LerpDiceTo(origin, target, diceThrowLength));
	}
}
