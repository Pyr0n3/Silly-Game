using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput input;
    [SerializeField]
    private float plrSpeed;

    private Vector2 move;
    private float jump;

    private Vector2 camRotation;
    private Transform plrCam;

    private void Awake()
    {
        input = new();
        input.ActivateInput();
        plrCam = Camera.main.transform;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        transform.Translate((plrSpeed * Time.deltaTime) * (move * new Vector3(0, jump, 0)));


        Vector2 mouseDelta = camRotation * 0.5f;
        Vector3 cameraRot = plrCam.localEulerAngles;

        plrCam.localEulerAngles = cameraRot;

        Vector3 plrRot = transform.eulerAngles;

        plrRot.y += mouseDelta.y;
        transform.eulerAngles = plrRot;
    }

    public void Move(InputAction.CallbackContext context)
    {
      move = context.ReadValue<Vector3>();
    }

    public void Jump(InputAction.CallbackContext context) 
    {  jump = context.ReadValue<float>(); }
}
