using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// Interaccion a distancia: al seleccionar la lampara con el rayo se enciende/apaga
// la luz y cambia el color de la esfera.
[RequireComponent(typeof(XRSimpleInteractable))]
public class LightToggle : MonoBehaviour
{
    [SerializeField] Light targetLight;
    [SerializeField] Renderer lampRenderer;
    [SerializeField] Color onColor = new Color(1f, 0.92f, 0.4f);
    [SerializeField] Color offColor = new Color(0.2f, 0.2f, 0.2f);

    XRSimpleInteractable interactable;
    bool isOn = true;

    void Awake()
    {
        interactable = GetComponent<XRSimpleInteractable>();
        Apply();
    }

    void OnEnable() => interactable.selectEntered.AddListener(OnSelected);
    void OnDisable() => interactable.selectEntered.RemoveListener(OnSelected);

    void OnSelected(SelectEnterEventArgs args)
    {
        isOn = !isOn;
        Apply();
    }

    void Apply()
    {
        if (targetLight != null) targetLight.enabled = isOn;
        if (lampRenderer != null)
        {
            var color = isOn ? onColor : offColor;
            lampRenderer.material.color = color;
            lampRenderer.material.SetColor("_BaseColor", color);
        }
    }
}
