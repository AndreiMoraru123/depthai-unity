using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [Header("Ammo")]
    public TextMeshProUGUI magazineAmmoUI;
    public TextMeshProUGUI totalAmmoUI;
    public Image ammoTypeUI;

    [Header("Weapon")]
    public Image activeWeaponUI;
    public Image unActiveWeaponUI;

    [Header("Throwables")]
    public Image lethalUI;
    public TextMeshProUGUI lethalAmountUI;

    public Image tacticalUI;
    public TextMeshProUGUI tacticalAmountUI;
    public Sprite emptySlot;
    public Sprite greySlot;

    public GameObject crosshair;

    public static HUDManager Instance { get; set; }
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

    void Update()
    {
        var activeWeapon = WeaponManager.Instance.activeWeaponSlot.GetComponentInChildren<PlayerWeapon>();
        var unActiveWeapon = GetUnActiveWeaponSlot().GetComponentInChildren<PlayerWeapon>();

        if (activeWeapon)
        {
            magazineAmmoUI.text = $"{activeWeapon.bulletsLeft / activeWeapon.bulletsPerBurst}";
            totalAmmoUI.text = WeaponManager.Instance.CheckAmmoLeft(activeWeapon.weaponModel).ToString();

            var model = activeWeapon.weaponModel;
            ammoTypeUI.sprite = GetAmmoSprite(model);
            activeWeaponUI.sprite = GetWeaponSprite(model);

            if (unActiveWeapon)
            {
                unActiveWeaponUI.sprite = GetWeaponSprite(unActiveWeapon.weaponModel);
            }
        }
        else
        {
            magazineAmmoUI.text = "";
            totalAmmoUI.text = "";

            ammoTypeUI.sprite = emptySlot;

            activeWeaponUI.sprite = emptySlot;
            unActiveWeaponUI.sprite = emptySlot;
        }

        if (WeaponManager.Instance.lethalsCount <= 0)
        {
            lethalUI.sprite = greySlot;
        }

        if (WeaponManager.Instance.tacticalsCount <= 0)
        {
            tacticalUI.sprite = greySlot;
        }
    }

    private Sprite GetWeaponSprite(WeaponModel model)
    {
        return model switch
        {
            WeaponModel.M1911 => Resources.Load<GameObject>("M1911_Weapon").GetComponent<SpriteRenderer>().sprite,
            WeaponModel.AK47 => Resources.Load<GameObject>("AK47_Weapon").GetComponent<SpriteRenderer>().sprite,
            _ => null,
        };
    }

    private Sprite GetAmmoSprite(WeaponModel model)
    {
        return model switch
        {
            WeaponModel.M1911 => Resources.Load<GameObject>("M1911_Ammo").GetComponent<SpriteRenderer>().sprite,
            WeaponModel.AK47 => Resources.Load<GameObject>("AK47_Ammo").GetComponent<SpriteRenderer>().sprite,
            _ => null,
        };
    }

    private GameObject GetUnActiveWeaponSlot()
    {
        foreach (var slot in WeaponManager.Instance.weaponSlots)
        {
            if (slot != WeaponManager.Instance.activeWeaponSlot)
            {
                return slot;
            }
        }
        return null;
    }

    internal void UpdateThrowablesUI()
    {
        lethalAmountUI.text = $"{WeaponManager.Instance.lethalsCount}";
        tacticalAmountUI.text = $"{WeaponManager.Instance.tacticalsCount}";

        switch (WeaponManager.Instance.equippedLethalType)
        {
            case ThrowableType.Grenade:
                lethalUI.sprite = Resources.Load<GameObject>("Grenade").GetComponent<SpriteRenderer>().sprite;
                break;
        }
        switch (WeaponManager.Instance.equippedTacticalType)
        {
            case ThrowableType.Smoke:
                tacticalUI.sprite = Resources.Load<GameObject>("SmokeGrenade").GetComponent<SpriteRenderer>().sprite;
                break;
        }
    }
}
