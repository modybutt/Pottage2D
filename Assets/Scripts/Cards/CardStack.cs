using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider2D))]
public class CardStack : CardSlot, IPointerClickHandler
{
    public CardSlot targetSlot;
    public int maxCardShift;
    public bool enableShift;

    private int currCardShift;
    //private Stack<Card> cards = new Stack<Card>();
    private TextMesh text;

    //private void Start()
    //{
    //    text = GetComponentInChildren<TextMesh>();
    //    text.text = GetComponentsInChildren<Card>().Length.ToString();
    //}

    //public void PushCard(Card card)
    //{
    //    cards.Push(card);
    //    card.transform.SetParent(transform, false);
    //    card.GetComponent<BoxCollider2D>().enabled = false;
    //}

    //public override bool CanDrop(Card dropCard)
    //{
    //    return (dropCard != null) && (IsOccupied() == false);
    //}

    public override void DropCard(Card dropCard)
    {
        if (CanDrop(dropCard))
        {
            base.DropCard(dropCard);
            dropCard.GetComponent<BoxCollider2D>().enabled = enableShift;
            //dropCard.transform.position = new Vector2(transform.position.x, transform.position.y);
            //Debug.Log(dropCard.transform.position);

        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount > 0)
        {
            currCardShift = 0;
            foreach (Card card in GetComponentsInChildren<Card>())
            {
                if (maxCardShift > 0 && currCardShift >= maxCardShift) return;
                //card.MoveTo(targetSlot.gameObject);
                targetSlot.DropCard(card);
            }
        }
    }

    public override void OnCardDrained(Card drainCard)
    {
        currCardShift++;
    }
}
