using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class PauseMenuManager : Menu<PauseMenuManager>
{
    // Start is called before the first frame update
    public AudioMixerSnapshot paused;
    public AudioMixerSnapshot unpaused;
    private new void Awake()
    {
        base.Awake();
        GameManager.OnPause += Open;
        GameManager.OnUnpause += Close;
    }
    private void OnDestroy()
    {
        GameManager.OnPause -= Open;
        GameManager.OnUnpause -= Close;
    }
    public void Resume()
    {
        AudioSystem.Instance.PlaySFX("button");
        GameManager.Instance.ChangeState(GameManager.lastGameState);
    }
    public void Restart()
    {
        AudioSystem.Instance.PlaySFX("button");
        SceneSwitcher.Instance.RestartScene();

    }
    public void Options()
    {
        AudioSystem.Instance.PlaySFX("button");
        VolumeSettings.Instance.Open();
    }
    public void Main_Menu()
    {
        AudioSystem.Instance.PlaySFX("button");
        SceneSwitcher.Instance.ReturnToMenu();
    }
    public new void Open()
    {
        base.Open();
        Time.timeScale = 0f;
        paused.TransitionTo(.1f);
    }
    public new void Close()
    {
        base.Close();
        Time.timeScale = 1f;
        unpaused.TransitionTo(.1f);
    }
    public void OnEscape()
    {
        GameState State = GameManager.Instance.State;
        if (State == GameState.Paused)
            Resume();
        else GameManager.Instance.ChangeState(GameState.Paused);
    }

}
