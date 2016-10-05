// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   ERDEntityGenerateTest.cs
//   * Project: SqLauncher.Web.Test
//   * Description:
//   * Created at 2011 11 21 19:22
//   * Modified at: 2011  11 21  21:10
// / ******************************************************************************/ 

using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SqLauncher.Web.Model;
using SqLauncher.Web.Model.SqLite;
using SqLauncher.Web.Test2;
using SqLauncher.Web.Test2.SqLite;

namespace SqLauncher.Web.Test.SqLite
{
    [TestClass]
    public class ERDEntityGenerateTest
    {
        [TestMethod]
        public void CreateSimpleTable()
        {
            var entityGenerator = new SqLiteERDEntityGenerator();

            var entity = new ERDEntity();
            entity.Caption = new ItemName();
            entity.Caption.Physical = "Cert";
            entity.Caption.Title = "Тетс";

            entity.Attributes.Add( new EntityAttribute
                                   {Caption = new ItemName{Physical = "Id"}, DataLenght = 55, DbType = new SqLiteText()} );

            var ddl = entityGenerator.GenerateSql( entity );
            var reader = new ResourceReader( "SimpleTable.txt" );
            string sql = reader.Read();
            Assert.AreEqual( sql, ddl );
        }

        [TestMethod]
        public void CreateTable()
        {
            var entityGenerator = new SqLiteERDEntityGenerator();

            var entity = new ERDEntity();
            entity.Caption = new ItemName();
            entity.Caption.Physical = "Cert2";
            entity.Caption.Title = "Етс";

            entity.Attributes.Add( new EntityAttribute{
                                                          Caption = new ItemName{Physical = "Id"},
                                                          DataLenght = 55,
                                                          DbType = new SqLiteInteger(),
                                                          IsIdentity = true,
                                                          Key = AttributeKeyType.IsKey,
                                                          IsNotNull = true
                                                      } );
            entity.Attributes.Add( new EntityAttribute{
                                                          Caption = new ItemName{Physical = "Name"},
                                                          DataLenght = 55,
                                                          DbType = new SqLiteText(),
                                                          IsIdentity = false,
                                                          Key = AttributeKeyType.None,
                                                          IsNotNull = true,
                                                          IsUnique = true
                                                      } );
            entity.Attributes.Add( new EntityAttribute{
                                                          Caption = new ItemName{Physical = "Data"},
                                                          DataLenght = 55,
                                                          DbType = new SqLiteBlob(),
                                                          IsIdentity = false,
                                                          Key = AttributeKeyType.None,
                                                          IsNotNull = true,
                                                          IsUnique = false
                                                      } );

            var ddl = entityGenerator.GenerateSql( entity );

            var reader = new ResourceReader( "Table1.txt" );
            string sql = reader.Read();

            Assert.AreEqual( sql, ddl );
        }

        [TestMethod]
        public void CreateIndexedTable()
        {
            var entityGenerator = new SqLiteERDEntityGenerator();

            var entity = new ERDEntity();
            entity.Caption = new ItemName();
            entity.Caption.Physical = "Cert2";
            entity.Caption.Title = "Етс";

            entity.Attributes.Add(new EntityAttribute
            {
                Caption = new ItemName { Physical = "Id" },
                DataLenght = 55,
                DbType = new SqLiteInteger(),
                IsIdentity = true,
                Key = AttributeKeyType.IsKey,
                IsNotNull = true
            });
            entity.Attributes.Add(new EntityAttribute
            {
                Caption = new ItemName { Physical = "Name" },
                DataLenght = 55,
                DbType = new SqLiteText(),
                IsIdentity = false,
                Key = AttributeKeyType.None,
                IsNotNull = true,
                IsUnique = true
            });
            entity.Attributes.Add(new EntityAttribute
            {
                Caption = new ItemName { Physical = "Data" },
                DataLenght = 55,
                DbType = new SqLiteBlob(),
                IsIdentity = false,
                Key = AttributeKeyType.None,
                IsNotNull = true,
                IsUnique = false
            });

            var index = new EntityIndex{Caption = new ItemName(){Physical = "IndexName"}, IsUnique = true};
            index.Attributes.Add( new IndexAttribute{Attribute = entity.Attributes.ToList()[1],Order = SortOrder.Descending} );
            index.Parent = entity;
            entity.Indexes.Add( index );
            var ddl = entityGenerator.GenerateSql(entity);

            var reader = new ResourceReader("IndexedTable.txt");
            string sql = reader.Read();

            Assert.AreEqual(sql, ddl);
        }

        private static EntityRelation GetTestEntities( out ERDEntity childERDEntity, out ERDEntity parentERDEntity )
        {
            parentERDEntity = new ERDEntity{Caption = new ItemName{Physical = "Item", Title = "Item"}};
            parentERDEntity.Attributes.Add( new EntityAttribute{
                                                                   Caption =
                                                                       new ItemName
                                                                       {Title = "Test Id", Physical = "ItemId"},
                                                                   DataLenght = 0,
                                                                   DbType = new SqLiteInteger(),
                                                                   Key = AttributeKeyType.IsKey
                                                               } );

            childERDEntity = new ERDEntity{Caption = new ItemName{Physical = "Cert", Title = "Cert Object"}};
            childERDEntity.Attributes.Add( new EntityAttribute{
                                                                  Caption =
                                                                      new ItemName
                                                                      {Title = "Object Id", Physical = "CertId"},
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

            EntityRelation relation = new EntityRelation{Caption = new ItemName{Physical = "tets"}};

            relation.Parent = parentERDEntity;
            relation.Child = childERDEntity;
            return relation;
        }

        [TestMethod]
        public void SimpleFKTableTest()
        {
            var entityGenerator = new SqLiteERDEntityGenerator();

            ERDEntity parentEntity;
            ERDEntity childEntity;

            var relation = GetTestEntities( out childEntity, out parentEntity );

            EntityRelationWatcherTest.GetNewWatcher( relation );
            var ddl = entityGenerator.GenerateSql( childEntity );
            var reader = new ResourceReader( "SimpleFKTable.txt" );
            string sql = reader.Read();

            Assert.AreEqual( sql, ddl );
        }

        [TestMethod]
        public void FKTableTest()
        {
            var entityGenerator = new SqLiteERDEntityGenerator();

            ERDEntity parentEntity;
            ERDEntity parentEntity2;
            ERDEntity childEntity;

            var relation = GetTestEntities( out childEntity, out parentEntity );
            parentEntity2 = new ERDEntity{Caption = new ItemName{Physical = "Fast", Title = "Fast Object"}};
            parentEntity2.Attributes.Add( new EntityAttribute{
                                                                 Caption =
                                                                     new ItemName
                                                                     {Title = "Fast Id", Physical = "FastId"},
                                                                 DataLenght = 0,
                                                                 DbType = new SqLiteInteger(),
                                                                 Key = AttributeKeyType.IsKey
                                                             } );

            EntityRelation relation2 = new EntityRelation{Caption = new ItemName{Physical = "tets"}};

            relation2.Parent = parentEntity2;
            relation2.Child = childEntity;
           
            EntityRelationWatcherTest.GetNewWatcher( relation );
            EntityRelationWatcherTest.GetNewWatcher( relation2 );

            relation.Parent.Attributes.Add( new EntityAttribute{
                                                                   Caption = new ItemName{Physical = "ItemName"},
                                                                   DataLenght = 10,
                                                                   DbType = new SqLiteText(),
                                                                   Key = AttributeKeyType.IsKey
                                                               } );

            var ddl = entityGenerator.GenerateSql( childEntity );

            var reader = new ResourceReader( "FKTable.txt" );
            string sql = reader.Read();

            Assert.AreEqual( sql, ddl );
        }
    }
}