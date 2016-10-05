// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   EntityRelationWatcherTest.cs
//   * Project: SqLauncher.Web.Test
//   * Description:
//   * Created at 2011 11 21 19:22
//   * Modified at: 2011  11 21  20:45
// / ******************************************************************************/ 

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SqLauncher.Web.Controller.DataModelInterceptions;
using SqLauncher.Web.Model;
using SqLauncher.Web.Model.SqLite;

namespace SqLauncher.Web.Test.SqLite
{
    [TestClass]
    public class EntityRelationWatcherTest
    {
        public static EntityRelationWatcher GetNewWatcher( EntityRelation relation )
        {
            //wiring = ContainerWiring.SetUp(new SqLiteModelInterception());
            return new EntityRelationWatcher( relation );
        }

        private static EntityRelation GetTestEntities(ContainerWiring wiring, out ERDEntity childERDEntity, out ERDEntity parentERDEntity )
        {
            parentERDEntity = wiring.CreateInstance<ERDEntity>();
            parentERDEntity.Caption = wiring.CreateInstance<ItemName>();
            parentERDEntity.Caption.Physical = "Test";
            parentERDEntity.Caption.Title = "Test";
            parentERDEntity.Attributes.Add( new EntityAttribute{
                                                                   Caption =
                                                                       new ItemName{Title = "Test Id", Physical = "Id"},
                                                                   DataLenght = 0,
                                                                   DbType = new SqLiteInteger(),
                                                                   Key = AttributeKeyType.IsKey
                                                               } );
            parentERDEntity.Attributes.Add( new EntityAttribute{
                                                                   Caption =
                                                                       new ItemName
                                                                       {Title = "Test Name", Physical = "Name"},
                                                                   DataLenght = 10,
                                                                   DbType = new SqLiteText(),
                                                                   Key = AttributeKeyType.None
                                                               } );

            childERDEntity = wiring.CreateInstance<ERDEntity>();
            childERDEntity.Caption = wiring.CreateInstance<ItemName>();
            childERDEntity.Caption.Title = "Object";
            childERDEntity.Caption.Physical = "Object";
            childERDEntity.Attributes.Add( new EntityAttribute{
                                                                  Caption =
                                                                      new ItemName{Title = "Object Id", Physical = "Id"},
                                                                  DataLenght = 0,
                                                                  DbType = new SqLiteInteger(),
                                                                  Key = AttributeKeyType.IsKey
                                                              } );
            childERDEntity.Attributes.Add( new EntityAttribute{
                                                                  Caption =
                                                                      new ItemName
                                                                      {Title = "Object Name", Physical = "Name"},
                                                                  DataLenght = 10,
                                                                  DbType = new SqLiteText(),
                                                                  Key = AttributeKeyType.None
                                                              } );

            EntityRelation relation = wiring.CreateInstance<EntityRelation>();
            relation.Caption = wiring.CreateInstance<ItemName>();
            relation.Caption.Physical = "tets";

            relation.Parent = parentERDEntity;
            relation.Child = childERDEntity;
            return relation;
        }

        [TestMethod]
        public void DinamicForeignKeyTest2()
        {
            ERDEntity parentEntity;
            ERDEntity childEntity;

            ContainerWiring wiring = ContainerWiring.SetUp(new SqLiteModelInterception());

            var relation = GetTestEntities(wiring, out childEntity, out parentEntity);
            relation.Type = RelationshipType.Informative;

            Assert.IsTrue(relation.ParentAttributes.Count == 0, "relation.ParentAttributes.Count == 0");
            Assert.IsTrue(relation.ChildAttributes.Count == 0, "relation.ChildAttributes.Count == 0");

            foreach (var atr in childEntity.Attributes)
            {
                Assert.AreNotEqual(atr.Key, AttributeKeyType.IsForeignKey, "atr.Key, AttributeKeyType.IsForeignKey");
                Assert.AreNotEqual(atr.Key, AttributeKeyType.IsPrimaryForeignKey,
                                    "atr.Key, AttributeKeyType.IsPrimaryForeignKey");
            } //foreach

            Assert.AreEqual(parentEntity.Attributes.Count, 2, "parentEntity.Attributes.Count, 2");
            Assert.AreEqual(childEntity.Attributes.Count, 2, "childEntity.Attributes.Count, 2");
            Assert.AreEqual(0, childEntity.Relations.Count, "0, childEntity.Relations");
            Assert.AreEqual(0, parentEntity.Relations.Count, "0, parentEntity.Relations");

            var watcher = GetNewWatcher(relation);

            Assert.AreEqual(relation.ParentAttributes.Count, 0, "relation.ParentAttributes.Count == 0");
            Assert.AreEqual(relation.ChildAttributes.Count, 0, "relation.ChildAttributes.Count == 0");

            Assert.AreEqual(parentEntity.Attributes.Count, 2, "parentEntity.Attributes.Count, 2");
            Assert.AreEqual(childEntity.Attributes.Count, 2, "childEntity.Attributes.Count, 2");

            Assert.AreEqual(1, childEntity.Relations.Count, "1, childEntity.Relations");
            Assert.AreEqual(1, parentEntity.Relations.Count, "1, parentEntity.Relations");


            int coundFk = 0;

            foreach (var atr in childEntity.Attributes)
            {
                if (atr.Key == AttributeKeyType.IsForeignKey)
                {
                    coundFk++;
                } //if
            } //foreach
            Assert.IsTrue(coundFk == 0, "coundFk==0");

            coundFk = 0;

            foreach (var atr in childEntity.Attributes)
            {
                if (atr.Key == AttributeKeyType.IsPrimaryForeignKey)
                {
                    coundFk++;
                } //if
            } //foreach
            Assert.IsTrue(coundFk == 0, "coundFk==0");

            relation.Type = RelationshipType.Identifying;

            Assert.AreEqual(relation.ParentAttributes.Count, 1, "relation.ParentAttributes.Count == 1");
            Assert.AreEqual(relation.ChildAttributes.Count, 1, "relation.ChildAttributes.Count == 1");

            Assert.AreEqual(parentEntity.Attributes.Count, 2, "parentEntity.Attributes.Count, 2");
            Assert.AreEqual(childEntity.Attributes.Count, 3, "childEntity.Attributes.Count, 3");

            Assert.AreEqual(1, childEntity.Relations.Count, "1, childEntity.Relations");
            Assert.AreEqual(1, parentEntity.Relations.Count, "1, parentEntity.Relations");


            coundFk = 0;

            foreach (var atr in childEntity.Attributes)
            {
                if (atr.Key == AttributeKeyType.IsForeignKey)
                {
                    coundFk++;
                } //if
            } //foreach
            Assert.IsTrue(coundFk == 0, "coundFk==0");

            coundFk = 0;

            foreach (var atr in childEntity.Attributes)
            {
                if (atr.Key == AttributeKeyType.IsPrimaryForeignKey)
                {
                    coundFk++;
                } //if
            } //foreach
            Assert.IsTrue(coundFk == 1, "coundFk==1");

            watcher.Dispose();

            foreach (var atr in childEntity.Attributes)
            {
                Assert.AreNotEqual(atr.Key, AttributeKeyType.IsForeignKey,
                                    "atr.Key, AttributeKeyType.IsForeignKey after dispose");
            } //foreach

            Assert.AreEqual(relation.ParentAttributes.Count, 0, "relation.ParentAttributes.Count == 0");
            Assert.AreEqual(relation.ChildAttributes.Count, 0, "relation.ChildAttributes.Count == 0");
        }

        [TestMethod]
        public void DinamicForeignKeyTest1()
        {
            ERDEntity parentEntity;
            ERDEntity childEntity;

            ContainerWiring wiring = ContainerWiring.SetUp( new SqLiteModelInterception() );

            var relation = GetTestEntities( wiring, out childEntity, out parentEntity );
            relation.Type = RelationshipType.NonIdentifying;

            Assert.IsTrue( relation.ParentAttributes.Count == 0, "relation.ParentAttributes.Count == 0" );
            Assert.IsTrue( relation.ChildAttributes.Count == 0, "relation.ChildAttributes.Count == 0" );

            foreach ( var atr in childEntity.Attributes ){
                Assert.AreNotEqual( atr.Key, AttributeKeyType.IsForeignKey, "atr.Key, AttributeKeyType.IsForeignKey" );
                Assert.AreNotEqual( atr.Key, AttributeKeyType.IsPrimaryForeignKey,
                                    "atr.Key, AttributeKeyType.IsPrimaryForeignKey" );
            } //foreach

            Assert.AreEqual( parentEntity.Attributes.Count, 2, "parentEntity.Attributes.Count, 2" );
            Assert.AreEqual( childEntity.Attributes.Count, 2, "childEntity.Attributes.Count, 2" );
            Assert.AreEqual( 0, childEntity.Relations.Count, "0, childEntity.Relations" );
            Assert.AreEqual( 0, parentEntity.Relations.Count, "0, parentEntity.Relations" );

            var watcher = GetNewWatcher( relation );

            Assert.AreEqual( relation.ParentAttributes.Count, 1, "relation.ParentAttributes.Count == 1" );
            Assert.AreEqual( relation.ChildAttributes.Count, 1, "relation.ChildAttributes.Count == 1" );

            Assert.AreEqual( parentEntity.Attributes.Count, 2, "parentEntity.Attributes.Count, 2" );
            Assert.AreEqual( childEntity.Attributes.Count, 3, "childEntity.Attributes.Count, 3" );

            Assert.AreEqual( 1, childEntity.Relations.Count, "1, childEntity.Relations" );
            Assert.AreEqual( 1, parentEntity.Relations.Count, "1, parentEntity.Relations" );


            int coundFk = 0;

            foreach ( var atr in childEntity.Attributes ){
                if ( atr.Key == AttributeKeyType.IsForeignKey ){
                    coundFk++;
                } //if
            } //foreach
            Assert.IsTrue( coundFk == 1, "coundFk==1" );

            relation.Type = RelationshipType.Identifying;

            Assert.AreEqual(relation.ParentAttributes.Count, 1, "relation.ParentAttributes.Count == 1");
            Assert.AreEqual(relation.ChildAttributes.Count, 1, "relation.ChildAttributes.Count == 1");

            Assert.AreEqual(parentEntity.Attributes.Count, 2, "parentEntity.Attributes.Count, 2");
            Assert.AreEqual(childEntity.Attributes.Count, 3, "childEntity.Attributes.Count, 3");

            Assert.AreEqual(1, childEntity.Relations.Count, "1, childEntity.Relations");
            Assert.AreEqual(1, parentEntity.Relations.Count, "1, parentEntity.Relations");


            coundFk = 0;

            foreach (var atr in childEntity.Attributes)
            {
                if (atr.Key == AttributeKeyType.IsForeignKey)
                {
                    coundFk++;
                } //if
            } //foreach
            Assert.IsTrue(coundFk == 0, "coundFk==0");

            coundFk = 0;

            foreach (var atr in childEntity.Attributes)
            {
                if (atr.Key == AttributeKeyType.IsPrimaryForeignKey)
                {
                    coundFk++;
                } //if
            } //foreach
            Assert.IsTrue(coundFk == 1, "coundFk==1");


            relation.Type = RelationshipType.Informative;

            Assert.AreEqual(relation.ParentAttributes.Count, 0, "relation.ParentAttributes.Count == 0");
            Assert.AreEqual(relation.ChildAttributes.Count, 0, "relation.ChildAttributes.Count == 0");

            Assert.AreEqual(parentEntity.Attributes.Count, 2, "parentEntity.Attributes.Count, 2");
            Assert.AreEqual(childEntity.Attributes.Count, 2, "childEntity.Attributes.Count, 2");

            Assert.AreEqual(1, childEntity.Relations.Count, "1, childEntity.Relations");
            Assert.AreEqual(1, parentEntity.Relations.Count, "1, parentEntity.Relations");


            coundFk = 0;

            foreach (var atr in childEntity.Attributes)
            {
                if (atr.Key == AttributeKeyType.IsForeignKey)
                {
                    coundFk++;
                } //if
            } //foreach
            Assert.IsTrue(coundFk == 0, "coundFk==0");

            coundFk = 0;

            foreach (var atr in childEntity.Attributes)
            {
                if (atr.Key == AttributeKeyType.IsPrimaryForeignKey)
                {
                    coundFk++;
                } //if
            } //foreach
            Assert.IsTrue(coundFk == 0, "coundFk==0");
            
            watcher.Dispose();

            foreach ( var atr in childEntity.Attributes ){
                Assert.AreNotEqual( atr.Key, AttributeKeyType.IsForeignKey,
                                    "atr.Key, AttributeKeyType.IsForeignKey after dispose" );
            } //foreach

            Assert.AreEqual( relation.ParentAttributes.Count, 0, "relation.ParentAttributes.Count == 0" );
            Assert.AreEqual( relation.ChildAttributes.Count, 0, "relation.ChildAttributes.Count == 0" );
        }

        [TestMethod]
        public void SimpleForeignKeyTest()
        {
            RelationTypeTest(AttributeKeyType.IsForeignKey, RelationshipType.NonIdentifying);
            RelationTypeTest(AttributeKeyType.IsPrimaryForeignKey, RelationshipType.Identifying);
            RelationTypeTest(AttributeKeyType.None, RelationshipType.Informative);
        }

        private static void RelationTypeTest( AttributeKeyType exceptedAttributeKeyType, RelationshipType relationshipType )
        {
            ERDEntity parentEntity;
            ERDEntity childEntity;

            ContainerWiring wiring = ContainerWiring.SetUp( new SqLiteModelInterception() );

            var relation = GetTestEntities( wiring, out childEntity, out parentEntity );
            relation.Type = relationshipType;

            Assert.IsTrue( relation.ParentAttributes.Count == 0, "relation.ParentAttributes.Count == 0" );
            Assert.IsTrue( relation.ChildAttributes.Count == 0, "relation.ChildAttributes.Count == 0" );

            foreach ( var atr in childEntity.Attributes ){
                Assert.AreNotEqual( atr.Key, AttributeKeyType.IsForeignKey, "atr.Key, AttributeKeyType.IsForeignKey" );
                Assert.AreNotEqual( atr.Key, AttributeKeyType.IsPrimaryForeignKey,
                                    "atr.Key, AttributeKeyType.IsPrimaryForeignKey" );
            } //foreach

            Assert.AreEqual( parentEntity.Attributes.Count, 2, "parentEntity.Attributes.Count, 2" );
            Assert.AreEqual( childEntity.Attributes.Count, 2, "childEntity.Attributes.Count, 2" );
            Assert.AreEqual( 0, childEntity.Relations.Count, "0, childEntity.Relations" );
            Assert.AreEqual( 0, parentEntity.Relations.Count, "0, parentEntity.Relations" );

            var watcher = GetNewWatcher( relation );

            if (relationshipType == RelationshipType.Informative){
                Assert.AreEqual( relation.ParentAttributes.Count, 0, "relation.ParentAttributes.Count == 1" );
                Assert.AreEqual( relation.ChildAttributes.Count, 0, "relation.ChildAttributes.Count == 1" );

                Assert.AreEqual( parentEntity.Attributes.Count, 2, "parentEntity.Attributes.Count, 2" );
                Assert.AreEqual( childEntity.Attributes.Count, 2, "childEntity.Attributes.Count, 3" );

                Assert.AreEqual( 1, childEntity.Relations.Count, "1, childEntity.Relations" );
                Assert.AreEqual( 1, parentEntity.Relations.Count, "1, parentEntity.Relations" );
            }
            else{
                Assert.AreEqual(relation.ParentAttributes.Count, 1, "relation.ParentAttributes.Count == 1");
                Assert.AreEqual(relation.ChildAttributes.Count, 1, "relation.ChildAttributes.Count == 1");

                Assert.AreEqual(parentEntity.Attributes.Count, 2, "parentEntity.Attributes.Count, 2");
                Assert.AreEqual(childEntity.Attributes.Count, 3, "childEntity.Attributes.Count, 3");

                Assert.AreEqual( 1, childEntity.Relations.Count, "1, childEntity.Relations" );
                Assert.AreEqual(1, parentEntity.Relations.Count, "1, parentEntity.Relations");
            } //else

            int coundFk = 0;
            if ( relationshipType == RelationshipType.Informative ){
                foreach (var atr in childEntity.Attributes)
                {
                    if (atr.Key == AttributeKeyType.IsPrimaryForeignKey || atr.Key == AttributeKeyType.IsForeignKey)
                    {
                        coundFk++;
                    } //if
                } //foreach
                Assert.IsTrue(coundFk == 0, "coundFk==0");
            } //if
            else{
                foreach ( var atr in childEntity.Attributes ){
                    if ( atr.Key == exceptedAttributeKeyType ){
                        coundFk++;
                    } //if
                } //foreach
                Assert.IsTrue(coundFk == 1, "coundFk==1");
            } //else

            watcher.Dispose();

            if (relationshipType == RelationshipType.Informative)
            {
                foreach (var atr in childEntity.Attributes)
                {
                    Assert.AreNotEqual(atr.Key, AttributeKeyType.IsPrimaryForeignKey,
                                        "atr.Key, AttributeKeyType.IsPrimaryForeignKey after dispose");

                    Assert.AreNotEqual(atr.Key, AttributeKeyType.IsForeignKey,
                                        "atr.Key, AttributeKeyType.IsForeignKey after dispose");
                } //foreach
            }
            else{
                foreach ( var atr in childEntity.Attributes ){
                    Assert.AreNotEqual( atr.Key, exceptedAttributeKeyType,
                                        "atr.Key, AttributeKeyType.IsForeignKey after dispose" );
                } //foreach
            } //else

            Assert.AreEqual( relation.ParentAttributes.Count, 0, "relation.ParentAttributes.Count == 0" );
            Assert.AreEqual( relation.ChildAttributes.Count, 0, "relation.ChildAttributes.Count == 0" );
        }

        [TestMethod]
        public void CompositeForeignKeyTest()
        {
            ERDEntity parentEntity;
            ERDEntity childEntity;

            ContainerWiring wiring = ContainerWiring.SetUp(new SqLiteModelInterception());

            var relation = GetTestEntities(wiring, out childEntity, out parentEntity);
            relation.Type = RelationshipType.NonIdentifying;
            var attribute = new EntityAttribute{
                                                   Caption =
                                                       new ItemName{Title = "Test Id2", Physical = "Id2"},
                                                   DataLenght = 0,
                                                   DbType = new SqLiteInteger(),
                                                   Key = AttributeKeyType.IsKey
                                               };
            
            parentEntity.Attributes.Add( attribute );
            
            var watcher = GetNewWatcher( relation );

            Assert.AreEqual( relation.ParentAttributes.Count, 2, "relation.ParentAttributes.Count == 2" );
            Assert.AreEqual( relation.ChildAttributes.Count, 2, "relation.ChildAttributes.Count == 2" );

            int coundFk = 0;

            foreach ( var atr in childEntity.Attributes ){
                if (atr.Key == AttributeKeyType.IsForeignKey)
                {
                    coundFk++;
                } //if
            } //foreach

            Assert.AreEqual( coundFk, 2, "coundFk==2" );

            parentEntity.Attributes.Remove( attribute );

            coundFk = 0;
            foreach ( var atr in childEntity.Attributes  ){
                if (atr.Key == AttributeKeyType.IsForeignKey )
                {
                    coundFk++;
                } //if
            } //foreach

            Assert.AreEqual( 1, coundFk, "coundFk==1" );

            Assert.AreEqual( relation.ParentAttributes.Count, 1, "relation.ParentAttributes.Count == 1" );
            Assert.AreEqual( relation.ChildAttributes.Count, 1, "relation.ChildAttributes.Count == 1" );

            attribute = wiring.CreateInstance<EntityAttribute>();
            attribute.Caption.Physical = "Id2";
            attribute.Caption.Title = "Titl";
            attribute.DbType = new SqLiteInteger();
            attribute.DataLenght = 1;
            attribute.Key = AttributeKeyType.None;

            //test add new not key attribute

            parentEntity.Attributes.Add( attribute );
            coundFk = 0;

            foreach ( var atr in childEntity.Attributes ){
                if (atr.Key == AttributeKeyType.IsForeignKey)
                {
                    coundFk++;
                } //if
            } //foreach

            Assert.AreEqual( 1, coundFk, "coundFk==1" );

            Assert.AreEqual( relation.ParentAttributes.Count, 1, "relation.ParentAttributes.Count == 1" );
            Assert.AreEqual( relation.ChildAttributes.Count, 1, "relation.ChildAttributes.Count == 1" );

            //test 

            attribute.Key = AttributeKeyType.IsKey;
            coundFk = 0;
            foreach ( var atr in childEntity.Attributes ){
                if (atr.Key == AttributeKeyType.IsForeignKey )
                {
                    coundFk++;
                } //if
            } //foreach

            Assert.AreEqual( 2, coundFk, "coundFk==1" );

            Assert.AreEqual( relation.ParentAttributes.Count, 2, "relation.ParentAttributes.Count == 1" );
            Assert.AreEqual( relation.ChildAttributes.Count, 2, "relation.ChildAttributes.Count == 1" );

            EntityAttribute childAttribute = null;

            foreach ( var atr in childEntity.Attributes ){
                if ( atr.Caption.Physical.Equals( "Id2" ) ){
                    childAttribute = atr;
                    break;
                } //if
            } //foreach

            Assert.IsNotNull( childAttribute );

            attribute.Caption.Physical = "Id23";

            Assert.AreEqual( attribute.Caption.Physical, attribute.Caption.Physical );

            watcher.Dispose();

            foreach ( var atr in childEntity.Attributes ){
                Assert.AreNotEqual( atr.Key, AttributeKeyType.IsForeignKey,
                                    "atr.Key, AttributeKeyType.IsForeignKey after dispose" );
                Assert.AreNotEqual(atr.Key, AttributeKeyType.IsPrimaryForeignKey,
                                    "atr.Key, AttributeKeyType.IsPrimaryForeignKey after dispose");
            } //foreach

            Assert.AreEqual( 0, relation.ParentAttributes.Count, "relation.ParentAttributes.Count == 0" );
            Assert.AreEqual( 0, relation.ChildAttributes.Count, "relation.ChildAttributes.Count == 0" );
        }

        [TestMethod]
        public void CompositeForeignKeyTest2()
        {
            ERDEntity parentEntity;
            ERDEntity childEntity;
            ContainerWiring wiring = ContainerWiring.SetUp( new SqLiteModelInterception() );

            var relation = GetTestEntities(wiring,out childEntity, out parentEntity);
            relation.Type = RelationshipType.NonIdentifying;
            var attribute = new EntityAttribute
            {
                Caption =
                    new ItemName { Title = "Test Id2", Physical = "Id2" },
                DataLenght = 0,
                DbType = new SqLiteInteger(),
                Key = AttributeKeyType.IsKey
            };

            parentEntity.Attributes.Add(attribute);

            relation.Parent = null;
            relation.Child = null;

            var watcher = GetNewWatcher(relation);

            relation.Parent = parentEntity;
            relation.Child = childEntity;

            Assert.AreEqual(relation.ParentAttributes.Count, 2, "relation.ParentAttributes.Count == 2");
            Assert.AreEqual(relation.ChildAttributes.Count, 2, "relation.ChildAttributes.Count == 2");

            int coundFk = 0;

            foreach (var atr in childEntity.Attributes)
            {
                if (atr.Key == AttributeKeyType.IsForeignKey)
                {
                    coundFk++;
                } //if
            } //foreach

            Assert.AreEqual(coundFk, 2, "coundFk==2");

            parentEntity.Attributes.Remove(attribute);

            coundFk = 0;
            foreach (var atr in childEntity.Attributes)
            {
                if (atr.Key == AttributeKeyType.IsForeignKey)
                {
                    coundFk++;
                } //if
            } //foreach

            Assert.AreEqual(1, coundFk, "coundFk==1");

            Assert.AreEqual(relation.ParentAttributes.Count, 1, "relation.ParentAttributes.Count == 1");
            Assert.AreEqual(relation.ChildAttributes.Count, 1, "relation.ChildAttributes.Count == 1");

            attribute = wiring.CreateInstance<EntityAttribute>();
            attribute.Caption.Physical = "Id2";
            attribute.Caption.Title = "Titl";
            attribute.DbType = new SqLiteInteger();
            attribute.DataLenght = 1;
            attribute.Key = AttributeKeyType.None;

            //test add new not key attribute

            parentEntity.Attributes.Add(attribute);
            coundFk = 0;

            foreach (var atr in childEntity.Attributes)
            {
                if (atr.Key == AttributeKeyType.IsForeignKey )
                {
                    coundFk++;
                } //if
            } //foreach

            Assert.AreEqual(1, coundFk, "coundFk==1");

            Assert.AreEqual(relation.ParentAttributes.Count, 1, "relation.ParentAttributes.Count == 1");
            Assert.AreEqual(relation.ChildAttributes.Count, 1, "relation.ChildAttributes.Count == 1");

            //test 

            attribute.Key = AttributeKeyType.IsKey;
            coundFk = 0;
            foreach (var atr in childEntity.Attributes)
            {
                if (atr.Key == AttributeKeyType.IsForeignKey)
                {
                    coundFk++;
                } //if
            } //foreach

            Assert.AreEqual(2, coundFk, "coundFk==1");

            Assert.AreEqual(relation.ParentAttributes.Count, 2, "relation.ParentAttributes.Count == 1");
            Assert.AreEqual(relation.ChildAttributes.Count, 2, "relation.ChildAttributes.Count == 1");

            EntityAttribute childAttribute = null;

            foreach (var atr in childEntity.Attributes)
            {
                if (atr.Caption.Physical.Equals("Id2"))
                {
                    childAttribute = atr;
                    break;
                } //if
            } //foreach

            Assert.IsNotNull(childAttribute);

            attribute.Caption.Physical = "Id23";

            Assert.AreEqual(attribute.Caption.Physical, attribute.Caption.Physical);

            watcher.Dispose();

            foreach (var atr in childEntity.Attributes)
            {
                Assert.AreNotEqual(atr.Key, AttributeKeyType.IsForeignKey,
                                    "atr.Key, AttributeKeyType.IsForeignKey after dispose");
            } //foreach

            Assert.AreEqual(0, relation.ParentAttributes.Count, "relation.ParentAttributes.Count == 0");
            Assert.AreEqual(0, relation.ChildAttributes.Count, "relation.ChildAttributes.Count == 0");
        }
    }
}