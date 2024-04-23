using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ADHD.Mat.GeoMathGame{
    public class GameController : MonoBehaviour
    {      
        public static GameController Instance;

        [SerializeField] LevelDifficulty levelDifficulty;

        [SerializeField] List<GameObject> shapes;

        public GameObject buttonObj;
        [SerializeField]
        GameObject buttonSpawn;
       
        [SerializeField]
        GameObject numberObj;
      
        private GameObject shownObj;

        [SerializeField]
        GameObject spawnParent;

        [SerializeField]
        AudioClip crossSfx;
        [SerializeField]
        AudioClip checkSfx;
        [SerializeField] AudioSource audioSource;

        [SerializeField]
        GameObject point;

        #region Clock
        public float totalTime = 15f; // Başlangıçta ayarlanacak toplam süre
        public bool autoStart = false; // Zamanlayıcının otomatik başlamasını sağlar

        private float currentTime; // Şu anki geçen süre
        private bool isTimerRunning = false; // Zamanlayıcının çalışıp çalışmadığını belirten bayrak

        [SerializeField] TextMeshProUGUI timerText;
        [SerializeField] AudioSource timerAudioSource;

        #endregion



        private GameObject btn;
        private Button trueButton;


        string deger;
        private float _point = 0;
        float incrementPoint = 1;
        int correct= 0, wrong= 0;

        int difficulty= 0;

        private void Awake()
        {
            DontDestroyOnLoad(this);

            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        void Start()
        {
            CloneButton();

            ShapeSelect();
        }

        void CloneButton()
        {
            DestroyClonedButton();
            for (int i = 0; i < ButtonCount(); i++)
            {
                Instantiate(buttonObj, buttonSpawn.transform);
            }
        }

        void DestroyClonedButton()
        {
            foreach (Transform child in buttonSpawn.transform)
            {
                Destroy(child.gameObject);
            }
        }

        private int ButtonCount() 
        {
            return difficulty + 3;
        }

        private void UpdateScore(float currentPoint, float increment)
        {
            float time = levelDifficulty.difficulties[difficulty].addTime;
            AddTime(time);

            _point = currentPoint + increment;
            point.transform.GetComponent<TextMeshProUGUI>().text = "Point: " + _point.ToString();
            if (levelDifficulty.difficulties[difficulty].levelUpControlValue <= _point)
            {
                difficulty++;
                CloneButton();
            }
        }

        void ShapeSelect()
        {
            int bound = shapes.Count;
            int rnd = Random.Range(0, bound);

            double minValue = levelDifficulty.difficulties[difficulty].minValue;
            double maxValue = levelDifficulty.difficulties[difficulty].maxValue;
            bool isDouble = levelDifficulty.difficulties[difficulty].isDouble;

            GenerateLevel(rnd, isDouble, minValue, maxValue);
                      
            calc(rnd);
        }

        void GenerateLevel(int rnd, bool isDouble, double minValue, double maxValue)
        {
            shownObj = shapes[rnd];
            int kenarSayisi = shownObj.GetComponent<Shape>().KENARLAR.Length;

            var scr = shownObj.GetComponent<Shape>();
            var obj = Instantiate(shownObj, spawnParent.transform);

            System.Collections.IList control;
            if (isDouble)
            {
                control = new List<double>();
            }
            else
            {
                control = new List<int>();
            }

            double doubleValue;
            int intValue;

            for (int i = 0; i < kenarSayisi; i++)
            {
                if (isDouble)
                {
                    doubleValue = Random.Range((float)minValue, (float)maxValue);
                    while (control.Contains(doubleValue))
                    {
                        doubleValue = Random.Range((float)minValue, (float)maxValue);
                    }
                    control.Add(doubleValue);
                    scr.KENARLAR[i] = doubleValue;
                }
                else
                {
                    intValue = Random.Range((int)minValue, (int)maxValue + 1);
                    while (control.Contains(intValue))
                    {
                        intValue = Random.Range((int)minValue, (int)maxValue + 1);
                    }
                    control.Add(intValue);
                    scr.KENARLAR[i] = intValue;
                }
            }

            System.Array.Sort(scr.KENARLAR);

            for (int i = 0; i < kenarSayisi; i++)
            {
                var num = Instantiate(numberObj, obj.transform.GetChild(i).transform);
                if (isDouble)
                {
                    num.GetComponentInChildren<TextMeshProUGUI>().text = scr.KENARLAR[i].ToString("#0.0");
                }
                else
                {
                    num.GetComponentInChildren<TextMeshProUGUI>().text = scr.KENARLAR[i].ToString();
                }
            }
        }


        void calc(int rnd)
            {           
             if (rnd == 0)//dikdörtgen
                {
                    double value = 0;
                    for (int i = 0; i <2 ; i++)
                    {                    
                        double kenar = 2 * shownObj.GetComponent<Shape>().KENARLAR[i]; 
                        value += kenar;                    
                    }
                CreateTextInButton(value);
                    Debug.Log("dik hesap: " + value);
                }
            if (rnd == 1)//dik ikiz kenar ücgen
            {                              
                  double kenar = 2 * shownObj.GetComponent<Shape>().KENARLAR[0];
                  double value = kenar+ shownObj.GetComponent<Shape>().KENARLAR[1];
                CreateTextInButton(value);
                Debug.Log("dikikiz hesap: " + value);
            }
            if (rnd == 2)//Eş kenar üçgen
            {
                double value = 0;
                for (int i = 0; i < 1; i++)
                {
                    double kenar = 3 * shownObj.GetComponent<Shape>().KENARLAR[i];
                    value += kenar;
                }
                CreateTextInButton(value);
                Debug.Log("eşkenar hesap: " + value);
            }
            if (rnd== 3)//İkiz kenar Ücgen
            {
                double kenar = 2 * shownObj.GetComponent<Shape>().KENARLAR[1];
                double value = kenar + shownObj.GetComponent<Shape>().KENARLAR[0];
                CreateTextInButton(value);
                Debug.Log("ikiz hesap: " + value);
            }
            if (rnd== 4)//kare
            {
                double value = 0;
                for (int i = 0; i < 1; i++)
                {
                    double kenar = 4 * shownObj.GetComponent<Shape>().KENARLAR[i];
                    value += kenar;
                }
                CreateTextInButton(value);
                Debug.Log("kare hesap: " + value);
            }
            if (rnd== 5)//Çok kenar üçgen
            {
                double value = 0;
                for (int i = 0; i < 3; i++)
                {
                    double kenar = shownObj.GetComponent<Shape>().KENARLAR[i];
                    value += kenar;
                }              
                CreateTextInButton(value);
                Debug.Log("ucgen hesap: " + value);
            }
        }

        void CreateTextInButton(double value)
        {
            int buttonSayisi = buttonSpawn.transform.childCount;
            List<int> aSayi = new List<int>();
            for (int i = 0; i < buttonSayisi; i++)
            {
                aSayi.Add(i);
            }

            int[] bSayi = new int[buttonSayisi];

            for (int i = 0; i < buttonSayisi; i++)
            {
                int idx = Random.Range(0, aSayi.Count);
                bSayi[i] = aSayi[idx];
                aSayi.RemoveAt(idx);
            }

            if (difficulty != 0)
                buttonSpawn.transform.GetChild(bSayi[0]).GetComponentInChildren<TextMeshProUGUI>().text=  value.ToString("#0.0");
            else
                buttonSpawn.transform.GetChild(bSayi[0]).GetComponentInChildren<TextMeshProUGUI>().text = value.ToString();

            deger = buttonSpawn.transform.GetChild(bSayi[0]).GetComponentInChildren<TextMeshProUGUI>().text;

            for (int i = 1; i < buttonSayisi; i++)
            {
                var btn = buttonSpawn.transform.GetChild(bSayi[i]).gameObject;

                if (difficulty != 0)
                    btn.GetComponentInChildren<TextMeshProUGUI>().text = Random.Range(3.0F, System.Convert.ToSingle(value) + 50.0F).ToString("#0.0");
                else
                    btn.GetComponentInChildren<TextMeshProUGUI>().text = Random.Range(3, System.Convert.ToInt32(value) + 50).ToString();
            }            
        }

        public void Clicked(string item)
        {
            //if (difficulty != 0)
            //    item = value.ToString("#0.0");
            //else
            //    buttonSpawn.transform.GetChild(bSayi[0]).GetComponentInChildren<TextMeshProUGUI>().text = value.ToString();

            if (deger.Equals(item))
            {
                StartCoroutine(Check(true));
            }
            else
            {
                StartCoroutine(Check(false));
            }
        }
                
        IEnumerator Check(bool st)
        {
            if (st)
            {
                audioSource.PlayOneShot(checkSfx);
                correct++;
               
                yield return new WaitForSecondsRealtime(1.0f);
                UpdateScore(_point, incrementPoint);
            }
            else
            {
                audioSource.PlayOneShot(crossSfx);
                wrong++;

                yield return new WaitForSecondsRealtime(1.0f);
            }
            ClearAll();
            ShapeSelect();
            yield return null;
        }

        void ClearAll()
        {
            Destroy(spawnParent.transform.GetChild(0).gameObject);            
        }

        void Update()
        {
            if (isTimerRunning)
            {
                currentTime -= Time.deltaTime;
                if (currentTime <= 0f)
                {
                    currentTime = 0f;
                    StopTimer();
                }
                UpdateTimerText();
            }
        }

        public void StartTimer()
        {
            currentTime = totalTime;
            isTimerRunning = true;
            timerAudioSource.Play();
            UpdateTimerText();
        }

        public void StopTimer()
        {
            isTimerRunning = false;
            timerAudioSource.Stop();
            //STOP GAME
        }

        public void AddTime(float timeToAdd)
        {
            currentTime += timeToAdd;
            UpdateTimerText();
        }

        private void UpdateTimerText()
        {
            timerText.text = Mathf.FloorToInt(currentTime).ToString();
        }
    }
}