using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AI Controller featuring a single behavior type which does not change. 
/// </summary>
public class AIControllerSingle : AIController
{
    protected override void Start()
    {
        base.Start();
        SetBehaviour(behaviours[0].GetType());
    }

}
