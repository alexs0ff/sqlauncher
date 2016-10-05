// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   SqLiteSerializer.cs
//   * Project: SqLauncher.Web.Controller
//   * Description:
//   * Created at 2012 01 05 18:53
//   * Modified at: 2012  01 10  20:13
// / ******************************************************************************/ 

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

using SqLauncher.Web.Controller.DataModelInterceptions;
using SqLauncher.Web.Model;
using SqLauncher.Web.UI.Model;

namespace SqLauncher.Web.Controller.XmlSerializes
{
    /// <summary>
    ///   Represents the implementation of sqlite serializing purposes.
    /// </summary>
    public class SqLiteXmlSerializer : XmlSerializerBase
    {
        /// <summary>
        ///   The registred versions.
        /// </summary>
        private readonly Dictionary<int, SqLiteDocumentSerializerVersionBase> _versions =
            new Dictionary<int, SqLiteDocumentSerializerVersionBase>();

        public SqLiteXmlSerializer()
        {
            _versions.Add( 1, new SqLiteDocumentSerializerVersion1() );
        }

        /// <summary>
        ///   Desiarilizes document from xml data.
        /// </summary>
        /// <param name = "xml">The saved data.</param>
        /// <param name = "wiring">The new container wiring.</param>
        /// <returns>The database document.</returns>
        public override DatabaseDocument Deserialize( string xml, out ContainerWiring wiring )
        {
            var xmlDocument = XDocument.Parse( xml );
            int number = 1;

            var version = xmlDocument.Root.Attributes().GetAttributeValue( XmlConstants.Version );

            if ( !string.IsNullOrEmpty( version ) ){
                number = int.Parse( version, CultureInfo.InvariantCulture );
            } //if

            if ( !_versions.ContainsKey( number ) ){
                throw new XmlSerializeException( "This version is incompatible" );
            } //if

            var versionSerializer = _versions[number];

            versionSerializer.Wiring = ContainerWiring.SetUp( new SqLiteModelInterception() );
            wiring = versionSerializer.Wiring;
            return versionSerializer.Deserialize( xml );
        }

        /// <summary>
        ///   Serializes document into xml data.
        /// </summary>
        /// <param name = "document">The document to serializing.</param>
        public override string Serialize( DatabaseDocument document )
        {
            var maxVersion =
                ( from sqLiteDocumentSerializerVersionBase in _versions select sqLiteDocumentSerializerVersionBase.Key )
                    .Max();

            var seriailzer = _versions[maxVersion];
            return seriailzer.Serialize( document );
        }

        /// <summary>
        ///   The container wiring.
        /// </summary>
        public ContainerWiring ContainerWiring { get; set; }
    }
}