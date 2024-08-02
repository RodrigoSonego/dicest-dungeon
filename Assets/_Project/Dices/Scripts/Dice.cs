using UnityEngine;

[CreateAssetMenu(fileName = "Dice", menuName = "Dice")]
public class Dice : ScriptableObject
{
    [SerializeField] public string diceName;
    [SerializeField] private int maxValue;
    [SerializeField] public Sprite uiSprite;
    [SerializeField] public Color color;

    public int Roll()
    {
        return Random.Range(1, maxValue);
    }
}
