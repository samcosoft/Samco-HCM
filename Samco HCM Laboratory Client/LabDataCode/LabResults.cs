using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace LabData
{

    public partial class LabResults
    {
        public LabResults() : base(Session.DefaultSession) { }
        public LabResults(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
