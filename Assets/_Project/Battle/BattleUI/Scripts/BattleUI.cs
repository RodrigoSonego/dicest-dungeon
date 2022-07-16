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

    void Start()
    {
        instance = this;
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
                break;
            case BattleState.Lost:
                break;
            default:
                break;
        }
    }

}
