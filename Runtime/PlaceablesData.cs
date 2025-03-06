using System;
using System.Collections;
using System.Collections.Generic;
using Toolkid.UIGrid;
using UnityEngine;
using UnityEngine.Serialization;
namespace Toolkid.UIGrid {
    [Serializable]
    public class PlaceablesData : ItemData, IPlaceables {

        public bool[] Sharp { get => sharp; set => sharp = value; }
        
        protected const int length = 5;
        [SerializeField, FormerlySerializedAs("enabled")]
        protected bool[] sharp = new bool[length * length];        

        public PlaceablesData() {
            sharp = new bool[length * length];
        }
    }
    public interface IPlaceables {
        bool[] Sharp { get; }
    }

    [Serializable]
    public class SeedData : PlaceablesData {        
        public TimeSpan RipenTime => TimeSpan.FromSeconds(ripenTime);
        
        [SerializeField]
        protected float ripenTime;

        public SeedData() : base() {
            sharp = new bool[length * length];
        }
    }
}