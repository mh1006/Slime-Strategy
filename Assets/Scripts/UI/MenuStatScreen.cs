using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SlimeStrategy.UI
{
    public class MenuStatScreen : MonoBehaviour
    {
        public GameObject ClassScr;
        public GameObject ElementScr;
        // Start is called before the first frame update
        public Sprite Warriorimage;
        public Sprite Knightimage;
        public Sprite Archerimage;
        public Sprite Mageimage;
        public Sprite TypeMimage;
        public Sprite TypePimage;


        // public Button charbtn;
        public GameObject CharnameText;
        private TMP_Text _CharnameText;
        public GameObject ChartypeText;
        private TMP_Text _ChartypeText;

        public Slider AttackS;
        public Slider PhysDS;
        public Slider MagDS;
        public Slider MovRS;
        public Slider ActRS;
        public Slider ActRS1;
        public Slider ActRS2;
        public Slider ActRS3;
        public Slider ActRS4;
        public Slider ActRS5;
        public Slider ActRS6;

        public GameObject AttackText;
        public GameObject PhysDText;
        public GameObject MagDText;
        public GameObject MovRText;
        public GameObject ActRText;

        private TMP_Text _AttackText;
        private TMP_Text _PhysDText;
        private TMP_Text _MagDText;
        private TMP_Text _MovRText;
        private TMP_Text _ActRText;

        // Start is called before the first frame update
        void Start()
        {
            ElementScr.SetActive(false);
        }
        void Awake()
        {
            _CharnameText = CharnameText.GetComponent<TMP_Text>();
            _ChartypeText = ChartypeText.GetComponent<TMP_Text>();
            _AttackText = AttackText.GetComponent<TMP_Text>();
            _PhysDText = PhysDText.GetComponent<TMP_Text>();
            _MagDText = MagDText.GetComponent<TMP_Text>();
            _MovRText = MovRText.GetComponent<TMP_Text>();
            _ActRText = ActRText.GetComponent<TMP_Text>();
        }
        // Update is called once per frame
        void Update()
        {

        }
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
        public void Warrior()
        {
            GameObject.Find("Char_Icon").GetComponent<Image>().sprite = Warriorimage;
            _CharnameText.text = "Warrior";
            GameObject.Find("type_icon").GetComponent<Image>().sprite = TypePimage;
            _ChartypeText.text = "Physical";
            AttackS.value = 13;
            PhysDS.value = 5;
            MagDS.value = 3;
            MovRS.value = 5;
            actionSelect(1);
            _AttackText.text = "13";
            _PhysDText.text = "5";
            _MagDText.text = "3";
            _MovRText.text = "5";
            _ActRText.text = "1";
        }
        public void Knight()
        {
            GameObject.Find("Char_Icon").GetComponent<Image>().sprite = Knightimage;
            _CharnameText.text = "Knight";
            GameObject.Find("type_icon").GetComponent<Image>().sprite = TypePimage;
            _ChartypeText.text = "Physical";
            AttackS.value = 11;
            PhysDS.value = 8;
            MagDS.value = 2;
            MovRS.value = 4;
            actionSelect(1);
            _AttackText.text = "11";
            _PhysDText.text = "8";
            _MagDText.text = "2";
            _MovRText.text = "4";
            _ActRText.text = "1";
        }
        public void Archer()
        {
            GameObject.Find("Char_Icon").GetComponent<Image>().sprite = Archerimage;
            _CharnameText.text = "Archer";
            GameObject.Find("type_icon").GetComponent<Image>().sprite = TypePimage;
            _ChartypeText.text = "Physical";
            AttackS.value = 11;
            PhysDS.value = 3;
            MagDS.value = 6;
            MovRS.value = 4;
            actionSelect(3);
            _AttackText.text = "11";
            _PhysDText.text = "3";
            _MagDText.text = "6";
            _MovRText.text = "4";
            _ActRText.text = "3";
        }
        public void Mage()
        {
            GameObject.Find("Char_Icon").GetComponent<Image>().sprite = Mageimage;
            _CharnameText.text = "Mage";
            GameObject.Find("type_icon").GetComponent<Image>().sprite = TypeMimage;
            _ChartypeText.text = "Magical";
            AttackS.value = 12;
            PhysDS.value = 2;
            MagDS.value = 5;
            MovRS.value = 4;
            actionSelect(2);
            _AttackText.text = "12";
            _PhysDText.text = "2";
            _MagDText.text = "5";
            _MovRText.text = "4";
            _ActRText.text = "2";
        }
        public void actionSelect(int action)
        {

            if (action == 6)
            {
                ActRS1.value = ActRS2.value = ActRS3.value = ActRS4.value = ActRS5.value = ActRS6.value = 1;
            }
            else if (action == 5)
            {
                ActRS1.value = ActRS2.value = ActRS3.value = ActRS4.value = ActRS5.value = 1;
                ActRS6.value = 0;
            }
            else if (action == 4)
            {
                ActRS1.value = ActRS2.value = ActRS3.value = ActRS4.value = 1;
                ActRS5.value = ActRS6.value = 0;
            }
            else if (action == 3)
            {
                ActRS1.value = ActRS2.value = ActRS3.value = 1;
                ActRS4.value = ActRS5.value = ActRS6.value = 0;
            }
            else if (action == 2)
            {
                ActRS1.value = ActRS2.value = 1;
                ActRS3.value = ActRS4.value = ActRS5.value = ActRS6.value = 0;
            }
            else if (action == 1)
            {
                ActRS1.value = 1;
                ActRS2.value = ActRS3.value = ActRS4.value = ActRS5.value = ActRS6.value = 0;
            }

        }
    }
}
