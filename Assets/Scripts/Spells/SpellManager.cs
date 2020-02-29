using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellManager : MonoBehaviour
{
    [SerializeField] private Transform Firepos = default;
    [SerializeField] private Transform Model = default;

    private SpellInventory spellInventory = default;
    private int casterType = default;
    private Transform target = default;

    private void Awake()
    {
        spellInventory = GetComponent<SpellInventory>();
        casterType = spellInventory.GetCasterType();
    }

    public void ActivateSpell(Spell spell, int spellSlot)
    {
        spell.SetSpellSlot(spellSlot);
        switch (spell.spellType)
        {
            case SpellType.AOE:
                ActivateAOESpell(spell);
                break;
            case SpellType.Projectile:
                ActivateProjectileSpell(spell);
                break;
            case SpellType.Utility:
                ActivateUtilitySpell(spell);
                break;
            case SpellType.Melee:
                ActivateMeleeSpell(spell);
                break;
            case SpellType.Summon:
                ActivateSummoningSpell(spell);
                break;
        }
    }

    private void ActivateAOESpell(Spell spell)
    {
        switch (spell.shape)
        {
            default:
            case 0:
                if (spell.instances <= 1)
                {
                    GameObject aoeObject = Instantiate(spell.aoe, Vector3.zero, Model.rotation);
                    AOESpell aoe = aoeObject.GetComponent<AOESpell>();
                    aoe.transform.SetParent(transform, false);
                    AoESpellHandler(spell, aoe);
                    aoe.Activate();
                }
                else if (spell.instances > 1)
                {
                    StartCoroutine("AoEInstanceHandler", spell);
                }
                break;
            case 1:
                for (int i = 0; i < spell.instances; i++)
                {
                    Quaternion rotation = CalcRotation(spell.instances, i, 360f);
                    GameObject aoeObject = Instantiate(spell.aoe, Vector3.zero, rotation);
                    AOESpell aoe = aoeObject.GetComponent<AOESpell>();
                    aoeObject.transform.SetParent(transform, false);
                    AoESpellHandler(spell, aoe);
                    aoe.Activate();
                }
                break;
            case 2:
                if (spell.instances <= 1)
                {
                    Vector3 targetPos;
                    if (casterType == 1)
                    {
                        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        mousePos.y = 0f;
                        targetPos = mousePos;
                    }
                    else 
                    {
                        targetPos = target.position;
                    }
                    GameObject aoeObject = Instantiate(spell.aoe, targetPos, Model.transform.rotation);
                    AOESpell aoe = aoeObject.GetComponent<AOESpell>();
                    AoESpellHandler(spell, aoe);
                    aoe.Activate();
                }
                else if (spell.instances > 1)
                {
                    StartCoroutine("AoEInstanceHandler", spell);
                }
                break;
        }
    }

    private void ActivateProjectileSpell(Spell spell)
    {
        if (spell.shape == 0 || spell.shape == 1)
        {
            if (spell.instances <= 1)
            {
                GameObject projectileObject = Instantiate(spell.projectile, Firepos.position, Model.rotation);
                ProjectileSpell projectile = projectileObject.GetComponent<ProjectileSpell>();
                projectileObject.transform.SetParent(Firepos);
                if (spell.shape == 1)
                {
                    projectileObject.transform.SetParent(null, true);
                }
                ProjectileSpellHandler(spell, projectile);
                projectile.Activate();
            }
            else if (spell.instances > 1)
            {
                ProjectileInstanceHandler(spell);
            }
        }
        else if (spell.shape == 2)
        {
            GameObject projectileObject = Instantiate(spell.projectile, Firepos.position, Model.rotation);
            ProjectileSpell projectile = projectileObject.GetComponent<ProjectileSpell>();
            projectileObject.transform.SetParent(Firepos);
            projectileObject.transform.SetParent(null, true);
            projectile.SetFirePos(Firepos);
            projectile.SetInstances(spell.instances);
            ProjectileSpellHandler(spell, projectile);
            projectile.Activate();
        }
    }

    private void ActivateUtilitySpell(Spell spell)
    {
        switch (spell.utilityType)
        {
            case (UtilityType.Shield):
                switch (spell.shape)
                {
                    default:
                    case 0:
                        UtilityBarrierHandler(spell);
                        break;
                    case 1:
                        UtilityShieldHandler(spell);
                        break;
                    case 2:
                        UtilityDeployHandler(spell);
                        break;
                }
                break;
            case (UtilityType.Heal):
                HealSpellHandler(spell);
                break;
            case (UtilityType.Movement):
                ActivateMovementSpell(spell);
                break;
        }
    }

    private void ActivateMeleeSpell(Spell spell)
    {
        switch (spell.shape)
        {
            case 0:
                StartCoroutine("MeleeSwordHandler", spell);
                break;
            case 1:
                StartCoroutine("MeleeSpearHandler", spell);
                break;
            case 2:
                StartCoroutine("MeleeAxeHandler", spell);
                break;
        }
    }

    private void ActivateMovementSpell(Spell spell)
    {
        if (spell.shape == 0 || spell.shape == 1)
        {
            if (GetComponentInChildren<MovementSpell>() != null)
                if (GetComponentInChildren<MovementSpell>().GetShape() == spell.shape) { return; }

            GameObject movementObject = Instantiate(spell.utility, Vector3.zero, Model.rotation);
            MovementSpell movementSpell = movementObject.GetComponent<MovementSpell>();
            movementSpell.transform.SetParent(transform, false);
            MovementSpellHandler(spell, movementSpell);
            movementSpell.Activate();
        }
        else if (spell.shape == 2)
        {
            PushInstanceHandler(spell);
        }
    }

    private void ActivateSummoningSpell(Spell spell)
    {
        switch (spell.shape)
        {
            case 0:
                if (spell.instances <= 1)
                {
                    GameObject summonObject = Instantiate(spell.summon, transform.position, Model.rotation);
                    SummoningSpell summon = summonObject.GetComponent<SummoningSpell>();
                    summonObject.transform.SetParent(transform, true);
                    summonObject.transform.localPosition = Vector3.zero;
                    SummoningSpellHandler(spell, summon);
                    summon.Activate();
                }
                else if (spell.instances > 1)
                {
                    SummonInstanceHandler(spell);
                }
                break;
            case 1:
                if (spell.instances <= 1)
                {
                    GameObject summonObject = Instantiate(spell.summon, transform.position, Model.rotation);
                    SummoningSpell summon = summonObject.GetComponent<SummoningSpell>();
                    summonObject.transform.SetParent(transform, true);
                    summonObject.transform.localPosition = Vector3.up;
                    summonObject.transform.Translate(Vector3.back * 1f);
                    SummoningSpellHandler(spell, summon);
                    summon.Activate();
                }
                else if (spell.instances > 1)
                {
                    SummonInstanceHandler(spell);
                }
                break;
            case 2:
                if (spell.instances <= 1)
                {
                    GameObject summonObject = Instantiate(spell.summon, transform.position, Model.rotation);
                    SummoningSpell summon = summonObject.GetComponent<SummoningSpell>();
                    summonObject.transform.SetParent(transform, true);
                    summonObject.transform.localPosition = Vector3.up;
                    summonObject.transform.Translate(Vector3.forward * 1f);
                    SummoningSpellHandler(spell, summon);
                    summon.Activate();
                }
                else if (spell.instances > 1)
                {
                    SummonInstanceHandler(spell);
                }
                break;
        }
    }

    private void ProjectileSpellHandler(Spell spell, ProjectileSpell projectile) 
    {
        projectile.SetElement(spell.element);
        projectile.SetUnique(spell.unique);
        projectile.SetSpeed(spell.speed);
        projectile.SetShape(spell.shape);
        projectile.SetDamage(spell.power);
        projectile.SetSize(spell.size);
        projectile.SetLifetime(spell.lifetime);
        projectile.SetSlot(spell.GetSpellSlot());
        projectile.SetSpellInventory(spellInventory);
    }

    private void AoESpellHandler(Spell spell, AOESpell aoe)
    {
        aoe.SetElement(spell.element);
        aoe.SetCaster(transform);
        aoe.SetSpeed(spell.speed);
        aoe.SetDamage(spell.power);
        aoe.SetSize(spell.size);
        aoe.SetLifetime(spell.lifetime);
        aoe.SetShape(spell.shape);
        aoe.SetSlot(spell.GetSpellSlot());
        aoe.SetSpellInventory(spellInventory);
    }

    private void UtilityBarrierHandler(Spell spell)
    {
        GameObject barrierObject = Instantiate(spell.utility, Vector3.zero, Model.rotation);
        ShieldSpell barrier = barrierObject.GetComponent<ShieldSpell>();
        barrier.transform.SetParent(transform, false);
        ShieldSpellHandler(spell, barrier);
        barrier.Activate();
    }

    private void UtilityShieldHandler(Spell spell)
    {
        for (int i = 0; i < spell.instances; i++)
        {
            Quaternion rotation = CalcRotation(spell.instances, i, 360f);
            GameObject shieldObject = Instantiate(spell.utility, Vector3.zero, rotation);
            ShieldSpell shield = shieldObject.GetComponent<ShieldSpell>();
            shieldObject.transform.SetParent(transform, false);
            ShieldSpellHandler(spell, shield);
            shield.Activate();
        }
    }

    private void UtilityDeployHandler(Spell spell)
    {
        for (int i = 0; i < spell.instances; i++)
        {
            Quaternion rotation = CalcRotation(spell.instances, i, 360f);
            GameObject deployObject = Instantiate(spell.utility, transform.position, rotation);
            ShieldSpell deploy = deployObject.GetComponent<ShieldSpell>();
            ShieldSpellHandler(spell, deploy);
            deploy.Activate();
        }
    }

    private void MeleeSpellHandler(Spell spell, MeleeSpell weapon)
    {
        weapon.SetElement(spell.element);
        weapon.SetLifetime(spell.lifetime);
        weapon.SetSpeed(spell.speed);
        weapon.SetDamage(spell.power);
        weapon.SetShape(spell.shape);
        weapon.SetSize(spell.size);
        weapon.SetSlot(spell.GetSpellSlot());
        weapon.SetSpellInventory(spellInventory);
    }

    private void ShieldSpellHandler(Spell spell, ShieldSpell shield)
    {
        shield.SetElement(spell.element);
        shield.SetCaster(transform);
        shield.SetLifetime(spell.lifetime);
        shield.SetSpeed(spell.speed);
        shield.SetDamage(spell.power);
        shield.SetShape(spell.shape);
        shield.SetSize(spell.size);
        shield.SetSlot(spell.GetSpellSlot());
        shield.SetSpellInventory(spellInventory);
    }

    private void HealSpellHandler(Spell spell)
    {
        GameObject healObject = Instantiate(spell.utility, Vector3.zero, Quaternion.identity);
        HealSpell heal = healObject.GetComponent<HealSpell>();
        heal.transform.SetParent(transform, false);
        heal.SetElement(spell.element);
        heal.SetShape(spell.shape);
        heal.SetLifetime(spell.lifetime);
        heal.SetDamage(spell.power);
        heal.SetSpeed(spell.speed);
        heal.SetInstances(spell.instances);
        heal.SetSize(spell.size);
        heal.SetCaster(gameObject);
        heal.SetSlot(spell.GetSpellSlot());
        heal.SetSpellInventory(spellInventory);
        heal.Activate();
    }

    private void MovementSpellHandler(Spell spell, MovementSpell movement)
    {
        movement.SetElement(spell.element);
        movement.SetShape(spell.shape);
        movement.SetLifetime(spell.lifetime);
        movement.SetDamage(spell.power);
        movement.SetSpeed(spell.speed);
        movement.SetInstances(spell.instances);
        movement.SetSize(spell.size);
        movement.SetCaster(gameObject);
        movement.SetFirePos(Firepos);
        movement.SetSlot(spell.GetSpellSlot());
        movement.SetSpellInventory(spellInventory);
    }

    private void SummoningSpellHandler(Spell spell, SummoningSpell summon)
    {
        summon.SetElement(spell.element);
        summon.SetSpeed(spell.speed);
        summon.SetShape(spell.shape);
        summon.SetDamage(spell.power);
        summon.SetSize(spell.size);
        summon.SetLifetime(spell.lifetime);
        summon.SetSlot(spell.GetSpellSlot());
        summon.SetSpellInventory(spellInventory);
        summon.SetCaster(transform);
    }

    private void ProjectileInstanceHandler(Spell spell)
    {
        int instances = spell.instances;
        float spreadAngle = 0f;
        if (spell.shape == 0 || spell.shape == 1)
        {
            spreadAngle = 15f * instances;
            spreadAngle = Mathf.Clamp(spreadAngle, 30f, 160f);
        }
        float perBulletAngle = spreadAngle / (instances - 1);
        float startAngle = spreadAngle * -0.5f;

        for (int i = 0; i < instances; i++)
        {
            GameObject projectileObject;
            if (spell.shape == 0 || spell.shape == 1)
            {
                projectileObject = Instantiate(spell.projectile, Firepos.position, Model.rotation);
                projectileObject.transform.SetParent(Firepos);
                projectileObject.transform.Rotate(Vector3.up, startAngle + i * perBulletAngle);
                if (spell.shape == 0)
                {
                    projectileObject.transform.SetParent(null, true);
                }
                ProjectileSpell projectile = projectileObject.GetComponent<ProjectileSpell>();
                ProjectileSpellHandler(spell, projectile);
                projectile.Activate();
            }
        }
    }

    private void SummonInstanceHandler(Spell spell)
    {
        int instances = spell.instances;

        for (int i = 0; i < instances; i++)
        {
            GameObject summonObject;
            switch (spell.shape)
            {
                case 0:
                    summonObject = Instantiate(spell.summon, Firepos.position, CalcRotation(spell.instances, i, 360f));
                    summonObject.transform.SetParent(transform, true);
                    summonObject.transform.localPosition = Vector3.zero;
                    SummoningSpell summon = summonObject.GetComponent<SummoningSpell>();
                    SummoningSpellHandler(spell, summon);
                    summon.Activate();
                    break;
                case 1:
                    summonObject = Instantiate(spell.summon, transform.position, CalcRotation(spell.instances, i, 140f));
                    SummoningSpell summon2 = summonObject.GetComponent<SummoningSpell>();
                    summonObject.transform.SetParent(transform, true);
                    summonObject.transform.localPosition = Vector3.up;
                    summonObject.transform.Translate(Vector3.back * 3f);
                    SummoningSpellHandler(spell, summon2);
                    summon2.Activate();
                    break;
                case 2:
                    summonObject = Instantiate(spell.summon, transform.position, CalcRotation(spell.instances, i, 140f));
                    SummoningSpell summon3 = summonObject.GetComponent<SummoningSpell>();
                    summonObject.transform.SetParent(transform, true);
                    summonObject.transform.localPosition = Vector3.up;
                    summonObject.transform.Translate(Vector3.forward * 3f);
                    SummoningSpellHandler(spell, summon3);
                    summon3.Activate();
                    break;
            }
        }
    }

    private void PushInstanceHandler(Spell spell)
    {
        int instances = spell.instances;
        float spreadAngle = 360f;
        float perBulletAngle = spreadAngle / (instances - 1);
        float startAngle = spreadAngle * -0.5f;

        for (int i = 0; i < instances; i++)
        {
            GameObject pushObject;
            pushObject = Instantiate(spell.utility, Firepos.position, Model.rotation);
            pushObject.transform.SetParent(Firepos);
            pushObject.transform.Rotate(Vector3.up, startAngle + i * perBulletAngle);
            pushObject.transform.SetParent(null, true);
            MovementSpell push = pushObject.GetComponent<MovementSpell>();
            MovementSpellHandler(spell, push);
            push.Activate();
        }
    }

    private IEnumerator AoEInstanceHandler(Spell spell)
    {
        Vector3 pos = Vector3.zero;
        if (spell.shape == 2)
        {
            if (casterType == 1)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.y = 0f;
                pos = mousePos;
            }
            else
            {
                pos = target.position;
            }
        }
        int instances = spell.instances;
        for (int i = 0; i < instances; i++)
        {
            GameObject aoeObject = Instantiate(spell.aoe, pos, Model.rotation);
            AOESpell aoe = aoeObject.GetComponent<AOESpell>();
            aoe.transform.SetParent(transform, false);
            AoESpellHandler(spell, aoe);
            aoe.Activate();
            yield return new WaitForSeconds(1f / (spell.speed * 2f));
        }
    }

    private IEnumerator MeleeSwordHandler(Spell spell)
    {
        bool invert = false;
        for (int i = 0; i < spell.instances; i++)
        {
            GameObject swordObject = Instantiate(spell.melee, Firepos.position, Model.rotation);
            if (invert)
                swordObject.transform.Rotate(Vector3.up, 75f);
            else
                swordObject.transform.Rotate(Vector3.up, -75f);
            swordObject.transform.Rotate(Vector3.right, Random.Range(-30f,30f));
            swordObject.transform.SetParent(Firepos, true);
            MeleeSpell sword = swordObject.GetComponent<MeleeSpell>();
            MeleeSpellHandler(spell, sword);
            sword.SetDirection(invert);
            sword.Activate();
            invert = !invert;
            yield return new WaitForSeconds(1f / (spell.speed * 2f));
        }
    }

    private IEnumerator MeleeSpearHandler(Spell spell) 
    {
        for (int i = 0; i < spell.instances; i++)
        {
            GameObject spearObject = Instantiate(spell.melee, Firepos.position, Model.rotation);
            spearObject.transform.SetParent(Firepos, true);
            spearObject.transform.Translate(new Vector3(Random.Range(0.75f + (spell.size/4f), 1.25f + (spell.size / 4f)), Random.Range(0f, 0.5f + (spell.size / 4f)), -1f));
            MeleeSpell spear = spearObject.GetComponent<MeleeSpell>();
            MeleeSpellHandler(spell, spear);
            spear.Activate();
            yield return new WaitForSeconds(1f / (spell.speed * 5f));
        }
    }

    private IEnumerator MeleeAxeHandler(Spell spell)
    {
        for (int i = 0; i < spell.instances; i++)
        {
            GameObject axeObject = Instantiate(spell.melee, Firepos.position, Model.rotation);
            axeObject.transform.Rotate(Vector3.right, -45f);
            axeObject.transform.Rotate(Vector3.forward, Random.Range(-5f, 5f));
            axeObject.transform.SetParent(Firepos, true);
            MeleeSpell axe = axeObject.GetComponent<MeleeSpell>();
            MeleeSpellHandler(spell, axe);
            axe.Activate();
            yield return new WaitForSeconds(1f / (spell.speed *2f));
        }
    }

    private Quaternion CalcRotation(int instances, int index, float rotationMax)
    {
        Quaternion rotation = Quaternion.Euler(0f, (rotationMax / instances) * index, 0f);
        return rotation;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
