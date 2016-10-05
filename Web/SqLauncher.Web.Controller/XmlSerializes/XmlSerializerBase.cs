// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   XmlSerializerBase.cs
//   * Project: SqLauncher.Web.Controller
//   * Description:
//   * Created at 2012 01 05 18:43
//   * Modified at: 2012  01 05  18:50
// / ******************************************************************************/ 

using SqLauncher.Web.Model;
using SqLauncher.Web.UI.Model;

namespace SqLauncher.Web.Controller.XmlSerializes
{
    /// <summary>
    ///   The base class for xml serialization purposes.
    /// </summary>
    public abstract class XmlSerializerBase : IDocumentXmlSerializer
    {
        /// <summary>
        ///   The current container wiring.
        /// </summary>
        public ContainerWiring Wiring { get; set; }

        /// <summary>
        ///   Desiarilizes document from xml data.
        /// </summary>
        /// <param name = "xml">The saved data.</param>
        /// <param name="wiring">The new container wiring.</param>
        /// <returns>The database document.</returns>
        public abstract DatabaseDocument Deserialize(string xml, out ContainerWiring wiring);

        /// <summary>
        ///   Serializes document into xml data.
        /// </summary>
        /// <param name = "document">The document to serializing.</param>
        public abstract string Serialize( DatabaseDocument document );
    }
}