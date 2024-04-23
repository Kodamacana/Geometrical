using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct Difficulty
{
    public int difficultyValue;
    public double minValue;
    public double maxValue;

    public float addTime;
    [SerializeField, Range(1,3)] public float increment;

    public bool isDouble;
    public double levelUpControlValue;
}

[CreateAssetMenu(fileName ="Difficulty",menuName ="LevelDifficulty", order = 0)]
public class LevelDifficulty : ScriptableObject
{
	public List<Difficulty> difficulties = new List<Difficulty>(); 
}
