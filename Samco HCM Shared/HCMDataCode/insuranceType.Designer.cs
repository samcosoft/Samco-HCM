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

    public partial class insuranceType : XPObject
    {
        string fname;
        public string name
        {
            get { return fname; }
            set { SetPropertyValue<string>("name", ref fname, value); }
        }
        byte[] fimage;
        [MemberDesignTimeVisibility(true)]
        public byte[] image
        {
            get { return fimage; }
            set { SetPropertyValue<byte[]>("image", ref fimage, value); }
        }
        [Association(@"VisitsReferencesinsuranceType")]
        public XPCollection<Visits> VisitsCollection { get { return GetCollection<Visits>("VisitsCollection"); } }
        [Association(@"PatientInfoReferencesinsuranceType")]
        public XPCollection<PatientInfo> PatientInfoes { get { return GetCollection<PatientInfo>("PatientInfoes"); } }
        [Association(@"PricesReferencesinsuranceType"), Aggregated]
        public XPCollection<Prices> PricesCollection { get { return GetCollection<Prices>("PricesCollection"); } }
    }

}
