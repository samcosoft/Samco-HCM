﻿using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace DentData
{

    public partial class ServiceList
    {
        public ServiceList() : base(Session.DefaultSession) { }
        public ServiceList(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
