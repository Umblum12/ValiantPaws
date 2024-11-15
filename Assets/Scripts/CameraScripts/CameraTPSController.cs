using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTPSController : MonoBehaviour
{
    [SerializeField]
    public Transform target;
    [SerializeField]
    private float sensitivity = 10.0f;
    public bool lookAtPlayer = false;
    public bool rotationActive;
    Vector3 mouseDelta = Vector3.zero;
    Vector2 scrollDelta = Vector2.zero; // Nuevo Vector2 para el scroll
    Vector3 amount = Vector3.zero;

    public Vector3 addPos = new Vector3(0, 1.63f, 0);

    Quaternion cameraRotation = Quaternion.identity;
    Vector3 cameraPosition = Vector3.zero;
    Vector3 cameraPositionNotOcc = Vector3.zero;
    Vector3 lookAt = Vector3.zero;

    public AnimalInput animalInput;

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

    private void Update()
    {
        if (rotationActive)
        {
            mouseDelta.Set(animalInput.Animal.LookCam.ReadValue<Vector2>().x * 0.01f,
                           -animalInput.Animal.LookCam.ReadValue<Vector2>().y * 0.01f,
                           0);

            scrollDelta = animalInput.Animal.Scroll.ReadValue<Vector2>();

            amount += new Vector3(-mouseDelta.x, mouseDelta.y, scrollDelta.y) * sensitivity;
            amount.z = Mathf.Clamp(amount.z, 50, 100);
            amount.y = Mathf.Clamp(amount.y, -50, 50);

            cameraRotation = Quaternion.AngleAxis(amount.x, Vector3.up) *
                             Quaternion.AngleAxis(-amount.y, Vector3.right);

            lookAt = cameraRotation * Vector3.forward;

            // Calculamos la posición deseada de la cámara
            cameraPosition = target.position + addPos - lookAt * amount.z * 0.1f;

            // Detectar colisiones
            RaycastHit hit;
            if (Physics.Linecast(target.position + addPos, cameraPosition, out hit))
            {
                cameraPositionNotOcc = target.position + addPos - lookAt * hit.distance;

                // Ajuste adicional si está muy cerca del obstáculo
                if (hit.distance < Camera.main.nearClipPlane * 2.5f)
                {
                    cameraPositionNotOcc -= lookAt * Camera.main.nearClipPlane;
                }

                // Interpolación suave hacia la posición segura
                transform.position = Vector3.Lerp(transform.position, cameraPositionNotOcc, Time.deltaTime * 10.0f);
            }
            else
            {
                // Interpolación suave hacia la posición sin colisiones
                transform.position = Vector3.Lerp(transform.position, cameraPosition, Time.deltaTime * 10.0f);
            }

            // Interpolación suave de la rotación
            transform.rotation = Quaternion.Lerp(transform.rotation, cameraRotation, Time.deltaTime * 10.0f);
        }

        if (lookAtPlayer || rotationActive)
        {
            transform.LookAt(target.position + addPos);
        }

        if (animalInput.Animal.UnlockCam.ReadValue<float>() > 0)
        {
            rotationActive = true;
        }
        else
        {
            rotationActive = false;
            transform.LookAt(target.position + addPos);
        }
    }
}
