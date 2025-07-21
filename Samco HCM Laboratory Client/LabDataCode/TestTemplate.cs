using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace LabData
{

    public partial class TestTemplate
    {
        public TestTemplate() : base(Session.DefaultSession) { }
        public TestTemplate(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
