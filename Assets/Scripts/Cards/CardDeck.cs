using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDeck : CardSlot
{
    public CardSlot targetSlot;

    public override bool CanDrop(Card dropCard)
    {
        foreach (CardSlot slot in GetComponentsInChildren<CardSlot>())
        {
            if (slot == this) continue;
            if (slot.CanDrop(dropCard)) return true;
        }

        return false;
    }

    public override void DropCard(Card dropCard)
    {
        foreach (CardSlot slot in GetComponentsInChildren<CardSlot>())
        {
            if (slot == this) continue;
            if (slot.CanDrop(dropCard))
            {
                if (dropCard.isFaceUp == false) dropCard.Flip();
                slot.DropCard(dropCard);

                dropCard.GetComponent<BoxCollider2D>().enabled = true;
                //card.MoveTo(slot.gameObject);
                return;
            }
        }
    }

    public override void OnCardDrained(Card drainCard)
    {
        foreach (Card card in GetComponentsInChildren<Card>())
        {
            if (card.isFaceUp) card.Flip();
            targetSlot.DropCard(card);
        }
    }
}
