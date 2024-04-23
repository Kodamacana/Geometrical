using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ADHD.Mat.GeoMathGame{
    public class GameController : MonoBehaviour
    {
        private enum ShapeType
        {
            Dikdortgen,
            DikIkizKenarUcgen,
            EsKenarUcgen,
            IkizKenarUcgen,
            Kare,
            CokKenarUcgen
        }

        public static GameController Instance;

        [Header("Scriptable Object")]
        [SerializeField] LevelDifficulty levelDifficulty;

        [Header("Geometric Shapes")]
        [SerializeField] List<GameObject> shapes;

        [Header("Game Objects")]
        [SerializeField] GameObject starButtonPrefab;
        [SerializeField] GameObject starButtonSpawnParent;
        
        [SerializeField] GameObject shapeEdgeNumberPrefab;
        [SerializeField] GameObject shapeSpawnParent;
        private GameObject shownShape;

        [Header("SFX and Audio Objects")]        
        [SerializeField] AudioClip crossSfx;
        [SerializeField] AudioClip checkSfx;
        [SerializeField] AudioSource audioSource;
        [SerializeField] AudioSource timerAudioSource;

        [Header("UI Objects")]
        [SerializeField] GameObject gameOverPanel;

        [SerializeField] TextMeshProUGUI pointText;
        [SerializeField] TextMeshProUGUI stopGameScoreText;
        [SerializeField] TextMeshProUGUI timerText;

        private float _point = 0;
        private float totalTime = 15f; 
        private float currentTime; 
        private int difficulty = 0;
        private bool isTimerRunning = false; 
        private string deger;

        private void Awake()
        {
            //DontDestroyOnLoad(this);

            //if (Instance == null)
            //    Instance = this;
            //else
            //    Destroy(gameObject);

            Instance = this;
        }

        private void Start()
        {
            CloneButton();

            ShapeSelect();
        }

        private void CloneButton()
        {
            DestroyClonedButton();
            for (int i = 0; i < ButtonCount(); i++)
            {
                Instantiate(starButtonPrefab, starButtonSpawnParent.transform);
            }
        }

        private void DestroyClonedButton()
        {
            foreach (Transform child in starButtonSpawnParent.transform)
            {
                Destroy(child.gameObject);
            }
        }

        private int ButtonCount() 
        {
            return difficulty + 3;
        }

        private void UpdateScore(float currentPoint)
        {
            float time = levelDifficulty.difficulties[difficulty].addTime;
            float increment = levelDifficulty.difficulties[difficulty].increment;
            AddTime(time);

            _point =  currentPoint + increment * 1;
            pointText.text = "Point: " + _point.ToString();
            if (levelDifficulty.difficulties[difficulty].levelUpControlValue <= _point)
            {
                difficulty++;
                CloneButton();
            }
        }

        private void ShapeSelect()
        {
            int bound = shapes.Count;
            int rnd = Random.Range(0, bound);

            double minValue = levelDifficulty.difficulties[difficulty].minValue;
            double maxValue = levelDifficulty.difficulties[difficulty].maxValue;
            bool isDouble = levelDifficulty.difficulties[difficulty].isDouble;

            GenerateLevel(rnd, isDouble, minValue, maxValue);
                      
            CalculateShapeEdgeNumber((ShapeType) rnd);
        }

        private void GenerateLevel(int rnd, bool isDouble, double minValue, double maxValue)
        {
            shownShape = shapes[rnd];
            int kenarSayisi = shownShape.GetComponent<Shape>().KENARLAR.Length;

            var scr = shownShape.GetComponent<Shape>();
            var obj = Instantiate(shownShape, shapeSpawnParent.transform);

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
                var num = Instantiate(shapeEdgeNumberPrefab, obj.transform.GetChild(i).transform);
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

        private void CalculateShapeEdgeNumber(ShapeType shapeType)
        {
            double value = 0;

            switch (shapeType)
            {
                case ShapeType.Dikdortgen:
                    for (int i = 0; i < 2; i++)
                    {
                        double kenar = 2 * shownShape.GetComponent<Shape>().KENARLAR[i];
                        value += kenar;
                    }
                    Debug.Log("dik hesap: " + value);
                    break;
                case ShapeType.DikIkizKenarUcgen:
                    double kenarDIK = 2 * shownShape.GetComponent<Shape>().KENARLAR[0];
                    value = kenarDIK + shownShape.GetComponent<Shape>().KENARLAR[1];
                    Debug.Log("dikikiz hesap: " + value);
                    break;
                case ShapeType.EsKenarUcgen:
                    for (int i = 0; i < 1; i++)
                    {
                        double kenar = 3 * shownShape.GetComponent<Shape>().KENARLAR[i];
                        value += kenar;
                    }
                    Debug.Log("eşkenar hesap: " + value);
                    break;
                case ShapeType.IkizKenarUcgen:
                    double kenarIKIZ = 2 * shownShape.GetComponent<Shape>().KENARLAR[1];
                    value = kenarIKIZ + shownShape.GetComponent<Shape>().KENARLAR[0];
                    Debug.Log("ikiz hesap: " + value);
                    break;
                case ShapeType.Kare:
                    for (int i = 0; i < 1; i++)
                    {
                        double kenar = 4 * shownShape.GetComponent<Shape>().KENARLAR[i];
                        value += kenar;
                    }
                    Debug.Log("kare hesap: " + value);
                    break;
                case ShapeType.CokKenarUcgen:
                    for (int i = 0; i < 3; i++)
                    {
                        double kenar = shownShape.GetComponent<Shape>().KENARLAR[i];
                        value += kenar;
                    }
                    Debug.Log("ucgen hesap: " + value);
                    break;
                default:
                    Debug.LogError("Bilinmeyen şekil tipi!");
                    break;
            }

            CreateTextInButton(value);
        }

        private void CreateTextInButton(double value)
        {
            int buttonSayisi = starButtonSpawnParent.transform.childCount;
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
                starButtonSpawnParent.transform.GetChild(bSayi[0]).GetComponentInChildren<TextMeshProUGUI>().text=  value.ToString("#0.0");
            else
                starButtonSpawnParent.transform.GetChild(bSayi[0]).GetComponentInChildren<TextMeshProUGUI>().text = value.ToString();

            deger = starButtonSpawnParent.transform.GetChild(bSayi[0]).GetComponentInChildren<TextMeshProUGUI>().text;

            for (int i = 1; i < buttonSayisi; i++)
            {
                var btn = starButtonSpawnParent.transform.GetChild(bSayi[i]).gameObject;

                if (difficulty != 0)
                    btn.GetComponentInChildren<TextMeshProUGUI>().text = Random.Range(3.0F, System.Convert.ToSingle(value) + 50.0F).ToString("#0.0");
                else
                    btn.GetComponentInChildren<TextMeshProUGUI>().text = Random.Range(3, System.Convert.ToInt32(value) + 50).ToString();
            }            
        }

        public void Clicked(string item)
        {         
            if (deger.Equals(item))
            {
                StartCoroutine(Check(true));
            }
            else
            {
                StartCoroutine(Check(false));
            }
        }
                
        private IEnumerator Check(bool st)
        {
            if (st)
            {
                audioSource.PlayOneShot(checkSfx);
               
                yield return new WaitForSecondsRealtime(1.0f);
                UpdateScore(_point);
            }
            else
            {
                audioSource.PlayOneShot(crossSfx);

                yield return new WaitForSecondsRealtime(1.0f);
            }

            ClearAll();
            ShapeSelect();
            yield return null;
        }

        private void ClearAll()
        {
            Destroy(shapeSpawnParent.transform.GetChild(0).gameObject);            
        }

        private void Update()
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
        }

        private void StopTimer()
        {
            isTimerRunning = false;
            timerAudioSource.Stop();
            stopGameScoreText.text = "Score: " + _point;
            gameOverPanel.SetActive(true);
        }

        private void AddTime(float timeToAdd)
        {
            currentTime += timeToAdd;
        }

        private void UpdateTimerText()
        {
            timerText.text = Mathf.FloorToInt(currentTime).ToString();
        }
    }
}