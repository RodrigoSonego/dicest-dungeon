using UnityEngine;

[CreateAssetMenu(fileName = "Dice", menuName = "Dice")]
public class Dice : ScriptableObject
{
    [SerializeField] public string diceName;
    [SerializeField] private int maxValue;
    [SerializeField] private Sprite uiSprite;
    [SerializeField] private Sprite RolledSprite;

    public int Roll()
    {
        return Random.Range(1, maxValue);
    }
}
