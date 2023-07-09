using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //FindObjectOfType<KeysController>().ReloadList();
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        transform.DOMoveY(2.3f, 2)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }
}
