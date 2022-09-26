// MondoHand
// version = '0.1.4'
// repo = 'MondoMegabits-TCG'
// Author(s): rttgnck
// Description: Hand Controller for mechanics of the hand of cards
//
// Changelog:
//   v0.1.4  + Initial release
//
// ToDo:
//  + Rearrange cards in hand

using System.Collections.Generic;
using Mondo.MLogging;
using Mondo.Players;
using Mondo.MondoCards;
using UnityEngine;

namespace Mondo.Controls
{
    public class MondoHand : MonoBehaviour
    {
        public PlayerCamera playerCamera;
        public int handCardsCount;
        private int _handCardsCount;

        private GameObject handOfCards;
        public List<MondoCard> handCards = new List<MondoCard>();
        private Vector3 handPos;

        public bool Raised = false;

        public string handOwner;

        private MLogger _logger = new MLogger("MondoHand", false);

        public void CreateHandOfCards(List<MondoCard> _handCards)
        {
            _logger.Log("[HandOfCardsController] CREATE_HAND_OF_CARDS");

            handCards = _handCards;
            handCardsCount = handCards.Count;

            ArrangeHandCards();
        }

        public void ArrangeHandCards()
        {
            if (handCardsCount > 1)
            {
                float cardCenterOffset = 0.075f;
                float cardHandWidth = cardCenterOffset * (handCardsCount - 1);
                float firstCardOffset = -cardHandWidth / 2;

                _logger.Log($"{cardCenterOffset} : {cardHandWidth} : {firstCardOffset}");

                for (int c = 0; c < handCardsCount; c++)
                {

                    MondoCard card = handCards[c];
                    _logger.Log($"card.name: {card.name}");
                    card.name = $"MondoCard-Hand_{c + 1}";

                    float newX = firstCardOffset + (c * cardCenterOffset);
                    float newY = 0;
                    float newZ = c * 0.002f;

                    Vector3 newPos = new Vector3(newX, newY, newZ);

                    _logger.Log($"newPos: {newPos}");

                    card.transform.localPosition = newPos;
                    if (handOwner == "PlayerTwo")
                    {
                        card.transform.eulerAngles = new Vector3(225f, 0, 0);
                    }
                    else
                    {
                        card.transform.eulerAngles = new Vector3(225f, 180f, 0);
                    }
                    
                }
            }
        }

        public void DestroyHandCards() {
            for (int c = handCardsCount-1; c >= 0; c--)
            {
                _logger.Log($"c: {c}");
                MondoCard card = handCards[c];
                Destroy(card.gameObject);
                handCards.RemoveAt(c);
            }
            handCardsCount = _handCardsCount;
        }

        public void RaiseHand()
        {
            handPos = handOfCards.transform.localPosition;
            handPos.y = -0.30f;
            handOfCards.transform.localPosition = handPos;
            Raised = true;
        }
        public void LowerHand()
        {
            handPos = handOfCards.transform.localPosition;
            handPos.y = -0.40f;
            handOfCards.transform.localPosition = handPos;
            Raised = false;
        }

        private void Start()
        {
            handOfCards = gameObject;
            _handCardsCount = handCardsCount;
        }

        public void RemoveCardFromHand(int c)
        {
            handCards.RemoveAt(c);
            handCardsCount = handCards.Count;

            ArrangeHandCards();
        }
    }
}
