using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 1.0f;
    [SerializeField]
    float rotateSpeed = 1.0f;

    void Start()
    {
        //地面に落としておく
        Physics.autoSimulation = false;
        Physics.Simulate(10.0f);
        Physics.autoSimulation = true;
    }

    void Update()
    {
        float moveStick = SwitchInput.GetVertical(0);
        float rotateStick = SwitchInput.GetHorizontal(0);

        transform.Translate(Vector3.forward * moveStick * moveSpeed * Time.deltaTime);
        transform.Rotate(0, rotateStick * rotateSpeed * Time.deltaTime, 0);
    }
}
