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

    public partial class EquipmentList : XPObject
    {
        Equipments fEquipName;
        [Association(@"EquipmentListReferencesEquipments")]
        public Equipments EquipName
        {
            get { return fEquipName; }
            set { SetPropertyValue<Equipments>("EquipName", ref fEquipName, value); }
        }
        int fCount;
        public int Count
        {
            get { return fCount; }
            set { SetPropertyValue<int>("Count", ref fCount, value); }
        }
        Visits fVisit;
        [Association(@"EquipmentListReferencesVisits")]
        public Visits Visit
        {
            get { return fVisit; }
            set { SetPropertyValue<Visits>("Visit", ref fVisit, value); }
        }
    }

}
