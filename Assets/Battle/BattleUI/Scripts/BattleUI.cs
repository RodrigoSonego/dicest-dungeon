using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    public static BattleUI instance { get; private set; }

    [SerializeField] private RectTransform diceButtonContainer;
    [SerializeField] private DiceButton diceButtonPrefab;

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

    public void OnDiceButtonClicked(Dice dice)
    {
        BattleSystem.instance.OnDiceClicked(dice);
    }

}
