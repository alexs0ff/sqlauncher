// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   SerializerTest.cs
//   * Project: SqLauncher.Web.Test2
//   * Description:
//   * Created at 2012 01 06 15:06
//   * Modified at: 2012  01 07  13:07
// / ******************************************************************************/ 

using System.Linq;
using System.Windows;
using System.Windows.Media;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SqLauncher.Web.Controller.DataModelInterceptions;
using SqLauncher.Web.Controller.XmlSerializes;
using SqLauncher.Web.Model;
using SqLauncher.Web.Model.SqLite;
using SqLauncher.Web.UI;
using SqLauncher.Web.UI.Model;

namespace SqLauncher.Web.Test.SqLite
{
    [TestClass]
    public class SerializerTest
    {
        [TestMethod]
        public void Serialize1()
        {
            var wiring = ContainerWiring.SetUp( new SqLiteModelInterception() );

            var version1 = new SqLiteDocumentSerializerVersion1();
            version1.Wiring = wiring;

            Assert.AreEqual( 1, version1.VersionNumber );

            var databaseDocument = wiring.CreateInstance<DatabaseDocument>();
            databaseDocument.IteractionState.PhysicalView = true;
            var databaseVersion = wiring.CreateInstance<DatabaseVersion>();
            databaseDocument.Name = "Name1";
            databaseVersion.Number = 2;
            databaseVersion.Locked = true;
            databaseVersion.Name = "Version1";
            databaseDocument.Versions.Add( databaseVersion );

            databaseVersion.ModelViewState.Width = 15;
            databaseVersion.ModelViewState.Height = 25;

            var dataModel = wiring.CreateInstance<DataModel>();
            dataModel.Caption.Physical = "PDMCaption";
            dataModel.Caption.Title = "TDMCaption";

            var entity = wiring.CreateInstance<ERDEntity>();
            entity.Caption.Physical = "PECaption";
            entity.Caption.Title = "TECaption";
            entity.Generated = true;
            entity.Notes = "Notes of entity <xml/> test</";
            var attribute = wiring.CreateInstance<EntityAttribute>();
            attribute.Caption.Physical = "APCaption";
            attribute.Caption.Title = "ATCaption";
            attribute.Key = AttributeKeyType.IsKey;
            attribute.IsIdentity = true;
            attribute.IsUnique = true;
            attribute.Default = "TestDefault";
            attribute.IsNotNull = true;
            attribute.DbType = new SqLiteInteger();

            entity.Attributes.Add( attribute );

            dataModel.Entities.Add( entity );

            databaseVersion.ModelViewState.DataModel = dataModel;

            var entityViewState = (EntityViewState) wiring.CreateInstance<IEntityViewState>();

            entityViewState.Location = new Point( 22.0, 55.0 );
            entityViewState.Height = 2015.55;
            entityViewState.Width = 458.99;
            entityViewState.Entity = entity;

            var backgroundBrush = new LinearGradientBrush();
            backgroundBrush.Opacity = 0.66;
            backgroundBrush.StartPoint = new Point( 33, 885 );
            backgroundBrush.EndPoint = new Point( 61, 887 );
            backgroundBrush.GradientStops.Add( new GradientStop{Color = Colors.Cyan, Offset = 0.24} );
            entityViewState.BackgroundBrush = backgroundBrush;


            databaseVersion.ModelViewState.EntityViewStates.Add( entityViewState );

            string xml = version1.Serialize( databaseDocument );

            Assert.IsNotNull( xml );

            var document2 = version1.Deserialize( xml );

            Assert.AreEqual( databaseDocument.InnerId, document2.InnerId );
            Assert.AreEqual( databaseDocument.IteractionState.PhysicalView, document2.IteractionState.PhysicalView );

            Assert.AreEqual( databaseDocument.Name, document2.Name );
            Assert.AreEqual( databaseDocument.Versions.Count, document2.Versions.Count );
            var savedVersion = document2.Versions.ToList()[0];

            Assert.AreEqual( databaseVersion.InnerId, savedVersion.InnerId );
            Assert.AreEqual( databaseVersion.Name, savedVersion.Name );
            Assert.AreEqual( databaseVersion.Locked, savedVersion.Locked );
            Assert.AreEqual( databaseVersion.Number, savedVersion.Number );
            Assert.AreEqual( databaseVersion.CreateDate, savedVersion.CreateDate );

            var savedModelViewState = savedVersion.ModelViewState;

            Assert.AreEqual( databaseVersion.ModelViewState.InnerId, savedModelViewState.InnerId );
            Assert.AreEqual( databaseVersion.ModelViewState.Height, savedModelViewState.Height );
            Assert.AreEqual( databaseVersion.ModelViewState.Width, savedModelViewState.Width );
            Assert.AreEqual( databaseVersion.ModelViewState.EntityRelationStates.Count,
                             savedModelViewState.EntityRelationStates.Count );
            Assert.AreEqual( databaseVersion.ModelViewState.EntityViewStates.Count,
                             savedModelViewState.EntityViewStates.Count );

            var savedDataModel = savedModelViewState.DataModel;

            Assert.AreEqual( dataModel.InnerId, savedDataModel.InnerId );
            Assert.AreEqual( dataModel.Caption.Physical, savedDataModel.Caption.Physical );
            Assert.AreEqual( dataModel.Caption.Title, savedDataModel.Caption.Title );
            Assert.AreEqual( dataModel.Entities.Count, savedDataModel.Entities.Count );
            Assert.AreEqual( dataModel.Relations.Count, savedDataModel.Relations.Count );

            var savedEntity = savedDataModel.Entities.ToList()[0];

            Assert.AreEqual( entity.InnerId, savedEntity.InnerId );
            Assert.AreEqual( entity.Caption.Physical, savedEntity.Caption.Physical );
            Assert.AreEqual( entity.Caption.Title, savedEntity.Caption.Title );
            Assert.AreEqual( entity.Attributes.Count, savedEntity.Attributes.Count );
            Assert.AreEqual( entity.Generated, savedEntity.Generated );
            Assert.AreEqual( entity.Notes, savedEntity.Notes );
            Assert.AreEqual( entity.Relations.Count, savedEntity.Relations.Count );

            var savedAttribute = savedEntity.Attributes.ToList()[0];

            Assert.AreEqual( attribute.InnerId, savedAttribute.InnerId );
            Assert.AreEqual( attribute.Caption.Title, savedAttribute.Caption.Title );
            Assert.AreEqual( attribute.Caption.Physical, savedAttribute.Caption.Physical );
            Assert.AreEqual( attribute.IsIdentity, savedAttribute.IsIdentity );
            Assert.AreEqual( attribute.IsNotNull, savedAttribute.IsNotNull );
            Assert.AreEqual( attribute.IsUnique, savedAttribute.IsUnique );
            Assert.AreEqual( attribute.Key, savedAttribute.Key );
            Assert.AreEqual( attribute.DataLenght, savedAttribute.DataLenght );
            Assert.AreEqual( attribute.Default, savedAttribute.Default );
            Assert.AreEqual( attribute.Decimal, savedAttribute.Decimal );

            var savedEntityViewState = savedModelViewState.EntityViewStates.ToList()[0];

            Assert.AreEqual( savedEntityViewState.InnerId, entityViewState.InnerId );
            Assert.AreEqual( savedEntityViewState.Width, entityViewState.Width );
            Assert.AreEqual( savedEntityViewState.Height, entityViewState.Height );
            Assert.AreEqual( savedEntityViewState.Location.X, entityViewState.Location.X );
            Assert.AreEqual( savedEntityViewState.Location.Y, entityViewState.Location.Y );
            Assert.AreEqual( savedEntityViewState.BackgroundBrush.Opacity,
                             entityViewState.BackgroundBrush.Opacity );
            Assert.AreEqual( savedEntityViewState.BackgroundBrush.GetType(),
                             entityViewState.BackgroundBrush.GetType() );

            var savedBrush = (LinearGradientBrush) savedEntityViewState.BackgroundBrush;
            Assert.AreEqual( savedBrush.GradientStops.Count, backgroundBrush.GradientStops.Count );
            Assert.AreEqual( savedBrush.GradientStops.ToList()[0].Color.A,
                             backgroundBrush.GradientStops.ToList()[0].Color.A );
            Assert.AreEqual( savedBrush.GradientStops.ToList()[0].Color.B,
                             backgroundBrush.GradientStops.ToList()[0].Color.B );
            Assert.AreEqual( savedBrush.GradientStops.ToList()[0].Color.G,
                             backgroundBrush.GradientStops.ToList()[0].Color.G );
            Assert.AreEqual( savedBrush.GradientStops.ToList()[0].Color.R,
                             backgroundBrush.GradientStops.ToList()[0].Color.R );
            Assert.AreEqual( savedBrush.GradientStops.ToList()[0].Offset,
                             backgroundBrush.GradientStops.ToList()[0].Offset );
        }

        [TestMethod]
        public void Serialize2()
        {
            var wiring = ContainerWiring.SetUp( new SqLiteModelInterception() );

            var version1 = new SqLiteDocumentSerializerVersion1();
            version1.Wiring = wiring;

            var databaseDocument = wiring.CreateInstance<DatabaseDocument>();
            var databaseVersion = wiring.CreateInstance<DatabaseVersion>();
            databaseDocument.Name = "Name1";
            databaseVersion.Number = 2;
            databaseVersion.Name = "Version1";
            databaseDocument.Versions.Add( databaseVersion );

            databaseVersion.ModelViewState.Width = 15;
            databaseVersion.ModelViewState.Height = 25;

            var dataModel = wiring.CreateInstance<DataModel>();
            dataModel.Caption.Physical = "PDMCaption";
            dataModel.Caption.Title = "TDMCaption";

            var entity1 = wiring.CreateInstance<ERDEntity>();
            entity1.Caption.Physical = "entity1";
            entity1.Caption.Title = "title1";

            var entity1Attribute1 = wiring.CreateInstance<EntityAttribute>();
            entity1Attribute1.Caption.Physical = "Id";
            entity1Attribute1.Caption.Title = "atrtitle1";
            entity1Attribute1.IsNotNull = true;
            entity1Attribute1.DbType = new SqLiteInteger();
            entity1Attribute1.Key = AttributeKeyType.IsKey;
            entity1.Attributes.Add( entity1Attribute1 );

            var entity1Attribute2 = wiring.CreateInstance<EntityAttribute>();
            entity1Attribute2.Caption.Physical = "Name";
            entity1Attribute2.Caption.Title = "atrtitle2";
            entity1Attribute2.IsNotNull = false;
            entity1Attribute2.DbType = new SqLiteText();
            entity1Attribute2.DataLenght = 255;
            entity1Attribute2.Key = AttributeKeyType.None;
            entity1.Attributes.Add( entity1Attribute2 );

            var index = wiring.CreateInstance<EntityIndex>();
            index.Caption.Physical = "Index1";
            index.Caption.Title = "Title1";
            index.IsUnique = true;
            index.Parent = entity1;
            entity1.Indexes.Add( index );

            var indexAttribute1 = wiring.CreateInstance<IndexAttribute>();
            indexAttribute1.Attribute = entity1Attribute1;
            indexAttribute1.Order = SortOrder.Ascending;
            index.Attributes.Add( indexAttribute1 );

            var indexAttribute2 = wiring.CreateInstance<IndexAttribute>();
            indexAttribute2.Attribute = entity1Attribute2;
            indexAttribute2.Order = SortOrder.Descending;
            index.Attributes.Add(indexAttribute2);

            var entity2 = wiring.CreateInstance<ERDEntity>();
            entity2.Caption.Physical = "entity2";
            entity2.Caption.Title = "title2";

            var entity2Attribute1 = wiring.CreateInstance<EntityAttribute>();
            entity2Attribute1.Caption.Physical = "Id";
            entity2Attribute1.Caption.Title = "atrtitle2";
            entity2Attribute1.IsNotNull = true;
            entity2Attribute1.DbType = new SqLiteInteger();
            entity2Attribute1.Key = AttributeKeyType.IsKey;
            entity2.Attributes.Add( entity1Attribute1 );

            var entity2Attribute2 = wiring.CreateInstance<EntityAttribute>();
            entity2Attribute2.Caption.Physical = "Name";
            entity2Attribute2.Caption.Title = "atrtitle2";
            entity2Attribute2.IsNotNull = false;
            entity2Attribute2.DbType = new SqLiteText();
            entity2Attribute2.DataLenght = 255;
            entity2Attribute2.Key = AttributeKeyType.None;
            entity2.Attributes.Add( entity1Attribute2 );

            var entityRelation = wiring.CreateInstance<EntityRelation>();
            entityRelation.Caption.Title = "relation1";
            entityRelation.Caption.Physical = "relationtitle1";

            entityRelation.Parent = entity1;
            entityRelation.Child = entity2;
            entityRelation.Cardinality.ChildFrom = 1;
            entityRelation.Cardinality.ChildTo = "N";
            entityRelation.Cardinality.ParentFrom = 0;
            entityRelation.Type = RelationshipType.Identifying;

            dataModel.Entities.Add( entity1 );
            dataModel.Entities.Add( entity2 );

            dataModel.Relations.Add( entityRelation );

            Assert.AreEqual( 3, entity2.Attributes.Count );

            var entityRelationViewState = (RelationViewState) wiring.CreateInstance<IRelationViewState>();
            entityRelationViewState.DestinationPoint = new RectConnector( RectSide.Left, new Point( 66.33, 74.12 ) );
            entityRelationViewState.StartPoint = new RectConnector( RectSide.Top, new Point( 22.31, 12.12 ) );
            entityRelationViewState.Relation = entityRelation;

            databaseVersion.ModelViewState.DataModel = dataModel;
            databaseVersion.ModelViewState.EntityRelationStates.Add( entityRelationViewState );

            var entityViewState1 = (EntityViewState) wiring.CreateInstance<IEntityViewState>();
            var backgroundBrush1 = new LinearGradientBrush();
            backgroundBrush1.Opacity = 0.66;
            backgroundBrush1.StartPoint = new Point( 33, 885 );
            backgroundBrush1.EndPoint = new Point( 61, 887 );
            backgroundBrush1.GradientStops.Add( new GradientStop{Color = Colors.Green, Offset = 0.24} );

            entityViewState1.BackgroundBrush = backgroundBrush1;
            entityViewState1.Entity = entity1;
            entityViewState1.Location = new Point( 93, 54 );
            databaseVersion.ModelViewState.EntityViewStates.Add( entityViewState1 );

            var backgroundBrush2 = new SolidColorBrush( Colors.Orange );
            backgroundBrush2.Opacity = 0.4;

            var entityViewState2 = (EntityViewState) wiring.CreateInstance<IEntityViewState>();
            entityViewState2.BackgroundBrush = backgroundBrush2;
            entityViewState2.Location = new Point( 99, 21 );
            entityViewState2.Entity = entity2;
            databaseVersion.ModelViewState.EntityViewStates.Add( entityViewState2 );

            var xml = version1.Serialize( databaseDocument );
            Assert.IsNotNull( xml );

            var savedDocument = version1.Deserialize( xml );

            Assert.AreEqual( databaseDocument.InnerId, savedDocument.InnerId );
            Assert.AreEqual( databaseDocument.Name, savedDocument.Name );
            Assert.AreEqual( databaseDocument.Versions.Count, savedDocument.Versions.Count );

            var savedModelViewState = databaseDocument.Versions.ToList()[0].ModelViewState;

            Assert.AreEqual( savedModelViewState.EntityViewStates.Count,
                             databaseVersion.ModelViewState.EntityViewStates.Count );
            Assert.AreEqual( savedModelViewState.EntityRelationStates.Count,
                             databaseVersion.ModelViewState.EntityRelationStates.Count );

            var savedDataModel = savedModelViewState.DataModel;

            Assert.AreEqual( dataModel.Entities.Count, savedDataModel.Entities.Count );
            Assert.AreEqual( dataModel.Relations.Count, savedDataModel.Relations.Count );

            var savedEntity1 = dataModel.Entities.ToList()[0];
            var savedEntity2 = dataModel.Entities.ToList()[1];

            Assert.AreEqual( entity1.Attributes.Count, savedEntity1.Attributes.Count );
            Assert.AreEqual( entity1.Indexes.Count, savedEntity1.Indexes.Count );
            Assert.AreEqual( entity2.Attributes.Count, savedEntity2.Attributes.Count );
            Assert.AreEqual( entity2.Indexes.Count, savedEntity2.Indexes.Count );

            var savedIndex = entity1.Indexes.ToList()[0];

            Assert.AreEqual(index.Parent.InnerId, savedIndex.Parent.InnerId);
            Assert.AreEqual(index.Attributes.Count, savedIndex.Attributes.Count);
            Assert.AreEqual(index.IsUnique, savedIndex.IsUnique);
            Assert.AreEqual(index.Caption.Physical, savedIndex.Caption.Physical);

            var savedIndexAttribute1 = savedIndex.Attributes.ToList()[0];
            Assert.AreEqual(indexAttribute1.Order, savedIndexAttribute1.Order);
            Assert.AreEqual(indexAttribute1.Attribute.InnerId, savedIndexAttribute1.Attribute.InnerId);

            var savedIndexAttribute2 = savedIndex.Attributes.ToList()[1];
            Assert.AreEqual(indexAttribute2.Order, savedIndexAttribute2.Order);
            Assert.AreEqual(indexAttribute2.Attribute.InnerId, savedIndexAttribute2.Attribute.InnerId);

            var savedRelation = savedDataModel.Relations.ToList()[0];

            Assert.AreEqual( entityRelation.InnerId, savedRelation.InnerId );
            Assert.AreEqual( entityRelation.Child.InnerId, savedRelation.Child.InnerId );
            Assert.AreEqual( entityRelation.Parent.InnerId, savedRelation.Parent.InnerId );
            Assert.AreEqual( entityRelation.Caption.Physical, savedRelation.Caption.Physical );
            Assert.AreEqual( entityRelation.Caption.Title, savedRelation.Caption.Title );
            Assert.AreEqual( entityRelation.Type, savedRelation.Type );
            Assert.AreEqual( entityRelation.Cardinality.ChildFrom, savedRelation.Cardinality.ChildFrom );
            Assert.AreEqual( entityRelation.Cardinality.ChildTo, savedRelation.Cardinality.ChildTo );
            Assert.AreEqual( entityRelation.Cardinality.ParentFrom, savedRelation.Cardinality.ParentFrom );

            var savedRelationViewState = (RelationViewState) savedModelViewState.EntityRelationStates.ToList()[0];

            Assert.AreEqual( entityRelationViewState.InnerId, savedRelationViewState.InnerId );
            Assert.AreEqual( entityRelationViewState.DestinationPoint.MiddleSidePoint.X,
                             savedRelationViewState.DestinationPoint.MiddleSidePoint.X );
            Assert.AreEqual( entityRelationViewState.DestinationPoint.MiddleSidePoint.Y,
                             savedRelationViewState.DestinationPoint.MiddleSidePoint.Y );
            Assert.AreEqual( entityRelationViewState.DestinationPoint.RectSide,
                             savedRelationViewState.DestinationPoint.RectSide );
            Assert.AreEqual( entityRelationViewState.StartPoint.MiddleSidePoint.X,
                             savedRelationViewState.StartPoint.MiddleSidePoint.X );
            Assert.AreEqual( entityRelationViewState.StartPoint.MiddleSidePoint.Y,
                             savedRelationViewState.StartPoint.MiddleSidePoint.Y );
            Assert.AreEqual( entityRelationViewState.StartPoint.RectSide, savedRelationViewState.StartPoint.RectSide );
            Assert.AreEqual( entityRelationViewState.Relation.InnerId, savedRelationViewState.Relation.InnerId );

            var savedEntityViewState2 = savedModelViewState.EntityViewStates.ToList()[1];

            var savedBrush = savedEntityViewState2.BackgroundBrush as SolidColorBrush;

            Assert.IsNotNull( savedBrush );
            Assert.AreEqual( backgroundBrush2.Color.A, savedBrush.Color.A );
            Assert.AreEqual( backgroundBrush2.Color.B, savedBrush.Color.B );
            Assert.AreEqual( backgroundBrush2.Color.G, savedBrush.Color.G );
            Assert.AreEqual( backgroundBrush2.Color.R, savedBrush.Color.R );
            Assert.AreEqual( backgroundBrush2.Opacity, savedBrush.Opacity );
        }

        [TestMethod]
        public void EmptySerialize()
        {
            var wiring = ContainerWiring.SetUp( new SqLiteModelInterception() );

            var version1 = new SqLiteDocumentSerializerVersion1();
            version1.Wiring = wiring;

            var databaseDocument = wiring.CreateInstance<DatabaseDocument>();
            var databaseVersion = wiring.CreateInstance<DatabaseVersion>();
            databaseDocument.Name = "Name1";
            databaseVersion.Number = 2;
            databaseVersion.Name = "Version1";
            databaseDocument.Versions.Add( databaseVersion );

            var xml = version1.Serialize( databaseDocument );
            Assert.IsFalse( string.IsNullOrEmpty( xml ) );

            var savedDocument = version1.Deserialize( xml );

            Assert.AreEqual( 1, savedDocument.Versions.Count );

            var savedVersion = savedDocument.Versions.ToList()[0];
            Assert.AreEqual( databaseVersion.ModelViewState.EntityRelationStates.Count,
                             savedVersion.ModelViewState.EntityRelationStates.Count );
            Assert.AreEqual( databaseVersion.ModelViewState.EntityViewStates.Count,
                             savedVersion.ModelViewState.EntityViewStates.Count );
        }
    }
}