using DG.Tweening;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardManager : StaticInstance<CardManager>
{
    public List<Card> cardsRevealed = new List<Card>();
    private List<Card> cards;
    List<Card> handCards = new List<Card>();
    public List<Transform> handSpaces;
    private int i=0;
    void Start()
    {
        cards = GetComponentsInChildren<Card>().ToList();

        print(cards.Count);
        List<string[]> cardValues = GenerateDistinctCards(cards.Count);
        for(int i = 0;i<cards.Count;i++)
        {
            cards[i].setCard(cardValues[i][0], cardValues[i][1]);
            print(cards[i] +""+ i);
        }
    }
    List<string[]> GenerateDistinctCards(int uniqueCardsToGenerate)
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

            if (!cards.Any(arr => arr.SequenceEqual(card)))
            {
                cards.Add(card);
            }
        }
        for(int i = 0;i<4;i++)
        {
            string[] card = cards[random.Next(cards.Count)];
            card[0] = "+";
            if (i < 1)
                card[1] = "50";
            else if (i < 3)
                card[1] = "30";
            else if (i < 5)
                card[1] = "20";
        }

        return cards;
    }
    // Update is called once per frame
    void Update()
    {
        if (cardsRevealed.Count > 7 && GameManager.Instance.State == GameState.Flip)
        {
            GameManager.Instance.ChangeState(GameState.Pick);
        }
        else if (handCards.Count > 4 && GameManager.Instance.State == GameState.Pick)
        {
            foreach (Card card in this.cards) 
                card.removeListeners();
            List<string[]> cards = new List<string[]>();
            foreach (var card in handCards)
                cards.Add(card.ToArray());
            PokerHandEval.HandRank handRank = PokerHandEval.GetPlayerHandRank(cards);
            double score = Math.Floor(handRank.Rank);
            TextController.Instance.changeText("Achieved "+handRank.RankName+"!\nScore: +"+score.ToString());
            GameManager.Instance.score += score;
            GameManager.Instance.ChangeState(GameState.Score);
        }
    }

    public void EnableAllCards()
    {
        foreach (Card card in cards)
        {
            if (card.rank != "+")
            {
                card.EnableCardInteraction();
                card.removeListeners();
                card.button.onClick.AddListener(card.tweenToHand);
            }
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
