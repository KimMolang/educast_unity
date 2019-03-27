using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private float beatInterval = 1.0f;

    void Start()
    {
        noteObjectPooler = gameObject.GetComponent<ObjectPooler>();

        notes.Add(new Note(GameManager.NoteType.FIRST, 1));
        notes.Add(new Note(GameManager.NoteType.SECOND, 2));
        notes.Add(new Note(GameManager.NoteType.THIRD, 3));
        notes.Add(new Note(GameManager.NoteType.FOURTH, 4));

        notes.Add(new Note(GameManager.NoteType.FIRST, 5));
        notes.Add(new Note(GameManager.NoteType.SECOND, 6));
        notes.Add(new Note(GameManager.NoteType.THIRD, 7));
        notes.Add(new Note(GameManager.NoteType.FOURTH, 8));

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

        yield return new WaitForSeconds(order * beatInterval);

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
