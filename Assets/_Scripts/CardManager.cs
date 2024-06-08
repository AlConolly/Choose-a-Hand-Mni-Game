using DG.Tweening;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardManager : StaticInstance<CardManager>
{
    public static List<Card> cardsRevealed = new List<Card>();
    List<Card> cards;
    List<Card> handCards = new List<Card>();
    public List<Transform> handSpaces;
    private int i=0;
    void Start()
    {
        cards = GetComponentsInChildren<Card>().ToList<Card>();

        List<string[]> cardValues = GenerateDistinctCards(cards.Count);
        for(int i = 0;i<cards.Count;i++)
        {
            cards[i].setCard(cardValues[i][0], cardValues[i][1]);
        }
    }
    static List<string[]> GenerateDistinctCards(int uniqueCardsToGenerate)
    {
        List<string[]> cards = new List<string[]>();
        System.Random random = new System.Random();

        string[] suits = Card.ranks;
        string[] ranks = Card.suits;

        while (cards.Count < uniqueCardsToGenerate)
        {
            string suit = suits[random.Next(suits.Length)];
            string rank = ranks[random.Next(ranks.Length)];
            string[] card = new string[2] { suit, rank };

            if (!cards.Contains(card))
            {
                cards.Add(card);
            }
        }

        return cards;
    }
    // Update is called once per frame
    void Update()
    {
        if (cardsRevealed.Count > 6 && GameManager.Instance.State == GameState.Flip)
        {
            GameManager.Instance.ChangeState(GameState.Pick);
        }
        else if (handCards.Count > 4 && GameManager.Instance.State == GameState.Pick)
        {
            foreach (Card card in this.cards) 
                card.removeListeners();
            GameManager.Instance.ChangeState(GameState.Score);
            List<string[]> cards = new List<string[]>();
            foreach (var card in handCards)
                cards.Add(card.ToArray());
            PokerHandEval.HandRank handRank = PokerHandEval.GetPlayerHandRank(cards);
            string score = Math.Floor(handRank.Rank).ToString();
            TextController.Instance.changeText("Achieved "+handRank.RankName+"!\nScore: "+score);
        }
    }

    public void EnableAllCards()
    {
        foreach (Card card in cards)
        {
            card.EnableCardInteraction();
            card.removeListeners();
            card.button.onClick.AddListener(card.tweenToHand);
        }
    }
    public Vector2 nextHandPosition()
    {
        return handSpaces[i++].position;
    }
    public void addHandCard(Card card)
    {
        handCards.Add(card);
    }
}
