using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SqLauncher.Web.Model;
using SqLauncher.Web.Model.SqLite;
using SqLauncher.Web.Test2;

namespace SqLauncher.Web.Test.SqLite
{
    [TestClass]
    public class ASCIIPainterTest
    {
        [TestMethod]
        public void EmptyEntityTest()
        {
            var painter = new ERDEntityASCIIPainter();

            var entity = new ERDEntity();
            entity.Caption = new ItemName();
            entity.Caption.Physical = "Cert";
            entity.Caption.Title = "Тетс";

            var ascii = painter.GenerateText( entity );

            var reader = new ResourceReader("ASCIIEmptyTable.txt");
            string data = reader.Read();

            Assert.AreEqual( data, ascii );
        }

        [TestMethod]
        public void EntityTest1()
        {
            var erdEntity = new ERDEntity { Caption = new ItemName { Physical = "Cert", Title = "Cert Object" } };
            erdEntity.Attributes.Add(new EntityAttribute
            {
                Caption =
                    new ItemName { Title = "Object Id", Physical = "CertId" },
                DataLenght = 0,
                DbType = new SqLiteInteger(),
                Key = AttributeKeyType.None
            });
            erdEntity.Attributes.Add(new EntityAttribute
            {
                Caption =
                    new ItemName { Title = "Object Name", Physical = "Name" },
                DataLenght = 10,
                DbType = new SqLiteText(),
                Key = AttributeKeyType.None
            });

            var painter = new ERDEntityASCIIPainter();
            var ascii = painter.GenerateText(erdEntity);

            var reader = new ResourceReader("ASCIITable1.txt");
            string data = reader.Read();

            Assert.AreEqual(data, ascii);
        }

        [TestMethod]
        public void EntityTest2()
        {
            var erdEntity = new ERDEntity { Caption = new ItemName { Physical = "Cert", Title = "Cert Object" } };
            erdEntity.Attributes.Add(new EntityAttribute
            {
                Caption =
                    new ItemName { Title = "Object Id", Physical = "CertId" },
                DataLenght = 0,
                DbType = new SqLiteInteger(),
                Key = AttributeKeyType.IsKey
            });
            erdEntity.Attributes.Add(new EntityAttribute
            {
                Caption =
                    new ItemName { Title = "City Id", Physical = "CityId" },
                DataLenght = 0,
                DbType = new SqLiteInteger(),
                Key = AttributeKeyType.IsForeignKey
            });
            erdEntity.Attributes.Add(new EntityAttribute
            {
                Caption =
                    new ItemName { Title = "Object Name", Physical = "Name" },
                DataLenght = 10,
                DbType = new SqLiteText(),
                Key = AttributeKeyType.None
            });

            var painter = new ERDEntityASCIIPainter();
            var ascii = painter.GenerateText(erdEntity);

            var reader = new ResourceReader("ASCIITable2.txt");
            string data = reader.Read();

            Assert.AreEqual(data, ascii);
        }

        [TestMethod]
        public void EntityTest3()
        {
            var erdEntity = new ERDEntity { Caption = new ItemName { Physical = "Cert", Title = "Cert Object" } };
            erdEntity.Attributes.Add(new EntityAttribute
            {
                Caption =
                    new ItemName { Title = "Object Id", Physical = "CertId" },
                DataLenght = 0,
                DbType = new SqLiteInteger(),
                Key = AttributeKeyType.IsKey,
                IsIdentity = true,
                IsNotNull = true,
            });
            
            erdEntity.Attributes.Add(new EntityAttribute
            {
                Caption =
                    new ItemName { Title = "Object Name", Physical = "Name" },
                DataLenght = 10,
                DbType = new SqLiteText(),
                Key = AttributeKeyType.None,
                IsNotNull = true,
                IsUnique = true
            });

            var painter = new ERDEntityASCIIPainter();
            var ascii = painter.GenerateText(erdEntity);

            var reader = new ResourceReader("ASCIITable3.txt");
            string data = reader.Read();

            Assert.AreEqual(data, ascii);
        }
    }
}
