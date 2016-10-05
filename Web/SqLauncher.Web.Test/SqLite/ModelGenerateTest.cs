// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   ModelGenerateTest.cs
//   * Project: SqLauncher.Web.Test
//   * Description:
//   * Created at 2011 11 22 20:36
//   * Modified at: 2011  12 16  21:50
// / ******************************************************************************/ 

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SqLauncher.Web.Controller.DataModelInterceptions;
using SqLauncher.Web.Model;
using SqLauncher.Web.Model.SqLite;

namespace SqLauncher.Web.Test2.SqLite
{
    [TestClass]
    public class ModelGenerateTest
    {
        [TestMethod]
        public void Model()
        {
            ContainerWiring wiring = ContainerWiring.SetUp( new SqLiteModelInterception() );

            var model = wiring.CreateInstance<DataModel>();

            var cityTable = wiring.CreateInstance<ERDEntity>();
            cityTable.Caption.Physical = "City";
            model.Entities.Add( cityTable );
            cityTable.Attributes.Add( new EntityAttribute{
                                                             Caption = new ItemName{Physical = "CityId"},
                                                             DbType = new SqLiteInteger(),
                                                             Key = AttributeKeyType.IsKey,
                                                             IsIdentity = true
                                                         } );

            cityTable.Attributes.Add( new EntityAttribute{
                                                             Caption = new ItemName{Physical = "Name"},
                                                             DbType = new SqLiteText(),
                                                             DataLenght = 50
                                                         } );

            var managerTable = wiring.CreateInstance<ERDEntity>();

            managerTable.Caption.Physical = "Manager";
            model.Entities.Add( managerTable );
            managerTable.Attributes.Add( new EntityAttribute{
                                                                Caption = new ItemName{Physical = "ManagerId"},
                                                                DbType = new SqLiteInteger(),
                                                                Key = AttributeKeyType.IsKey,
                                                                IsIdentity = true
                                                            } );

            managerTable.Attributes.Add( new EntityAttribute{
                                                                Caption = new ItemName{Physical = "FullName"},
                                                                DbType = new SqLiteText(),
                                                                DataLenght = 125
                                                            } );

            var managerCityRelation = wiring.CreateInstance<EntityRelation>();
            managerCityRelation.Caption.Physical = "ManagerCityRelation";
            model.Relations.Add( managerCityRelation );

            managerCityRelation.Child = managerTable;
            managerCityRelation.Parent = cityTable;

            //EntityRelationWatcherTest.GetNewWatcher( managerCityRelation );

            var employeeTable = wiring.CreateInstance<ERDEntity>();
            employeeTable.Caption.Physical = "Employee";
            model.Entities.Add( employeeTable );
            employeeTable.Attributes.Add( new EntityAttribute{
                                                                 Caption = new ItemName{Physical = "EmployeeId"},
                                                                 DbType = new SqLiteInteger(),
                                                                 Key = AttributeKeyType.IsKey,
                                                                 IsIdentity = true
                                                             } );

            var employeeCityRelation = wiring.CreateInstance<EntityRelation>();
            employeeCityRelation.Caption.Physical = "EmployeeCityRelation";
            model.Relations.Add( employeeCityRelation );
            employeeCityRelation.Child = employeeTable;
            employeeCityRelation.Parent = cityTable;

            employeeTable.Attributes.Add( new EntityAttribute{
                                                                 Caption = new ItemName{Physical = "FullName"},
                                                                 DbType = new SqLiteText(),
                                                                 DataLenght = 125
                                                             } );

            var managerEmployeeMapTable = wiring.CreateInstance<ERDEntity>();
            managerEmployeeMapTable.Caption.Physical = "ManagerEmployeeMap";
            model.Entities.Add( managerEmployeeMapTable );
            managerEmployeeMapTable.Attributes.Add( new EntityAttribute{
                                                                           Caption =
                                                                               new ItemName
                                                                               {Physical = "ManagerEmployeeMapId"},
                                                                           DbType = new SqLiteInteger(),
                                                                           Key = AttributeKeyType.IsKey,
                                                                           IsIdentity = true
                                                                       } );

            var managerRelation = wiring.CreateInstance<EntityRelation>();
            managerRelation.Caption.Physical = "ManagerRelation";
            model.Relations.Add( managerRelation );
            managerRelation.Child = managerEmployeeMapTable;
            managerRelation.Parent = managerTable;

            var employeeRelation = wiring.CreateInstance<EntityRelation>();
            employeeRelation.Caption.Physical = "EmployeeRelation";
            model.Relations.Add( employeeRelation );
            employeeRelation.Child = managerEmployeeMapTable;
            employeeRelation.Parent = employeeTable;

            var generator = new SqLiteDataModelGenerator();
            var ddl = generator.Generate( model );
            var reader = new ResourceReader( "Model1.txt" );
            string sql = reader.Read();
            Assert.AreEqual( sql, ddl );
        }
    }
}