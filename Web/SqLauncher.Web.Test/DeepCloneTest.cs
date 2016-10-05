// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   DeepCloneTest.cs
//   * Project: SqLauncher.Web.Test2
//   * Description:
//   * Created at 2012 01 07 13:59
//   * Modified at: 2012  01 07  13:59
// / ******************************************************************************/ 

using System.Linq;
using System.Windows;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SqLauncher.Web.Controller.DataModelInterceptions;
using SqLauncher.Web.Model;
using SqLauncher.Web.Model.SqLite;
using SqLauncher.Web.UI.Model;

namespace SqLauncher.Web.Test
{
    [TestClass]
    public class DeepCloneTest
    {
        [TestMethod]
        public void ItemNameTest()
        {
            var wiring = ContainerWiring.SetUp(new SqLiteModelInterception());
            var name1 = wiring.CreateInstance<ItemName>();
            name1.Physical = "Ttts1";
            name1.Title = "ds22s";
            var name2 = name1.Clone();
            Assert.AreEqual( name1.Physical, name2.Physical );
            Assert.AreEqual( name1.Title, name2.Title );
            Assert.AreNotEqual( name1.InnerId, name2.InnerId );

            name1.Physical = "aat1";
            name1.Title = "dsds";
            Assert.AreNotEqual( name1.Physical, name2.Physical );
            Assert.AreNotEqual( name1.Title, name2.Title );
        }

        [TestMethod]
        public void CardinalityTest()
        {
            var cardinality1 = new Cardinality();
            cardinality1.ChildFrom = 5;
            cardinality1.ChildTo = "Т";
            cardinality1.ParentFrom = 2;
            var cardinality2 = cardinality1.Clone();
            Assert.AreEqual( cardinality1.ChildFrom, cardinality2.ChildFrom );
            Assert.AreEqual( cardinality1.ChildTo, cardinality2.ChildTo );
            Assert.AreEqual( cardinality1.ParentFrom, cardinality2.ParentFrom );
            Assert.AreNotEqual( cardinality1.InnerId, cardinality2.InnerId );

            cardinality1.ChildFrom = 2;
            cardinality1.ChildTo = "4";
            cardinality1.ParentFrom = 33;

            Assert.AreNotEqual( cardinality1.ChildFrom, cardinality2.ChildFrom );
            Assert.AreNotEqual( cardinality1.ChildTo, cardinality2.ChildTo );
            Assert.AreNotEqual( cardinality1.ParentFrom, cardinality2.ParentFrom );
        }

        [TestMethod]
        public void IndexAttribute()
        {
            IndexAttribute attribute = new IndexAttribute();
            attribute.Order = SortOrder.Descending;
            attribute.Attribute = new EntityAttribute();

            var copy = attribute.Clone();
            
            Assert.AreNotEqual( copy.InnerId,attribute.InnerId );
            Assert.AreEqual( copy.Order,attribute.Order );
            Assert.IsNull( copy.Attribute );
        }

        [TestMethod]
        public void IndexTest()
        {
            EntityIndex index = new EntityIndex();
            index.Caption = new ItemName();
            index.Caption.Physical = "i1";
            index.Attributes.Add( new IndexAttribute{Attribute = new EntityAttribute(),Order = SortOrder.Descending} );
            index.Attributes.Add( new IndexAttribute{Attribute = new EntityAttribute(),Order = SortOrder.Ascending} );


            var copy = index.Clone();

            Assert.IsNull( copy.Parent );
            Assert.AreEqual( 2,index.Attributes.Count );
            Assert.AreEqual( 0,copy.Attributes.Count );
            Assert.AreEqual( "i1",copy.Caption.Physical );
        }


        [TestMethod]
        public void EntityAttributeTest()
        {
            var entityAttribute1 = new EntityAttribute();
            entityAttribute1.Caption = new ItemName();
            entityAttribute1.DbType = new SqLiteBlob();
            entityAttribute1.DataLenght = 55;
            entityAttribute1.IsIdentity = true;
            entityAttribute1.Default = "Default1";
            entityAttribute1.IsNotNull = true;
            entityAttribute1.IsUnique = true;
            entityAttribute1.Key = AttributeKeyType.IsKey;
            var entityAttribute2 = entityAttribute1.Clone();
            Assert.AreEqual( entityAttribute1.DataLenght, entityAttribute2.DataLenght );
            Assert.AreEqual( entityAttribute1.DbType, entityAttribute2.DbType );
            Assert.AreEqual( entityAttribute1.IsIdentity, entityAttribute2.IsIdentity );
            Assert.AreEqual( entityAttribute1.Default, entityAttribute2.Default );
            Assert.AreEqual( entityAttribute1.IsNotNull, entityAttribute2.IsNotNull );
            Assert.AreEqual( entityAttribute1.IsUnique, entityAttribute2.IsUnique );
            Assert.AreEqual( entityAttribute1.Key, entityAttribute2.Key );
            Assert.AreNotEqual( entityAttribute1.InnerId, entityAttribute2.InnerId );

            entityAttribute1.DbType = new SqLiteText();
            entityAttribute1.DataLenght = 45;
            entityAttribute1.IsIdentity = false;
            entityAttribute1.IsNotNull = false;
            entityAttribute1.IsUnique = false;
            entityAttribute1.Key = AttributeKeyType.IsForeignKey;

            Assert.AreNotEqual( entityAttribute1.DataLenght, entityAttribute2.DataLenght );
            Assert.AreNotEqual( entityAttribute1.DbType, entityAttribute2.DbType );
            Assert.AreNotEqual( entityAttribute1.IsIdentity, entityAttribute2.IsIdentity );
            Assert.AreNotEqual( entityAttribute1.IsNotNull, entityAttribute2.IsNotNull );
            Assert.AreNotEqual( entityAttribute1.IsUnique, entityAttribute2.IsUnique );
            Assert.AreNotEqual( entityAttribute1.Key, entityAttribute2.Key );
        }

        [TestMethod]
        public void IndexedERDEntityTest()
        {
            var erdEntity1 = new ERDEntity();
            erdEntity1.Caption = new ItemName();
            erdEntity1.Attributes.Add(new EntityAttribute { Caption = new ItemName { Physical = "LL" } });
            var atr1 = new EntityAttribute{Caption = new ItemName{Physical = "T"}};
            var atr2 = new EntityAttribute{Caption = new ItemName{Physical = "Po"}};
            erdEntity1.Attributes.Add( atr1 );
            erdEntity1.Attributes.Add( atr2 );

            erdEntity1.Generated = true;
            EntityIndex index = new EntityIndex();
            index.Caption = new ItemName();
            index.Caption.Physical = "dasd5sd";
            index.IsUnique = true;
            index.Attributes.Add(new IndexAttribute { Attribute = atr1, Order = SortOrder.Descending });
            index.Attributes.Add(new IndexAttribute { Attribute = atr2, Order = SortOrder.Ascending });
            erdEntity1.Indexes.Add( index );
            index.Parent = erdEntity1;

            var copy = erdEntity1.Clone();
            
            Assert.AreNotEqual( erdEntity1.InnerId,copy.InnerId );
            Assert.AreEqual( 3,copy.Attributes.Count );
            Assert.IsTrue( copy.Indexes.ToList()[0].IsUnique );
            Assert.AreEqual( 1,copy.Indexes.Count );
            Assert.AreEqual( erdEntity1.InnerId, copy.Indexes.ToList()[0].Parent.ClonedBy );
            Assert.AreEqual( 2,copy.Indexes.ToList()[0].Attributes.Count );
            Assert.AreEqual("dasd5sd", copy.Indexes.ToList()[0].Caption.Physical);
            Assert.AreEqual(SortOrder.Descending, copy.Indexes.ToList()[0].Attributes.ToList()[0].Order);
            Assert.AreEqual(SortOrder.Ascending, copy.Indexes.ToList()[0].Attributes.ToList()[1].Order);
            Assert.AreEqual(atr1.InnerId, copy.Indexes.ToList()[0].Attributes.ToList()[0].Attribute.ClonedBy);
            Assert.AreEqual(atr1.Caption.Physical, copy.Indexes.ToList()[0].Attributes.ToList()[0].Attribute.Caption.Physical);
            Assert.AreEqual(atr2.Caption.Physical, copy.Indexes.ToList()[0].Attributes.ToList()[1].Attribute.Caption.Physical);
        }

        [TestMethod]
        public void EntityRelationTest()
        {
            var entityRelation1 = new EntityRelation();
            entityRelation1.Caption = new ItemName();
            entityRelation1.Cardinality = new Cardinality();
            entityRelation1.Cardinality.ParentFrom = 1;
            entityRelation1.Type = RelationshipType.NonIdentifying;
            entityRelation1.Caption.Physical = "Test1";
            entityRelation1.Child = new ERDEntity();
            entityRelation1.Parent = new ERDEntity();

            var entityRelation2 = entityRelation1.Clone();

            Assert.AreEqual( entityRelation1.Type, entityRelation2.Type );
            Assert.AreEqual( entityRelation1.Caption.Physical, entityRelation2.Caption.Physical );
            Assert.IsNull( entityRelation2.Child );
            Assert.IsNull( entityRelation2.Parent );
            Assert.IsNotNull( entityRelation1.Child );
            Assert.IsNotNull( entityRelation1.Parent );

            Assert.AreEqual( entityRelation1.Cardinality.ParentFrom, entityRelation2.Cardinality.ParentFrom );
            entityRelation1.Cardinality.ParentFrom = 0;
            entityRelation1.Caption.Physical = "22";

            Assert.AreNotEqual( entityRelation1.Cardinality.ParentFrom, entityRelation2.Cardinality.ParentFrom );
            Assert.AreNotEqual( entityRelation1.Caption.Physical, entityRelation2.Caption.Physical );

            Assert.AreEqual( entityRelation1.InnerId, entityRelation2.ClonedBy );
        }

        [TestMethod]
        public void ERDEntityTest()
        {
            var erdEntity1 = new ERDEntity();
            erdEntity1.Caption = new ItemName();
            erdEntity1.Notes = "Notes of element";
            erdEntity1.Attributes.Add( new EntityAttribute{Caption = new ItemName{Physical = "LL"}} );
            erdEntity1.Attributes.Add( new EntityAttribute
                                       {Caption = new ItemName{Physical = "T"}, Key = AttributeKeyType.IsForeignKey} );
            erdEntity1.Generated = true;
            var erdEntity2 = erdEntity1.Clone();

            Assert.IsFalse( ReferenceEquals( erdEntity1.Caption, erdEntity2.Caption ) );
            Assert.AreEqual( erdEntity1.Generated, erdEntity2.Generated );
            Assert.AreEqual( erdEntity1.Notes, erdEntity2.Notes );

            Assert.AreEqual( erdEntity2.Attributes.Count, 1 );
            Assert.AreEqual( erdEntity2.ClonedBy, erdEntity1.InnerId );
            Assert.AreEqual( erdEntity2.Attributes.ToList()[0].Caption.Physical, "LL" );
        }

        [TestMethod]
        public void ModelViewStateTest()
        {
            var wiring = ContainerWiring.SetUp(new SqLiteModelInterception());
            var modelViewState = wiring.CreateInstance<IModelViewState>();
            modelViewState.Height = 15.00;
            modelViewState.Width = 33.55;


            var model = modelViewState.DataModel;

            var erdEntity1 = wiring.CreateInstance<ERDEntity>();
            erdEntity1.Caption.Physical = "entity1";
            erdEntity1.Attributes.Add(new EntityAttribute { Caption = new ItemName { Physical = "Name1" }, DbType = new SqLiteInteger() });
            erdEntity1.Attributes.Add(new EntityAttribute { Caption = new ItemName { Physical = "atr2" }, DbType = new SqLiteText() });
            model.Entities.Add(erdEntity1);

            var erdEntity2 = wiring.CreateInstance<ERDEntity>();
            erdEntity2.Caption.Physical = "entity1";
            erdEntity2.Attributes.Add(new EntityAttribute { Caption = new ItemName { Physical = "Name1" }, DbType = new SqLiteInteger() });
            erdEntity2.Attributes.Add(new EntityAttribute { Caption = new ItemName { Physical = "atr2" }, DbType = new SqLiteText() });
            model.Entities.Add(erdEntity2);

            var entityState1 = wiring.CreateInstance<IEntityViewState>();
            entityState1.Entity = erdEntity1;
            entityState1.Height = 10;
            entityState1.Width = 15;
            entityState1.Location = new Point(10, 11);
            modelViewState.EntityViewStates.Add(entityState1);

            var entityState2 = wiring.CreateInstance<IEntityViewState>();
            entityState2.Entity = erdEntity2;
            entityState2.Height = 10;
            entityState2.Width = 15;
            entityState2.Location = new Point(10,11);
            modelViewState.EntityViewStates.Add( entityState2 );

            var copy = modelViewState.Clone();

            Assert.AreEqual(copy.ClonedBy, modelViewState.InnerId);
            Assert.AreEqual( modelViewState.EntityViewStates.Count, copy.EntityViewStates.Count );
            Assert.AreEqual( modelViewState.Width, copy.Width );
            Assert.AreEqual( modelViewState.Height, copy.Height );
        }

        [TestMethod]
        public void ModelTest()
        {
            var wiring = ContainerWiring.SetUp( new SqLiteModelInterception() );
            var model = wiring.CreateInstance<DataModel>();
            var erdEntity1 = wiring.CreateInstance<ERDEntity>();
            erdEntity1.Caption.Physical = "entity1";
            erdEntity1.Attributes.Add( new EntityAttribute
                                       {Caption = new ItemName{Physical = "Name1"}, DbType = new SqLiteInteger()} );
            erdEntity1.Attributes.Add( new EntityAttribute
                                       {Caption = new ItemName{Physical = "atr2"}, DbType = new SqLiteText()} );
            model.Entities.Add( erdEntity1 );

            var erdEntity2 = wiring.CreateInstance<ERDEntity>();
            erdEntity2.Caption.Physical = "entity2";
            erdEntity2.Attributes.Add( new EntityAttribute
                                       {Caption = new ItemName{Physical = "Id"}, DbType = new SqLiteInteger()} );
            erdEntity2.Attributes.Add( new EntityAttribute
                                       {Caption = new ItemName{Physical = "atr2"}, DbType = new SqLiteText()} );
            erdEntity2.Attributes.Add( new EntityAttribute
                                       {Caption = new ItemName{Physical = "atr3"}, DbType = new SqLiteText()} );

            model.Entities.Add( erdEntity2 );

            var erdEntity3 = wiring.CreateInstance<ERDEntity>();
            erdEntity3.Caption.Physical = "entity3";
            erdEntity3.Attributes.Add( new EntityAttribute
                                       {Caption = new ItemName{Physical = "TreadId"}, DbType = new SqLiteInteger()} );

            model.Entities.Add( erdEntity3 );

            var erdEntity4 = wiring.CreateInstance<ERDEntity>();
            erdEntity4.Caption.Physical = "entity4";
            erdEntity4.Attributes.Add( new EntityAttribute
                                       {Caption = new ItemName{Physical = "CityId"}, DbType = new SqLiteInteger()} );
            model.Entities.Add( erdEntity4 );

            var relation1 = wiring.CreateInstance<EntityRelation>();
            relation1.Caption.Physical = "relation1";
            relation1.Cardinality.ChildFrom = 1;
            relation1.Child = erdEntity1;
            relation1.Parent = erdEntity4;

            model.Relations.Add( relation1 );

            var relation2 = wiring.CreateInstance<EntityRelation>();
            relation2.Caption.Physical = "relation2";
            relation2.Cardinality.ChildFrom = 0;
            relation2.Child = erdEntity2;
            relation2.Parent = erdEntity4;

            model.Relations.Add( relation2 );

            var clone = model.Clone();

            Assert.AreEqual( relation1.Child.InnerId, erdEntity1.InnerId );
            Assert.AreEqual(relation2.Child.InnerId, erdEntity2.InnerId);
            Assert.AreEqual(clone.Relations.ToList()[1].Child.ClonedBy, erdEntity2.InnerId);
            Assert.AreEqual(clone.Relations.ToList()[0].Child.ClonedBy, erdEntity1.InnerId);

            Assert.AreEqual(clone.Relations.ToList()[1].Parent.ClonedBy, erdEntity4.InnerId);
            Assert.AreEqual(clone.Relations.ToList()[0].Parent.ClonedBy, erdEntity4.InnerId);
            
            Assert.AreEqual( model.Entities.Count, clone.Entities.Count );
            Assert.AreEqual( model.Entities.ToList()[0].Attributes.Count, clone.Entities.ToList()[0].Attributes.Count );
            Assert.AreEqual( model.Entities.ToList()[1].Attributes.Count, clone.Entities.ToList()[1].Attributes.Count );
            Assert.AreEqual( model.Entities.ToList()[2].Attributes.Count, clone.Entities.ToList()[2].Attributes.Count );
            Assert.AreEqual( model.Entities.ToList()[3].Attributes.Count, clone.Entities.ToList()[3].Attributes.Count );

            Assert.AreEqual(model.Entities.ToList()[0].InnerId, clone.Entities.ToList()[0].ClonedBy);
            Assert.AreEqual(model.Entities.ToList()[1].InnerId, clone.Entities.ToList()[1].ClonedBy);
            Assert.AreEqual(model.Entities.ToList()[2].InnerId, clone.Entities.ToList()[2].ClonedBy);
            Assert.AreEqual(model.Entities.ToList()[3].InnerId, clone.Entities.ToList()[3].ClonedBy);

            Assert.AreEqual( model.Relations.Count, clone.Relations.Count );
        }
    }
}