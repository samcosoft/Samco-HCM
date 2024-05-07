using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace HCMData
{

    public partial class EquipmentList
    {
        public EquipmentList() : base(Session.DefaultSession) { }
        public EquipmentList(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
