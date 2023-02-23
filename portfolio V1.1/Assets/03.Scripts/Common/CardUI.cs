using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardUI : MonoBehaviour, IPointerDownHandler
{
    public Image chr;
    public Text cardName;
    public CardAbility ability;
    Animator animator;
    public GameObject Card;
    public Card cardInfo;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    // 카드의 정보를 초기화
    public void CardUISet(Card card)
    {
        chr.sprite = card.CardImg;
        cardName.text = card.CardName;
    }
    // 카드가 클릭되면 뒤집는 애니메이션 재생
    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameManager.instance.isSelect == false)
        {
            animator.SetTrigger("Flip");
            GameManager.instance.Ability(cardInfo);
        }
    }
}
