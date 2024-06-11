using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
[Serializable]
public enum GameState
{
    Starting = 0,
    Paused = 1,
    Flip,
    Pick,
    Score
}
/// <summary>
/// Base for a Game Manager. Mainly features: State machine for switching between game states
/// </summary>
public class GameManager : Singleton<GameManager> {
    public static event Action<GameState> OnBeforeStateChanged;
    public static event Action<GameState> OnAfterStateChanged;
    public static event Action OnScore;
    public static event Action OnPause;
    public static event Action OnUnpause;
    public static GameState lastGameState = GameState.Flip;
    public TMP_Text scoreText;
    public TMP_Text yourScoreText;
    public Canvas scoreCanvas;
    public Text winLoseText;
    public Text winLoseScoreText;
    public TMP_Text winStreakText;
    public GameState State { get; private set; }
    public static int scoreToBeat = 300;
    public double score = 0;

    /// <summary>
    /// Stops all state changes from the first state to any of the listed states. OnBeforeStateChanged will not be called.
    /// </summary>
    [SerializedDictionary("Can't change from this State", "To these States")]
    [SerializeField] private SerializedDictionary<GameState,List<GameState>> ForbidenStateTransitions;
    // Kick the game off with the first state
    void Start()
    {
        ChangeState(GameState.Flip);
        scoreText.text = "Score To Beat: " + "<color=red>"+scoreToBeat.ToString()+"</color>";
    }
    private void Update()
    {
        yourScoreText.text = "Your Score: " + "<color=green>" + score.ToString() + "</color>";
    }

    public void ChangeState(GameState newState) {
        if (isValidTransition(State,newState)==false) return;

        lastGameState = State;
        OnBeforeStateChanged?.Invoke(newState);

        State = newState;
        switch (newState) {
            case GameState.Starting:
                HandleStarting();
                break;
            case GameState.Flip:
                break;
            case GameState.Pick:
                CardManager.Instance.EnableAllCards();
                break;
            case GameState.Score:
                OnScore?.Invoke();
                PresentScore();
                UpdateHighScore(score>scoreToBeat);
                break;
            case GameState.Paused:
                OnPause?.Invoke();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnAfterStateChanged?.Invoke(newState);
        
        Debug.Log($"New state: {newState}");
    }
    public bool isValidTransition(GameState oldState, GameState newState)
    {
        bool inForbiddenTable = ForbidenStateTransitions.ContainsKey(oldState) && ForbidenStateTransitions[oldState].Contains(newState);
        bool isRepeat = oldState == newState;
        return !(inForbiddenTable || isRepeat);
    }
    ///automatically resumes game when exiting the pause GameState
    public void AutoUnPause(GameState newState)
    {
        if(State == GameState.Paused && newState != GameState.Paused)
            OnUnpause?.Invoke();
    }
    private void OnEnable()
    {
        OnBeforeStateChanged += AutoUnPause;
    }
    private void OnDisable()
    {
        OnBeforeStateChanged -= AutoUnPause;
    }
    private void HandleStarting() {
        // Do some start setup, could be environment, cinematics etc

        // Eventually call ChangeState again with your next state
        
        ChangeState(GameState.Flip);
    }

    private void PresentScore()
    {
        if (score > scoreToBeat)
        {
            winLoseText.text = "<color=#66ff00>You Win!</color>";
            winLoseScoreText.text = "Your Score: <color=#66ff00>" + score + "</color> beat Opponent's Score: " + scoreToBeat;
        }
        else if (score == scoreToBeat)
        {
            winLoseText.text = "<color=808080>You Tied.</color>";
            winLoseScoreText.text = "Your Score: <color=#66ff00>" + score + "</color> matched your Opponent's Score: " + scoreToBeat;
        }
        else if (score < scoreToBeat)
        {
            winLoseText.text = "<color=808080>You Lost</color>";
            winLoseScoreText.text = "Your Score: <color=#66ff00>" + score + "</color> was less than your Opponent's Score: " + scoreToBeat;
        }
        scoreCanvas.enabled = true;
    }
    private void UpdateHighScore(bool won)
    {
        if (!PlayerPrefs.HasKey("highScore"))
            PlayerPrefs.SetInt("highScore", (int)score);
        if (score > PlayerPrefs.GetInt("highScore"))
            PlayerPrefs.SetInt("highScore", (int)score);

        if (!PlayerPrefs.HasKey("winStreak"))
            PlayerPrefs.SetInt("winStreak", 0);
        if(won)
            PlayerPrefs.SetInt("winStreak",PlayerPrefs.GetInt("winStreak")+1);
        else
            PlayerPrefs.SetInt("winStreak", 0);
        winStreakText.text = "Current Win Streak: " + PlayerPrefs.GetInt("winStreak");
    }
}

