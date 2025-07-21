using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace LabData
{

    public partial class LabVisits
    {
        public LabVisits() : base(Session.DefaultSession) { }
        public LabVisits(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
