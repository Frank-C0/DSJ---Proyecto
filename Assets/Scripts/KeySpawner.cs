using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySpawner : MonoBehaviour
{
    [SerializeField] private GameObject keyPrefab; // Prefab de la llave.
    [SerializeField] private Transform[] keySpawnPoints; // Puntos de aparición de la llave.

    void Start()
    {
        SpawnKeyAtRandomLocation();
    }

    private void SpawnKeyAtRandomLocation()
    {
        // Verificar que tengamos al menos un punto de aparición.
        if (keySpawnPoints.Length == 0)
        {
            Debug.LogError("No hay puntos de aparición de la llave asignados.");
            return;
        }

        // Seleccionar aleatoriamente un índice entre 0 y el tamaño del arreglo de puntos de aparición.
        int randomIndex = Random.Range(0, keySpawnPoints.Length);

        // Instanciar la llave en la posición aleatoria.
        Instantiate(keyPrefab, keySpawnPoints[randomIndex].position, Quaternion.identity);
    }


    // make a gizmo to see the spawn points
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;

    //    foreach (Transform spawnPoint in keySpawnPoints)
    //    {
    //        Gizmos.DrawSphere(spawnPoint.position, 0.5f);
    //    }
    //}
}
