using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace HCMData
{

    public partial class HealthServices
    {
        public HealthServices() : base(Session.DefaultSession) { }
        public HealthServices(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
