using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider2D))]
public class Card : MonoBehaviour, IPointerClickHandler//, IPointerEnterHandler, IPointerExitHandler
{
    public CardData cardData;
    public GameObject face;
    public GameObject back;
    public bool isFaceUp = false;
    public bool isFlipable = true;

    // back
    public Text typeText;
    // front
    public Text nameText;
    public SpriteRenderer artwork;
    public Text eventText;

    private bool isFlipping;
    private bool isMoving;

    //Vector3 origin;
    //Vector2 offset;
    //private bool isDragging;
    //private Collider2D[] zoneColliders;

    private void Awake()
    {
        if (cardData != null)
        {
            artwork.sprite = cardData.GetElement().artwork;

            //nameText.text = card.name;
            //artwork.sprite = card.artwork;
            //eventText.text = card.weather.ToString();
        }

        face.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        if (isFaceUp)
        {
            face.SetActive(true);
            back.SetActive(false);
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else
        {
            face.SetActive(false);
            back.SetActive(true);
        }
    }

    public void SetCardData(CardData cardData)
    {
        this.cardData = cardData;
        this.artwork.sprite = cardData.GetElement().artwork;
    }

    //public void SetArtwork(int elementIndex)
    //{
    //    if (cardData.elementIndex == elementIndex) return;
    //    cardData.elementIndex = elementIndex;
    //    artwork.sprite = cardData.GetArtwork();
    //}

//public void OnPointerEnter(PointerEventData eventData)
//{
//    Debug.Log("doit");
//}

//public void OnPointerExit(PointerEventData eventData)
//{
//    Debug.Log("yolo");
//}

//public void OnPointerEnter(PointerEventData eventData)
//{
//    eventData.Use();
//    Debug.Log("enterCard: " + eventData.selectedObject + " on " + name + "/" + eventData.pointerEnter);
//}

//public void OnPointerExit(PointerEventData eventData)
//{
//    eventData.Use();
//    Debug.Log("exitCard: " + eventData.selectedObject + " on " + name + "/" + eventData.pointerEnter);
//}

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GetComponent<IDragable>().IsDragging == false)
        {
            //eventData.Use();
            Flip();
        }
    }

    public void Flip()
    {
        if (isFlipable && isFlipping == false)
        {
            StartCoroutine(FlipCard());
        }
    }

    private IEnumerator FlipCard()
    {
        isFlipping = true;

        if (isFaceUp == false)
        {
            for (float i = 0f; i <= 180f; i += 10f)
            {
                transform.rotation = Quaternion.Euler(0f, i, 0f);

                if (i == 90f)
                {
                    face.SetActive(true);
                    back.SetActive(false);
                }

                yield return new WaitForSeconds(0.01f);
            }
        }
        else if (isFaceUp)
        {
            for (float i = 180f; i >= 0f; i -= 10f)
            {
                transform.rotation = Quaternion.Euler(0f, i, 0f);

                if (i == 90f)
                {
                    face.SetActive(false);
                    back.SetActive(true);
                }

                yield return new WaitForSeconds(0.01f);
            }
        }

        isFaceUp = !isFaceUp;
        isFlipping = false;
    }

    //public void MoveTo(CardSlot target)
    //{
    //    if (isMoving == false)
    //    {
    //        StartCoroutine(MoveCard(target));
    //    }
    //}

    //private IEnumerator MoveCard(CardSlot target)
    //{
    //    isMoving = true;
    //    //transform.SetParent(target.transform, false);

    //    while (target.transform.position != transform.position)
    //    {
    //        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 1f);
    //        transform.localScale = Vector3.MoveTowards(transform.localScale, target.transform.lossyScale * 0.9f, 1f / Vector3.Distance(target.transform.position, transform.position));
    //        // FIXME scale size by distacne
    //        //Debug.Log(transform.localScale + " " + target.transform.lossyScale);
    //        yield return new WaitForSeconds(Time.fixedDeltaTime);
    //    }

    //    //transform.localScale = target.transform.localScale * 0.9f;
    //    target.DropCard(this);
    //    isMoving = false;
    //}

}
