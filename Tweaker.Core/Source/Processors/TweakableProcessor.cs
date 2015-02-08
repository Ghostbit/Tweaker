﻿using Ghostbit.Tweaker.AssemblyScanner;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Ghostbit.Tweaker.Core
{
    /// <summary>
    /// This is an IScanner processor that processes types or members annotated with Tweakable
    /// and produces an ITweakable as a result.
    /// </summary>
    /// <remarks>
    /// Tweaker does not directly enforce registered names to have a path or group separator
    /// but if a Type is annotated with Tweakable a period will be used to separate the provided
    /// group name with the name of the tweakable members.
    /// </remarks>
    public class TweakableProcessor : IAttributeScanProcessor<Tweakable, ITweakable>
    {
        public void ProcessAttribute(Tweakable input, Type type, object instance = null)
        {
            foreach (MemberInfo memberInfo in type.GetMembers(ReflectionUtil.GetBindingFlags(instance)))
            {
                if (memberInfo.MemberType == MemberTypes.Property ||
                    memberInfo.MemberType == MemberTypes.Field)
                {
                    if (memberInfo.GetCustomAttributes(typeof(Tweakable), false).Length == 0)
                    {
                        Tweakable inner = new Tweakable(input.Name + "." + memberInfo.Name);
                        ProcessAttribute(inner, memberInfo, instance);
                    }
                }
            }
        }

        public void ProcessAttribute(Tweakable input, MemberInfo memberInfo, object instance = null)
        {
            ITweakable tweakable = null;
            if (memberInfo.MemberType == MemberTypes.Property)
            {
                var propertyInfo = (PropertyInfo)memberInfo;
                tweakable = TweakableFactory.MakeTweakable(input, propertyInfo, instance);
            }
            else if (memberInfo.MemberType == MemberTypes.Field)
            {
                var fieldInfo = (FieldInfo)memberInfo;
                tweakable = TweakableFactory.MakeTweakable(input, fieldInfo, instance);
            }
            else
            {
                throw new ProcessorException("TweakableProcessor cannot process non MethodInfo or EventInfo types");
            }

            if(tweakable != null)
            {
                ProvideResult(tweakable);
            }
        }

        public event EventHandler<ScanResultArgs<ITweakable>> ResultProvided;

        private void ProvideResult(ITweakable tweakable)
        {
            if (ResultProvided != null)
                ResultProvided(this, new ScanResultArgs<ITweakable>(tweakable));
        }
    }
}
