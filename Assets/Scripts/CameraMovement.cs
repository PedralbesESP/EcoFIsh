using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    float _xAxisValue;
    float _yAxisValue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _xAxisValue = Input.GetAxis("Horizontal");
        _yAxisValue = Input.GetAxis("Vertical");
        if(Camera.main != null)
        {
            Camera.main.transform.Translate(new Vector3(_xAxisValue, _yAxisValue, 0f));
        }
        
    }
}
