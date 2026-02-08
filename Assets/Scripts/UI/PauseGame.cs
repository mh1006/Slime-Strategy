using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SlimeStrategy
{
    public class PauseGame : MonoBehaviour
    {
        public GameObject PauseScr;
        public GameObject StatScr;
        // Start is called before the first frame update
        void Start()
        {
            PauseScr.SetActive(false);
            StatScr.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        public void PauseBtn()
        {
            PauseScr.SetActive(true);
            /*Pause the game*/
        }
        public void ContinueBtn()
        {
            /*Continue the game*/
            PauseScr.SetActive(false);
        }
        public void RestartBtn()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            // PauseScr.SetActive(false);
            // JS: Don't believe is necessary because the scene is reloaded 
        }
        public void StatBtn()
        {
            StatScr.SetActive(true);
        }
        public void ExitBtn()
        {
            /*Turn off the game*/
            Application.Quit();
        }
    }
}
