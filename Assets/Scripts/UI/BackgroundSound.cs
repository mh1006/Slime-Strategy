using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SlimeStrategy
{
    public class BackgroundSound : MonoBehaviour
    {
        public Sprite Muteimage;
        public Sprite Unmuteimage;
        [SerializeField] private GameObject MuteBtn;
        private bool Ismute=false;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (Ismute == false)
            {
                unmute();
            }
            else if (Ismute == true)
            {
                mute();
            }
        }
        public void click()
        {
            if (Ismute == false) {
                Ismute = true;
            }
            else if (Ismute == true)
            {
                Ismute = false;
            }

        }
        void mute()
        {
            MuteBtn.GetComponent<Image>().sprite = Muteimage;
            /*music.Pause();*/
        }
        void unmute()
        {
            MuteBtn.GetComponent<Image>().sprite = Unmuteimage;
            /*music.Play();*/
        }
    }
}
