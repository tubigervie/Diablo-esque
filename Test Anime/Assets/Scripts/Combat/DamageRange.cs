﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    [System.Serializable]
    public struct DamageRange
    {
        public float min, max;

        public DamageRange(float min, float max)
        {
            this.min = min;
            this.max = max;
        }

        public static implicit operator DamageRange(float damage)
        {
            return new DamageRange(damage, damage);
        }

        public static DamageRange operator +(DamageRange a, DamageRange b)
        {
            return new DamageRange(a.min + b.min, a.max + b.max);
        }

        public static DamageRange FromPercentage(float damage, float percentRange)
        {
            var min = Mathf.Clamp(damage - damage * .5f * percentRange, 0, float.PositiveInfinity);
            var max = Mathf.Clamp(damage + damage * .5f * percentRange, 0, float.PositiveInfinity);
            return new DamageRange(min, max);
        }

        public static DamageRange operator *(DamageRange a, float b)
        {
            return new DamageRange(a.min * b, a.max * b);
        }

        public float RandomlyChooseDamage()
        {
            return UnityEngine.Random.Range(min, max);
        }
    }

}
