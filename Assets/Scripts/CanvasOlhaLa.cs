using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasOlhaLa : MonoBehaviour
{
    private void Update()
    {
        transform.LookAt(Camera.main.transform);
    }
}
