// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   IDocumentXmlSerializer.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 12 28 22:11
//   * Modified at: 2011  12 28  22:11
// / ******************************************************************************/ 

using SqLauncher.Web.Model;

namespace SqLauncher.Web.UI.Model
{
    /// <summary>
    ///   The interface for serialization purposes.
    /// </summary>
    public interface IDocumentXmlSerializer
    {
        /// <summary>
        ///   The current container wiring.
        /// </summary>
        ContainerWiring Wiring { get; set; }

        /// <summary>
        ///  Serializes document into xml data.
        /// </summary>
        /// <param name = "document">The document to serializing..</param>
        string Serialize( DatabaseDocument document );

        /// <summary>
        /// Desiarilizes document from xml data.
        /// </summary>
        /// <param name="xml">The xml data.</param>
        /// <param name="containerWiring">The new container wiring.</param>
        /// <returns>The database document.</returns>
        DatabaseDocument Deserialize( string xml, out ContainerWiring containerWiring );
    }
}