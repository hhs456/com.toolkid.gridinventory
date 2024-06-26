﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Toolkid.UIGrid {
    public class Stackables : MonoBehaviour {
        [SerializeField] int m_ItemID;
        [SerializeField] StackablesData m_Data = new StackablesData();
        [SerializeField] Text m_Text;
        [SerializeField] Text m_Cont;

        public StackablesData Data { get => m_Data; set => m_Data = value; }

        public void Initialize(int itemID) {
            m_ItemID = itemID;
            m_Data.Name = PlaceablesDatas.Datas[m_ItemID].Name;
            m_Text.text = m_Data.Name;
            m_Cont.text = m_Data.Count.ToString();
        }
    }
}