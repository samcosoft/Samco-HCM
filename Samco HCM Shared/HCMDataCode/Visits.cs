using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace HCMData
{

    public partial class Visits
    {
        public Visits() : base(Session.DefaultSession) { }
        public Visits(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
