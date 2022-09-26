// MondoDeck
// version = '0.1.4'
// repo = 'MondoMegabits-TCG'
// Author(s): rttgnck
// Description: Deck Controller for mechanics of the deck of cards
//
// Changelog:
//   v0.1.4  + Initial release
//
// ToDo:
//  + Add card back to deck
//  + Reset deck, recall hand cards
//  + When a card is picked up from deck, allow it to be added to hand or to the table

using System.Collections;
using System.Collections.Generic;
using Mondo.MLogging;
using Mondo.MondoCards;
using Mondo.Players;
using UnityEngine;
using UnityEditor;
using System.IO;
using SimpleFileBrowser;

namespace Mondo.Controls
{
    public class MondoDeck : MonoBehaviour
    {
        [Header("Mondo Card Settings")]
        [Tooltip("The Player the cards belong to")]
        [SerializeField]
        private Player _player;

        [Tooltip("The camera of the player")]
        [SerializeField]
        private PlayerCamera _playerCamera;

        [Tooltip("Card to use as a prefab for deck cards")]
        [SerializeField]
        private MondoCard prefabMondoCard;

        [Tooltip("The card deck of the player")]
        [SerializeField]
        private MondoDeck deck;

        [Tooltip("The number of cards in deck (min 40)")]
        [SerializeField]
        private int deckCardCount = 40;

        [Tooltip("The card hand of the player")]
        [SerializeField]
        private MondoHand _hand;


        //PRIVATE VARS
        private bool testing = true;
        public List<MondoCard> deckCards = new List<MondoCard>();

        private MondoCard topCard;

        private string deckOwner;

        private string mondoDeckFilePath;

        private List<string> _deckCardList = new List<string>();
        private List<string> deckCardList = new List<string>();

        private int shuffleCount = 3;

        private MLogger _logger;

        //METHODS
        private IEnumerator CreateDeck()
        {
            _logger.Log("[DeckController] CREATE_DECK");
            MondoCardDictionary mcd = deck.GetComponent<MondoCardDictionary>();

            for (int i = deckCardCount; i > 0; i--)
            {
                MondoCard newMondoCard = Instantiate(prefabMondoCard);
                newMondoCard.name = $"MondoCard_{i}";
                newMondoCard.cardOwner = deckOwner;

                newMondoCard.transform.parent = deck.transform;
                if (deckOwner == "PlayerTwo")
                {
                    newMondoCard.transform.eulerAngles = new Vector3(90, 180, 0);
                }

                newMondoCard.transform.localScale = prefabMondoCard.transform.localScale;
                newMondoCard.TurnOffCardPhysics();
                newMondoCard.transform.localPosition = new Vector3(0, 0.0007f * i, 0);

                string cardId = _deckCardList[i - 1];
                _deckCardList.RemoveAt(i-1);

                newMondoCard.CardFaceId = cardId;
                deckCards.Add(newMondoCard);
            }

            Destroy(prefabMondoCard.gameObject);

            if (testing == true)
            {
                _logger.Log($"-----CreatedDeck - DeckCards - Preshuffle-----", true);
                for (int c = 0; c < deckCards.Count; c++)
                {
                    MondoCard card = deckCards[c];
                    
                    MondoCardModel mcm = mcd.GetInfoByCardId(card.CardFaceId);
                    _logger.Log($"{card.name}: id: {card.CardFaceId}: {mcm.id}, {mcm.name}", true);
                }
                _logger.Log($"-----END LIST - CreatedDeck - DeckCards - Preshuffle-----", true);
            }

            for (int s = 0; s < shuffleCount; s++) {
                StartCoroutine(ShuffleDeck());

                yield return new WaitForSeconds(1.5f);
            }
            
            TransferCardsToHand();
        }

        private void TransferCardsToHand() {
            List<MondoCard> handCards = new List<MondoCard>();
            int handCardsCount = _hand.handCardsCount;

            _logger.Log($"[DeckController] handCardsCount: {handCardsCount}");

            for (int c = 0; c < handCardsCount; c++)
            {
                MondoCard deckCard = deckCards[0];
                _logger.Log($"[DeckController] oldDeckCardName: {deckCard.name} => newHandCard: {$"MondoCard-Hand_{c + 1}"}");

                deckCard.name = $"MondoCard-Hand_{c+1}";
                Vector3 tempScale = deckCard.transform.localScale;
                deckCard.transform.parent = _hand.transform;
                deckCard.transform.localScale = tempScale;
                deckCard.TurnOffCardPhysics();

                var r = Random.Range(1, 101);
                var _r = r.ToString().PadLeft(4, '0');

                //deckCard.SetCardFace($"CardFiles/fullres/{_r}");
                deckCard.TurnOnCardFace();
                BoxCollider cardBoxCollider = deckCard.GetComponent<BoxCollider>();
                cardBoxCollider.enabled = true;
                handCards.Add(deckCard);
                deckCards.RemoveAt(0);
            }

            _hand.CreateHandOfCards(handCards);
        }

        private IEnumerator ShuffleDeck()
        {
            yield return new WaitForSeconds(0.25f);
            _logger.Log($"-----Shuffling Deck-----");

            for (int c = 0; c < deckCards.Count; c++)
            {
                MondoCard tempGo = deckCards[c];
                int randomIndex = Random.Range(c, deckCards.Count);
                deckCards[c] = deckCards[randomIndex];

                deckCards[randomIndex] = tempGo;
            }

            if (testing == true)
            {
                _logger.Log($"-----List of DeckCards-----");
                for (int c = 0; c < deckCards.Count; c++)
                {
                    MondoCard card = deckCards[c];
                    _logger.Log($"[DeckCards] {c}: {card.name}");
                }
            }

            for (int c = 0; c < deckCards.Count; c++)
            {
                MondoCard tempGo = deckCards[c];
                tempGo.TurnOnCardPhysics();
                tempGo.transform.localPosition = new Vector3(0, 0.05f * (deckCards.Count - c), 0);
                tempGo.transform.eulerAngles = new Vector3(90, 0, 0);
                if (deckOwner == "PlayerTwo")
                {
                    tempGo.transform.eulerAngles = new Vector3(90, 180, 0);
                }
            }

            yield return new WaitForSeconds(1.0f);

            for (int c = 0; c < deckCards.Count; c++)
            {
                MondoCard tempGo = deckCards[c];
                tempGo.TurnOffCardPhysics();
                tempGo.transform.localPosition = new Vector3(0, 0.0007f * (deckCards.Count - c), 0);
                tempGo.transform.eulerAngles = new Vector3(90, 0, 0);
                if (deckOwner == "PlayerTwo")
                {
                    tempGo.transform.eulerAngles = new Vector3(90, 180, 0);
                }
            }

            if (testing == true)
            {
                _logger.Log($"-----List of DeckCards-----");
                for (int c = 0; c < deckCards.Count; c++)
                {
                    MondoCard card = deckCards[c];
                    _logger.Log($"[DeckCards] {c}: {card.name}");
                }
            }
        }

        private void TakeTopCard() {
            _logger.Log($"-----Taking Top Card-----");
            topCard = deckCards[0];
            _logger.Log($"topCard.name: {topCard.name}");

            deckCards.RemoveAt(0);

            var r = Random.Range(1, 101);
            var _r = r.ToString().PadLeft(4, '0');

            //topCard.SetCardFace($"CardFiles/fullres/{_r}");
            topCard.TurnOnCardFace();
            topCard.FlipCard();
            topCard.PickUpCard();
        }

        public bool SetupDeck(Player player, PlayerCamera playerCamera, MondoHand hand, string playerName) {
            _logger = new MLogger($"MondoDeck-{playerName}", false);
            _logger.Log("SETTING UP DECK");

            deckOwner = playerName;

            _player = player;

            GameObject deckGO = gameObject;
            deck = deckGO.GetComponent<MondoDeck>();

            deckCardCount = Mathf.Clamp(deckCardCount, 40, 300);
            _playerCamera = playerCamera;
            _hand = hand;

            prefabMondoCard = Instantiate(Resources.Load("Prefabs/MondoCard", typeof(MondoCard))) as MondoCard;
            prefabMondoCard.name = $"MondoCard_1";

            Vector3 tempScale = prefabMondoCard.transform.localScale;

            prefabMondoCard.transform.parent = deck.transform;

            prefabMondoCard.transform.localScale = tempScale;

            bool completeCardSetup = prefabMondoCard.SetupFirstCard(player, playerCamera, deck, hand, playerName);

            _logger.Log($"{deckCards}");

            _logger.Log("COMPLETE SETTING UP DECK");

            if (completeCardSetup) {
                return true;
            } else {
                return false;
            }
        }

        private void ReadMondoDeckFile() {
            StreamReader reader = new StreamReader(mondoDeckFilePath);

            while (!reader.EndOfStream)
            {
                string cardLine = reader.ReadLine();

                string[] cardLineData = cardLine.Split("|");

                int cardCount = int.Parse(cardLineData[0]);

                string cardName = cardLineData[1];

                MondoCardDictionary mcd = deck.GetComponent<MondoCardDictionary>();
                MondoCardModel mcm = mcd.GetInfoByCardName(cardName);

                for (int c = 0; c < cardCount; c++) {
                    string cardId = mcm.id.ToString().PadLeft(4, '0');
                    deckCardList.Add(cardId);
                }

                _logger.Log($"[ReadMondoDeckFile] mcm.id: {mcm.id} | .name: {mcm.name} | .rarity: {mcm.rarity}");
            }

            for (int c = 0; c < deckCardList.Count; c++)
            {
                _logger.Log($"[ReadMondoDeckFile] deckCardList[c]: {deckCardList[c]}");
            }

            deckCardCount = deckCardList.Count;
            _deckCardList = deckCardList;

            reader.Close();
        }
        IEnumerator ShowLoadDialogCoroutine()
        {
            // Show a load file dialog and wait for a response from user
            // Load file/folder: both, Allow multiple selection: true
            // Initial path: default (Documents), Initial filename: empty
            // Title: "Load File", Submit button text: "Load"
            yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Files and Folders", "Load");

            // Dialog is closed
            // Print whether the user has selected some files/folders or cancelled the operation (FileBrowser.Success)
            Debug.Log(FileBrowser.Success);

            if (FileBrowser.Success)
            {
                Debug.Log(FileBrowser.Result[0]);
                mondoDeckFilePath = FileBrowser.Result[0];
                ReadMondoDeckFile();
                StartCoroutine(CreateDeck());

                //// Print paths of the selected files (FileBrowser.Result) (null, if FileBrowser.Success is false)
                //for (int i = 0; i < FileBrowser.Result.Length; i++) {}
                //    Debug.Log(FileBrowser.Result[i]);


                //// Read the bytes of the first file via FileBrowserHelpers
                //// Contrary to File.ReadAllBytes, this function works on Android 10+, as well
                //byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result[0]);

                //// Or, copy the first file to persistentDataPath
                //string destinationPath = Path.Combine(Application.persistentDataPath, FileBrowserHelpers.GetFilename(FileBrowser.Result[0]));
                //FileBrowserHelpers.CopyFile(FileBrowser.Result[0], destinationPath);
            }
        }

        private void StartFileBrowser() {
            // Set filters (optional)
            // It is sufficient to set the filters just once (instead of each time before showing the file browser dialog), 
            // if all the dialogs will be using the same filters
            FileBrowser.SetFilters(true, new FileBrowser.Filter("MondoDeckFile", ".mm"), new FileBrowser.Filter("Text Files", ".txt"));

            // Set default filter that is selected when the dialog is shown (optional)
            // Returns true if the default filter is set successfully
            // In this case, set Images filter as the default filter
            FileBrowser.SetDefaultFilter(".mm");

            // Set excluded file extensions (optional) (by default, .lnk and .tmp extensions are excluded)
            // Note that when you use this function, .lnk and .tmp extensions will no longer be
            // excluded unless you explicitly add them as parameters to the function
            FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");

            // Add a new quick link to the browser (optional) (returns true if quick link is added successfully)
            // It is sufficient to add a quick link just once
            // Name: Users
            // Path: C:\Users
            // Icon: default (folder icon)
            //FileBrowser.AddQuickLink("Users", "C:\\Users", null);

            // Show a save file dialog 
            // onSuccess event: not registered (which means this dialog is pretty useless)
            // onCancel event: not registered
            // Save file/folder: file, Allow multiple selection: false
            // Initial path: "C:\", Initial filename: "Screenshot.png"
            // Title: "Save As", Submit button text: "Save"
            // FileBrowser.ShowSaveDialog( null, null, FileBrowser.PickMode.Files, false, "C:\\", "Screenshot.png", "Save As", "Save" );

            // Show a select folder dialog 
            // onSuccess event: print the selected folder's path
            // onCancel event: print "Canceled"
            // Load file/folder: folder, Allow multiple selection: false
            // Initial path: default (Documents), Initial filename: empty
            // Title: "Select Folder", Submit button text: "Select"
            // FileBrowser.ShowLoadDialog( ( paths ) => { Debug.Log( "Selected: " + paths[0] ); },
            //						   () => { Debug.Log( "Canceled" ); },
            //						   FileBrowser.PickMode.Folders, false, null, null, "Select Folder", "Select" );

            // Coroutine example
            StartCoroutine(ShowLoadDialogCoroutine());
        }


        void Update()
        {
            Ray ray = _playerCamera.ScreenPointToRay(Input.mousePosition);

            RaycastHit[] hits = Physics.RaycastAll(ray);

            System.Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));

            for (int i = 0; i < hits.Length; i++)
            {
                if (deck.name == hits[i].collider.gameObject.name)
                {
                    if (Input.GetMouseButtonDown(0))
                    { //Left-Click 
                        if (deckCards.Count == 0)
                        {
                            StartFileBrowser();
                            //mondoDeckFilePath = SimpleFileBrowser.FileBrowser("Upload MondoDeck File", "", "mm");
                            //mondoDeckFilePath = GUILayoutUtility.OpenFilePanel("Upload MondoDeck File", "", "mm");

                            //ReadMondoDeckFile();

                            //StartCoroutine(CreateDeck());
                        }
                        else
                        {
                            TakeTopCard();
                        }
                    }
                    else if (Input.GetMouseButtonDown(1))
                    { //Right-Click
                        StartCoroutine(ShuffleDeck());
                    }
                    else if (Input.GetMouseButtonDown(2))
                    { //Middle-Click
                        //TurnCardSideways();
                        continue;
                    }
                    else
                    { //All Other states
                        continue;
                    }
                }

            }
        }
    }
}