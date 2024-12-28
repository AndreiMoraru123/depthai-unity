using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; set; }
    public PlayerWeapon hoveredWeapon = null;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject objectHit = hit.transform.gameObject;
            if (objectHit.GetComponent<PlayerWeapon>())
            {
                hoveredWeapon = objectHit.gameObject.GetComponent<PlayerWeapon>();
                WeaponPickUp pickUp = objectHit.GetComponent<WeaponPickUp>();

                hoveredWeapon.GetComponent<Outline>().enabled = true;

                if (pickUp != null && pickUp.ValidateInteraction() && pickUp.IsReadyForPickup())
                {
                    pickUp.BaseInteract();
                }
            }
            else
            {
                if (hoveredWeapon)
                {
                    hoveredWeapon.GetComponent<Outline>().enabled = false;
                }
            }
        }
        else
        {
            if (hoveredWeapon)
            {
                hoveredWeapon.GetComponent<Outline>().enabled = false;
            }
        }
    }
}
