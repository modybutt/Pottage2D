using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Cards/Default")]
public class CardData : ScriptableObject
{
    [System.Serializable]
    public struct CardElement
    {
        public Sprite artwork;
        public int maxStacks;
    }

    //public int id;
    public new string name;
    public int groupId;
    public CardElement[] elements;
    public int elementIndex;

    //public int groupOrder;
    //public int groupStack;

    public bool isEvent;
    public bool isAction;
    public int weather;
    
    public CardElement GetElement()
    {
        return elements[elementIndex];
    }
}
