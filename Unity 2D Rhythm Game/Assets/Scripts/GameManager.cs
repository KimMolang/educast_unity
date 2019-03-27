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

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
