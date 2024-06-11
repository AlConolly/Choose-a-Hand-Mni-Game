using UnityEngine.UI;

public class TextController : StaticInstance<TextController>
{
    Text text;
    void Update()
    {
        if (GameManager.Instance.State == GameState.Flip)
            changeText("Flip 8 cards \n(" + CardManager.Instance.cardsRevealed.Count + "/8)");
        else if(GameManager.Instance.State == GameState.Pick)
            changeText("Choose 5 cards for your poker hand\n unrevealed cards can be chosen too!");
    }
    private void OnEnable()
    {
        GameManager.OnScore += showScore;
    }
    private void OnDisable()
    {
        GameManager.OnScore -= showScore;
    }

    // Update is called once per frame
    void Start()
    {
        text = GetComponent<Text>();
    }
    public void changeText(string t)
    {
        text.text = t;
    }
    void showScore()
    {
        
    }
}
