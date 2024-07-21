using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
using System;
using Codice.Client.BaseCommands.Differences;
namespace Toolkid.UIGrid {

    public class Stackables : ItemBase, IStackables {

        public StackablesData Data => stackablesData;

        public int Amount => stackablesData.Amount;

        public int Maximum => stackablesData.Maximum;

        public Text NameText => nameText;

        public Text AmountText => amountText;


        [SerializeField] private StackablesData stackablesData;
        [SerializeField] private Text nameText;
        [SerializeField] private Text amountText;

        public event EventHandler<int> Null;
        public event EventHandler<int> Full;

        // Only for testing version.
        public void Initialize(string Identifier) {
            ItemData.Identifier = Identifier;
            ItemData.Name = PlaceablesDatas.Datas[Identifier].Name;
            NameText.text = ItemData.Name;
            AmountText.text = stackablesData.Amount.ToString();
        }

        public void Increase(int amount = 1) {            
            if (Amount + amount > Maximum) {
                stackablesData.Increase(Maximum - Amount);
                int overflow = Amount + amount - Maximum;
                Full?.Invoke(this, overflow);
            }
            else {
                stackablesData.Increase(amount);
            }            
        }

        public void Decrease(int amount = 1) {                        
            if (Amount - amount >= 0) {
                stackablesData.Decrease(amount);
            }
            else {
                int oweAmount = amount - Amount;
                stackablesData.Decrease(Amount);
                Null?.Invoke(this, oweAmount);
            }
        }
    }
}