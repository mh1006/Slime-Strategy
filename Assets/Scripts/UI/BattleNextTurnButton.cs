using System;
using SlimeStrategy.BattleMap;
using UnityEngine;

namespace SlimeStrategy.UI
{
    public class BattleNextTurnButton : MonoBehaviour
    {
        private BattleManager _battleManager;
        
        private void Start()
        {
            _battleManager = GameObject.FindWithTag("BattleManager").GetComponent<BattleManager>();
        }

        public void OnButtonPress()
        {
            _battleManager.PlayerEndTurnAction();
        }
    }
}