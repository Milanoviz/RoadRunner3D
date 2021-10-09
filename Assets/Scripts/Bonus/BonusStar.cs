using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusStar : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(0, 40 * Time.deltaTime, 0);
    }
}
