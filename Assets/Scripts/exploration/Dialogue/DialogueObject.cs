using UnityEngine;

namespace SlimeStrategy.Exploration.Dialogue
{
    [CreateAssetMenu(menuName = "Dialogue/DialogueObject")]
    public class DialogueObject : ScriptableObject
    {
        [Tooltip("Does this dialogue need to load a scene when it completes?")]
        [SerializeField] public bool loadSceneAtEnd;
        [Tooltip("Exact name of scene to load")]
        [SerializeField] public string sceneToLoad;

        [Tooltip("Sprite to show during this dialogue")]
        [SerializeField] public Sprite character;
        [SerializeField][TextArea] private string[] dialogue;

        public string[] Dialogue => dialogue;

        public bool HasCharacter => !ReferenceEquals(character, null);
    }
}
