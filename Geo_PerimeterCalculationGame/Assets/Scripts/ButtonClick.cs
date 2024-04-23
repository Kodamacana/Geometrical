using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ADHD.Mat.GeoMathGame
{
    public class ButtonClick : MonoBehaviour
    {
        GameController gameController;

        private void Awake()
        {
            gameController = GameController.Instance;
            GetComponent<Button>().onClick.AddListener(delegate { onClick(); });
        }

        void onClick()
        {
            gameController.Clicked(GetComponentInChildren<TextMeshProUGUI>().text);
        }


    }
}