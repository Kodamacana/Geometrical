using System.Collections;
using System.Collections.Generic;
using UnityEngine;                                                                                              //Sorunlar 
using UnityEngine.Events;                                                                                       //Arada bir nedenini çözemediğim halde 0 döndürüyor
using UnityEngine.UI;                                                                                           

namespace ADHD.Mat.GeoMathGame{
    public class GameController : MonoBehaviour
    {      
        public GameObject buttonObj;
        [SerializeField]
        GameObject ButtonSpawn;
        [SerializeField]
        GameObject Button1;
        [SerializeField]
        GameObject Button2;
        [SerializeField]
        GameObject Button3;        
        [SerializeField]
        GameObject NumberObj;
        [SerializeField]
        List<GameObject> shapes;        
        [SerializeField]
        GameObject shownObj;
        [SerializeField]
        GameObject shownParent;
        [SerializeField]
        GameObject currentButton;
        [SerializeField]
        GameObject cross;
        [SerializeField]
        GameObject check;
        [SerializeField]
        GameObject Point;
        GameObject btn;

        int buttonSayisi= 3;
        double deger;
        float point = 0;
        float incrementPoint = 1;
        int correct= 0, wrong= 0;
        int difficulty= 3;// değişken

        void Start()
        {
            //if (BaseMiniGameController.Instance == null) StartGame();
            StartGame();
            shapeSelect();
        }

        public void StartGame() 
        {
            Point.SetActive(true);
            if (difficulty == 1)
            {
                currentButton = ButtonSpawn.transform.GetChild(0).gameObject;
                buttonSayisi = 3;
                ButtonSpawn.transform.GetChild(0).gameObject.SetActive(true);
            }
            else if (difficulty == 2)
            {
                currentButton = ButtonSpawn.transform.GetChild(1).gameObject;
                buttonSayisi = 4;
                ButtonSpawn.transform.GetChild(1).gameObject.SetActive(true);
            }
            else if (difficulty == 3)
            {
                currentButton = ButtonSpawn.transform.GetChild(2).gameObject;
                buttonSayisi = 5;
                ButtonSpawn.transform.GetChild(2).gameObject.SetActive(true);
            }
        }

        public void UpdateScore(float currentPoint, float increment)
        {
            point = currentPoint + increment;
            Point.transform.GetComponent<Text>().text = "Point: " + point.ToString();
            //if(BaseMiniGameController.Instance) BaseMiniGameController.Instance.AdjustScore(increment);
        }

        void shapeSelect()
        {
            int bound = shapes.Count;
            int rnd = Random.Range(0, bound);
            if (difficulty == 1)
            {
                LevelOne(rnd);
            }
            else if (difficulty == 2)
            {
                LevelTwo(rnd);
            }
            else if (difficulty== 3)
            {
                LevelThree(rnd);
            }            
            calc(rnd);
        }

        void LevelOne(int rnd)
        {
            Debug.Log("level 1");
            shownObj = shapes[rnd];
            int kenarSayisi = shownObj.GetComponent<Shape>().KENARLAR.Length;
            Debug.Log(kenarSayisi);
            var scr = shownObj.GetComponent<Shape>();
            var obj = Instantiate(shownObj, shownParent.transform);
            List<int> control = new List<int>();
            int value = 0;
            for (int i = 0; i < kenarSayisi; i++)
            {
                value = Random.Range(3, 9);
                while (control.Contains(value))
                {                    
                    value = Random.Range(3, 9);
                }
                control.Add(value);
                scr.KENARLAR[i] = value;                
            }

            System.Array.Sort(scr.KENARLAR);
            for (int i = 0; i < kenarSayisi; i++)
            {
                var num = Instantiate(NumberObj, obj.transform.GetChild(i).transform);
                num.GetComponentInChildren<Text>().text = scr.KENARLAR[i].ToString();
            }
        }
        void LevelTwo(int rnd)
        {
            shownObj = shapes[rnd];
            int kenarSayisi = shownObj.GetComponent<Shape>().KENARLAR.Length;
            var scr = shownObj.GetComponent<Shape>();
            var obj = Instantiate(shownObj, shownParent.transform);
            List<double> control = new List<double>();
            float value = 0;
            for (int i = 0; i < kenarSayisi; i++)
            {                
                value = Random.Range(1.0f, 20.0f);
                string val1 = value.ToString("#0.0");
                
                while (control.Contains(System.Convert.ToDouble(val1)))
                {
                    value = Random.Range(1.0f, 20.0f);
                    val1 = value.ToString("#0.0");
                }
                control.Add((System.Convert.ToDouble(val1)));
                scr.KENARLAR[i] = (System.Convert.ToDouble(val1));
            }
            System.Array.Sort(scr.KENARLAR);
            for (int i = 0; i < kenarSayisi; i++)
            {
                var num = Instantiate(NumberObj, obj.transform.GetChild(i).transform);
                num.GetComponentInChildren<Text>().text = scr.KENARLAR[i].ToString("#0.0");
            }
        }
        void LevelThree(int rnd)
        {
            Debug.Log("level 3");
            shownObj = shapes[rnd];
            int kenarSayisi = shownObj.GetComponent<Shape>().KENARLAR.Length;
            Debug.Log(kenarSayisi);
            var scr = shownObj.GetComponent<Shape>();
            var obj = Instantiate(shownObj, shownParent.transform);
            List<double> control = new List<double>();
            float value = 0;
            for (int i = 0; i < kenarSayisi; i++)
            {
                value = Random.Range(15f, 50f);
                string val1 = value.ToString("#0.0");

                while (control.Contains(System.Convert.ToDouble(val1)))
                {
                    value = Random.Range(15f, 50f);
                    val1 = value.ToString("#0.0");
                }
                control.Add((System.Convert.ToDouble(val1)));
                scr.KENARLAR[i] = (System.Convert.ToDouble(val1));
            }
            System.Array.Sort(scr.KENARLAR);
            for (int i = 0; i < kenarSayisi; i++)
            {
                var num = Instantiate(NumberObj, obj.transform.GetChild(i).transform);
                num.GetComponentInChildren<Text>().text = scr.KENARLAR[i].ToString("#0.0");
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
                startButton(value);
                    Debug.Log("dik hesap: " + value);
                }
            if (rnd == 1)//dik ikiz kenar ücgen
            {                              
                  double kenar = 2 * shownObj.GetComponent<Shape>().KENARLAR[0];
                  double value = kenar+ shownObj.GetComponent<Shape>().KENARLAR[1];
                startButton(value);
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
                startButton(value);
                Debug.Log("eşkenar hesap: " + value);
            }
            if (rnd== 3)//İkiz kenar Ücgen
            {
                double kenar = 2 * shownObj.GetComponent<Shape>().KENARLAR[1];
                double value = kenar + shownObj.GetComponent<Shape>().KENARLAR[0];
                startButton(value);
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
                startButton(value);
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
                startButton(value);
                Debug.Log("ucgen hesap: " + value);
            }
        }

        void startButton(double value)
        {
            deger = value;
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
            var bttn = "";
            bttn =currentButton.transform.GetChild(bSayi[0]).GetComponentInChildren<Text>().text= value.ToString();
            
            for (int i = 1; i < buttonSayisi; i++)
            {
                btn = currentButton.transform.GetChild(bSayi[i]).gameObject;
                if (difficulty == 1)
                {
                    btn.GetComponentInChildren<Text>().text = Random.Range(3, System.Convert.ToInt32(value) + 50).ToString();
                }
                else
                {
                   btn.GetComponentInChildren<Text>().text = Random.Range(3.0F, System.Convert.ToSingle(value) + 50.0F).ToString("#0.0");
                }
            }            
        }

        public void Clicked(string item)
        {
            if (deger.ToString() == item)
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
                UpdateScore(point, incrementPoint);
                check.SetActive(true);
                correct++;

                //if (BaseMiniGameController.Instance) BaseMiniGameController.Instance.IncreaseTrueAnswers();
                //if ((correct != 0) && (correct % 2 == 0))
                //{
                //    tier++;
                //    if (BaseMiniGameController.Instance) BaseMiniGameController.Instance.ChangeActiveTierID(Tier, true);
                //}

                yield return new WaitForSecondsRealtime(1.0f);                
                check.SetActive(false);
            }
            else
            {
                wrong++;
                cross.SetActive(true);
                //if (BaseMiniGameController.Instance) BaseMiniGameController.Instance.IncreaseWrongAnswers();
                yield return new WaitForSecondsRealtime(1.0f);
                cross.SetActive(false);
            }
            ClearAll();
            shapeSelect();
            yield return null;
        }

        void ClearAll()
        {
            Destroy(shownParent.transform.GetChild(0).gameObject);            
        }      
    }
}