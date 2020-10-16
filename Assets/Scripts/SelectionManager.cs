using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private string selectableTag = "Selectable";
    public Image img;

    void Start()
    {
        img.color = new Color(1f, 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {   
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            var selection = hit.transform;
            if (selection.CompareTag(selectableTag))
                img.color = new Color(0f, 1f, 0f);
        }
        else
        {
            img.color = new Color(1f, 1f, 1f);
        }
    }
}
