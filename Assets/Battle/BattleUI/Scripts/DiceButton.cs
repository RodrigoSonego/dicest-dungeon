using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class DiceButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameContainer;
    [SerializeField] private Image diceIcon;

    private Button button;

    public Dice dice { get; private set; }

    void Start()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(OnButtonClicked);
    }

    public void UpdateWithDice(Dice dice)
    {
        nameContainer.text = dice.diceName;
        this.dice = dice;
    }

    private void OnButtonClicked()
    {
        BattleUI.instance.OnDiceButtonClicked(dice);
        Destroy(gameObject);
    }
}
