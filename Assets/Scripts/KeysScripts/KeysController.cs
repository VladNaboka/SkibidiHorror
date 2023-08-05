using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeysController : MonoBehaviour
{
    private List<Key> keysPrefab;

    [SerializeField] private GameObject keyUI;
    [SerializeField] private GameObject canvasUI;
    private float spacing = 120f;

    private GameObject[] objects;
    public int currentIndex = 0;
    private void Start()
    {
        keysPrefab = new List<Key>(FindObjectsOfType<Key>());
        objects = new GameObject[keysPrefab.Count];

        for (int i = 0; i < keysPrefab.Count; i++)
        {
            Vector3 spawnPosition = transform.position + new Vector3(795 + i * spacing, 475f, 0f);
            GameObject uiElement = Instantiate(keyUI, spawnPosition, Quaternion.identity);
            objects[i] = uiElement;
            uiElement.transform.SetParent(canvasUI.transform);
        }
    }

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

    public void CollectKey()
    {
        ChangeColor(currentIndex);

        currentIndex++;

        if (currentIndex >= objects.Length)
        {
            currentIndex = 0;
        }
    }

    private void ChangeColor(int index)
    {
        if (index >= 0 && index < objects.Length)
        {
            Image img = objects[index].GetComponent<Image>();
            if (img != null)
            {
                img.color = Color.white;
            }
        }
    }
}
