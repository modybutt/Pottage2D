using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public static class CardRules
{
    public static bool CanCombine(this Card self, Card[] others) // FIXME: combined by same elementIndex
    {
        int leftStacks = self.cardData.GetElement().maxStacks;
        
        foreach (Card other in others)
        {
            if (self == other) return false;
            if (self.isFaceUp != other.isFaceUp) return false;

            if (self.cardData == null && other.cardData == null) return true;
            if (self.cardData == null || other.cardData == null) return false;
            if (self.cardData.groupId != other.cardData.groupId) return false;
            
            if (self.cardData.elementIndex == other.cardData.elementIndex)
            {
                if (--leftStacks <= 0) return false;
            }
        }

        return true;
    }










    //public static bool CanCombine(this Card self, Card other, Card[] combined) // FIXME: combined by same elementIndex
    //{
    //    if (self == other) return false;
    //    if (self == null || other == null) return false;
    //    if (self.isFaceUp != other.isFaceUp) return false;

    //    if (self.isFaceUp == false) return false;   // disable faceDown combine

    //    if (self.cardData == null && other.cardData == null) return true;
    //    if (self.cardData == null || other.cardData == null) return false;

    //    if (self.cardData.groupId == other.cardData.groupId)
    //    {
    //        int maxStacks = self.cardData.GetElement().maxStacks;
    //        if (maxStacks != 0)
    //        {
    //            int count = 0;
    //            foreach (Card com in combined)
    //            {
    //                if (self.cardData.elementIndex == com.cardData.elementIndex)
    //                {
    //                    if (++count >= maxStacks) return false;
    //                }
    //            }
    //        }

    //        return true;
    //    }

    //    return false;
    //}
}
