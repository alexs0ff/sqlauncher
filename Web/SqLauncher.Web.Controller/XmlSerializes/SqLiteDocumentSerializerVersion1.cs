// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   SqLiteDocumentSerializerVersion1.cs
//   * Project: SqLauncher.Web.Controller
//   * Description:
//   * Created at 2012 01 05 19:13
//   * Modified at: 2012  01 05  19:38
// / ******************************************************************************/ 

using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Xml;
using System.Xml.Linq;

using SqLauncher.Web.Model;
using SqLauncher.Web.Model.SqLite;
using SqLauncher.Web.UI;
using SqLauncher.Web.UI.Model;

namespace SqLauncher.Web.Controller.XmlSerializes
{
    /// <summary>
    ///   Represents the first sqlite version.
    /// </summary>
    public class SqLiteDocumentSerializerVersion1 : SqLiteDocumentSerializerVersionBase
    {
        public SqLiteDocumentSerializerVersion1()
        {
            VersionNumber = 1;
        }

        /// <summary>
        ///   Makes the passed schema to current version state.
        /// </summary>
        /// <param name = "xml">The xml which contains serialized previous version.</param>
        /// <returns></returns>
        public override string ChangeSchemaFromPreviousVersion( string xml )
        {
            return xml;
        }

        /// <summary>
        ///   Serializes document into xml data.
        /// </summary>
        /// <param name = "document">The document to serializing..</param>
        public override string Serialize(DatabaseDocument document)
        {
            var result = new StringBuilder();

            var settings = new XmlWriterSettings{Indent = true};
            using (var xmlWriter = XmlWriter.Create(result, settings))
            {
                xmlWriter.WriteStartDocument();
                ProcessSaveDocument(document, xmlWriter);

                xmlWriter.WriteEndDocument();
            }

            return result.ToString();
        }

        #region Serialize

        /// <summary>
        ///   Processes saving the database document to xml.
        /// </summary>
        /// <param name = "document">The document.</param>
        /// <param name = "xmlWriter">The xml writer.</param>
        private void ProcessSaveDocument(DatabaseDocument document, XmlWriter xmlWriter)
        {
            xmlWriter.WriteStartElement(XmlConstants.DatabaseDocument);
            WriteInnerIdAttribute(xmlWriter, document.InnerId);

            xmlWriter.WriteAttributeString(XmlConstants.Name, document.Name);
            xmlWriter.WriteAttributeString(XmlConstants.DatabaseType, XmlConstants.SqLite);
            xmlWriter.WriteAttributeString( XmlConstants.Version, VersionNumber.ToString( CultureInfo.InvariantCulture ) );

            ProcessSaveIteractionState(document.IteractionState ,xmlWriter );

            xmlWriter.WriteStartElement(XmlConstants.Versions);

            foreach (var databaseVersion in document.Versions)
            {
                ProcessSaveVersion(databaseVersion, xmlWriter);
            } //foreach

            xmlWriter.WriteEndElement(); //Versions
            xmlWriter.WriteEndElement(); //DatabaseDocument
        }

        /// <summary>
        ///   Processes saving the iteraction state to xml.
        /// </summary>
        /// <param name = "iteractionState">The iteraction state.</param>
        /// <param name = "xmlWriter">The xml writer.</param>
        private void ProcessSaveIteractionState( IIteractionState iteractionState, XmlWriter xmlWriter )
        {
            xmlWriter.WriteStartElement( XmlConstants.IteractionState );
            xmlWriter.WriteAttributeString( XmlConstants.PhysicalView, iteractionState.PhysicalView.ToString() );
            xmlWriter.WriteEndElement();//XmlConstants.IteractionState
        }

        /// <summary>
        ///   Processes saving the document document to xml.
        /// </summary>
        /// <param name = "databaseVersion">The database version</param>
        /// <param name = "xmlWriter">The xml writer.</param>
        private void ProcessSaveVersion(DatabaseVersion databaseVersion, XmlWriter xmlWriter)
        {
            xmlWriter.WriteStartElement(XmlConstants.Version);
            WriteInnerIdAttribute(xmlWriter, databaseVersion.InnerId);
            xmlWriter.WriteAttributeString(XmlConstants.Name, databaseVersion.Name);
            xmlWriter.WriteAttributeString(XmlConstants.Locked, databaseVersion.Locked.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteAttributeString(XmlConstants.CreateDate,
                                            databaseVersion.CreateDate.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteAttributeString(XmlConstants.Number,
                                            databaseVersion.Number.ToString(CultureInfo.InvariantCulture));
            ProcessSaveModelViewState((ModelViewState)databaseVersion.ModelViewState, xmlWriter);

            xmlWriter.WriteEndElement(); //XmlConstants.Version
        }

        /// <summary>
        ///   Processes saving the model view state to xml.
        /// </summary>
        /// <param name = "modelViewState">The model view state</param>
        /// <param name = "xmlWriter">The xml writer.</param>
        private void ProcessSaveModelViewState(ModelViewState modelViewState, XmlWriter xmlWriter)
        {
            xmlWriter.WriteStartElement(XmlConstants.ModelViewState);

            WriteInnerIdAttribute(xmlWriter, modelViewState.InnerId);

            xmlWriter.WriteAttributeString(XmlConstants.Height,
                                          modelViewState.Height.ToString(CultureInfo.InvariantCulture));

            xmlWriter.WriteAttributeString(XmlConstants.Width,
                                          modelViewState.Width.ToString(CultureInfo.InvariantCulture));

            xmlWriter.WriteStartElement(XmlConstants.EntityRelationStates);

            foreach (var entityRelationState in modelViewState.EntityRelationStates)
            {
                ProcessSaveRelationViewState((RelationViewState)entityRelationState, xmlWriter);
            } //foreach

            xmlWriter.WriteEndElement(); //XmlConstants.EntityRelationStates


            xmlWriter.WriteStartElement(XmlConstants.EntityViewStates);

            foreach (var entityViewState in modelViewState.EntityViewStates)
            {
                ProcessSaveEntityViewState((EntityViewState)entityViewState, xmlWriter);
            } //foreach

            xmlWriter.WriteEndElement(); //XmlConstants.EntityViewStates

            ProcessSaveModel(modelViewState.DataModel, xmlWriter);

            xmlWriter.WriteEndElement(); //XmlConstants.ModelViewState
        }

        /// <summary>
        ///   Processes saving the entity view state to xml.
        /// </summary>
        /// <param name = "dataModel">The data model.</param>
        /// <param name = "xmlWriter">The xml writer.</param>
        private void ProcessSaveModel(DataModel dataModel, XmlWriter xmlWriter)
        {
            xmlWriter.WriteStartElement(XmlConstants.DataModel);

            WriteInnerIdAttribute(xmlWriter, dataModel.InnerId);
            WriteCaption(xmlWriter, dataModel.Caption);

            xmlWriter.WriteStartElement(XmlConstants.Entities);

            foreach (var entity in dataModel.Entities)
            {
                ProcessSaveEntity(entity, xmlWriter);
            } //foreach

            xmlWriter.WriteEndElement(); //XmlConstants.Entities

            xmlWriter.WriteStartElement(XmlConstants.Relations);

            foreach (var relation in dataModel.Relations)
            {
                ProcessSaveRelation(relation, xmlWriter);
            } //foreach

            xmlWriter.WriteEndElement(); //XmlConstants.Relations

            xmlWriter.WriteEndElement(); //XmlConstants.DataModel
        }

        /// <summary>
        ///   Processes saving the relation to xml.
        /// </summary>
        /// <param name = "relation">The relation.</param>
        /// <param name = "xmlWriter">The xml writer.</param>
        private void ProcessSaveRelation(EntityRelation relation, XmlWriter xmlWriter)
        {
            xmlWriter.WriteStartElement(XmlConstants.Relation);
            WriteInnerIdAttribute(xmlWriter, relation.InnerId);
            xmlWriter.WriteAttributeString(XmlConstants.RelationType, relation.Type.ToString());
            WriteCaption(xmlWriter, relation.Caption);

            WriteEntityReference(xmlWriter, relation.Parent, XmlConstants.Parent);
            WriteEntityReference(xmlWriter, relation.Child, XmlConstants.Child);

            WriteAttributesReference(xmlWriter, relation.ParentAttributes, XmlConstants.ParentAttributes);
            WriteAttributesReference(xmlWriter, relation.ChildAttributes, XmlConstants.ChildAttributes);

            ProceesSaveCardinality( relation.Cardinality, xmlWriter );

            xmlWriter.WriteEndElement(); //XmlConstants.Relation
        }

        /// <summary>
        /// Processes saving the cardinality.
        /// </summary>
        /// <param name="cardinality">The cardinality object.</param>
        /// <param name="xmlWriter">The xml writer.</param>
        private void ProceesSaveCardinality( Cardinality cardinality, XmlWriter xmlWriter )
        {
            xmlWriter.WriteStartElement( XmlConstants.Cardinality );
            WriteInnerIdAttribute( xmlWriter, cardinality.InnerId );
            xmlWriter.WriteAttributeString( XmlConstants.ParentFrom, cardinality.ParentFrom.ToString( CultureInfo.InvariantCulture ) );
            xmlWriter.WriteAttributeString( XmlConstants.ChildFrom, cardinality.ChildFrom.ToString( CultureInfo.InvariantCulture ) );
            xmlWriter.WriteAttributeString( XmlConstants.ChildTo, cardinality.ChildTo );
            
            xmlWriter.WriteEndElement();//XmlConstants.Cardinality
        }

        /// <summary>
        ///   Writes the references of the attributes.
        /// </summary>
        /// <param name = "xmlWriter">The xml writer.</param>
        /// <param name = "attributes">The attributes collection.</param>
        /// <param name = "name">The name.</param>
        private void WriteAttributesReference(XmlWriter xmlWriter, IEnumerable<EntityAttribute> attributes, string name)
        {
            xmlWriter.WriteStartElement( name );

            foreach (var entityAttribute in attributes)
            {
                xmlWriter.WriteStartElement(XmlConstants.Attribute);
                WriteInnerIdAttribute(xmlWriter, entityAttribute.InnerId);
                xmlWriter.WriteEndElement(); //XmlConstants.Attribute
            } //foreach

            xmlWriter.WriteEndElement(); //name
        }

        /// <summary>
        ///   Writes the reference of erd entity.
        ///   Processed the null entities.
        /// </summary>
        /// <param name = "xmlWriter">The xml writer.</param>
        /// <param name = "entity">The erd entity.</param>
        /// <param name = "name">The name.</param>
        private void WriteEntityReference(XmlWriter xmlWriter, ERDEntity entity, string name)
        {
            if (entity != null)
            {
                xmlWriter.WriteStartElement(name);
                WriteInnerIdAttribute(xmlWriter, entity.InnerId);
                xmlWriter.WriteEndElement(); //name
            } //if
        }

        /// <summary>
        ///   Processes saving the entity to xml.
        /// </summary>
        /// <param name = "entity">The entity.</param>
        /// <param name = "xmlWriter">The xml writer.</param>
        private void ProcessSaveEntity(ERDEntity entity, XmlWriter xmlWriter)
        {
            xmlWriter.WriteStartElement(XmlConstants.Entity);
            WriteInnerIdAttribute(xmlWriter, entity.InnerId);
            xmlWriter.WriteAttributeString(XmlConstants.Generated,
                                            entity.Generated.ToString(CultureInfo.InvariantCulture));

            WriteCaption(xmlWriter, entity.Caption);

            xmlWriter.WriteStartElement(XmlConstants.Relations);

            foreach (var entityRelation in entity.Relations)
            {
                xmlWriter.WriteStartElement(XmlConstants.Relation);
                WriteInnerIdAttribute(xmlWriter, entityRelation.InnerId);
                xmlWriter.WriteEndElement(); //XmlConstants.Relation
            } //foreach

            xmlWriter.WriteEndElement(); //XmlConstants.Relations

            xmlWriter.WriteStartElement(XmlConstants.Attributes);

            foreach (var entityAttribute in entity.Attributes)
            {
                ProcessSaveAttribute(entityAttribute, xmlWriter);
            } //foreach

            xmlWriter.WriteEndElement(); //XmlConstants.Attributes
            xmlWriter.WriteStartElement( XmlConstants.Notes );
            xmlWriter.WriteCData( entity.Notes );
            xmlWriter.WriteEndElement();//XmlConstants.Notes

            xmlWriter.WriteStartElement(XmlConstants.Indexes);

            foreach ( var entityIndex in entity.Indexes ){
                ProcessSaveIndex(entityIndex, xmlWriter);
            } //foreach

            xmlWriter.WriteEndElement();//XmlConstants.Indexes

            xmlWriter.WriteEndElement(); //XmlConstants.Entity
        }

        /// <summary>
        ///   Processes saving the entity index to xml.
        /// </summary>
        /// <param name = "entityIndex">The entity index.</param>
        /// <param name = "xmlWriter">The xml writer.</param>
        private void ProcessSaveIndex(EntityIndex entityIndex, XmlWriter xmlWriter)
        {
            xmlWriter.WriteStartElement(XmlConstants.EntityIndex);
            WriteInnerIdAttribute(xmlWriter, entityIndex.InnerId);
            xmlWriter.WriteAttributeString(XmlConstants.IsUnique, entityIndex.IsUnique.ToString(CultureInfo.InvariantCulture));
            WriteCaption(xmlWriter, entityIndex.Caption);

            xmlWriter.WriteStartElement(XmlConstants.IndexAttributes);

            foreach ( var indexAttribute in entityIndex.Attributes ){
                ProcessSaveIndexAttribute( indexAttribute, xmlWriter );
            } //foreach

            xmlWriter.WriteEndElement();//XmlConstants.IndexAttributes

            xmlWriter.WriteEndElement(); //XmlConstants.EntityIndex
        }

        /// <summary>
        ///   Processes saving the index attribute to xml.
        /// </summary>
        /// <param name = "indexAttribute">The index attribute.</param>
        /// <param name = "xmlWriter">The xml writer.</param>
        private void ProcessSaveIndexAttribute( IndexAttribute indexAttribute, XmlWriter xmlWriter )
        {
            xmlWriter.WriteStartElement(XmlConstants.IndexAttribute);

            WriteInnerIdAttribute( xmlWriter, indexAttribute.InnerId );
            xmlWriter.WriteAttributeString( XmlConstants.SortOrder, indexAttribute.Order.ToString() );
            xmlWriter.WriteAttributeString( XmlConstants.AttributeId, indexAttribute.Attribute.InnerId.ToString() );

            xmlWriter.WriteEndElement();//XmlConstants.IndexAttribute
        }

        /// <summary>
        ///   Processes saving the entity attribute to xml.
        /// </summary>
        /// <param name = "entityAttribute">The entity attribute.</param>
        /// <param name = "xmlWriter">The xml writer.</param>
        private void ProcessSaveAttribute(EntityAttribute entityAttribute, XmlWriter xmlWriter)
        {
            if (entityAttribute.Key != AttributeKeyType.IsForeignKey && entityAttribute.Key != AttributeKeyType.IsPrimaryForeignKey)
            {
                xmlWriter.WriteStartElement(XmlConstants.Attribute);
                WriteInnerIdAttribute(xmlWriter, entityAttribute.InnerId);

                xmlWriter.WriteAttributeString(XmlConstants.DataLenght,
                                              entityAttribute.DataLenght.ToString(CultureInfo.InvariantCulture));
                xmlWriter.WriteAttributeString(XmlConstants.IsIdentity,
                                              entityAttribute.IsIdentity.ToString(CultureInfo.InvariantCulture));
                xmlWriter.WriteAttributeString(XmlConstants.IsNotNull,
                                              entityAttribute.IsNotNull.ToString(CultureInfo.InvariantCulture));
                xmlWriter.WriteAttributeString(XmlConstants.IsUnique,
                                              entityAttribute.IsUnique.ToString(CultureInfo.InvariantCulture));


                xmlWriter.WriteAttributeString(XmlConstants.Key,
                                              entityAttribute.Key.ToString());
                xmlWriter.WriteAttributeString(XmlConstants.Decimal,
                                              entityAttribute.Decimal.ToString(CultureInfo.InvariantCulture));

                xmlWriter.WriteAttributeString(XmlConstants.Default,
                                              entityAttribute.Default);

                WriteCaption(xmlWriter, entityAttribute.Caption);

                WriteDbType(xmlWriter, entityAttribute.DbType);

                xmlWriter.WriteEndElement(); //XmlConstants.Attribute
            } //if
        }

        /// <summary>
        ///   Writes the db type element.
        /// </summary>
        /// <param name = "xmlWriter">The xml writer</param>
        /// <param name = "dbType">The db type value.</param>
        private void WriteDbType(XmlWriter xmlWriter, SqlTypeBase dbType)
        {
            xmlWriter.WriteStartElement(XmlConstants.SqlType);
            xmlWriter.WriteAttributeString(XmlConstants.Name, dbType.Name);
            xmlWriter.WriteAttributeString(XmlConstants.HasLenght,
                                          dbType.HasLenght.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteAttributeString(XmlConstants.HasDecimal,
                                          dbType.HasDecimal.ToString(CultureInfo.InvariantCulture));

            xmlWriter.WriteEndElement(); //XmlConstants.SqlType
        }

        /// <summary>
        ///   Writes the item name element.
        /// </summary>
        /// <param name = "xmlWriter">The xml writer</param>
        /// <param name = "caption">The item name value.</param>
        private void WriteCaption(XmlWriter xmlWriter, ItemName caption)
        {
            xmlWriter.WriteStartElement(XmlConstants.Caption);
            WriteInnerIdAttribute(xmlWriter, caption.InnerId);
            xmlWriter.WriteAttributeString(XmlConstants.Physical, caption.Physical);
            xmlWriter.WriteAttributeString(XmlConstants.Title, caption.Title);
            xmlWriter.WriteEndElement();
        }

        /// <summary>
        ///   Processes saving the entity view state to xml.
        /// </summary>
        /// <param name = "entityViewState">The entity view state</param>
        /// <param name = "xmlWriter">The xml writer.</param>
        private void ProcessSaveEntityViewState(EntityViewState entityViewState, XmlWriter xmlWriter)
        {
            xmlWriter.WriteStartElement(XmlConstants.EntityViewState);
            WriteInnerIdAttribute(xmlWriter, entityViewState.InnerId);
            xmlWriter.WriteAttributeString(XmlConstants.IsEditing, entityViewState.IsEditing.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteAttributeString(XmlConstants.Width, entityViewState.Width.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteAttributeString(XmlConstants.Height, entityViewState.Height.ToString(CultureInfo.InvariantCulture));

            xmlWriter.WriteStartElement(XmlConstants.Location);
            WritePointElement(xmlWriter, entityViewState.Location);
            xmlWriter.WriteEndElement(); //XmlConstants.Location

            xmlWriter.WriteStartElement(XmlConstants.BackgroundBrush);
            WriteBrush(xmlWriter, entityViewState.BackgroundBrush);
            xmlWriter.WriteEndElement(); //XmlConstants.BackgroundBrush 

            xmlWriter.WriteStartElement(XmlConstants.Entity);
            WriteInnerIdAttribute(xmlWriter, entityViewState.Entity.InnerId);
            xmlWriter.WriteEndElement(); //XmlConstants.Entity 

            xmlWriter.WriteEndElement(); //XmlConstants.EntityViewState 
        }

        /// <summary>
        ///   Processes saving the rect connector to xml.
        /// </summary>
        /// <param name = "rectConnector">The rect connector.</param>
        /// <param name = "xmlWriter">The xml writer.</param>
        private void ProcessSaveRectConnector(RectConnector rectConnector, XmlWriter xmlWriter)
        {
            xmlWriter.WriteStartElement(XmlConstants.RectConnector);
            xmlWriter.WriteAttributeString(XmlConstants.RectSide, rectConnector.RectSide.ToString());

            xmlWriter.WriteStartElement(XmlConstants.MiddleSidePoint);
            WritePointElement(xmlWriter, rectConnector.MiddleSidePoint);
            xmlWriter.WriteEndElement(); //XmlConstants.MiddleSidePoint

            xmlWriter.WriteEndElement(); //XmlConstants.RectConnector
        }

        /// <summary>
        ///   Processes saving the relation view state to xml.
        /// </summary>
        /// <param name = "entityViewState">The relation view state.</param>
        /// <param name = "xmlWriter">The xml writer.</param>
        private void ProcessSaveRelationViewState(RelationViewState entityViewState, XmlWriter xmlWriter)
        {
            xmlWriter.WriteStartElement(XmlConstants.EntityRelationState);
            WriteInnerIdAttribute(xmlWriter, entityViewState.InnerId);


            xmlWriter.WriteStartElement(XmlConstants.DestinationPoint);
            ProcessSaveRectConnector(entityViewState.DestinationPoint, xmlWriter);
            xmlWriter.WriteEndElement(); //XmlConstants.DestinationPoint

            xmlWriter.WriteStartElement(XmlConstants.StartPoint);
            ProcessSaveRectConnector(entityViewState.StartPoint, xmlWriter);
            xmlWriter.WriteEndElement(); //XmlConstants.StartPoint

            xmlWriter.WriteStartElement(XmlConstants.Relation);
            WriteInnerIdAttribute(xmlWriter, entityViewState.Relation.InnerId);
            xmlWriter.WriteEndElement(); //XmlConstants.Relation

            xmlWriter.WriteEndElement(); //XmlConstants.EntityRelationState
        }

        /// <summary>
        ///   Writes the inner id attribute.
        /// </summary>
        /// <param name = "xmlWriter">The xml w.riter</param>
        /// <param name = "innerId">The inner id value.</param>
        private static void WriteInnerIdAttribute(XmlWriter xmlWriter, Guid innerId)
        {
            xmlWriter.WriteAttributeString(XmlConstants.InnerId, innerId.ToString());
        }

        /// <summary>
        ///   Writes the point attribute.
        /// </summary>
        /// <param name = "xmlWriter">The xml writer</param>
        /// <param name = "point">The inner id value.</param>
        private static void WritePointElement(XmlWriter xmlWriter, Point point)
        {
            xmlWriter.WriteStartElement(XmlConstants.Point);

            xmlWriter.WriteAttributeString(XmlConstants.x, point.X.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteAttributeString(XmlConstants.y, point.Y.ToString(CultureInfo.InvariantCulture));

            xmlWriter.WriteEndElement(); //XmlConstants.Point
        }

        /// <summary>
        ///   Writes information about brush.
        /// </summary>
        /// <param name = "xmlWriter">The xml writer.</param>
        /// <param name = "brush">The brush.</param>
        private static void WriteBrush(XmlWriter xmlWriter, Brush brush)
        {
            xmlWriter.WriteStartElement(XmlConstants.Brush);
            xmlWriter.WriteAttributeString(XmlConstants.Opacity, brush.Opacity.ToString(CultureInfo.InvariantCulture));

            if (brush is SolidColorBrush)
            {
                xmlWriter.WriteAttributeString(XmlConstants.Type, XmlConstants.SolidColorBrush);
                var solidBrush = (SolidColorBrush)brush;
                WriteColor(xmlWriter, solidBrush.Color);
            } //if
            else if (brush is LinearGradientBrush)
            {
                xmlWriter.WriteAttributeString(XmlConstants.Type, XmlConstants.LinearGradientBrush);
                var gradientBrush = (LinearGradientBrush)brush;

                xmlWriter.WriteStartElement(XmlConstants.StartPoint);
                WritePointElement(xmlWriter, gradientBrush.StartPoint);
                xmlWriter.WriteEndElement(); //XmlConstants.StartPoint

                xmlWriter.WriteStartElement(XmlConstants.EndPoint);
                WritePointElement(xmlWriter, gradientBrush.EndPoint);
                xmlWriter.WriteEndElement(); //XmlConstants.EndPoint

                xmlWriter.WriteStartElement(XmlConstants.GradientStops);

                foreach (var gradientStop in gradientBrush.GradientStops)
                {
                    WriteGradientStop(xmlWriter, gradientStop);
                } //foreach

                xmlWriter.WriteEndElement(); //XmlConstants.GradientStops
            } //else

            xmlWriter.WriteEndElement(); //XmlConstants.Brush 
        }

        /// <summary>
        ///   Writes information gradient stop.
        /// </summary>
        /// <param name = "xmlWriter">The xml writer.</param>
        /// <param name = "gradientStop">The gradient stop.</param>
        private static void WriteGradientStop(XmlWriter xmlWriter, GradientStop gradientStop)
        {
            xmlWriter.WriteStartElement(XmlConstants.GradientStop);
            xmlWriter.WriteAttributeString(XmlConstants.Offset,
                                            gradientStop.Offset.ToString(CultureInfo.InvariantCulture));
            WriteColor(xmlWriter, gradientStop.Color);
            xmlWriter.WriteEndElement(); //XmlConstants.GradientStop 
        }

        /// <summary>
        ///   Writes information about color.
        /// </summary>
        /// <param name = "xmlWriter">The xml writer.</param>
        /// <param name = "color">The color.</param>
        private static void WriteColor(XmlWriter xmlWriter, Color color)
        {
            xmlWriter.WriteStartElement(XmlConstants.Color);
            xmlWriter.WriteAttributeString(XmlConstants.A, color.A.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteAttributeString(XmlConstants.R, color.R.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteAttributeString(XmlConstants.G, color.G.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteAttributeString(XmlConstants.B, color.B.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteEndElement(); //XmlConstants.Color
        }

        #endregion Serialize

        /// <summary>
        ///   Desiarilizes document from xml data.
        /// </summary>
        /// <param name = "xml">The saved data.</param>
        /// <returns>The database document.</returns>
        public override DatabaseDocument Deserialize(string xml)
        {
            Wiring.BeginSuspendValueChangeEvent();

            var document = Wiring.CreateInstance<DatabaseDocument>();

            var xmlDocument = XDocument.Parse( xml );

            ProcessLoadDocument( document, xmlDocument.Root );

            Wiring.EndSuspendValueChangeEvent();

            return document;
        }

        #region Deserialize

        /// <summary>
        ///   Processed load data to database document.
        /// </summary>
        /// <param name = "document">The database document.</param>
        /// <param name = "element">The xml document.</param>
        private void ProcessLoadDocument(DatabaseDocument document, XElement element)
        {
            document.Name = element.Attributes().GetAttributeValue(XmlConstants.Name);
            document.SetInnerId(ReadInnerId(element));

            var iteractionStateElement = element.Elements().GetElementByName( XmlConstants.IteractionState );
            ProcessLoadIteractionState( document.IteractionState, iteractionStateElement );

            var versions = element.Elements().GetElementByName(XmlConstants.Versions);

            if (versions != null)
            {
                foreach (var versionElement in versions.Elements())
                {
                    var databaseVersion = Wiring.CreateInstance<DatabaseVersion>();
                    ProcessLoadVersion(databaseVersion, versionElement);
                    document.Versions.Add(databaseVersion);
                } //foreach
            } //if
        }

        /// <summary>
        ///   Processes loading iteraction statefrom x element.
        /// </summary>
        /// <param name = "iteractionState">The iteraction state object.</param>
        /// <param name = "iteractionStateElement">The iteraction state element.</param>
        private void ProcessLoadIteractionState( IIteractionState iteractionState, XElement iteractionStateElement )
        {
            if (StringComparer.OrdinalIgnoreCase.Compare(iteractionStateElement.Name.LocalName, XmlConstants.IteractionState) != 0)
            {
                return;
            } //if
            iteractionState.PhysicalView =
                bool.Parse( iteractionStateElement.Attributes().GetAttributeValue( XmlConstants.PhysicalView ) );
        }

        /// <summary>
        ///   Processes loading database version from x element.
        /// </summary>
        /// <param name = "databaseVersion">The database version.</param>
        /// <param name = "versionElement">The version element.</param>
        private void ProcessLoadVersion(DatabaseVersion databaseVersion, XElement versionElement)
        {
            if (StringComparer.OrdinalIgnoreCase.Compare(versionElement.Name.LocalName, XmlConstants.Version) != 0)
            {
                return;
            } //if

            databaseVersion.SetInnerId(ReadInnerId(versionElement));
            databaseVersion.Name = versionElement.Attributes().GetAttributeValue(XmlConstants.Name);
            databaseVersion.Locked = bool.Parse( versionElement.Attributes().GetAttributeValue(XmlConstants.Locked));
            databaseVersion.CreateDate =
                DateTime.Parse(versionElement.Attributes().GetAttributeValue(XmlConstants.CreateDate),
                                CultureInfo.InvariantCulture);
            databaseVersion.Number = int.Parse(versionElement.Attributes().GetAttributeValue(XmlConstants.Number),
                                                CultureInfo.InvariantCulture);

            var modelViewStateElement = versionElement.Elements().GetElementByName(XmlConstants.ModelViewState);
            var modelViewState = (ModelViewState)databaseVersion.ModelViewState;

            ProcessLoadModelViewState(modelViewState, modelViewStateElement);
        }

        /// <summary>
        ///   Processes loading the model view state.
        /// </summary>
        /// <param name = "modelViewState">The model view state object.</param>
        /// <param name = "modelViewStateElement">The model view state element.</param>
        private void ProcessLoadModelViewState(ModelViewState modelViewState, XElement modelViewStateElement)
        {
            if (
                StringComparer.OrdinalIgnoreCase.Compare(modelViewStateElement.Name.LocalName,
                                                          XmlConstants.ModelViewState) != 0)
            {
                return;
            } //if

            modelViewState.SetInnerId(ReadInnerId(modelViewStateElement));
            modelViewState.Height =
                int.Parse(modelViewStateElement.Attributes().GetAttributeValue(XmlConstants.Height),
                           CultureInfo.InvariantCulture);
            modelViewState.Width =
                int.Parse(modelViewStateElement.Attributes().GetAttributeValue(XmlConstants.Width),
                           CultureInfo.InvariantCulture);
            var dataModelElement = modelViewStateElement.Elements().GetElementByName(XmlConstants.DataModel);
            var dataModel = Wiring.CreateInstance<DataModel>();

            DesirializedEntitiesContainer container;
            ProcessLoadDataModel(dataModel, dataModelElement, out container);
            modelViewState.DataModel = dataModel;

            var entityRelationStatesElement =
                modelViewStateElement.Elements().GetElementByName(XmlConstants.EntityRelationStates);

            foreach (var entityRelationStateElement in entityRelationStatesElement.Elements())
            {
                var entityRelationState = Wiring.CreateInstance<RelationViewState>();
                ProcessLoadEntityRelationState(entityRelationState, entityRelationStateElement, container);
                modelViewState.EntityRelationStates.Add(entityRelationState);
            } //foreach

            var entityViewStatesElement =
                modelViewStateElement.Elements().GetElementByName(XmlConstants.EntityViewStates);

            foreach (var entityViewStateElement in entityViewStatesElement.Elements())
            {
                var entityViewState = Wiring.CreateInstance<EntityViewState>();
                ProcessLoadEntityViewState(entityViewState, entityViewStateElement, container);
                modelViewState.EntityViewStates.Add(entityViewState);
            } //foreach
        }

        /// <summary>
        ///   Processes loading the entity view state.
        /// </summary>
        /// <param name = "entityViewState">The entity view state object.</param>
        /// <param name = "entityViewStateElement">The entity view state element.</param>
        /// <param name = "container">The container.</param>
        private void ProcessLoadEntityViewState(EntityViewState entityViewState, XElement entityViewStateElement,
                                                 DesirializedEntitiesContainer container)
        {
            if (
                StringComparer.OrdinalIgnoreCase.Compare(entityViewStateElement.Name.LocalName,
                                                          XmlConstants.EntityViewState) != 0)
            {
                return;
            } //if

            entityViewState.SetInnerId(ReadInnerId(entityViewStateElement));
            entityViewState.IsEditing =
                bool.Parse( entityViewStateElement.Attributes().GetAttributeValue( XmlConstants.IsEditing ) );
            entityViewState.Height =
                double.Parse( entityViewStateElement.Attributes().GetAttributeValue( XmlConstants.Height ),
                              CultureInfo.InvariantCulture );
            entityViewState.Width =
                double.Parse( entityViewStateElement.Attributes().GetAttributeValue( XmlConstants.Width ),
                              CultureInfo.InvariantCulture );

            var locationElement = entityViewStateElement.Elements().GetElementByName(XmlConstants.Location);
            entityViewState.Location = ReadPointElement(locationElement);

            var backgroundBrushElement =
                entityViewStateElement.Elements().GetElementByName(XmlConstants.BackgroundBrush);

            Brush brush;
            ProcessLoadBrush(out brush, backgroundBrushElement);
            entityViewState.BackgroundBrush = brush;

            var entityElement = entityViewStateElement.Elements().GetElementByName(XmlConstants.Entity);

            entityViewState.Entity = container.GetEntityById(ReadInnerId(entityElement));
        }

        /// <summary>
        ///   Processes loading the brush object.
        /// </summary>
        /// <param name = "brush">The brush object.</param>
        /// <param name = "backgroundBrushElement">The x element.</param>
        private void ProcessLoadBrush(out Brush brush, XElement backgroundBrushElement)
        {
            var brushElement = backgroundBrushElement.Elements().GetElementByName(XmlConstants.Brush);
            var opacity = double.Parse(brushElement.Attributes().GetAttributeValue(XmlConstants.Opacity),
                                        CultureInfo.InvariantCulture);

            string type = brushElement.Attributes().GetAttributeValue(XmlConstants.Type);

            if (StringComparer.OrdinalIgnoreCase.Compare(type, XmlConstants.SolidColorBrush) == 0)
            {
                var solidColorBrush = new SolidColorBrush();
                solidColorBrush.Color = ReadColor(brushElement);
                brush = solidColorBrush;
            } //if
            else if (StringComparer.OrdinalIgnoreCase.Compare(type, XmlConstants.LinearGradientBrush) == 0)
            {
                var linearGradientBrush = new LinearGradientBrush();

                var startPointElement = brushElement.Elements().GetElementByName(XmlConstants.StartPoint);
                linearGradientBrush.StartPoint = ReadPointElement(startPointElement);

                var endPointElement = brushElement.Elements().GetElementByName(XmlConstants.EndPoint);
                linearGradientBrush.EndPoint = ReadPointElement(endPointElement);

                var gradientStopsElement = brushElement.Elements().GetElementByName(XmlConstants.GradientStops);

                foreach (var gradientStopElement in gradientStopsElement.Elements())
                {
                    var gradientStop = new GradientStop();
                    ProcessLoadGradientStop(gradientStop, gradientStopElement);
                    linearGradientBrush.GradientStops.Add(gradientStop);
                } //foreach

                brush = linearGradientBrush;
            } //if
            else
            {
                brush = new SolidColorBrush(Colors.Transparent);
            } //else

            brush.Opacity = opacity;
        }

        /// <summary>
        ///   Processes loading the gradient stop.
        /// </summary>
        /// <param name = "gradientStop">The gradient stop object.</param>
        /// <param name = "gradientStopElement">The gradient stop element.</param>
        private void ProcessLoadGradientStop(GradientStop gradientStop, XElement gradientStopElement)
        {
            if (
                StringComparer.OrdinalIgnoreCase.Compare(gradientStopElement.Name.LocalName, XmlConstants.GradientStop) !=
                0)
            {
                return;
            } //if

            gradientStop.Offset =
                double.Parse(gradientStopElement.Attributes().GetAttributeValue(XmlConstants.Offset),
                              CultureInfo.InvariantCulture);
            gradientStop.Color = ReadColor(gradientStopElement);
        }

        /// <summary>
        ///   Reads a color.
        /// </summary>
        /// <param name = "element">The x element.</param>
        /// <returns>The color value.</returns>
        private Color ReadColor(XElement element)
        {
            var colorElement = element.Elements().GetElementByName(XmlConstants.Color);
            var result = new Color();
            result.A = byte.Parse(colorElement.Attributes().GetAttributeValue(XmlConstants.A),
                                   CultureInfo.InvariantCulture);
            result.B = byte.Parse(colorElement.Attributes().GetAttributeValue(XmlConstants.B),
                                   CultureInfo.InvariantCulture);
            result.R = byte.Parse(colorElement.Attributes().GetAttributeValue(XmlConstants.R),
                                   CultureInfo.InvariantCulture);
            result.G = byte.Parse(colorElement.Attributes().GetAttributeValue(XmlConstants.G),
                                   CultureInfo.InvariantCulture);
            return result;
        }

        /// <summary>
        ///   Processes loading the entity relation view state..
        /// </summary>
        /// <param name = "entityRelationState">The entity relation view state object.</param>
        /// <param name = "entityRelationStateElement">The relation view state element.</param>
        /// <param name = "container">The container.</param>
        private void ProcessLoadEntityRelationState(RelationViewState entityRelationState,
                                                     XElement entityRelationStateElement,
                                                     DesirializedEntitiesContainer container)
        {
            if (
                StringComparer.OrdinalIgnoreCase.Compare(entityRelationStateElement.Name.LocalName,
                                                          XmlConstants.EntityRelationState) != 0)
            {
                return;
            } //if

            entityRelationState.SetInnerId(ReadInnerId(entityRelationStateElement));

            RectConnector destinationPoint;
            var destinationPointElement =
                entityRelationStateElement.Elements().GetElementByName(XmlConstants.DestinationPoint);
            ProcessLoadRectConnector(out destinationPoint, destinationPointElement);

            entityRelationState.DestinationPoint = destinationPoint;

            RectConnector startPoint;
            var startPointElement =
                entityRelationStateElement.Elements().GetElementByName(XmlConstants.StartPoint);
            ProcessLoadRectConnector(out startPoint, startPointElement);

            entityRelationState.StartPoint = startPoint;

            var relationElement = entityRelationStateElement.Elements().GetElementByName(XmlConstants.Relation);
            entityRelationState.Relation = container.GetRelationById(ReadInnerId(relationElement));
        }

        /// <summary>
        ///   Processes loading the rect connector.
        /// </summary>
        /// <param name = "rectConnector">The rect connector object.</param>
        /// <param name = "rectConnectorElement">The destination point element.</param>
        private void ProcessLoadRectConnector(out RectConnector rectConnector, XElement rectConnectorElement)
        {
            var rectPointElement = rectConnectorElement.Elements().GetElementByName(XmlConstants.RectConnector);
            RectSide rectSide;

            Enum.TryParse(rectPointElement.Attributes().GetAttributeValue(XmlConstants.RectSide), true, out rectSide);
            var middleSidePointElement = rectPointElement.Elements().GetElementByName(XmlConstants.MiddleSidePoint);
            var point = ReadPointElement(middleSidePointElement);

            rectConnector = new RectConnector(rectSide, point);
        }

        /// <summary>
        ///   Processes loading the data model.
        /// </summary>
        /// <param name = "dataModel">The data model object.</param>
        /// <param name = "dataModelElement">The data model element.</param>
        /// <param name = "container">The container.</param>
        private void ProcessLoadDataModel(DataModel dataModel, XElement dataModelElement,
                                           out DesirializedEntitiesContainer container)
        {
            container = new DesirializedEntitiesContainer();
            if (StringComparer.OrdinalIgnoreCase.Compare(dataModelElement.Name.LocalName, XmlConstants.DataModel) !=
                 0)
            {
                return;
            } //if

            dataModel.SetInnerId(ReadInnerId(dataModelElement));
            dataModel.Caption = ReadCaption(dataModelElement);
            var entityElements = dataModelElement.Elements().GetElementByName(XmlConstants.Entities);


            foreach (var entityElement in entityElements.Elements())
            {
                var entity = Wiring.CreateInstance<ERDEntity>();
                ProcessLoadEntity(entity, entityElement);
                dataModel.Entities.Add(entity);
            } //foreach

            container.Entities = dataModel.Entities;

            var relationsElement = dataModelElement.Elements().GetElementByName(XmlConstants.Relations);

            foreach (var relationElement in relationsElement.Elements())
            {
                var relation = Wiring.CreateInstance<EntityRelation>();
                ProcessLoadRelation(relation, relationElement, container);
                dataModel.Relations.Add(relation);
            } //foreach
            container.Relations = dataModel.Relations;
        }

        /// <summary>
        ///   Processes loading the relation.
        /// </summary>
        /// <param name = "relation">The relation object.</param>
        /// <param name = "relationElement">The relation element.</param>
        /// <param name = "container">The container.</param>
        private void ProcessLoadRelation(EntityRelation relation, XElement relationElement,
                                          DesirializedEntitiesContainer container)
        {
            if (StringComparer.OrdinalIgnoreCase.Compare(relationElement.Name.LocalName, XmlConstants.Relation) != 0)
            {
                return;
            } //if

            relation.SetInnerId(ReadInnerId(relationElement));
            relation.Caption = ReadCaption(relationElement);

            RelationshipType relationshipType;
            Enum.TryParse(relationElement.Attributes().GetAttributeValue(XmlConstants.RelationType), true,
                           out relationshipType);
            relation.Type = relationshipType;

            relation.Parent =
                container.GetEntityById(ReadInnerId(relationElement.Elements().GetElementByName(XmlConstants.Parent)));

            relation.Child =
                container.GetEntityById(ReadInnerId(relationElement.Elements().GetElementByName(XmlConstants.Child)));

            var cardinalityElement = relationElement.Elements().GetElementByName( XmlConstants.Cardinality );
            ProcessLoadCardinality( relation.Cardinality, cardinalityElement );
        }

        /// <summary>
        /// Processes loading the cardinality.
        /// </summary>
        /// <param name="cardinality">The cardinality element.</param>
        /// <param name="cardinalityElement">The cardinality x element.</param>
        private void ProcessLoadCardinality(Cardinality cardinality, XElement cardinalityElement)
        {
            if ( StringComparer.OrdinalIgnoreCase.Compare( cardinalityElement.Name.LocalName,XmlConstants.Cardinality )!=0 ){
                return;
            } //if

            cardinality.SetInnerId( ReadInnerId( cardinalityElement ) );
            cardinality.ParentFrom =
                int.Parse( cardinalityElement.Attributes().GetAttributeValue( XmlConstants.ParentFrom ),
                           CultureInfo.InvariantCulture );
            cardinality.ChildFrom =
                int.Parse( cardinalityElement.Attributes().GetAttributeValue( XmlConstants.ChildFrom ) );
            cardinality.ChildTo = cardinalityElement.Attributes().GetAttributeValue( XmlConstants.ChildTo );
        }

        /// <summary>
        ///   Processes loading the erd entity.
        /// </summary>
        /// <param name = "entity">The erd entity object.</param>
        /// <param name = "entityElement">The erd entity element.</param>
        private void ProcessLoadEntity(ERDEntity entity, XElement entityElement)
        {
            if (StringComparer.OrdinalIgnoreCase.Compare(entityElement.Name.LocalName, XmlConstants.Entity) != 0)
            {
                return;
            } //if

            entity.Caption = ReadCaption(entityElement);
            entity.SetInnerId(ReadInnerId(entityElement));
            entity.Generated = bool.Parse( entityElement.Attributes().GetAttributeValue( XmlConstants.Generated ) );
            var attributesElement = entityElement.Elements().GetElementByName(XmlConstants.Attributes);

            foreach (var attributeElement in attributesElement.Elements())
            {
                var attribute = Wiring.CreateInstance<EntityAttribute>();
                ProcessLoadAttribute(attribute, attributeElement);
                entity.Attributes.Add(attribute);
            } //foreach

            var indexesElement = entityElement.Elements().GetElementByName( XmlConstants.Indexes );

            foreach ( var indexElement in indexesElement.Elements() ){
                var index = Wiring.CreateInstance<EntityIndex>();
                ProcessLoadIndex(entity, index, indexElement);
                entity.Indexes.Add( index );
            } //foreach

            var notesElement = entityElement.Elements().GetElementByName( XmlConstants.Notes );

            if ( notesElement!=null ){
                entity.Notes = notesElement.Value;
            } //if
        }

        /// <summary>
        ///   Processes loading the index.
        /// </summary>
        /// <param name="entity">The parent entity.</param>
        /// <param name = "index">The entity index object.</param>
        /// <param name = "indexElement">The index element.</param>
        private void ProcessLoadIndex(ERDEntity entity, EntityIndex index, XElement indexElement)
        {
            if (StringComparer.OrdinalIgnoreCase.Compare(indexElement.Name.LocalName, XmlConstants.EntityIndex) !=
                0)
            {
                return;
            } //if

            index.Parent = entity;
            
            index.SetInnerId( ReadInnerId( indexElement ) );
            index.Caption = ReadCaption( indexElement );
            index.IsUnique = bool.Parse( indexElement.Attributes().GetAttributeValue( XmlConstants.IsUnique ) );

            var indexAttributesElement = indexElement.Elements().GetElementByName( XmlConstants.IndexAttributes );

            foreach ( var indexAttributeElement in indexAttributesElement.Elements() ){
                var indexAttribute = Wiring.CreateInstance<IndexAttribute>();
                ProcessLoadIndexAttribute( entity, indexAttribute, indexAttributeElement );
                index.Attributes.Add( indexAttribute );
            } //foreach
        }

        /// <summary>
        ///   Processes loading the index attribute.
        /// </summary>
        /// <param name="entity">The parent entity.</param>
        /// <param name = "indexAttribute">The index attribute object.</param>
        /// <param name = "indexAttributeElement">The index attribute element.</param>
        private void ProcessLoadIndexAttribute( ERDEntity entity, IndexAttribute indexAttribute, XElement indexAttributeElement )
        {
            if (StringComparer.OrdinalIgnoreCase.Compare(indexAttributeElement.Name.LocalName, XmlConstants.IndexAttribute) !=
               0)
            {
                return;
            } //if

            indexAttribute.SetInnerId( ReadInnerId( indexAttributeElement ) );

            SortOrder sortOrder;
            Enum.TryParse( indexAttributeElement.Attributes().GetAttributeValue( XmlConstants.SortOrder ),
                                out sortOrder );
            indexAttribute.Order = sortOrder;

            var attributeId = new Guid(indexAttributeElement.Attributes().GetAttributeValue( XmlConstants.AttributeId ));

            indexAttribute.Attribute = entity.Attributes.FirstOrDefault( attribute => attribute.InnerId == attributeId );
        }

        /// <summary>
        ///   Processes loading the attribute.
        /// </summary>
        /// <param name = "attribute">The entity attribute object.</param>
        /// <param name = "attributeElement">The attribute element.</param>
        private void ProcessLoadAttribute(EntityAttribute attribute, XElement attributeElement)
        {
            if (StringComparer.OrdinalIgnoreCase.Compare(attributeElement.Name.LocalName, XmlConstants.Attribute) !=
                 0)
            {
                return;
            } //if

            attribute.Caption = ReadCaption(attributeElement);
            attribute.SetInnerId(ReadInnerId(attributeElement));
            attribute.DataLenght = int.Parse(
                attributeElement.Attributes().GetAttributeValue(XmlConstants.DataLenght), CultureInfo.InvariantCulture);
            attribute.Decimal = int.Parse(
                attributeElement.Attributes().GetAttributeValue(XmlConstants.Decimal), CultureInfo.InvariantCulture);
            attribute.IsIdentity = bool.Parse(
                attributeElement.Attributes().GetAttributeValue(XmlConstants.IsIdentity));
            attribute.IsNotNull = bool.Parse(
                attributeElement.Attributes().GetAttributeValue(XmlConstants.IsNotNull));
            attribute.IsUnique = bool.Parse(
                attributeElement.Attributes().GetAttributeValue(XmlConstants.IsUnique));

            attribute.Default = 
               attributeElement.Attributes().GetAttributeValue(XmlConstants.Default);

            AttributeKeyType result;
            Enum.TryParse(attributeElement.Attributes().GetAttributeValue(XmlConstants.Key), true,
                           out result);
            attribute.Key = result;

            SqlTypeBase dbType;
            XElement dbTypeElement = attributeElement.Elements().GetElementByName(XmlConstants.SqlType);

            ProcessLoadDbType(out dbType, dbTypeElement);

            attribute.DbType = dbType;
        }

        /// <summary>
        ///   Processes loading the db type.
        /// </summary>
        /// <param name = "dbType">The dbtype.</param>
        /// <param name = "dbTypeElement">The type xml element.</param>
        private void ProcessLoadDbType(out SqlTypeBase dbType, XElement dbTypeElement)
        {
            string name = dbTypeElement.Attributes().GetAttributeValue(XmlConstants.Name);
            var hasLength = bool.Parse(dbTypeElement.Attributes().GetAttributeValue(XmlConstants.HasLenght));
            var hasDecimal = bool.Parse(dbTypeElement.Attributes().GetAttributeValue(XmlConstants.HasDecimal));

            switch (name)
            {
                case SqLiteInteger.TypeName:
                    dbType = new SqLiteInteger();
                    break;
                case SqLiteBlob.TypeName:
                    dbType = new SqLiteBlob();
                    break;
                case SqLiteReal.TypeName:
                    dbType = new SqLiteReal();
                    break;
                case SqLiteText.TypeName:
                    dbType = new SqLiteText();
                    break;
                default:
                    dbType = new SqLiteAffinedType(name, hasLength, hasDecimal);
                    break;
            } //switch
        }

        /// <summary>
        ///   Reads the point element.
        /// </summary>
        /// <param name = "element">The x element.</param>
        /// <returns>The point value.</returns>
        private Point ReadPointElement(XElement element)
        {
            var pointElement = element.Elements().GetElementByName(XmlConstants.Point);
            return
                new Point(
                    double.Parse(pointElement.Attributes().GetAttributeValue(XmlConstants.x),
                                  CultureInfo.InvariantCulture),
                    double.Parse(pointElement.Attributes().GetAttributeValue(XmlConstants.y),
                                  CultureInfo.InvariantCulture));
        }

        /// <summary>
        ///   Gets the caption.
        /// </summary>
        /// <param name = "element">The x element.</param>
        /// <returns>The item name instance.</returns>
        private ItemName ReadCaption(XElement element)
        {
            var result = Wiring.CreateInstance<ItemName>();

            var captionElement = element.Elements().GetElementByName(XmlConstants.Caption);

            if (captionElement != null)
            {
                result.SetInnerId(ReadInnerId(captionElement));
                result.Physical = captionElement.Attributes().GetAttributeValue(XmlConstants.Physical);
                result.Title = captionElement.Attributes().GetAttributeValue(XmlConstants.Title);
            } //if

            return result;
        }

        /// <summary>
        ///   Gets the inner id attribute.
        /// </summary>
        /// <param name = "element">The x element.</param>
        /// <returns>The readed guid.</returns>
        private static Guid ReadInnerId(XElement element)
        {
            var result = Guid.Empty;

            var value = element.Attributes().GetAttributeValue(XmlConstants.InnerId);

            if (!string.IsNullOrEmpty(value))
            {
                result = Guid.Parse(value);
            } //if

            return result;
        }

        #endregion Deserialize
    }
}