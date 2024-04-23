using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ADHD.Mat.GeoMathGame
{
    public class Shape : MonoBehaviour
    {
        [SerializeField]
        double[] Kenarlar;

        public double[] KENARLAR { get { return Kenarlar; } set { Kenarlar = value; } }

    }
}