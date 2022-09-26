// MondoCard
// version = '0.1.4'
// repo = 'MondoMegabits-TCG'
// Author(s): rttgnck
// Description: Basic functionality of a single Mondo Card 
//
// Changelog:
//   v0.1.4  + Initial release
//
// ToDo:
//   + ?Allow cards to be turned sideways?
//   + In ReleaseCard, add a way to detect the collider it hits and set it
//     to belong to that transform, the play area objects for each player's cards

using UnityEngine;
using Mondo.MLogging;
using Mondo.Players;
using Mondo.Controls;
using UnityEngine.Video;

namespace Mondo.MondoCards
{
    public class MondoCard : MonoBehaviour
    {
        [Header("Mondo Card Settings")]
        [Tooltip("The Player the cards belong to")]
        [SerializeField]
        private Player _player;

        [Tooltip("The camera of the player")]
        [SerializeField]
        private PlayerCamera _playerCamera;

        [Tooltip("The camera of the player UI")]
        [SerializeField]
        private PlayerUiOverlay uiOverlay;

        [Tooltip("The UI card for zoom and information")]
        [SerializeField]
        private MondoCard_UI uiMondoCard;

        [Tooltip("The card this script controls")]
        [SerializeField]
        private MondoCard mondoCard;

        [Tooltip("Deck of cards this a card can belong to")]
        [SerializeField]
        private MondoDeck _deck;

        [Tooltip("Hand of cards this a card can belong to")]
        [SerializeField]
        private MondoHand _hand;

        [Tooltip("Object to hold cards in play")]
        [SerializeField]
        private GameObject _playerCards;

        [Tooltip("Game area the player can place cards in")]
        [SerializeField]
        private GameObject gameArea;

        private MLogger _logger;

        private MondoCard selectedCard;

        public string cardOwner;

        private Vector3 mOffset;
        private float mZCoord;

        public string CardFaceId;


        bool isDragging;
        bool isHovering;

        public void TurnOffCardPhysics()
        {
            Rigidbody cardRigidbody = mondoCard.GetComponent<Rigidbody>();
            cardRigidbody.isKinematic = true;
            cardRigidbody.constraints =
                RigidbodyConstraints.FreezePositionX |
                RigidbodyConstraints.FreezePositionY |
                RigidbodyConstraints.FreezePositionZ |
                RigidbodyConstraints.FreezeRotationX |
                RigidbodyConstraints.FreezeRotationY |
                RigidbodyConstraints.FreezeRotationZ;

            BoxCollider cardBoxCollider = mondoCard.GetComponent<BoxCollider>();
            cardBoxCollider.enabled = false;
        }

        public void TurnOnCardPhysics()
        {
            Rigidbody cardRigidbody = mondoCard.GetComponent<Rigidbody>();
            cardRigidbody.isKinematic = false;
            cardRigidbody.constraints =
                RigidbodyConstraints.FreezeRotationX |
                RigidbodyConstraints.FreezeRotationY |
                RigidbodyConstraints.FreezeRotationZ;

            BoxCollider cardBoxCollider = mondoCard.GetComponent<BoxCollider>();
            cardBoxCollider.enabled = true;
        }

        public void SetCardFace(string cardFaceUrl)
        {
            RenderTexture texture = new RenderTexture(844, 1420, 32);
            Renderer renderer = mondoCard.GetComponent<Renderer>();
            renderer.materials[1].mainTexture = texture;

            VideoPlayer videoPlayer = mondoCard.gameObject.AddComponent<VideoPlayer>();
            videoPlayer.playOnAwake = true;
            videoPlayer.clip = Resources.Load<VideoClip>(cardFaceUrl) as VideoClip;
            //videoPlayer.url = "https://cdn.mondomegabits.com/card/vid/full/0001.mp4";
            videoPlayer.waitForFirstFrame = false;
            videoPlayer.isLooping = true;
            videoPlayer.SetDirectAudioMute(0, true);
            videoPlayer.renderMode = VideoRenderMode.RenderTexture;
            videoPlayer.targetTexture = texture;
            videoPlayer.Play();
        }

        public void TurnOnCardFace()
        {
            RenderTexture texture = new RenderTexture(844, 1420, 32);
            Renderer renderer = mondoCard.GetComponent<Renderer>();
            renderer.materials[1].mainTexture = texture;

            VideoPlayer videoPlayer = mondoCard.gameObject.AddComponent<VideoPlayer>();
            videoPlayer.playOnAwake = true;
            videoPlayer.clip = Resources.Load<VideoClip>($"CardFiles/fullres/{CardFaceId}");
            //videoPlayer.url = "https://cdn.mondomegabits.com/card/vid/full/0001.mp4";
            videoPlayer.waitForFirstFrame = false;
            videoPlayer.isLooping = true;
            videoPlayer.SetDirectAudioMute(0, true);
            videoPlayer.renderMode = VideoRenderMode.RenderTexture;
            videoPlayer.targetTexture = texture;
            videoPlayer.Play();
        }

        public void FlipCard()
        {
            _logger.Log($"-----Flipping Over Card-----");
            _logger.Log($"cardOwner: {cardOwner}");
            if (cardOwner == "PlayerTwo")
            {
                mondoCard.transform.eulerAngles = new Vector3(270f, 0, 0);
            }
            else {
                mondoCard.transform.eulerAngles = new Vector3(270f, 0, 180f);
            }
        }

        private bool IsFaceUp()
        {
            _logger.Log($"card.transform.eulerAngles: {mondoCard.transform.eulerAngles}");
            if (mondoCard.transform.eulerAngles.x == 270) { return true; }
            else { return false; }
        }

        public void PickUpCard()
        {
            if (cardOwner == "PlayerTwo")
            {
                mondoCard.transform.eulerAngles = new Vector3(270f, 0, 0);
            }
            else
            {
                mondoCard.transform.eulerAngles = new Vector3(270f, 0, 180f);
            }

            mZCoord = _playerCamera.WorldToScreenPoint(mondoCard.transform.position).z;
            _logger.Log($"mZCoord: {mZCoord}");
            mOffset = mondoCard.transform.position - GetMouseWorldPos();
            mOffset.y = mOffset.y + 0.025f;

            isDragging = true;
            selectedCard = mondoCard;
        }

        public void HudShow()
        {
            _logger.Log("HudShow");
            bool cardStillInDeck = false;
            foreach (MondoCard card in _deck.deckCards)
            {
                if (card.name == gameObject.name)
                {
                    cardStillInDeck = true;
                }
            }

            if (!cardStillInDeck)
            {
                if ((mondoCard.transform.eulerAngles.x == 270f) || mondoCard.name.Contains("MondoCard-Hand"))
                {

                    if (mondoCard.name.Contains("MondoCard-Hand"))
                    {
                        _hand.RaiseHand();
                    }

                    isHovering = true;
                    uiOverlay.TurnOnUI(mondoCard);
                }

            }
        }


        public void HudHide()
        {
            _logger.Log("HudHide");
            if (uiOverlay.Enabled)
            {
                if (mondoCard.name.Contains("MondoCard-Hand"))
                {
                    _hand.LowerHand();
                }

                isHovering = false;
                uiOverlay.TurnOffUI();
            }
        }

        private void ReleaseCard()
        {
            _logger.Log("This is a test of the RELEASECARD");
            mondoCard.ChangeParent();
            
            isDragging = false;
            selectedCard.TurnOnCardPhysics();

            _logger.Log($"transform.name == selectedCard.name: {transform.name} == {selectedCard.name}");
            if (mondoCard.name.Contains("MondoCard-Hand"))
            {
                string name = selectedCard.name;

                string[] splitStr = name.Split("_");

                int c = int.Parse(splitStr[1]) - 1;
                _logger.Log($"[CardController] cardIndexHand c: {c}");
                _hand.RemoveCardFromHand(c);
            }

            int start = 0;

            mondoCard.name = $"MondoCard-InPlay_{start + 1}";

            foreach (Transform playerCard in _playerCards.transform)
            {
                if (playerCard.name == mondoCard.name)
                {
                    int c = int.Parse(selectedCard.name.Split("_")[1]);
                    mondoCard.name = $"MondoCard-InPlay_{c + 1}";
                }
            }

            selectedCard = null;
        }

        private void ReleaseCardToHand()
        {
            _logger.Log("This is a test of the RELEASECARDTOHAND");

            mondoCard.AddCardToHand();

            isDragging = false;

            selectedCard = null;
        }

        private void AddCardToHand()
        {
            mondoCard.transform.parent = _hand.transform;
            _hand.handCards.Add(mondoCard);
            _hand.handCardsCount += 1;
            mondoCard.name = $"MondoCard-Hand_{_hand.handCardsCount}";
            mondoCard.TurnOffCardPhysics();
            _hand.ArrangeHandCards();
            //if (cardOwner == "PlayerTwo")
            //{
            //    mondoCard.transform.eulerAngles = new Vector3(270f, 0, 0);
            //}
            //else
            //{
            //    mondoCard.transform.eulerAngles = new Vector3(270f, 0, 180f);
            //}
        }

        private void ChangeParent() {
            mondoCard.transform.parent = _playerCards.transform;
            if (cardOwner == "PlayerTwo")
            {
                mondoCard.transform.eulerAngles = new Vector3(270f, 0, 0);
            }
            else
            {
                mondoCard.transform.eulerAngles = new Vector3(270f, 0, 180f);
            }
        }

        private Vector3 GetMouseWorldPos()
        {
            Vector3 mousePoint = Input.mousePosition;
            mousePoint.z = mZCoord;

            return _playerCamera.ScreenToWorldPoint(mousePoint);
        }

        public bool SetupFirstCard(Player player, PlayerCamera playerCamera, MondoDeck deck, MondoHand hand, string playerName) {
            mondoCard = gameObject.GetComponent<MondoCard>();
            _logger = new MLogger($"{mondoCard.name}");
            _logger.Log($"SETTING UP FIRST CARD IN DECK");

            cardOwner = playerName;

            _player = player;
            _playerCamera = playerCamera;

            _deck = deck;
            _hand = hand;

            _playerCards = GameObject.Find("PlayerCards"); ;

            uiOverlay = _playerCamera.transform.GetChild(2).gameObject.GetComponent<PlayerUiOverlay>();
            uiMondoCard = uiOverlay.transform.GetChild(0).gameObject.GetComponent<MondoCard_UI>();

            mondoCard.transform.parent = _deck.transform;
            mondoCard.transform.localPosition = new Vector3(0, 0.1f, 0);

            if (cardOwner == "PlayerOne")
            {
                mondoCard.transform.eulerAngles = new Vector3(90, 0, 0);
                _logger.Log($"{transform.eulerAngles}");
            }

            if (cardOwner == "PlayerTwo")
            {
                mondoCard.transform.eulerAngles = new Vector3(90, 180, 0);
                _logger.Log($"{transform.eulerAngles}");
            }

            return true;
        }

        private void Start()
        {
            _logger = new MLogger("MondoCard");
        }

        void Update()
        {
            Ray ray = _playerCamera.ScreenPointToRay(Input.mousePosition);

            if (isDragging)
            {
                Vector3 newCardPosition = GetMouseWorldPos() + mOffset;
                newCardPosition.y = Mathf.Clamp(newCardPosition.y, 0.85f, 0.95f);
                selectedCard.transform.position = newCardPosition;
                if (Input.GetMouseButtonUp(0))
                {
                    ReleaseCard();
                }

                if (Physics.Raycast(ray, out RaycastHit _hit))
                {
                    if (_hit.collider.name.Contains("MondoCard-Hand"))
                    {
                        if (Input.GetMouseButtonUp(0))
                        {
                            ReleaseCardToHand();
                        }
                    }
                }
            }

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (mondoCard.name == hit.collider.name)
                {
                    if (Input.GetMouseButtonDown(0)) //Left-Click
                    {
                        mondoCard.PickUpCard();
                    }
                    else if (Input.GetMouseButtonDown(1)) //Left-Click
                    {
                        mondoCard.FlipCard();
                    }
                    else
                    {
                        if (!isDragging)
                        {
                            mondoCard.HudShow();
                        }
                    }
                }
                else
                {
                    if (isHovering)
                    {
                        mondoCard.HudHide();
                    }
                }
            }
        }
    }
}