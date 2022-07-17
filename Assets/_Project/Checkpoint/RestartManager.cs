using UnityEngine;
using UnityEngine.UI;

public class RestartManager : MonoBehaviour
{
    public static RestartManager instance;
    [SerializeField] private RectTransform gameOverUI;
    [SerializeField] private Button restartButton;

    private Checkpoint lastCheckpoint;
    private EncounterArea lastEncounter;

    void OnEnable()
    {
        instance = this;

        HideGameOverUI();
        restartButton.onClick.AddListener(RestartFromCheckpoint);
    }

    public void OnCheckpointPassed(Checkpoint checkpoint)
    {
        lastCheckpoint = checkpoint;
    }

    public void OnEncounterStart(EncounterArea encounter)
    {
        lastEncounter = encounter;
    }

    public void FadeInGameOverUI()
    {
        var graphics = gameOverUI.GetComponentsInChildren<Graphic>();
        foreach (var graphic in graphics)
        {
            graphic.enabled = true;
            StartCoroutine(LerpMovement.LerpOpacity(graphic, 255, 1.5f));
        }

        restartButton.interactable = true;
    }

    public void FadeOutGameOverUI()
    {
        var graphics = gameOverUI.GetComponentsInChildren<Graphic>();
        foreach (var graphic in graphics)
        {
            graphic.enabled = false;
            StartCoroutine(LerpMovement.LerpOpacity(graphic, 0, 1.5f));
        }
    }

    public void HideGameOverUI()
    {
        var graphics = gameOverUI.GetComponentsInChildren<Graphic>();
        foreach (var graphic in graphics)
        {
            graphic.enabled = false;
            graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, 0);
        }
    }

    public void RestartFromCheckpoint()
    {
        Player.instance.transform.position = lastCheckpoint.transform.position;
        lastEncounter.Reset();

        restartButton.interactable = false;
        FadeOutGameOverUI();
        Player.instance.EnableCollisionAndMovement();
        PlayerCamera.instance.willFollow = true;
    }
}
