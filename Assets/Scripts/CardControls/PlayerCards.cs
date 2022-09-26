using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mondo.MondoCards;

namespace Mondo.Controls {
    public class PlayerCards : MonoBehaviour
    {
        public void DestroyPlayerCards(string playerName)
        {
            foreach (Transform mondoCardTrans in transform)
            {
                MondoCard mondoCard = mondoCardTrans.GetComponent<MondoCard>();
                Debug.Log($"mondoCard.cardOwner: {mondoCard.cardOwner}");
                if (mondoCard.cardOwner == playerName)
                {
                    Destroy(mondoCard.gameObject);
                }
            }
        }

    }
}

