using System;
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

        battleStateTitle.transform.position += new Vector3(0, 100, 0);
        diceButtonContainer.transform.position += new Vector3(0, -200, 0);

        HideUI();
    }

    public void SetDiceButtons(List<Dice> dices)
    {
        ClearDiceButtons();

        foreach (Dice dice in dices)
        {
            DiceButton button = Instantiate(diceButtonPrefab, diceButtonContainer);
            button.UpdateWithDice(dice);
        }
    }

    public void ClearDiceButtons()
    {
        foreach (var button in diceButtonContainer.GetComponentsInChildren<Button>())
        {
            Destroy(button.gameObject);
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
                RestartManager.instance.FadeInGameOverUI();
                break;
            default:
                break;
        }
    }

    public void OnAttackerChanged(Vector2 attackerPosition)
    {
        Vector2 viewPortAttackerPos = Camera.main.WorldToScreenPoint(attackerPosition);

        Vector2 newIndicatorPos = viewPortAttackerPos + TurnIndicatorOffset;

        turnIndicator.gameObject.SetActive(true);
        turnIndicator.transform.position = newIndicatorPos;
    }

    public void OnDamageDealt(int damage, Vector2 damagedUnitPosition)
    {
        Vector2 viewPortDamagePos = Camera.main.WorldToScreenPoint(damagedUnitPosition);

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

    public void HideUI()
    {
        var graphics = GetComponentsInChildren<Graphic>();
        foreach (var graphic in graphics)
        {
            graphic.enabled = false;
        }
    }

    public void ShowUI()
    {
        var graphics = GetComponentsInChildren<Graphic>();
        foreach (var graphic in graphics)
        {
            graphic.enabled = true;
        }
    }

    public void OnBattleStart()
    {
        ShowUI();
        damageIndicator.enabled = false;
        turnIndicator.gameObject.SetActive(false);
        DisableDiceButtons();

        SlideToOffset(battleStateTitle.transform, new Vector3(0,-100,0));
        SlideToOffset(diceButtonContainer.transform, new Vector3(0, 200, 0));
    }

    internal void OnBattleEnd()
    {
        SlideToOffset(battleStateTitle.transform, new Vector3(0, 100, 0));
        SlideToOffset(diceButtonContainer.transform, new Vector3(0, -200, 0));
        FadeOut(turnIndicator.GetComponent<Graphic>());
    }

    public void SlideFromOffset(Transform current, Vector3 offset)
    {
        Vector3 initialPos = current.position;
        battleStateTitle.transform.position += offset;
        StartCoroutine(LerpMovement.LerpToPosition(current, initialPos, 2f));
    }

    public void SlideToOffset(Transform current, Vector3 offset)
    {
        Vector3 initialPos = current.position;
        Vector3 offsetPos = initialPos + offset;
        StartCoroutine(LerpMovement.LerpToPosition(current, offsetPos, 2f));
    }

    private void FadeOut(Graphic graphic)
    {
        StartCoroutine(LerpMovement.LerpOpacity(graphic, 0, 1.5f));
    }
}
