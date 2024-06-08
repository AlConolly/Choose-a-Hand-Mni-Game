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
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Reveal);
    }
    public void setCard(string rank, string suit)
    {
        this.rank = rank;
        this.suit = suit;
        frontSide = Resources.Load<Sprite>("PlayingCards/" + rank + suit);
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
        GetComponent<Image>().sprite = frontSide;
    }
    public void Reveal()
    {
        Flip();
        CardManager.cardsRevealed.Add(this);
        button.interactable = false;
    }
    public void EnableCardInteraction()
    {
        button.interactable = true;
    }
    public void Hide()
    {
        revealed = false;
        GetComponent<Image>().sprite = Resources.Load<Sprite>("PlayingCards/cardBack");
    }
    
    public void removeListeners()
    {
        button.onClick.RemoveAllListeners();
    }
    public void tweenToHand()
    {
        Flip();
        Vector2 pos = CardManager.Instance.nextHandPosition();
        CardManager.Instance.addHandCard(this);
        transform.DOMove(pos, 1f);
        removeListeners();
    }
}
