using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;

public class FirstPersonShooterController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private Transform pfBulletProjectile;
    [SerializeField] private Transform spawnBulletPosition;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float rayDistance = 500f;
    [SerializeField] private LayerMask layerMask;

    private StarterAssetsInputs starterAssetsInputs;
    private FirstPersonController firstPersonController;

    private void Awake()
    {
        firstPersonController = GetComponent<FirstPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
    }

    private void Update()
    {
        if (starterAssetsInputs.aim)
        {
            aimVirtualCamera.gameObject.SetActive(true);
            firstPersonController.RotationSpeed = aimSensitivity;
        } else
        {
            aimVirtualCamera.gameObject.SetActive(false);
            firstPersonController.RotationSpeed = normalSensitivity;
        }

        if (starterAssetsInputs.shoot)
        {
            Ray ray = new Ray(spawnBulletPosition.transform.position, mainCamera.transform.forward);
            Instantiate(pfBulletProjectile, spawnBulletPosition.position, mainCamera.transform.rotation);
            Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.green);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, rayDistance, layerMask))
            {
                Debug.Log("Hit interactable");
            }
            starterAssetsInputs.shoot = false;
        }
    }
}
