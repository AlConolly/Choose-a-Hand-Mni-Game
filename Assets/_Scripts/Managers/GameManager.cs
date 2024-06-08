using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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
    public GameState State { get; private set; }
    public int scoreToBeat = 300;
    public int score = 0;

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
}

