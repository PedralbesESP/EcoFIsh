using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private int _coral;
    [SerializeField] private int _Meal;
    [SerializeField] private int _camaron;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    public int getCoral()
    {
        return _coral;
    }
    public int getMeal()
    {
        return _Meal;
    }

    public int getCamaron()
    {
        return _camaron;
    }

    public void SumCoral(int num)
    {
        _coral += num;
    }
    public void SumMeal(int num)
    {
        _Meal += num;
    }
    public void sumCamaron(int num)
    {
        _camaron += num;
    }
}
