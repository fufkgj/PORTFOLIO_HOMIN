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
        randomSelect.CardCreate();
    }
    private void OnDisable()
    {
        for(int i = 0; i<Card.Length; i++)
        {
            Destroy(Card[i]);
        }
        randomSelect.RemoveCard();
    }
}
