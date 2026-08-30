using UnityEngine;
using System.Collections;

public class GameStateManager : MonoBehaviour
{
    public enum GameState { Normal, Fever, Raining }

    public static GameStateManager Instance { get; private set; }

    // Other scripts (RiceCrop, FarmerInventory) subscribe to these instead of polling every frame
    public static event System.Action OnFeverStart;
    public static event System.Action OnRainingStart;

    [Header("References")]
    [SerializeField] private Timer timer;
    [SerializeField] private ScoreManager scoreManager;

    [Header("Timing")]
    [SerializeField] private float transitionAtElapsedSeconds = 120f; // switch check happens at 120s elapsed
    [SerializeField] private float feverDuration = 10f;

    [Header("Fever Trigger")]
    [field: SerializeField] public int feverScoreThreshold { get; private set; } = 1500;

    public GameState CurrentState { get; private set; } = GameState.Normal;

    private float initialTime;
    private bool transitionChecked = false;

    private void Awake()
    {
        Instance = this;
        initialTime = timer.timeremaining; // capture starting time before Timer's Update ticks it down
    }

    private void Update()
    {
        if (CurrentState != GameState.Normal || transitionChecked) return;

        float elapsed = initialTime - timer.timeremaining;
        if (elapsed >= transitionAtElapsedSeconds)
        {
            transitionChecked = true;

            if (scoreManager.CurrentScore >= feverScoreThreshold)
            {
                StartFever();
            }
            else
            {
                StartRaining();
            }
        }
    }

    private void StartFever()
    {
        CurrentState = GameState.Fever;
        OnFeverStart?.Invoke();
        StartCoroutine(FeverCountdown());
    }

    private IEnumerator FeverCountdown()
    {
        yield return new WaitForSeconds(feverDuration);
        StartRaining();
    }

    private void StartRaining()
    {
        CurrentState = GameState.Raining;
        OnRainingStart?.Invoke();
    }
}