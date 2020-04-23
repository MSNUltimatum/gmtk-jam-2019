using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TreeGeneratorMod", menuName = "ScriptableObject/BulletModifier/TreeGeneratorMod", order = 1)]
public class TreeGeneratorMod : BulletModifier
{
    public override void KillModifier(BulletLife bullet, MonsterLife enemy)
    {
        base.KillModifier(bullet, enemy);
        Generate(enemy.transform);
    }

    [SerializeField]
    public List<GameObject> treePrefab = null;

    private void Generate(Transform pos)
    {
        var tree = Instantiate(treePrefab[Random.Range(0, treePrefab.Count - 1)], pos.position, pos.rotation);
        tree.transform.rotation = Quaternion.identity;
    }
}
