// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   SqLiteDocumentSerializerVersionBase.cs
//   * Project: SqLauncher.Web.Controller
//   * Description:
//   * Created at 2012 01 05 19:14
//   * Modified at: 2012  01 05  19:30
// / ******************************************************************************/ 

using SqLauncher.Web.Model;
using SqLauncher.Web.UI.Model;

namespace SqLauncher.Web.Controller.XmlSerializes
{
    /// <summary>
    ///   Represents the base class for all sql schema versions.
    /// </summary>
    public abstract class SqLiteDocumentSerializerVersionBase
    {
        /// <summary>
        ///   The current container wiring.
        /// </summary>
        public ContainerWiring Wiring { get; set; }

        /// <summary>
        ///   The version number.
        ///   Must be initialized in derived class.
        /// </summary>
        public int VersionNumber { get; protected set; }

        /// <summary>
        ///   Desiarilizes document from xml data.
        /// </summary>
        /// <param name = "xml">The saved data.</param>
        /// <returns>The database document.</returns>
        public abstract DatabaseDocument Deserialize( string xml );

        /// <summary>
        ///   Serializes document into xml data.
        /// </summary>
        /// <param name = "document">The document to serializing.</param>
        public abstract string Serialize( DatabaseDocument document );

        /// <summary>
        /// Makes the passed schema to current version state.
        /// </summary>
        /// <param name="xml">The xml which contains serialized previous version.</param>
        /// <returns></returns>
        public abstract string ChangeSchemaFromPreviousVersion(string xml);
    }
}