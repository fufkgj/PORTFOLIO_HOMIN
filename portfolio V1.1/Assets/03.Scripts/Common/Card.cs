using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardGrade{ Legendary, Rare, Normal, Fun }
public enum CardAbility { Get_bullet, PlayerHP_ten , Enemy_Speed, Enemy_Power,ReloadTime, FireRateTime, MaxEnemy , Money }

[System.Serializable]
public class Card 
{
    public string CardName;  //카드 이름
    public Sprite CardImg;   //카드 이미지
    public CardGrade CardGrade; //카드 등급
    public CardAbility CardAbility; //카드 효과
    public int weight; //카드가 나올 확률

    public Card(Card card)
    {
        this.CardName = card.CardName;
        this.CardImg = card.CardImg;
        this.CardGrade = card.CardGrade;
        this.CardAbility = card.CardAbility;
        this.weight = card.weight;
    }
}
