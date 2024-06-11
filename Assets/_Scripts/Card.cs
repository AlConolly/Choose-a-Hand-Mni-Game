using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class Card : MonoBehaviour
{
    public static string[] ranks = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14" };
    public static string[] suits = { "Club", "Diamond", "Heart", "Spade" };
    public string rank;
    public string suit;
    public bool revealed = false;
    public Sprite frontSide;
    public Button button;
    public bool special = false;
    public Image image;
    void Start()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        button.onClick.AddListener(Reveal);
        transform.DOMove(new Vector2(Screen.width/2, Screen.height/2),1f).From();
    }
    public void setCard(string rank, string suit)
    {
        this.rank = rank;
        this.suit = suit;
        frontSide = Resources.Load<Sprite>("PlayingCards/" + rank + suit);
        if(rank== "+")
        {
            button = GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(RevealPlus);
        }
    }

    public override string ToString()
    {
        return rank + " " + suit;
    }
    public string[] ToArray()
    {
        string[] a = {rank, suit};
        return a;
    }
    public void Flip()
    {
        revealed = true;
        AudioSystem.Instance.PlaySFX("card");
        StartCoroutine(FlipAni());
    }
    private IEnumerator FlipAni()
    {
        for (float i = 180f; i >= 0f; i -= 10f)
        {
            transform.rotation = Quaternion.Euler(0f, i, 0f);
            if (i == 90f)
            {
                image.sprite = frontSide;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
    public void Reveal()
    {
        Flip();
        CardManager.Instance.cardsRevealed.Add(this);
        button.interactable = false;
    }
    public void RevealPlus()
    {
        Flip();
        image.DOFade(0f, 1f);
        transform.DOLocalMoveY(60, 1f);
        GameManager.Instance.score += Convert.ToDouble(suit);

        button.interactable = false;
    }
    public void EnableCardInteraction()
    {
        button.interactable = true;
    }
    public void Hide()
    {
        revealed = false;
        image.sprite = Resources.Load<Sprite>("PlayingCards/cardBack");
    }
    
    public void removeListeners()
    {
        button.onClick.RemoveAllListeners();
    }
    public void tweenToHand()
    {
        if (revealed == false)
            GameManager.Instance.score += 20;
        Flip();
        Vector2 pos = CardManager.Instance.nextHandPosition();
        CardManager.Instance.addHandCard(this);
        transform.DOMove(pos, 1f);
        removeListeners();
    }
}
