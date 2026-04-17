using UnityEngine;

// Script desactivado: los sprites placeholder blancos sobrescribian el sprite real del jugador.
// El setup del sprite + collider ahora se hace en ScoreManager.SetupPlayerColliders().
public class SetupAnimations : MonoBehaviour
{
    void Awake()
    {
        // No-op. Conservado para no romper referencias en la escena.
    }
}
