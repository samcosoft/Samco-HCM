using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace LabData
{

    public partial class LabInsuranceType
    {
        public LabInsuranceType() : base(Session.DefaultSession) { }
        public LabInsuranceType(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
