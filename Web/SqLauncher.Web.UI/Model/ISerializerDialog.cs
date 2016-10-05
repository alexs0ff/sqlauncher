// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   ISerializerDialog.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2012 01 08 12:29
//   * Modified at: 2012  01 08  12:56
// / ******************************************************************************/ 

using System;

namespace SqLauncher.Web.UI.Model
{
    /// <summary>
    ///   The interface for serializing purposes.
    /// </summary>
    public interface ISerializerDialog
    {
        /// <summary>
        ///   Starts the serializing process.
        /// </summary>
        void ChoiceFileToSave();

        /// <summary>
        ///   Occurs when user pick a file to save data..
        /// </summary>
        event EventHandler<SerializingEventArgs> Serializing;

        /// <summary>
        ///   Starts the deserializing process.
        /// </summary>
        void ChoiceFileToLoad();

        /// <summary>
        ///   Occurs when user pick a file to load data.
        /// </summary>
        event EventHandler<DeserializingEventArgs> Deserializing;
    }
}