using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System;

public class NoteController : MonoBehaviour
{
    class Note
    {
        public GameManager.NoteType noteType { get; set; }
        public int order { get; set; }

        public Note(GameManager.NoteType noteType, int order)
        {
            this.noteType = noteType;
            this.order = order;
        }
    }

    private ObjectPooler noteObjectPooler;
    private List<Note> notes = new List<Note>();

    private float x = 0.0f, startY = 8.0f, z = 0.0f;


    private string musicTitle;
    private string musicArtist;
    private int bpm, divider;
    private float startingPoint;
    private float beatCount;
    private float beatInterval;


    void Start()
    {
        noteObjectPooler = gameObject.GetComponent<ObjectPooler>();

        //리소스에서 비트 텍스트 파일을 불러옵니다.
        TextAsset textAsset = Resources.Load<TextAsset>("Musics/" + GameManager.instance.musicName);
        StringReader reader = new StringReader(textAsset.text);

        Debug.Log("Musics/" + GameManager.instance.musicName);
        musicTitle = reader.ReadLine();
        musicArtist = reader.ReadLine();

        string[] beatInformation = reader.ReadLine().Split(' ');
        Debug.Log(beatInformation);
        bpm = Convert.ToInt32(beatInformation[0]);
        divider = Convert.ToInt32(beatInformation[1]);
        startingPoint = (float)Convert.ToDouble(beatInformation[2]);

        // bpm 1분당 비트 개수
        // divider는 기본으로 60. // 즉 1초마다 떨어 비트 개수
        // 지금 divider 는 30. // 즉 1초마다 떨어지는 비트 수가 많아짐
        beatCount = (float)bpm / divider; // 160/30
        // 비트가 떨어지는 간격 시간
        beatInterval = 1 / beatCount; // 30/160

        // read 각 비트들이 떨어지는 위치 및 시간 정보
        string line;
        while((line = reader.ReadLine()) != null )
        {
            string[] noteInfo = line.Split(' ');
            GameManager.NoteType noteType = (GameManager.NoteType)Convert.ToInt32(noteInfo[0]);
            int order = Convert.ToInt32(noteInfo[1]);

            notes.Add(new Note(noteType, order));
        }

        // 모든 노트를 정해진 시간에 출발하도록 설정
        for(int i = 0; i < notes.Count; ++i)
        {
            StartCoroutine(AwaitMakeNote(notes[i]));
        }
    }

    IEnumerator AwaitMakeNote(Note note)
    {
        GameManager.NoteType noteType = note.noteType;
        int order = note.order;

        yield return new WaitForSeconds(startingPoint + (order * beatInterval));

        MakeNote(note);
    }

    private void MakeNote(Note note)
    {
        GameObject obj = noteObjectPooler.GetObject(note.noteType);
        x = obj.transform.position.x;
        z = obj.transform.position.z;

        obj.transform.position = new Vector3(x, startY, z);
        //obj.GetComponent<NoteBebavior>().Initialize();
        // SetActive를 통해서 OnEnable 가 불린다. 그 함수 안에 이미 초기화 함수 있음.
        obj.SetActive(true);
    }

    void Update()
    {
        
    }
}
