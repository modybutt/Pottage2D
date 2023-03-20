using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider2D))]
public class IDragable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler//, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public float layerDelta = -0.1f;

    public bool IsDragging { get; private set; }
    //private new BoxCollider2D collider;

    private Vector3 origin;
    private Vector3 offset;

    private void Awake()
    {
        //collider = GetComponent<BoxCollider2D>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("beginDrag: " + name);
        IsDragging = true;
        //eventData.selectedObject = gameObject;
        //collider.enabled = false;

        origin = transform.position;
        offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - origin + (Vector3.back * layerDelta);

        //Camera.main.GetComponent<Physics2DRaycaster>().maxRayIntersections--;
        Camera.main.GetComponent<Physics2DRaycaster>().eventMask &= ~LayerMask.GetMask("Cards");
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("onDrag");
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePos - offset;

        //foreach (GameObject o in eventData.hovered)
        //{
        //    Debug.Log(o.name);
        //}
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("endDrag: " + name);
        //if (eventData.used) return;
        
        //if (eventData.selectedObject != null)
        if (eventData.used == false)
        {
            transform.position = origin;
        }

        //collider.enabled = true;
        //eventData.selectedObject = null;
        Camera.main.GetComponent<Physics2DRaycaster>().eventMask |= LayerMask.GetMask("Cards");
        IsDragging = false;

        //GetComponent<BoxCollider2D>().edgeRadius = 1;
        //Debug.Log(GetComponent<BoxCollider2D>().gameObject.name);
        //Camera.main.GetComponent<Physics2DRaycaster>().maxRayIntersections++;
    }

    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    //Debug.Log("pointerDown");
    //}

    //public void OnPointerUp(PointerEventData eventData)
    //{
    //    //Debug.Log("pointerUp");
    //}

    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    //Debug.Log("pointerClick");
    //}

    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    //Debug.Log("pointerEnter");
    //}

    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    //Debug.Log("pointerExit");
    //}
}
