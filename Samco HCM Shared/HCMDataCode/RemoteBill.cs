using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace HCMData
{

    public partial class RemoteBill
    {
        public RemoteBill() : base(Session.DefaultSession) { }
        public RemoteBill(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
