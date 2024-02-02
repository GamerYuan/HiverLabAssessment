using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    public void Damage(float damage);
    public void Heal(float healAmount);
}
