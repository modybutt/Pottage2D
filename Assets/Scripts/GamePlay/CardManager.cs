using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CardManager : MonoBehaviour
{
    public GameObject prefabCard;
    public CardStack rootStack;

    private List<CardData> cardDictionary;

    private void Awake()
    {
        cardDictionary = Resources.LoadAll<CardData>("Cards").ToList();
        //Debug.Log("cardDictionary: " + cardDictionary.Count);
    }

    private void Start()
    {
        //CreateCards(1, Instantiate<CardData>(Resources.Load<CardData>("Cards/Carrot")));
        //CreateCards(1, Instantiate<CardData>(Resources.Load<CardData>("Cards/Carrot")));

        //CardData data = Instantiate<CardData>(Resources.Load<CardData>("Cards/Carrot"));
        //data.elementIndex = 1;
        //CreateCards(1, data);

        //data = Instantiate<CardData>(Resources.Load<CardData>("Cards/Carrot"));
        //data.elementIndex = 1;
        //CreateCards(1, data);

        //data = Instantiate<CardData>(Resources.Load<CardData>("Cards/Carrot"));
        //data.elementIndex = 1;
        //CreateCards(1, data);

        //data = Instantiate<CardData>(Resources.Load<CardData>("Cards/Carrot"));
        //data.elementIndex = 2;
        //CreateCards(1, data);

        //data = Instantiate<CardData>(Resources.Load<CardData>("Cards/Carrot"));
        //data.elementIndex = 2;
        //CreateCards(1, data);

        CreateCards(30);  // FIXME debug only
    }

    public void CreateCards(int amount, CardData cardData = null)
    {
        bool random = (cardData == null);

        for (int i=0; i < amount; i++)
        {
            if (random)
            {
                cardData = Instantiate<CardData>(cardDictionary[Random.Range(0, cardDictionary.Count)], transform);
                cardData.elementIndex = Random.Range(0, cardData.elements.Length);
            }
            
            GameObject cardUI = Instantiate<GameObject>(prefabCard, transform);
            Card card = cardUI.GetComponent<Card>();
            
            card.SetCardData(cardData);
            cardUI.name = card.cardData.name + " --#" + i;  // FIXME debug only
            //card.SetArtwork(Random.Range(0, card.cardData.elements.Length));
            
            rootStack.DropCard(card);
        }
    }
}
