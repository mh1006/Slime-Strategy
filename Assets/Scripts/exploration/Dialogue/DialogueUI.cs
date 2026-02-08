using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SlimeStrategy.Exploration.Dialogue
{
    public class DialogueUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text textLabel;
        [SerializeField] public GameObject dialogueBox;
        [SerializeField] private Image characterSprite;

        private Player_move _playerMove;
        
        private DialogueObject _currentDialogue;

        private TypewriterEffect typewriterEffect;

        private void Start()
        {
            typewriterEffect = GetComponent<TypewriterEffect>();
            CloseDialogueBox();
            _playerMove = GameObject.FindWithTag("Player").GetComponent<Player_move>();
        }

        public void StartDialogue(DialogueObject dialogue)
        {
            CloseDialogueBox();
            _currentDialogue = dialogue;
            
            // Disable player movement
            _playerMove.DisableMovement();
            
            ShowDialogue(_currentDialogue);
        }

        public void ShowDialogue(DialogueObject dialogueObject)
        {
            dialogueBox.SetActive(true);
            if (dialogueObject.HasCharacter)
            {
                characterSprite.sprite = dialogueObject.character;
            }
            StartCoroutine(routine: StepThroughDialogue(dialogueObject));
        }

        private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
        {
            yield return new WaitForSeconds(0.2f);
            foreach (string line in dialogueObject.Dialogue)
            {
                yield return typewriterEffect.Run(line, textLabel);
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            }

            CloseDialogueBox();
            _playerMove.EnableMovement();

            if (!dialogueObject.loadSceneAtEnd) yield break;
            // var scene = SceneManager.GetSceneByName(dialogueObject.sceneToLoad);
            // if (!scene.IsValid()) yield break;
            SceneManager.LoadScene(dialogueObject.sceneToLoad);
        }

        private void CloseDialogueBox()
        {
            dialogueBox.SetActive(false);
            textLabel.text = string.Empty;
        }
    }
}