using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingLegend : MonoBehaviour
{
    [SerializeField] private GameObject _legend;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.CurrentState == GameState.Drawing && Input.GetKeyDown(KeyCode.E))
        {
            _legend.SetActive(!_legend.activeInHierarchy);
        }
    }
}
