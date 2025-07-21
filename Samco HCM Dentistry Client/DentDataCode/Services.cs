using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace DentData
{

    public partial class Services
    {
        public Services() : base(Session.DefaultSession) { }
        public Services(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
