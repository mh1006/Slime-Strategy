using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SlimeStrategy
{
    public class nextbattle : MonoBehaviour
    {
        int level = 1;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        public void clickNext()
        {
            level++;
            if (level == 2)
            {
                SceneManager.LoadScene("Explore2");
            }
            else if(level == 3)
            {
                SceneManager.LoadScene("Explore3");
            }
            else if (level == 4)
            {
                SceneManager.LoadScene("Explore4");
            }

        } 
    }
}
