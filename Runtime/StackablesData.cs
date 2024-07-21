using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Toolkid.UIGrid {
    [Serializable]
    public class StackablesData : IStackables {        
        [SerializeField, FormerlySerializedAs("m_Count")] protected int amount = 0;
        [SerializeField, FormerlySerializedAs("m_Max")] protected int maximum = 0;
        public int Amount { get => amount; set => amount = value; }
        public int Maximum { get => maximum; set => maximum = value; }

        public void Increase(int amount = 1) {
            this.amount += amount;
        }
        public void Decrease(int amount = 1) {
            this.amount -= amount;
        }
    }
    public interface IStackables {
        int Amount { get; }
        int Maximum { get; }

        void Increase(int amount);
        void Decrease(int amount);
    }
}
