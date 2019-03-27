using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; set; }

    private void Awake()
    {
        if( instance == null )
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject); // gameObject 는 자기 자신
        }
    }


    [Range(0.0f, 10.0f)]
    public float noteSpeed = 0.05f;

    public enum Judges
    {
        BAD,
        GOOD,
        PERFECTLINE,
        MISS,

        MAX
    };

    // ref : https://j07051.tistory.com/567
    // ref : https://stackoverflow.com/questions/5142349/declare-a-const-array
    // public static readonly string[]
    // public const string[]
    public static readonly string[] judgeLinesStringList 
        = { "BadLine", "GoodLine", "PerfectLine", "MissLine" };


    public enum NoteType
    {
        FIRST,
        SECOND,
        THIRD,
        FOURTH,

        MAX
    };

    void Start()
    {

    }

    
    void Update()
    {
        
    }
}
