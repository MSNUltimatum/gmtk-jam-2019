using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NextMonsterSkill", menuName = "ScriptableObject/ActiveSkill/NextMonsterSkill", order = 1)]
public class NextMonsterSkill : ActiveSkill
{
    [SerializeField]
    private GameObject outlinePrefab = null;

    protected NextMonsterSkill()
    {
        description = "Yoo could show a enemy";
        cooldownDuration = 7f;
        activeDuration = 3f;
    }

    public override void ActivateSkill()
    {
        currentBoy = Labirint.instance.GetComponent<CurrentEnemySelector>().currentBoy;
        if (currentBoy)
        {
            var animator = currentBoy.GetComponentInChildren<Animator>();
            outlineGameObj = Instantiate(outlinePrefab, animator.transform);
            //outlineGameObj.transform.rotation = currentBoy.transform.rotation;
            AnimatorStateInfo animationState = animator.GetCurrentAnimatorStateInfo(0);
            outlineGameObj.GetComponent<Animator>().runtimeAnimatorController = animator.runtimeAnimatorController;
            outlineGameObj.GetComponent<Animator>().Play(animationState.fullPathHash, -1, animationState.normalizedTime);
        }
    }

    public override void EndOfSkill()
    {
        Destroy(outlineGameObj); 
    }



    private GameObject currentBoy;
    private GameObject outlineGameObj;
}
