using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.UI;

public class CardGarden : CardSlot
{
    //private List<CardBed> cardBeds = new List<CardBed>();


    //private void Start()
    //{
    //    foreach (CardBed child in GetComponentsInChildren<CardBed>())
    //    {
    //        //Debug.Log("bed: " + child);
    //        //List<CardSlot> newBed = new List<CardSlot>();

    //        //foreach (CardSlot slot in child.GetComponentsInChildren<CardSlot>())
    //        //{
    //        //    Debug.Log("slot: " + slot);
    //        //    newBed.Add(slot);
    //        //}

    //        cardBeds.Add(child);
    //    }
    //}

    public override bool CanDrop(Card dropCard)
    {
        if (dropCard.GetComponentInParent<CardGarden>() == this) return false;

        foreach (CardBed cardBed in GetComponentsInChildren<CardBed>())
        {
            if (cardBed.CanDrop(dropCard)) return true;
        }

        return false;
    }

    public override void DropCard(Card dropCard)
    {
        if (CanDrop(dropCard))
        {
            foreach (CardBed cardBed in GetComponentsInChildren<CardBed>()
                .Where((cardBed) => cardBed.GetComponentInChildren<Card>() != null))
            {
                if (cardBed.CanDrop(dropCard))
                {
                    cardBed.DropCard(dropCard);
                    return;
                }
            }

            foreach (CardBed cardBed in GetComponentsInChildren<CardBed>())
            {
                if (cardBed.CanDrop(dropCard))
                {
                    cardBed.DropCard(dropCard);
                    return;
                }
            }
        }
    }

    //public override bool CanDrop(Card dropCard)
    //{
    //    return GetFreeSlot(dropCard) != null;
    //}

    //public override void DropCard(Card dropCard)
    //{
    //    foreach (CardBed bed in cardBeds)
    //    {
    //        if (bed.CanDrop(dropCard))
    //        {
    //            bed.DropCard(dropCard);
    //            break;
    //        }
    //    }

    //    //int[] idx = GetFreeSlot(dropCard);

    //    //if (idx != null)
    //    //{
    //    //    cardBeds[idx[0]].slots[idx[1]].DropCard(dropCard);
    //    //}

    //    // get beds of same type
    //    // check for stackable
    //    // if no bed -> create new
    //    // combine with dropCard

    //    //List<CardSlot> bedSlots = new List<CardSlot>();
    //    //cardBeds.Add(bedSlots);
    //    //bedSlots[0].DropCard(dropCard);
    //}

    //private int[] GetFreeSlot(Card dropCard)
    //{
    //    if (dropCard != null && dropCard.cardData != null)
    //    {
    //        if (dropCard.cardData.id == -1) return null;
    //    }

    //    foreach (CardBed bed in cardBeds)
    //    {
    //        if (bed.CanDrop(dropCard))
    //        {
    //            bed.DropCard(dropCard); // FIXME x,y
    //            break;
    //        }
    //    }

    //    return new int[] { 0, 0 };
    //}
}