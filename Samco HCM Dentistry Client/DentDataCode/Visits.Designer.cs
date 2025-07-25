﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace DentData
{

    public partial class Visits : XPObject
    {
        DateTime fvisitDate;
        public DateTime visitDate
        {
            get { return fvisitDate; }
            set { SetPropertyValue<DateTime>(nameof(visitDate), ref fvisitDate, value); }
        }
        PatientInfo fpatient;
        [Association(@"VisitsReferencesPatientInfo")]
        public PatientInfo patient
        {
            get { return fpatient; }
            set { SetPropertyValue<PatientInfo>(nameof(patient), ref fpatient, value); }
        }
        InsuranceTypes fInsurance;
        [Association(@"VisitsReferencesInsuranceTypes")]
        public InsuranceTypes Insurance
        {
            get { return fInsurance; }
            set { SetPropertyValue<InsuranceTypes>(nameof(Insurance), ref fInsurance, value); }
        }
        int fprice;
        public int price
        {
            get { return fprice; }
            set { SetPropertyValue<int>(nameof(price), ref fprice, value); }
        }
        bool fIsTargetPopulation;
        public bool IsTargetPopulation
        {
            get { return fIsTargetPopulation; }
            set { SetPropertyValue<bool>(nameof(IsTargetPopulation), ref fIsTargetPopulation, value); }
        }
        bool fIsExtraVisit;
        public bool IsExtraVisit
        {
            get { return fIsExtraVisit; }
            set { SetPropertyValue<bool>(nameof(IsExtraVisit), ref fIsExtraVisit, value); }
        }
        string fComment;
        [Size(SizeAttribute.Unlimited)]
        public string Comment
        {
            get { return fComment; }
            set { SetPropertyValue<string>(nameof(Comment), ref fComment, value); }
        }
        Users fUser;
        [Association(@"VisitsReferencesDentUsers")]
        public Users User
        {
            get { return fUser; }
            set { SetPropertyValue<Users>(nameof(User), ref fUser, value); }
        }
        bool fCreated;
        public bool Created
        {
            get { return fCreated; }
            set { SetPropertyValue<bool>(nameof(Created), ref fCreated, value); }
        }
        bool fPaid;
        public bool Paid
        {
            get { return fPaid; }
            set { SetPropertyValue<bool>(nameof(Paid), ref fPaid, value); }
        }
        bool fIsReservation;
        public bool IsReservation
        {
            get { return fIsReservation; }
            set { SetPropertyValue<bool>(nameof(IsReservation), ref fIsReservation, value); }
        }
        [Association(@"ServiceListReferencesVisits"), Aggregated]
        public XPCollection<ServiceList> ServiceLists { get { return GetCollection<ServiceList>(nameof(ServiceLists)); } }
    }

}
