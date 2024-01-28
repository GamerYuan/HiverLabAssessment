using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollectable
{
    void Init(float difficultyModifier);
    void DoAction(Component sender);
}
