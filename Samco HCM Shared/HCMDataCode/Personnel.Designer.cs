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

    public partial class Personnel : XPObject
    {
        string fName;
        public string Name
        {
            get { return fName; }
            set { SetPropertyValue<string>("Name", ref fName, value); }
        }
        string fPhone;
        public string Phone
        {
            get { return fPhone; }
            set { SetPropertyValue<string>("Phone", ref fPhone, value); }
        }
        string fRole;
        public string Role
        {
            get { return fRole; }
            set { SetPropertyValue<string>("Role", ref fRole, value); }
        }
        [Association(@"VisitsReferencesPersonnel"), Aggregated]
        public XPCollection<Visits> VisitsCollection { get { return GetCollection<Visits>("VisitsCollection"); } }
    }

}
