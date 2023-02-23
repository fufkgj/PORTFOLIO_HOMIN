using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardGrade{ Legendary, Rare, Normal, Fun }
public enum CardAbility { Get_bullet, PlayerHP_ten , Enemy_Speed, Enemy_Power,ReloadTime, FireRateTime, MaxEnemy , Money }

[System.Serializable]
public class Card 
{
    public string CardName;
    public Sprite CardImg;
    public CardGrade CardGrade;
    public CardAbility CardAbility;
    public int weight;

    public Card(Card card)
    {
        this.CardName = card.CardName;
        this.CardImg = card.CardImg;
        this.CardGrade = card.CardGrade;
        this.CardAbility = card.CardAbility;
        this.weight = card.weight;
    }
}
