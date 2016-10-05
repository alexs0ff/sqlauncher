// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   EntityRelationGeneratorTest.cs
//   * Project: SqLauncher.Web.Test
//   * Description:
//   * Created at 2011 11 17 20:29
//   * Modified at: 2011  11 17  20:42
// / ******************************************************************************/ 


using Microsoft.VisualStudio.TestTools.UnitTesting;

using SqLauncher.Web.Model;
using SqLauncher.Web.Model.SqLite;

namespace SqLauncher.Web.Test2.SqLite
{
    [TestClass]
    public class EntityRelationGenerateTest
    {
        public string SimpleForeignKeySql = "FOREIGN KEY (ArtistId) REFERENCES Artist(Id)";

        public string ForeignKeySql = "FOREIGN KEY (ArtistId,ArtistName) REFERENCES Artist(Id,Name)";

        [TestMethod]
        public void SimpleRelationTest()
        {
            ERDEntity childEntity = new ERDEntity();
            childEntity.Caption = new ItemName();
            childEntity.Caption.Physical = "Track";


            var childAttribute = new EntityAttribute();
            childAttribute.DbType = new SqLiteInteger();
            childAttribute.DataLenght = 55;
            childAttribute.Caption = new ItemName();
            childAttribute.Caption.Physical = "ArtistId";
            childAttribute.Caption.Title = "FK";
            childEntity.Attributes.Add( childAttribute );

            var parentEntity = new ERDEntity();
            parentEntity.Caption = new ItemName();
            parentEntity.Caption.Physical = "Artist";

            var parentAttribute = new EntityAttribute();
            parentAttribute.DbType = new SqLiteInteger();
            parentAttribute.DataLenght = 55;
            parentAttribute.Caption = new ItemName();
            parentAttribute.Caption.Physical = "Id";
            parentAttribute.Caption.Title = "Code";
            parentEntity.Attributes.Add( parentAttribute );

            EntityRelation relation = new EntityRelation();
            relation.Parent = parentEntity;
            relation.Child = childEntity;
            relation.ParentAttributes.Add( parentAttribute );
            relation.ChildAttributes.Add( childAttribute );

            SqLiteEntityRelationGenerator relationGenerator = new SqLiteEntityRelationGenerator();
            var ddl = relationGenerator.GenerateSql( relation );

            Assert.AreEqual( ddl, SimpleForeignKeySql );
        }

        [TestMethod]
        public void RelationTest()
        {
            EntityRelation relation = new EntityRelation();

            ERDEntity childEntity = new ERDEntity();
            childEntity.Caption = new ItemName();
            childEntity.Caption.Physical = "Track";


            var childAttribute = new EntityAttribute();
            childAttribute.DbType = new SqLiteInteger();
            childAttribute.DataLenght = 55;
            childAttribute.Caption = new ItemName();
            childAttribute.Caption.Physical = "ArtistId";
            childAttribute.Caption.Title = "FK";

            childEntity.Attributes.Add(childAttribute);
            relation.ChildAttributes.Add(childAttribute);

            childAttribute = new EntityAttribute();
            childAttribute.Caption = new ItemName();
            childAttribute.Caption.Physical = "ArtistName";
            childAttribute.Caption.Title = "FK1";
            childEntity.Attributes.Add(childAttribute);

            var parentEntity = new ERDEntity();
            parentEntity.Caption = new ItemName();
            parentEntity.Caption.Physical = "Artist";

            var parentAttribute = new EntityAttribute();
            parentAttribute.DbType = new SqLiteInteger();
            parentAttribute.DataLenght = 55;
            parentAttribute.Caption = new ItemName();
            parentAttribute.Caption.Physical = "Id";
            parentAttribute.Caption.Title = "Code";
            parentEntity.Attributes.Add(parentAttribute);
            relation.ParentAttributes.Add(parentAttribute);
            parentAttribute = new EntityAttribute();
            parentAttribute.DbType = new SqLiteInteger();
            parentAttribute.DataLenght = 55;
            parentAttribute.Caption = new ItemName();
            parentAttribute.Caption.Physical = "Name";
            parentAttribute.Caption.Title = "FirstName";
            parentEntity.Attributes.Add(parentAttribute);

            
            relation.Parent = parentEntity;
            relation.Child = childEntity;
            relation.ParentAttributes.Add(parentAttribute);
            relation.ChildAttributes.Add(childAttribute);

            SqLiteEntityRelationGenerator relationGenerator = new SqLiteEntityRelationGenerator();
            var ddl = relationGenerator.GenerateSql(relation);

            Assert.AreEqual(ddl, ForeignKeySql);
        }
    }
}