using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoard : MonoBehaviour
{
    public PlayerBoardData playerBoardData;

    public Sprite background;
    public CardStack stackIn;
    public CardStack stackOut;
    public CardDeck cardDeck;
    public CardGarden cardGarden;
    //public CardSlot eventSlot;
    //public CardSlot actionSlot;
    //public CardSlot basketSlot;
    //public CardSlot heapSlot;

    void Start()
    {
        //background = playerBoardData.background;

    }

}
