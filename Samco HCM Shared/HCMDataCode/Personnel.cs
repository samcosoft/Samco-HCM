using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace HCMData
{

    public partial class Personnel
    {
        public Personnel() : base(Session.DefaultSession) { }
        public Personnel(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
