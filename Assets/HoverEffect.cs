using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float hoverScale = 1.5f; // Escala al hacer hover
    public Color normalTextColor = Color.black; // Texto negro en estado normal
    public Color hoverTextColor = Color.black; // Texto negro en estado hover también
    public Color normalBackgroundColor = new Color(1, 1, 1, 0.3f); // Fondo translúcido en estado normal
    public Color hoverBackgroundColor = Color.white; // Fondo blanco en estado hover
    public float transitionDuration = 0.2f; // Duración de la transición en segundos

    private Vector3 initialScale;
    private Image buttonImage;
    private TextMeshProUGUI buttonText;
    private Coroutine transitionCoroutine;

    private void Start()
    {
        initialScale = transform.localScale;
        buttonImage = GetComponent<Image>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();

        // Configura el color inicial (fondo translúcido y texto negro)
        buttonImage.color = normalBackgroundColor;
        buttonText.color = normalTextColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Detiene cualquier transición en progreso y comienza una nueva (cambia a fondo blanco y texto negro)
        if (transitionCoroutine != null) StopCoroutine(transitionCoroutine);
        transitionCoroutine = StartCoroutine(TransitionEffect(hoverBackgroundColor, hoverTextColor, initialScale * hoverScale));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Detiene cualquier transición en progreso y comienza una nueva hacia el estado normal
        if (transitionCoroutine != null) StopCoroutine(transitionCoroutine);
        transitionCoroutine = StartCoroutine(TransitionEffect(normalBackgroundColor, normalTextColor, initialScale));
    }

    private IEnumerator TransitionEffect(Color targetBackgroundColor, Color targetTextColor, Vector3 targetScale)
    {
        float timeElapsed = 0f;
        Color startBackgroundColor = buttonImage.color;
        Color startTextColor = buttonText.color;
        Vector3 startScale = transform.localScale;

        while (timeElapsed < transitionDuration)
        {
            // Interpola los valores de color y escala con el tiempo
            buttonImage.color = Color.Lerp(startBackgroundColor, targetBackgroundColor, timeElapsed / transitionDuration);
            buttonText.color = Color.Lerp(startTextColor, targetTextColor, timeElapsed / transitionDuration);
            transform.localScale = Vector3.Lerp(startScale, targetScale, timeElapsed / transitionDuration);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Asegúrate de que al final de la transición los valores sean exactamente los objetivos
        buttonImage.color = targetBackgroundColor;
        buttonText.color = targetTextColor;
        transform.localScale = targetScale;
    }
}
