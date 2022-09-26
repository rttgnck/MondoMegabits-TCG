// MondoCardDictionary
// version = '0.1.0'
// repo = 'MondoMegabits-TCG'
// Author(s): rttgnck
// Description: Setup a class for reading in information about and getting information about a card 
//
// Changelog:
//   v0.1.0  + Initial release
//
// ToDo:
//   + ...

using UnityEngine;

namespace Mondo.MondoCards
{
    [System.Serializable]
    public class MondoCardsJSON
    {
        public MondoCardModel[] mondoCards;
    }

    public class MondoCardDictionary : MonoBehaviour
    {
        public TextAsset mondoCardDictFile;

        private MondoCardsJSON mondoCardJson;

        public MondoCardModel GetInfoByCardName(string cardName) {
            MondoCardModel mondoCardInfo = new MondoCardModel();
            foreach (MondoCardModel mondoCard in mondoCardJson.mondoCards)
            {
                if (mondoCard.name == cardName) {
                    mondoCardInfo = mondoCard;
                }
            }
            return mondoCardInfo;
        }

        public MondoCardModel GetInfoByCardId(string cardFaceId)
        {
            MondoCardModel mondoCardInfo = new MondoCardModel();
            foreach (MondoCardModel mondoCard in mondoCardJson.mondoCards)
            {
                int cardId = int.Parse(cardFaceId);
                if (mondoCard.id == cardId)
                {
                    mondoCardInfo = mondoCard;
                }
            }
            return mondoCardInfo;
        }

        public void Start()
        {
            Debug.Log($"[SEARCHFUNCTION] Testing MondoCardDictionary");
            mondoCardJson = JsonUtility.FromJson<MondoCardsJSON>(mondoCardDictFile.text);
        }
    }

}

