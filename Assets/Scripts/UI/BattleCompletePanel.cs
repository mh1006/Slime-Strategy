using System;
using SlimeStrategy.BattleMap;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SlimeStrategy.UI
{
    public class BattleCompletePanel : MonoBehaviour
    {
        [SerializeField] private StatScreen statScreen;
        [SerializeField] private GameObject battleCompletePanel;
        [SerializeField] private TextMeshProUGUI battleOutcomeText;
        [SerializeField] private TextMeshProUGUI buttonText;

        private BattleManager BattleManager => statScreen.battleManager;
        private BattleState BattleState => BattleManager.State;

        private void Update()
        {
            if (battleCompletePanel.activeInHierarchy) return;
            
            var isBattleOver = BattleState == BattleState.Victory || BattleState == BattleState.Defeat;
            if (!isBattleOver) return;
            
            if (BattleState == BattleState.Victory)
            {
                battleOutcomeText.SetText("Battle victory!");
                buttonText.SetText("Continue");
            }
            else
            {
                battleOutcomeText.SetText("Battle defeat!");
                buttonText.SetText("Retry");
            }

            battleCompletePanel.SetActive(true);
        }

        public void ButtonPressed()
        {
            if (BattleState == BattleState.Victory)
            {
                SceneManager.LoadScene(BattleManager.nextScene);
            }
            else
            {
                var sceneID = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene(sceneID);
            }
        }
    }
}