using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private string selectableTag = "Selectable";
    public UnityEngine.UI.Image img;

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
            else
            {
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    img.color = new Color(1f, 0f, 0f);
                }
                else
                    img.color = new Color(1f, 1f, 1f);
            }
        }
        else
            img.color = new Color(1f, 1f, 1f);
    }
}
