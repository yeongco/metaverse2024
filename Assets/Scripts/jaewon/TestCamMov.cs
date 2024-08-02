using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCamMov : MonoBehaviour
{
    private void Update()
    {
        this.gameObject.transform.position += new Vector3(1,1,-1) * Time.deltaTime * 0.6f;
    }
}
