using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// Reto libre: contador de objetos agarrables dejados dentro de la zona.
[RequireComponent(typeof(Collider))]
public class DeliveryZone : MonoBehaviour
{
    [SerializeField] TextMesh counterText;
    [SerializeField] int goal = 2;

    readonly HashSet<XRGrabInteractable> inside = new HashSet<XRGrabInteractable>();

    void Reset() => GetComponent<Collider>().isTrigger = true;

    void Start() => UpdateText();

    void OnTriggerEnter(Collider other)
    {
        var grab = other.GetComponentInParent<XRGrabInteractable>();
        if (grab != null && inside.Add(grab)) UpdateText();
    }

    void OnTriggerExit(Collider other)
    {
        var grab = other.GetComponentInParent<XRGrabInteractable>();
        if (grab != null && inside.Remove(grab)) UpdateText();
    }

    void UpdateText()
    {
        if (counterText == null) return;
        counterText.text = inside.Count >= goal
            ? "¡Reto completado!\n" + inside.Count + " / " + goal
            : "Objetos en zona: " + inside.Count + " / " + goal;
    }
}
