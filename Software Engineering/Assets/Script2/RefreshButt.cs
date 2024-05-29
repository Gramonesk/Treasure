using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RefreshButt : MonoBehaviour
{
    private Button RefButton;

    private void Awake()
    {
        RefButton.onClick.AddListener(Refresh);
    }

    private void Refresh()
    {
        StartCoroutine(RefreshWait());
    }

    private IEnumerator RefreshWait()
    {
        RefButton.interactable = false;
        yield return null;
    }

}
