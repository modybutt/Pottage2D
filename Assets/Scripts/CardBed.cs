using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class CardBed : CardSlot
{
    public override bool CanDrop(Card dropCard)
    {
        if (dropCard.GetComponentInParent<CardBed>() == this) return false;

        Card[] slotCards = GetComponentsInChildren<Card>();
        if (slotCards.Length > 0 && dropCard.CanCombine(slotCards) == false) return false;

        //Card[] embedded = GetComponentsInChildren<Card>();
        //foreach (Card card in embedded)
        //{
        //    if (card.CanCombine(dropCard, embedded.Length) == false) return false;
        //}



        foreach (CardSlot slot in GetComponentsInChildren<CardSlot>())
        {
            if (slot == this) continue;
            if (slot.CanDrop(dropCard)) return true;
        }

         return false;
    }

    public override void DropCard(Card dropCard)
    {
        if (CanDrop(dropCard) == false) return;

        List<Card> dropCards = null;
        foreach (CardSlot slot in GetComponentsInChildren<CardSlot>())
        {
            if (slot == this) continue;
            if (slot.CanDrop(dropCard))
            {
                if (dropCards != null)
                {
                    dropCards.ForEach((slotCard) => slot.DropCard(slotCard));
                    dropCards = null;
                    return;
                }

                slot.DropCard(dropCard);
                return;
            }
            else
            {
                List<Card> slotCards = slot.GetComponentsInChildren<Card>().ToList();

                if (dropCard.cardData.elementIndex < slotCards.First().cardData.elementIndex)
                {
                    dropCards = slotCards;
                    dropCards.ForEach((slotCard) => slotCard.transform.SetParent(slotCard.transform.parent.parent));
                    slot.DropCard(dropCard);
                    dropCard = dropCards.First();
                    continue;
                }
            }
        }
    }

    public override void OnCardDrained(Card drainCard)
    {
        CardSlot freeSlot = null;

        foreach (CardSlot slot in GetComponentsInChildren<CardSlot>())
        {
            if (slot == this) continue;
            
            if (slot.IsOccupied() && freeSlot != null)
            {
                slot.GetComponentsInChildren<Card>().ToList().ForEach((slotCard) => freeSlot.DropCard(slotCard));
            }

            freeSlot = slot;
        }
    }
}
