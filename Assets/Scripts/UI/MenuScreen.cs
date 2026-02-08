using UnityEngine;

namespace SlimeStrategy.UI
{
    public class MenuScreen : MonoBehaviour
    {


        public GameObject StatScr;
        public GameObject HowToScr;
        public GameObject ClassScr;
        // Start is called before the first frame update
        void Start()
        {
            StatScr.SetActive(false);
            HowToScr.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        public void Stat()
        {
            StatScr.SetActive(true);
        }
        public void HowTo()
        {
            HowToScr.SetActive(true);
        }
        public void ElementScreen()
        {
            ClassScr.SetActive(false);
        }
    }
}
