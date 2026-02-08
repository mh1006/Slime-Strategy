using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SlimeStrategy.UI
{
    public class MenuBtn : MonoBehaviour
    {
        //[SerializeField] private MenuStatScreen statscreen;
        //[SerializeField] private MenuScreen menuscreen;
        MenuStatScreen statscreen;
        MenuScreen menuscreen;

        public GameObject StatScr;
        public GameObject HowToScr;
        /*
        public GameObject ClassScr;
        public GameObject ElementScr;
        */
        public GameObject StrategyScr;
        public GameObject ExploreScr;
        public GameObject CreditScr;

        // Start is called before the first frame update
        void Start()
        {
            statscreen = GameObject.Find("Stat_infom_Panel").GetComponent<MenuStatScreen>();
            menuscreen = GameObject.Find("Menu_Panel").GetComponent<MenuScreen>();
            StatScr.SetActive(false);
            HowToScr.SetActive(false);
            StrategyScr.SetActive(false);
            ExploreScr.SetActive(false);
            CreditScr.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }
        public void StartPlay()
        {
            SceneManager.LoadScene("Explore1-Final");
        }
        /*public void SettingScreen()
        {
            SettingScr.SetActive(true);
        }*/
        public void Stat()
        {
            StatScr.SetActive(true);
        }
        public void HowTo()
        {
            HowToScr.SetActive(true);
        }
        public void ExploreScreen()
        {
            StrategyScr.SetActive(false);
            ExploreScr.SetActive(true);
        }
        public void StrategyScreen()
        {
            ExploreScr.SetActive(false);
            StrategyScr.SetActive(true);
        }
       /*
        public void ClassScreen()
        {
            ElementScr.SetActive(false);
            ClassScr.SetActive(true);
        }
        public void ElementScreen()
        {
            ClassScr.SetActive(false);
            ElementScr.SetActive(true);
        }
       */
        public void CreditScreen()
        {
            CreditScr.SetActive(true);
        }
        public void Quitbtn()
        {
            Application.Quit();
        }
    }
    }
