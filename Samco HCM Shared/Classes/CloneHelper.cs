using System;
using System.Collections.Generic;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;

namespace Samco_HCM_Shared.Classes;

public class CloneHelper : IDisposable
{

    private readonly Dictionary<object, object> _clonedObjects;
    private readonly Session? _targetSession;

    public CloneHelper(Session targetSession)
    {
        _clonedObjects = new Dictionary<object, object>();
        _targetSession = targetSession;
    }

    public T? Clone<T>(T? source)
    {
        return Clone(source, false);
    }
    public T? Clone<T>(T source, bool synchronize)
    {
        return (T?)Clone((object?)source, synchronize);
    }
    public object? Clone(object source)
    {
        return Clone(source, false);
    }
    
    public object? Clone(object? source, bool synchronize)
    {
        if (source is null)
        {
            return null;
        }
        var targetClassInfo = _targetSession?.GetClassInfo(source.GetType());
        var target = targetClassInfo!.CreateNewObject(_targetSession);
        _clonedObjects.Add(source, target);

        foreach (XPMemberInfo m in targetClassInfo.PersistentProperties)
            CloneProperty(m, source, target, synchronize);
        foreach (XPMemberInfo m in targetClassInfo.CollectionProperties)
            CloneCollection(m, source, target, synchronize);
        return target;
    }
    private void CloneProperty(XPMemberInfo memberInfo, object source, object target, bool synchronize)
    {
        if (memberInfo is DevExpress.Xpo.Metadata.Helpers.ServiceField || memberInfo.IsKey)
        {
            return;
        }
        object? clonedValue = null;
        if (memberInfo.ReferenceType is not null)
        {
            var value = memberInfo.GetValue(source);
            if (value is not null)
            {
                clonedValue = CloneValue(value, synchronize, false);
            }
        }
        else
        {
            clonedValue = memberInfo.GetValue(source);
        }
        memberInfo.SetValue(target, clonedValue);
    }
    private void CloneCollection(XPMemberInfo memberInfo, object source, object target, bool synchronize)
    {
        if (memberInfo.IsAssociation && (memberInfo.IsManyToMany || memberInfo.IsAggregated))
        {
            var colTarget = (XPBaseCollection)memberInfo.GetValue(target);
            var colSource = (XPBaseCollection)memberInfo.GetValue(source);
            foreach (IXPSimpleObject obj in colSource)
                colTarget.BaseAdd(CloneValue(obj, synchronize, !memberInfo.IsManyToMany));
        }
    }
    private object? CloneValue(object propertyValue, bool synchronize, bool cloneAlways)
    {
        if (_clonedObjects.TryGetValue(propertyValue, out var cloneValue))
        {
            return cloneValue;
        }
        object? clonedValue = null;
        if (synchronize && !cloneAlways)
        {
            clonedValue = _targetSession?.GetObjectByKey(_targetSession.GetClassInfo(propertyValue), _targetSession.GetKeyValue(propertyValue));
        }
        clonedValue ??= Clone(propertyValue, synchronize);
        return clonedValue;
    }

    public void Dispose()
    {
        _targetSession?.Dispose();
    }
}