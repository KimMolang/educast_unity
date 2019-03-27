using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public List<GameObject> NotePrefabs; // First, Second, Third, Fourth Note
    private List<List<GameObject>> poolsOfNotes;
    public int noteCount = 10;
    private bool more = true;

    void Start()
    {
        poolsOfNotes = new List<List<GameObject>>();

        for(int i = 0; i < NotePrefabs.Count; ++i)
        {
            poolsOfNotes.Add(new List<GameObject>());

            for(int j = 0; j < noteCount; ++j )
            {
                GameObject obj = Instantiate(NotePrefabs[i]);
                obj.SetActive(false);
                poolsOfNotes[i].Add(obj);
            }
        }
    }
    
    public GameObject GetObject(GameManager.NoteType noteType)
    {
        int noteTypeInteger = (int)noteType;

        foreach (GameObject obj in poolsOfNotes[noteTypeInteger])
        {
            if( obj.activeInHierarchy == false )
            {
                return obj;
            }
        }

        if( more )
        {
            GameObject obj = Instantiate(NotePrefabs[noteTypeInteger]);
            poolsOfNotes[noteTypeInteger].Add(obj);

            return obj;
        }

        return null;
    }

    void Update()
    {
        
    }
}
