// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   UnityHelper.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2011 08 21 12:28
//   * Modified at: 2011  11 16  20:57
// / ******************************************************************************/ 

using System;
using System.Reflection;

namespace SqLauncher.Web.Model.Interception
{
    public static class UnityHelper
    {
        public static bool HasAttribute<T>( this PropertyInfo property )
        {
            return HasAttribute( property, typeof ( T ) );
        }

        public static bool HasAttribute( this PropertyInfo property, Type attributeType )
        {
            return property.GetCustomAttributes( attributeType, true ).Length > 0;
        }
    }
}