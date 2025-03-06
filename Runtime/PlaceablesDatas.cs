using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolkid.UIGrid {
    [CreateAssetMenu(fileName = "Placeables Datas", menuName = "Placeables Datas")]
    public class PlaceablesDatas : ScriptableObject {
        [SerializeField] private SeedData[] datas;
        public static Dictionary<string, SeedData> Datas { get; protected set; }

        public void Initializes() {
            Datas = new Dictionary<string, SeedData>();
            foreach (var data in datas) {
                Datas.Add(data.Identifier, data);
            }
        }
    }
}
