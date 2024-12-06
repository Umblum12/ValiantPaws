using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropdownHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Color normalTextColor = Color.black; // Color del texto en estado normal
    public Color hoverTextColor = Color.red;   // Color del texto al hacer hover
    public Color normalBackgroundColor = Color.white;  // Color de fondo en estado normal
    public Color hoverBackgroundColor = Color.yellow;  // Color de fondo al hacer hover
    public float transitionDuration = 0.2f;   // Duración de la transición en segundos

    private Image optionImage; // Imagen de fondo de la opción
    private TextMeshProUGUI optionText; // Texto de la opción
    private Coroutine transitionCoroutine;

    private void Start()
    {
        optionImage = GetComponent<Image>();
        optionText = GetComponentInChildren<TextMeshProUGUI>();

        // Configura los colores iniciales
        if (optionImage != null) optionImage.color = normalBackgroundColor;
        if (optionText != null) optionText.color = normalTextColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Detén cualquier transición previa y comienza una nueva (hover)
        if (transitionCoroutine != null) StopCoroutine(transitionCoroutine);
        transitionCoroutine = StartCoroutine(TransitionEffect(hoverBackgroundColor, hoverTextColor));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Detén cualquier transición previa y regresa al estado normal
        if (transitionCoroutine != null) StopCoroutine(transitionCoroutine);
        transitionCoroutine = StartCoroutine(TransitionEffect(normalBackgroundColor, normalTextColor));
    }

    private IEnumerator TransitionEffect(Color targetBackgroundColor, Color targetTextColor)
    {
        float timeElapsed = 0f;
        Color startBackgroundColor = optionImage.color;
        Color startTextColor = optionText.color;

        while (timeElapsed < transitionDuration)
        {
            // Interpola los colores del fondo y texto
            if (optionImage != null)
                optionImage.color = Color.Lerp(startBackgroundColor, targetBackgroundColor, timeElapsed / transitionDuration);
            if (optionText != null)
                optionText.color = Color.Lerp(startTextColor, targetTextColor, timeElapsed / transitionDuration);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Asegúrate de que los colores finales sean los objetivos
        if (optionImage != null) optionImage.color = targetBackgroundColor;
        if (optionText != null) optionText.color = targetTextColor;
    }
}
