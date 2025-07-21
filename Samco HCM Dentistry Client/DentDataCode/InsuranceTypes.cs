using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace DentData
{

    public partial class InsuranceTypes
    {
        public InsuranceTypes() : base(Session.DefaultSession) { }
        public InsuranceTypes(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
