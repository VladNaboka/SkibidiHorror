using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeysController : MonoBehaviour
{
    [SerializeField] private List<Key> keysPrefab;

    private void Start()
    {
        keysPrefab = new List<Key>(FindObjectsOfType<Key>());
        //ReloadList();
    }

    //public void ReloadList()
    //{
    //    keysPrefab.Clear();
    //    keysPrefab = new List<Key>(FindObjectsOfType<Key>());
    //    Debug.Log("Reload list");
    //}

    private void Update()
    {
        for (int i = 0; i < keysPrefab.Count; i++)
        {
            if (keysPrefab[i] == null)
                keysPrefab.RemoveAt(i);
        }

        if (keysPrefab.Count == 0)
        {
            Debug.Log("Вы собрали все ключи!");
        }
    }
}
