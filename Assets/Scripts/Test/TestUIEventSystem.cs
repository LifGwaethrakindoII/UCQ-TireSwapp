using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ulyssess;

public class TestUIEventSystem : MonoBehaviour
{
    public float distance;
    public LayerMask layer;
    private Button3D button;

    private void Update()
    {
        CastRay();
        if (button != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //button.OnPointerDown(null);
                button.OnPointerDown(null);
            }
            if (Input.GetMouseButtonUp(0))
            {
                button.OnPointerUp(null);
                //button.onClick.Invoke();
            }
        }
    }

        private void CastRay()
        {
            Ray mousePoint = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(mousePoint, out hit, distance, layer))
            {
                button = hit.transform.GetComponent<Button3D>();
                if(button != null)
                {
                    button.OnPointerEnter(null);
                }
            }
            else if (button != null)
            {
                 button.OnPointerExit(null);
                 button.OnPointerUp(null);
                 button = null;
            }

            Debug.DrawRay(mousePoint.origin, mousePoint.direction * distance, Color.blue);
        }

    public int Log(int x, int b = 2)
    {
        int count = 0;
        while (x > 1)
        {
            x /= b;
            count++;
        }
        return count;
    }
}


