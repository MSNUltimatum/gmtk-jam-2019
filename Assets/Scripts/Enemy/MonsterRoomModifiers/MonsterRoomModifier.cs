using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterRoomModifier : ScriptableObject
{
    public virtual void ApplyModifier(MonsterLife monster) { }
}
