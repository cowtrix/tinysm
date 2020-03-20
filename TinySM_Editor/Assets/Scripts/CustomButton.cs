using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomButton : MonoBehaviour
{
    void Update()
    {
        if (GetComponentInParent<LayoutGroup>() != null)
        {
            GetComponent<ContentSizeFitter>().enabled = false;
        }
    }
}
