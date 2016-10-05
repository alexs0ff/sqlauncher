// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   IndexGenerateTest.cs
//   * Project: SqLauncher.Web.Test
//   * Description:
//   * Created at 2012 02 24 20:47
//   * Modified at: 2012  02 24  21:13
// / ******************************************************************************/ 

using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SqLauncher.Web.Model;
using SqLauncher.Web.Model.SqLite;

namespace SqLauncher.Web.Test.SqLite
{
    [TestClass]
    public class IndexGenerateTest
    {
        [TestMethod]
        public void EmptyIndexTest()
        {
            var indexGenerator = new SqLiteIndexGenerator();
            var index = new EntityIndex();
            index.Caption = new ItemName{Physical = "IndexTest", Title = "rr22"};
            index.IsUnique = true;
            var ddl = indexGenerator.GenerateSql( index );
            Assert.AreEqual( string.Empty, ddl );
        }

        [TestMethod]
        public void IndexOneTest()
        {
            const string ddl1 = "CREATE UNIQUE INDEX IndexTest ON Entity(Id)";

            const string ddl2 = "CREATE INDEX IndexTest ON Entity(Id)";

            const string ddl3 = "CREATE INDEX IndexTest ON Entity(Id desc)";

            var indexGenerator = new SqLiteIndexGenerator();

            var entity = new ERDEntity();
            entity.Caption = new ItemName{Physical = "Entity", Title = "en2"};
            var index = new EntityIndex();
            index.Caption = new ItemName{Physical = "IndexTest", Title = "rr22"};
            index.IsUnique = true;
            var attribute = new EntityAttribute();
            attribute.Caption = new ItemName{Physical = "Id", Title = "Id2"};
            attribute.DbType = new SqLiteText();
            attribute.DataLenght = 55;

            entity.Attributes.Add( attribute );

            index.Parent = entity;
            index.Attributes.Add( new IndexAttribute{Attribute = attribute, Order = SortOrder.Ascending} );


            var ddl = indexGenerator.GenerateSql( index );
            Assert.AreEqual( ddl1, ddl );

            index.IsUnique = false;

            ddl = indexGenerator.GenerateSql( index );
            Assert.AreEqual( ddl2, ddl );

            index.Attributes.ToList()[0].Order = SortOrder.Descending;
            ddl = indexGenerator.GenerateSql( index );

            Assert.AreEqual( ddl3, ddl );
        }

        [TestMethod]
        public void IndexManyTest()
        {
            var indexGenerator = new SqLiteIndexGenerator();
            const string ddl1 = "CREATE UNIQUE INDEX IndexManyTest ON Entity2(Id desc, Name desc)";

            const string ddl2 = "CREATE INDEX IndexManyTest ON Entity2(Id desc, Name)";

            var entity = new ERDEntity();
            entity.Caption = new ItemName{Physical = "Entity2", Title = "en1"};

            var index = new EntityIndex();
            index.Caption = new ItemName{Physical = "IndexManyTest", Title = "rr"};
            index.IsUnique = true;

            var attribute1 = new EntityAttribute();
            attribute1.Caption = new ItemName{Physical = "Id", Title = "Id2"};
            attribute1.DbType = new SqLiteText();
            attribute1.DataLenght = 55;

            var attribute2 = new EntityAttribute();
            attribute2.Caption = new ItemName{Physical = "Name", Title = "Name test"};
            attribute2.DbType = new SqLiteInteger();
            attribute2.DataLenght = 55;

            entity.Indexes.Add( index );
            entity.Attributes.Add( attribute1 );
            entity.Attributes.Add( attribute2 );

            index.Attributes.Add( new IndexAttribute{Attribute = attribute1, Order = SortOrder.Descending} );
            index.Attributes.Add( new IndexAttribute{Attribute = attribute2, Order = SortOrder.Descending} );
            index.Parent = entity;

            var ddl = indexGenerator.GenerateSql( index );
            Assert.AreEqual( ddl1, ddl );

            index.IsUnique = false;
            index.Attributes.ToList()[1].Order = SortOrder.Ascending;
            ddl = indexGenerator.GenerateSql( index );
            Assert.AreEqual( ddl2, ddl );
        }
    }
}