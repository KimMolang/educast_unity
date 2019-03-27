using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteBebavior : MonoBehaviour
{
    private GameManager.Judges judge = GameManager.Judges.MAX;

    public GameManager.NoteType noteType;
    private KeyCode keyCode;

    void Start()
    {
        switch(noteType)
        {
            case GameManager.NoteType.FIRST:
                keyCode = KeyCode.D;
                break;
            case GameManager.NoteType.SECOND:
                keyCode = KeyCode.F;
                break;
            case GameManager.NoteType.THIRD:
                keyCode = KeyCode.J;
                break;
            case GameManager.NoteType.FOURTH:
                keyCode = KeyCode.K;
                break;
        }
    }

    public void Initialize()
    {
        judge = GameManager.Judges.MAX;
    }

    // 스크립트가 켜질때와 꺼질 때 호출되는 함수
    // SetActivate()에 따라 객체 상태가 바뀔 때도 스크립트의 상태가 바뀐다.
    private void OnEnable()
    {
        Initialize();
    }

    private void OnDisable()
    {
        
    }

    void Update()
    {
        transform.Translate(Vector3.down * GameManager.instance.noteSpeed);

        if( Input.GetKey(keyCode))
        {
            // 노트가 판정 선에 닿기 시작한 이후로는 해당 노트를 제거
            if( judge != GameManager.Judges.MAX)
            {
                Debug.Log("NodeBebavior::Update::Input.GetKey(keyCode) ->" + judge);
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log(GameManager.Judges.BAD.ToString());
        // 또는 StringArray 하면 코드가 짧아지겠군
        // 그래서 바꿨습니다.
        for (GameManager.Judges i = 0; i < GameManager.Judges.MISS; ++i)
        {
            if (collision.gameObject.tag == GameManager.judgeLinesStringList[(int)i])
            {
                judge = i;
                break;
            }
        }

        switch (collision.gameObject.tag)
        {
            case "MissLine":
                {
                    judge = GameManager.Judges.MISS;
                    gameObject.SetActive(false);
                }
                break;
        }

        //Debug.Log("NodeBebavior::OnTriggerEnter2D::judge ->" + judge);
    }
}
