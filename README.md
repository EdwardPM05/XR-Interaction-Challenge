# XR Interaction Challenge

## Datos del estudiante

- **Apellidos y nombres:** Pittman Medina, Edward
- **Código del estudiante:** 2221899353
- **Curso:** Laboratorio de Realidad Extendida (XR) para Videojuegos
- **Docente:** Victor Alejandro Arroyo Castro
- **Universidad:** Universidad Autónoma del Perú — Facultad de Ingeniería y Arquitectura, Ingeniería de Software

## Descripción del proyecto

Proyecto realizado para el ec del curso, con una experiencia interactiva de entrenamiento XR. Dentro de una casa simple con objetos primarios y algunoss de decoracion. Presenta una mesa con dos objetos con el reto de colocarlos dentro del area verde indicada, y una lampara para la accion a distancia que se enciende y apaga.

ESCENA PRINCIPAL `Assets/Scenes/EC_XR_PittmanEdward.unity`.

## Funcionalidades implementadas

- **Configuración XR:** proyecto con URP, XR Plug-in Management, OpenXR y XR Interaction Toolkit, con XR Origin y XR Device Simulator.
- **Escenario:** piso, iluminación direccional, límites visuales (casa con paredes, puerta, ventanas y tejado) y más de cinco objetos 3D (mesa, sofá, chimenea, árbol, camino, lámpara, cubo y esfera).
- **Objetos manipulables:** un cubo y una esfera con `Rigidbody` y `XR Grab Interactable`; se pueden agarrar, mover y lanzar.
- **Interacción a distancia:** una lámpara con `XR Simple Interactable`; al apuntarla con el rayo y seleccionarla se enciende o apaga la luz y cambia su color (`LightToggle.cs`).
- **Reto libre:** zona de entrega con contador. Al dejar los dos objetos agarrables sobre la zona verde, el texto muestra "¡Reto completado! 2 / 2" (`DeliveryZone.cs`).

## Controles o instrucciones

Con visor VR se usan los mandos. Sin visor, con el **XR Device Simulator**:

| Acción | Control |
|---|---|
| Mover la cabeza / mirar | Mantener clic derecho y mover el ratón |
| Controlar mando izquierdo / derecho | Mantener Shift izquierdo / Espacio |
| Acercar o alejar el mando | Rueda del ratón mientras se mantiene Shift o Espacio |
| Apuntar con el rayo | Mover el mando hacia el objetivo |
| Agarrar un objeto | Botón Grip (tecla G) |
| Seleccionar (encender la lámpara) | Gatillo (clic izquierdo) |

Objetivo: agarrar el cubo y la esfera de la mesa, dejarlos sobre la zona verde y encender o apagar la lámpara con el rayo.

## Capturas de pantalla

1. **Vista general del escenario**

   ![Vista general](Screenshots/01_vista_general.png)

2. **Configuración XR / componentes en el Inspector**

   ![Inspector XR](Screenshots/02_inspector_xr.png)

3. **Interacción funcionando**

   ![Interacción](Screenshots/03_interaccion.png)

4. **Jerarquía de la escena (Hierarchy)**

   ![Hierarchy](Screenshots/04_hierarchy.png)

5. **Estructura del proyecto (Project)**

   ![Project](Screenshots/05_project.png)

## Video demostrativo (máx. 1 minuto)

[Ver video en YouTube](https://youtu.be/fLMTVzC6Rgk)

## Tecnologías y paquetes utilizados

- Unity 2022.3.62f3 con Universal Render Pipeline (URP 14.0.12)
- XR Interaction Toolkit 2.6.5 (Starter Assets y XR Device Simulator)
- XR Plug-in Management 4.6.1 y OpenXR 1.14.3
- AR Foundation 5.2.2
- C# para los scripts de interacción y un script de editor que genera la escena

## Cómo abrir el proyecto

1. Clonar el repositorio: `git clone https://github.com/EdwardPM05/XR-Interaction-Challenge.git`
2. Abrir la carpeta con Unity Hub usando la versión 2022.3.62f3.
3. Abrir la escena `Assets/Scenes/EC_XR_PittmanEdward.unity`.
4. Presionar Play.
