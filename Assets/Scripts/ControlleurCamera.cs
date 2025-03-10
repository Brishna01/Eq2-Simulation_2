using System;
using UnityEngine;

public class ControlleurCamera : MonoBehaviour
{
    private new Camera camera;

    [SerializeField]
    private RectTransform rectangleLimites;

    [SerializeField]
    private float tailleMinimale;
    [SerializeField]
    private float tailleMaximale;
    [SerializeField]
    private bool peutDeplacer;

    private Vector3 positionSourisPrecedente;
    private bool boutonDroitEnfonce = false;

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
        positionSourisPrecedente = Input.mousePosition;
    }

    // Update is called once per frame
    void Update()
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

        if (rectangleLimites != null)
        {
            float tailleCameraX = camera.pixelWidth / camera.pixelHeight * camera.orthographicSize * 2;
            (float, float) limitesX = CalculerLimitesCamera(rectangleLimites.rect.xMin, rectangleLimites.rect.xMax,
                    tailleCameraX);
            (float, float) limitesY = CalculerLimitesCamera(rectangleLimites.rect.yMin, rectangleLimites.rect.yMax,
                    camera.orthographicSize);

            float nouveauX = Math.Clamp(camera.transform.position.x, limitesX.Item1, limitesX.Item2);
            float nouveauY = Math.Clamp(camera.transform.position.y, limitesY.Item1, limitesY.Item2);

            camera.transform.position = new Vector3(nouveauX, nouveauY, camera.transform.position.z);
        }

        camera.orthographicSize = Math.Clamp(camera.orthographicSize - 0.5f * Input.mouseScrollDelta.y, tailleMinimale,
            tailleMaximale);

        positionSourisPrecedente = Input.mousePosition;
    }

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
