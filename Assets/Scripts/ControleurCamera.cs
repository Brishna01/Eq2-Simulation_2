using System;
using UnityEngine;

/// <summary>
/// Contrôle le mouvement de la caméra.
/// </summary>
public class ControleurCamera : MonoBehaviour
{
    [SerializeField]
    private GameObject circuit;
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

        if (circuit != null)
        {
            grilleCircuit = circuit.GetComponent<GrilleCircuit>();
            float largeur = grilleCircuit.nombreCellules.x * grilleCircuit.tailleCellule.x;
            float hauteur = grilleCircuit.nombreCellules.y * grilleCircuit.tailleCellule.y;

            camera.transform.position += new Vector3(
                grilleCircuit.origine.x + largeur / 2, 
                grilleCircuit.origine.y + hauteur / 2, 
                0
            );
            camera.orthographicSize = 0.4f * Mathf.Sqrt(Mathf.Pow(largeur, 2) + Mathf.Pow(hauteur, 2));
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

    /// <summary>
    /// Fait déplacer la caméra selon le mouvement de la souris lorsque le 
    /// bouton droit est enfoncé.
    /// </summary>
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

    /// <summary>
    /// Garde la caméra dedans une certaine région centrée sur la grille.
    /// </summary>
    private void ContraindreCamera()
    {
        if (grilleCircuit != null)
        {
            // 1.9f est un nombre arbitraire obtenu par essai-erreur qui fonctionne
            float tailleCameraX = camera.pixelWidth / camera.pixelHeight * camera.orthographicSize * 1.9f;

            (float, float) limitesX = CalculerLimitesCamera(
                grilleCircuit.origine.x - 2, 
                grilleCircuit.origine.x + grilleCircuit.nombreCellules.x * grilleCircuit.tailleCellule.x + 2,
                tailleCameraX
            );
            (float, float) limitesY = CalculerLimitesCamera(
                grilleCircuit.origine.y - 2, 
                grilleCircuit.origine.y + grilleCircuit.nombreCellules.y * grilleCircuit.tailleCellule.y + 2,
                camera.orthographicSize);

            float nouveauX = Math.Clamp(camera.transform.position.x, limitesX.Item1, limitesX.Item2);
            float nouveauY = Math.Clamp(camera.transform.position.y, limitesY.Item1, limitesY.Item2);

            camera.transform.position = new Vector3(nouveauX, nouveauY, camera.transform.position.z);
        }
    }

    /// <summary>
    /// Retourne les nouvelles limites d'une composante de la position de la 
    /// caméra de sorte qu'aucune position hors-limites ne soit visible sur 
    /// l'écran. Si ce n'est pas possible, le milieu est retourné.
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
