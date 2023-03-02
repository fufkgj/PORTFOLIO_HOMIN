using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardUIManager : MonoBehaviour
{
    public GameObject[] Card;
    public RandomSelect randomSelect;
    // Update is called once per frame
    void Update()
    {
        Card = GameObject.FindGameObjectsWithTag("Card");
    }
    private void OnEnable()
    {
        randomSelect.CardCreate();  //카드 UI가 켜질때 Card를 생성
    }
    private void OnDisable()
    {
        for(int i = 0; i<Card.Length; i++)  //카드 UI가 꺼지면 카드를 제거
        {
            Destroy(Card[i]);
        }
        randomSelect.RemoveCard();
    }
}
