using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnknownSpawnOnDeathMod", menuName = "ScriptableObject/MRMods/SpawnOnDeathMod", order = 1)]
public class MRSpawnOnDeathMod : MonsterRoomModifier
{
    [SerializeField] private float probability = 0.5f;
    [SerializeField] private GameObject objectToSpawn = null;
    [SerializeField] private GameObject infusedVFX = null;

    public override void ApplyModifier(MonsterLife monster)
    {
        base.ApplyModifier(monster);
        if (Random.Range(0, 1f) <= probability)
        {
            var comp = monster.gameObject.AddComponent<SpawnOnDeath>();
            comp.toSpawn = objectToSpawn;
            comp.keepParentRotation = true;
            comp.infusedVFX = infusedVFX;
        }
    }
}
