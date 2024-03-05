using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolkid.GridInventory {
    [CreateAssetMenu(fileName = "Placeables Datas", menuName = "Placeables Datas")]
    public class PlaceablesDatas : ScriptableObject {
        [SerializeField] private PlaceablesData[] datas;
        public Dictionary<int, PlaceablesData> Datas = new Dictionary<int, PlaceablesData>();

        public void Initialize() {
            Datas = new Dictionary<int, PlaceablesData>();
            foreach (var data in datas) {
                Datas.Add(data.ID, data);
            }
        }
    }
}
