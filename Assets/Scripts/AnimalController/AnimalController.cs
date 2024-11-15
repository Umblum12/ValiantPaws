using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour
{
    public CharacterController controller;
    public float velocidadInicial;
    public float cruochSpeed = 5.0f;
    public float walkSpeed = 10f;
    public float runSpeed = 20f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private bool isGrounded;
    private Vector3 velocity;
    private bool isRunning;
    public Camera mainCamera;


    public AnimalInput animalInput;

    public AnimalStats animalStats;

    private void Awake()
    {
        animalInput = new AnimalInput();
    }

    private void OnEnable()
    {
        animalInput.Enable();
    }

    private void OnDisable()
    {
        animalInput.Disable();
    }

    private void Start()
    {
        mainCamera = Camera.main;
        animalStats = GetComponent<AnimalStats>();
    }

    void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Vector2 movementInput = animalInput.Animal.Move.ReadValue<Vector2>();
        Vector3 movement = GetCameraRelativeMovement(movementInput.y, movementInput.x);

        velocidadInicial = +walkSpeed;



        if (isGrounded)
        {
            //Veridica que preciones espacio para saltar y que no se presiona Control al mismo tiempo
            if (animalInput.Animal.Jump.triggered && animalInput.Animal.Crouch.ReadValue<float>() == 0)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
            // Verifica si se presiona Shift, que el personaje tnega energia y que no se presiona Control al mismo tiempo
            if (animalInput.Animal.Run.ReadValue<float>() > 0 && animalStats.currentEnergy > 0 && animalInput.Animal.Crouch.ReadValue<float>() == 0)
            {
                animalStats.ConsumeEnergy(1);  // Consumir energía solo cuando se corre
                velocidadInicial = +runSpeed;
            }
            //Veridica que preciones control para agacharte
            if (animalInput.Animal.Crouch.ReadValue<float>() > 0)
            {
                velocidadInicial = +cruochSpeed;
            }
            else
            {
            }
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
        controller.Move(movement * velocidadInicial * Time.deltaTime);

        if (movement.magnitude > 0.1f)
        {
            Quaternion newRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, 0.15f);
        }
    }

    private Vector3 GetCameraRelativeMovement(float movementInputY, float movementInputX)
    {
        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        return cameraForward * movementInputY + cameraRight * movementInputX;
    }
}
