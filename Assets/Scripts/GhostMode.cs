using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMode : ActiveSkill
{
    private CharacterMovement character = null;

        public GhostMode()
        {
            Name = "GhostMode";
            Description = "Yoo could walk";
            CoolDown = 5f;
           // isInstantSkill = true;   //This skill is instant or not
            ActionTime = 3f;            
        //If it's not instant,than after Action Time the EndOfSkill function will be called
        }

    public override void ActiveResult()
    {
        /*GameObject Player = GameObject.FindGameObjectWithTag("Player");
        character = Player.GetComponent<CharacterMovement>();
        character.speed *= 2;*/
        Debug.Log("Ghost");
    }

    public override void EndOfSkill()
    {
        character.speed /= 2;
    }
}
