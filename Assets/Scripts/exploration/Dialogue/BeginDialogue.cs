using UnityEngine;

namespace SlimeStrategy.Exploration.Dialogue
{
    public class BeginDialogue : MonoBehaviour
    {
        [SerializeField] private DialogueUI dialogueUI;
        [SerializeField] private DialogueObject dialogueObject;
    
        private void OnTriggerEnter(Collider other)
        {
            CollectItems collectItems = other.GetComponent<CollectItems>();

            if (collectItems != null)
            {
                dialogueUI.StartDialogue(dialogueObject);
                gameObject.SetActive(false);
            }
        }
    }
}

