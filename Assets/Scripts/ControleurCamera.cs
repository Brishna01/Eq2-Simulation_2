using System;
using UnityEngine;

public class ControleurCamera : MonoBehaviour
{
    [SerializeField]
    private GameObject terrain;
    private GrilleCircuit grilleCircuit;

    [SerializeField]
    private float tailleInitiale;
    [SerializeField]
    private float tailleMinimale;
    [SerializeField]
    private float tailleMaximale;
    [SerializeField]
    private bool peutDeplacer;

    private Vector3 positionSourisPrecedente;
    private bool boutonDroitEnfonce = false;

    private new Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
        camera.orthographicSize = tailleInitiale;

        if (terrain != null)
        {
            grilleCircuit = terrain.GetComponent<GrilleCircuit>();
            camera.transform.position += new Vector3(
                grilleCircuit.origineGrilleX + (grilleCircuit.colonnes - 1) / 2 + 0.5f, 
                grilleCircuit.origineGrilleY + (grilleCircuit.lignes - 1) / 2 + 0.5f, 
                0
            );
            camera.orthographicSize = 0.4f * Mathf.Sqrt(Mathf.Pow(grilleCircuit.colonnes, 2) + Mathf.Pow(grilleCircuit.lignes, 2));
        }

        positionSourisPrecedente = Input.mousePosition;
    }

    // Update is called once per frame
    void Update()
    {
        GererDeplacement();
        ContraindreCamera();

        camera.orthographicSize = Math.Clamp(camera.orthographicSize - 0.5f * Input.mouseScrollDelta.y, tailleMinimale,
            tailleMaximale);
    }

    private void GererDeplacement()
    {
        if (peutDeplacer) 
        {
            if (Input.GetMouseButtonDown(1))
            {
                boutonDroitEnfonce = true;
            }
            else if (Input.GetMouseButtonUp(1))
            {
                boutonDroitEnfonce = false;
            }
            else if (boutonDroitEnfonce)
            {
                Vector3 deltaPositionSouris = Input.mousePosition - positionSourisPrecedente;
                camera.transform.Translate(
                    -deltaPositionSouris.x / camera.pixelHeight * camera.orthographicSize * 2,
                    -deltaPositionSouris.y / camera.pixelHeight * camera.orthographicSize * 2,
                    0
                );
            }
        }
        
        positionSourisPrecedente = Input.mousePosition;
    }

    private void ContraindreCamera()
    {
        if (grilleCircuit != null)
        {
            float tailleCameraX = camera.pixelWidth / camera.pixelHeight * camera.orthographicSize * 1.9f;

            (float, float) limitesX = CalculerLimitesCamera(
                grilleCircuit.origineGrilleX - 2, 
                grilleCircuit.origineGrilleY + grilleCircuit.colonnes + 1,
                tailleCameraX
            );
            (float, float) limitesY = CalculerLimitesCamera(
                grilleCircuit.origineGrilleY - 2, 
                grilleCircuit.origineGrilleY + grilleCircuit.lignes + 1,
                camera.orthographicSize);

            float nouveauX = Math.Clamp(camera.transform.position.x, limitesX.Item1, limitesX.Item2);
            float nouveauY = Math.Clamp(camera.transform.position.y, limitesY.Item1, limitesY.Item2);

            camera.transform.position = new Vector3(nouveauX, nouveauY, camera.transform.position.z);
        }
    }

    /// <summary>
    /// Retourne les nouvelles limites d'une composante de la position de la 
    /// caméra de sorte qu'aucune position hors-limites ne soit visible sur 
    /// l'écran.
    /// </summary>
    /// <param name="min">le minimum de la composante</param>
    /// <param name="max">le maximum de la composante</param>
    /// <param name="dimensionCamera"></param>
    /// <returns>les nouveaux minimum et maximum</returns>
    private (float, float) CalculerLimitesCamera(float min, float max, float dimensionCamera)
    {
        float nouveauMin = min + dimensionCamera < max - dimensionCamera
                ? min + dimensionCamera
                : (min + max) / 2;
        float nouveauMax = max - dimensionCamera > min + dimensionCamera
                ? max - dimensionCamera
                : (min + max) / 2;

        return (nouveauMin, nouveauMax);
    }
}
