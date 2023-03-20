using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

[RequireComponent(typeof(SpriteRenderer))]
public class CardSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int maxCards = 1;
    public float cardScale = 0.9f;
    public Color selectedColor;
    public Color pickedColor;
    
    private Color originalColor;
    //private bool isPicked;

    private void Awake()
    {
        originalColor = GetComponent<SpriteRenderer>().color;
    }

    //public virtual CardSlot GetSlotForCard(Card dropCard)
    //{
    //    return this;
    //}

    public virtual bool CanDrop(Card dropCard)
    {
        return (dropCard != null) && (IsOccupied() == false);
    }

    public virtual void DropCard(Card dropCard)
    {
        if (CanDrop(dropCard))
        {
            CardSlot oldSlot = dropCard.GetComponentInParent<CardSlot>();
            
            dropCard.transform.SetParent(transform);
            //dropCard.transform.position = transform.position;// new Vector3(transform.position.x, transform.position.y, transform.position.z);
            dropCard.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.1f);
            dropCard.transform.localScale = transform.localScale * cardScale;
            
            if (oldSlot != null) oldSlot.OnCardDrained(dropCard);
        }
    }

    public virtual void OnCardDrained(Card drainCard)
    {
        foreach (CardSlot slot in GetComponentsInParent<CardSlot>())
        {
            if (slot == this) { continue; }
            slot.OnCardDrained(drainCard);
        }
    }

    public bool IsOccupied()
    {
        return GetComponentsInChildren<Card>().Length >= maxCards;
    }

    public void OnDrop(PointerEventData eventData)
    {
        //if (eventData.pointerEnter != this) return;
        //Debug.Log("dropped: " + eventData.selectedObject + " on " + name);

        Card card = eventData.pointerDrag.GetComponent<Card>();

        if (CanDrop(card))
        {
            //card.MoveTo(this);
            DropCard(card);
            eventData.Use();
        }
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        //if (eventData.pointerEnter != gameObject) return;
        //if (eventData.used) return;
        //Debug.Log("enterSlot: " + " on " + name + "/" + eventData.used);

        //Debug.Log((eventData.pointerEnter != gameObject) + " " + this);
        //eventData.Use();
        GetComponent<SpriteRenderer>().color = selectedColor;

        if (eventData.pointerDrag != null)
        {
            Card card = eventData.pointerDrag.GetComponent<Card>();

            if (CanDrop(card) == false)    // TODO drop logic
            {
                GetComponent<SpriteRenderer>().color = pickedColor;
            }
            else
            {
                GetComponent<SpriteRenderer>().color = selectedColor;
            }
        }
    }
    
    public virtual void OnPointerExit(PointerEventData eventData)
    {
        //if (eventData.pointerEnter != gameObject) return;
        //Debug.Log("exitSlot: " + " on " + name + "/" + eventData.pointerEnter);
        //if (eventData.poi != this) return;
        //eventData.Use();
        GetComponent<SpriteRenderer>().color = originalColor;
    }
}
