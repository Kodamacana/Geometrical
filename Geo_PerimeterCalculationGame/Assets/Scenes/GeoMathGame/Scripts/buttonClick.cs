using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ADHD.Mat.GeoMathGame
{
    public class buttonClick : MonoBehaviour
    {
        [SerializeField]
        GameController gameController;
        public Button yourButton;
        private void Awake()
        {
            gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
            transform.GetComponent<Button>().onClick.AddListener(delegate { onClick(); });
        }

        void onClick()
        {
            gameController.Clicked(transform.GetComponentInChildren<Text>().text);
        }


    }
}