# 🎮 Nombre de tu Prototipo de Videojuego

* **Desarrollador:** Florencia Itati Quispe
* **LU (Libreta Universitaria):** TUV000805
---

## 📖 1. Descripción del Juego
Resumen conceptual del nivel presentado: Un prototipo de aventura y plataformas en 3D desarrollado en Unity donde el jugador debe explorar un entorno diseñado con *Terrain Tools*, interactuar con objetos coleccionables y cumplir las condiciones de victoria del nivel.
---

## ⚙️ 2. Mecánicas Principales del Juego
* **Sistema de Movimiento y Gravedad:** Desplazamiento fluido basado en `Rigidbody` con restricciones de rotación y soporte para plataformas móviles (*ascensores*).
* **Sistema de Salud y Daño:** Control de vida del jugador (`HealthSystem`) afectado por interacciones de combate y peligros ambientales persistentes.
* **Recolección y Entrega:** Mecánica para tomar objetos del escenario, transportarlos y entregarlos de forma exitosa en la zona de meta (`GoalZone`).
* **Condición de Victoria Dual:** Posibilidad de completar el nivel eliminando a todos los enemigos de la escena o mediante la entrega del ítem en la meta.

---

## ⌨️ 3. Guía de Controles
* **Movimiento:** Teclas `W`, `A`, `S`, `D` .
* **Salto:** Barra Espaciadora (`Space`).
* **Ataque:** `click izquierdo`.
* **Recolección de ítems:** Tecla 'E'.

---

## 🛠️ 4. Detalle de Opcionales Elegidos
Explicación técnica de las 3 opciones implementadas:

1. **Plataformas Móviles:** Programado en `PlayerMovement.cs` mediante detección por colisiones continuas (`OnCollisionStay`) y emparentamiento dinámico (`transform.SetParent`) para acompañar el recorrido de la plataforma.
2. **Zonas de Daño Continuo (Gas Letal):** Programado en `LetalGas.cs`. Utiliza `OnTriggerStay` aplicando daño constante por segundo (`damagePerSecond * Time.deltaTime`) sobre el `HealthSystem`.
3. **Entrega de Ítems en Zona de Meta:** Programado en `GoalZone.cs`. Valida la llegada del jugador, extrae el ítem transportado (`PickItem.DropItem()`), lo reubica en la meta y notifica al `GameManager`.

---

## 🧰 5. Requisitos y Configuración de Unity

* **Versión de Unity:** Unity6.5
* **Render Pipeline:** Built-in Render Pipeline (o URP si lo utilizaste).
* **Paquetes / Packages Requeridos:**
  * **Terrain Tools:** Para la generación y modelado del mapa de terreno.
  * **Input Manager:** Configuración por defecto para ejes `Horizontal`, `Vertical` y botón `Jump`.
* **Capas / Layers Configuradas:**
  * Capa `Ground` (o `Default`) asignada al Terrain para la detección del Raycast de suelo.
* **Etiquetas / Tags Requeridas:**
  * `Player`: Asignado al objeto principal del jugador.
  * `Plataforma`: Asignado a las plataformas móviles para activar el emparentamiento.
  * `Character` : Asignado al enemigo para detectar el ataque.

---

## 📸 6. Capturas del Nivel en Ejecución

| Vista General del Terreno y Jugador | Zona de Meta (`GoalZone`) |
| :---: | :---: |
| ![Terreno](images/captura1.jpeg) | ![Meta](images/captura2.jpeg) |