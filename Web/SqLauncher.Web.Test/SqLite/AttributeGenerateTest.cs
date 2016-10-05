// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   AttributeGenerateTest.cs
//   * Project: SqLauncher.Web.Test
//   * Description:
//   * Created at 2011 11 15 20:41
//   * Modified at: 2011  11 15  20:43
// / ******************************************************************************/ 

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SqLauncher.Web.Model;
using SqLauncher.Web.Model.SqLite;

namespace SqLauncher.Web.Test2.SqLite
{
    [TestClass]
    public class AttributeGenerateTest
    {
        /// <summary>
        /// Tests the int type.
        /// </summary>
        [TestMethod]
        public void TestIntType()
        {
            var attribute = new EntityAttribute();

            attribute.DbType = new SqLiteInteger();
            attribute.DataLenght = 55;
            attribute.Caption = new ItemName();
            attribute.Caption.Physical = "Id";
            attribute.Caption.Title = "Code";

            var generator = new SqLiteEntityAttributeGenerator();

            var ddl = generator.GenerateSql( attribute );
            Assert.AreEqual( "Id Integer", ddl );

            attribute.Caption.Physical = "Id1";
            ddl = generator.GenerateSql( attribute );
            Assert.AreEqual("Id1 Integer", ddl);

            attribute.IsIdentity = true;
            ddl = generator.GenerateSql(attribute);
            Assert.AreEqual("Id1 Integer", ddl);

            attribute.IsIdentity = false;
            attribute.Key = AttributeKeyType.IsKey;
            ddl = generator.GenerateSql( attribute );
            Assert.AreEqual("Id1 Integer PRIMARY KEY", ddl);

            attribute.IsIdentity = true;
            ddl = generator.GenerateSql( attribute );
            Assert.AreNotEqual( "Id1 Integer PRIMARY KEY", ddl );
            Assert.AreEqual("Id1 Integer PRIMARY KEY AUTOINCREMENT", ddl);

            attribute.Key = AttributeKeyType.None;

            attribute.IsNotNull = true;
            ddl = generator.GenerateSql( attribute );
            Assert.AreEqual( "Id1 Integer NOT NULL", ddl );

            attribute.IsNotNull = false;
            attribute.IsUnique = true;
            ddl = generator.GenerateSql( attribute );
            Assert.AreEqual( "Id1 Integer UNIQUE", ddl );

            attribute.Key = AttributeKeyType.IsKey;
            attribute.IsNotNull = true;
            attribute.IsUnique = true;
            attribute.IsIdentity = true;
            attribute.Caption.Physical = "Id";
            ddl = generator.GenerateSql( attribute );
            Assert.AreEqual("Id Integer PRIMARY KEY AUTOINCREMENT UNIQUE NOT NULL", ddl);
        }
        
        /// <summary>
        /// Tests the Blob type.
        /// </summary>
        [TestMethod]
        public void TestBlobType()
        {
            var generator = new SqLiteEntityAttributeGenerator();
            var attribute = new EntityAttribute();

            attribute.DbType = new SqLiteBlob();
            attribute.DataLenght = 66;
            attribute.Caption = new ItemName();
            attribute.Caption.Physical = "Data";
            attribute.Caption.Title = "Code";
            var ddl = generator.GenerateSql(attribute);
            Assert.AreEqual( "Data Blob(66)", ddl );

            attribute.Key = AttributeKeyType.IsKey;
            attribute.IsNotNull = true;
            attribute.IsUnique = true;
            attribute.IsIdentity = true;
            attribute.Caption.Physical = "Data1";
            ddl = generator.GenerateSql(attribute);
            Assert.AreEqual("Data1 Blob(66) PRIMARY KEY AUTOINCREMENT UNIQUE NOT NULL", ddl);
        }
    }
}