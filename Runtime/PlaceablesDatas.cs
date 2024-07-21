using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolkid.UIGrid {
    [CreateAssetMenu(fileName = "Placeables Datas", menuName = "Placeables Datas")]
    public class PlaceablesDatas : ScriptableObject {
        [SerializeField] private PlaceablesData[] datas;
        public static Dictionary<string, PlaceablesData> Datas = new Dictionary<string, PlaceablesData>();

        public void Initializes() {
            Datas = new Dictionary<string, PlaceablesData>();
            foreach (var data in datas) {
                Datas.Add(data.Identifier, data);
            }
        }
    }
}
