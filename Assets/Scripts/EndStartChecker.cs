using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndStartChecker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Manager.instance.GenerateRank();
    }
}
