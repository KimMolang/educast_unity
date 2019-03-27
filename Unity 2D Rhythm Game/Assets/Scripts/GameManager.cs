using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;


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
            Destroy(gameObject);
            // gameObject 는 이 스크립트를 가지고 있는 객체를 뜻함
        }
    }


    [Range(0.0f, 10.0f)]
    public float noteSpeed = 0.05f;

    public enum NoteType
    {
        FIRST,
        SECOND,
        THIRD,
        FOURTH,

        MAX
    };

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

    public GameObject scoreUI;
    private float score;
    private Text scoreText;

    public GameObject comboUI;
    private int combo;
    private Text comboText;
    private Animator comboAnimator;

    public GameObject judgeUI;
    private Sprite[] judgeSprites;
    private Image judgementSpriteRenderer;
    private Animator judgementSpriteAnimator;


    public GameObject[] trails;
    private SpriteRenderer[] trailSpriteRenderers;


    private AudioSource audioSource;
    public string musicName = "40.Cyphers_Theme_of_Denise";

    void Start()
    {
        Invoke("PlayMusic", 2.0f);  // 2초 뒤에 함수 호출

        scoreText = scoreUI.GetComponent<Text>();
        comboText = comboUI.GetComponent<Text>();
        comboAnimator = comboUI.GetComponent<Animator>();

        judgementSpriteRenderer = judgeUI.GetComponent<Image>();
        judgementSpriteAnimator = judgeUI.GetComponent<Animator>();

        judgeSprites = new Sprite[(int)Judges.MAX];
        judgeSprites[0] = Resources.Load<Sprite>("Sprites/Bad");
        judgeSprites[1] = Resources.Load<Sprite>("Sprites/Good");
        judgeSprites[2] = Resources.Load<Sprite>("Sprites/Perfect");
        judgeSprites[3] = Resources.Load<Sprite>("Sprites/Miss");

        trailSpriteRenderers = new SpriteRenderer[trails.Length];

        for(int i = 0; i < trails.Length; ++i)
        {
            trailSpriteRenderers[i] = trails[i].GetComponent<SpriteRenderer>();
        }
    }

    
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            ShineTrail(0);
        }
        if (Input.GetKey(KeyCode.F))
        {
            ShineTrail(1);
        }
        if (Input.GetKey(KeyCode.J))
        {
            ShineTrail(2);
        }
        if (Input.GetKey(KeyCode.K))
        {
            ShineTrail(3);
        }

        for (int i = 0; i < trailSpriteRenderers.Length; ++i)
        {
            Color color = trailSpriteRenderers[i].color;
            color.a -= 0.01f;

            trailSpriteRenderers[i].color = color;
        }
    }

    // 특정한 키를 눌러 해당 라인을 빛나게 처리
    public void ShineTrail(int index)
    {
        Color color = trailSpriteRenderers[index].color;
        color.a = 0.32f;

        trailSpriteRenderers[index].color = color;
    }

    // 노트 판정 이후에 판정 결과를 화면에 보여줍니다.
    void ShowJudgement()
    {
        // 점수를 보여줍니다.
        string scoreFormat = "000000";
        scoreText.text = score.ToString(scoreFormat);

        // 판정 이미지를 보여줍니다.
        judgementSpriteAnimator.SetTrigger("Show");

        // 콤보가 2 이상일 때만 콤보 이미지를 보여줍니다.
        if( combo >= 2 )
        {
            comboText.text = "COMBO " + combo.ToString();
            comboAnimator.SetTrigger("Show");
        }
    }

    // 노트 판정 진행 및 UI 업데이트
    public void ProcessJudge(Judges judge, NoteType noteType)
    {
        if (judge == Judges.MAX)
        {
            return;
        }
            

        switch(judge)
        {
            case Judges.GOOD:
                {
                    ++combo;
                    score += 100;
                }
                break;

            case Judges.PERFECTLINE:
                {
                    ++combo;
                    score += 500;
                }
                break;

            case Judges.BAD:
            case Judges.MISS:
                {
                    combo = 0;
                }
                break;
        }

        // 점수는 만점에서 전체 노드 수 나누어서 각 노트 점수값을 배열에 넣으면
        // 아래처럼 처리 가능쓰
        judgementSpriteRenderer.sprite = judgeSprites[(int)judge];
        ShowJudgement();
    }

    // 음악을 실행하는 함수
    void PlayMusic()
    {
        AudioClip audioClip = Resources.Load<AudioClip>("Musics/" + musicName);
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
