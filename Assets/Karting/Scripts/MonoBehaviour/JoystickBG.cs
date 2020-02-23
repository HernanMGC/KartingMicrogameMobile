using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickBG : MonoBehaviour
{
    Vector3 m_StartPos;

    public GameObject joystick;

    // Start is called before the first frame update
    void Start()
    {
        m_StartPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
