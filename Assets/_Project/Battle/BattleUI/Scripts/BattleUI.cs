using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    public static BattleUI instance { get; private set; }

    [SerializeField] private RectTransform diceButtonContainer;
    [SerializeField] private DiceButton diceButtonPrefab;
    [Space]
    [SerializeField] private TextMeshProUGUI battleStateTitle;
    [Space]
    [SerializeField] private RectTransform turnIndicator;
    [SerializeField] private Vector2 TurnIndicatorOffset;
    [Space]
    [SerializeField] private TextMeshProUGUI damageIndicator;

    void Start()
    {
        instance = this;

        turnIndicator.gameObject.SetActive(false);
        damageIndicator.enabled = false;
    }

    public void SetDiceButtons(List<Dice> dices)
    {
        foreach (Dice dice in dices)
        {
            DiceButton button = Instantiate(diceButtonPrefab, diceButtonContainer);
            button.UpdateWithDice(dice);
        }
    }

    public void EnableDiceButtons()
    {
        Button[] buttons = diceButtonContainer.GetComponentsInChildren<Button>();

        foreach (var button in buttons)
        {
            button.interactable = true;
        }
    }

    public void DisableDiceButtons()
    {
        Button[] buttons = diceButtonContainer.GetComponentsInChildren<Button>();

        foreach (var button in buttons)
        {
            button.interactable = false;
        }
    }

    public void OnDiceButtonClicked(DiceButton diceButton)
    {
        BattleSystem.instance.OnDiceClicked(diceButton.dice);
        Destroy(diceButton.gameObject);
    }

    public void OnBattleStateChanged(BattleState state)
    {
        switch (state)
        {
            case BattleState.Start:
                battleStateTitle.text = "Betting Start!";
                break;
            case BattleState.PlayerTurn:
                battleStateTitle.text = "Player's Bet";
                break;
            case BattleState.EnemyTurn:
                battleStateTitle.text = "Enemy's Bet";
                break;
            case BattleState.Won:
                battleStateTitle.text = "You Won!";
                break;
            case BattleState.Lost:
                battleStateTitle.text = "You Lost";
                break;
            default:
                break;
        }
    }

    public void OnAttackerChanged(Vector2 attackerPosition)
    {
        Vector2 viewPortAttackerPos = Camera.main.WorldToScreenPoint(attackerPosition);

        turnIndicator.gameObject.SetActive(true);
        turnIndicator.position = viewPortAttackerPos + TurnIndicatorOffset;
    }

    public void OnDamageDealt(int damage, Vector2 damagedUnitPosition)
    {
        Vector2 viewPortDamagePos = Camera.main.WorldToScreenPoint(damagedUnitPosition);

        turnIndicator.position = viewPortDamagePos + TurnIndicatorOffset;

        StartCoroutine(ShowDamageAtPosition(damage, viewPortDamagePos));
    }

    IEnumerator ShowDamageAtPosition(int damage, Vector2 position)
    {
        damageIndicator.enabled = true;

        damageIndicator.transform.position = position;
        damageIndicator.text = damage.ToString();

        yield return new WaitForSeconds(1.3f);

        damageIndicator.enabled = false;
    }

}
