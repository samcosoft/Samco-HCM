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
namespace LabData
{

    public partial class TestCard : XPObject
    {
        LabVisits fVisit;
        [Association(@"TestCardReferencesLabVisits")]
        public LabVisits Visit
        {
            get { return fVisit; }
            set { SetPropertyValue<LabVisits>("Visit", ref fVisit, value); }
        }
        TestName fTestName;
        [Association(@"TestCardReferencesTestName")]
        public TestName TestName
        {
            get { return fTestName; }
            set { SetPropertyValue<TestName>("TestName", ref fTestName, value); }
        }
        string fResult;
        public string Result
        {
            get { return fResult; }
            set { SetPropertyValue<string>("Result", ref fResult, value); }
        }
    }

}
