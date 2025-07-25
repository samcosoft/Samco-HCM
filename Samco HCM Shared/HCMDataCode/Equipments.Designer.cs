﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace HCMData
{

    public partial class Equipments : XPObject
    {
        string fName;
        public string Name
        {
            get { return fName; }
            set { SetPropertyValue<string>("Name", ref fName, value); }
        }
        int fPrice;
        public int Price
        {
            get { return fPrice; }
            set { SetPropertyValue<int>("Price", ref fPrice, value); }
        }
        [Association(@"DefEquipReferencesEquipments"), Aggregated]
        public XPCollection<DefEquip> DefEquips { get { return GetCollection<DefEquip>("DefEquips"); } }
        [Association(@"EquipmentListReferencesEquipments"), Aggregated]
        public XPCollection<EquipmentList> EquipmentLists { get { return GetCollection<EquipmentList>("EquipmentLists"); } }
    }

}
