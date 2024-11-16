using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour
{
    public CharacterController controller;
    public float velocidadInicial;
    public float crouchSpeed = 5.0f;
    public float walkSpeed = 10f;
    public float runSpeed = 20f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Header("Debug")]
    public bool isGrounded; // Variable pública para ver si está tocando el suelo

    private Vector3 velocity;
    private Vector3 horizontalVelocity; // Velocidad horizontal almacenada durante el salto
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
        // Verifica si está en el suelo
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Vector2 movementInput = animalInput.Animal.Move.ReadValue<Vector2>();
        Vector3 movement = Vector3.zero;

        if (isGrounded)
        {
            // Movimiento relativo a la cámara
            movement = GetCameraRelativeMovement(movementInput.y, movementInput.x);

            // Velocidades según acción (correr, caminar o agacharse)
            velocidadInicial = walkSpeed;

            if (animalInput.Animal.Jump.ReadValue<float>() > 0 && animalInput.Animal.Crouch.ReadValue<float>() == 0)
            {
                // Salto con dirección
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                horizontalVelocity = movement * velocidadInicial; // Almacena la velocidad horizontal actual
            }
            if (animalInput.Animal.Run.ReadValue<float>() > 0 && animalStats.currentEnergy > 0 && animalInput.Animal.Crouch.ReadValue<float>() == 0)
            {
                animalStats.ConsumeEnergy(1); // Consumir energía solo al correr
                velocidadInicial = runSpeed;
            }
            if (animalInput.Animal.Crouch.ReadValue<float>() > 0)
            {
                velocidadInicial = crouchSpeed;
            }
        }
        else
        {
            // Conserva la velocidad horizontal mientras está en el aire
            movement = horizontalVelocity / velocidadInicial;
        }

        // Aplicar gravedad
        velocity.y += gravity * Time.deltaTime;

        // Movimiento horizontal
        controller.Move(movement * velocidadInicial * Time.deltaTime);

        // Movimiento vertical (gravedad y salto)
        controller.Move(velocity * Time.deltaTime);

        // Rotar hacia la dirección del movimiento cuando está en el suelo
        if (isGrounded && movement.magnitude > 0.1f)
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
