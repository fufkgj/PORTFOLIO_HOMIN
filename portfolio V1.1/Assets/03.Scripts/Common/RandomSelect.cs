using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSelect : MonoBehaviour
{
    public List<Card> deck = new List<Card>();
    public int total = 0;
    public void CardCreate()  //카드를 생성하는 스크립트
    {
        for (int i = 0; i < deck.Count; i++)
        { 
            total += deck[i].weight;
        }
        ResultSelect();
    }
    public List<Card> result = new List<Card>();

    public Transform parent;
    public GameObject cardprefab;
    public void ResultSelect()   //랜덤 값을 정하는 스크립트  
    {
        for (int i = 0; i < 3; i++)   
        {
            result.Add(RandomCard());
            CardUI cardUI = Instantiate(cardprefab, parent).GetComponent<CardUI>();
            cardUI.CardUISet(result[i]);           
        }
    }
    public Card RandomCard() //카드를 랜덤으로 선택하는 스크립트
    {
        int weight = 0;
        int selectNum = 0;

        selectNum = Mathf.RoundToInt(total * Random.Range(0.0f, 1.0f)); //가중치 랜덤

        for (int i = 0; i < deck.Count; i++)
        {
            weight += deck[i].weight;
            if (selectNum <= weight)
            {
                Card temp = new Card(deck[i]);
                return temp;
            }
        }
        return null;
    }
  public void RemoveCard() //생성된 카드를 제거하는 스크립트
    {
        for(int i = result.Count-1; i>=0; i--)
        {
            result.Remove(result[i]);
        }
        total = 0;
    }
}
