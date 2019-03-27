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

    public GameObject[] Notes;
    private List<Note> notes = new List<Note>();
    private float beatInterval = 1.0f;

    void Start()
    {
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

        Instantiate(Notes[(int)noteType]);
    }

    void Update()
    {
        
    }
}
