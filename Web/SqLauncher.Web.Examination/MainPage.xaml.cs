// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   MainPage.xaml.cs
//   * Project: SqLauncher.Web.Examination
//   * Description:
//   * Created at 2011 08 16 8:27 PM
//   * Modified at: 2011  08 21  12:00 PM
// / ******************************************************************************/ 

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using Microsoft.Practices.Unity;

using SqLauncher.Web.Controller;
using SqLauncher.Web.Model;
using SqLauncher.Web.UI;
using SqLauncher.Web.UI.Common;

namespace SqLauncher.Web.Examination
{
    public partial class MainPage : UserControl
    {
       
        public MainPage()
        {
            InitializeComponent();
            //ContainerWiring.Instance.SetUp();
            //ContainerWiring.Instance.Container.RegisterType<IModelView, ModelView>();
            //modelView.DataEntity = ContainerWiring.Instance.CreateInstance<DataModel>();
            //ModelController.Controller.ModelViewManager = new ModelViewManager( modelView );
            var testData = new TestBindable();
            testData.TestItems = new Collection<TestItem>();
            testData.TestItems.Add( new TestItem{Age = 44,Name = "Aaa1"} );
            testData.TestItems.Add( new TestItem{Age = 12,Name = "bbb1"} );
            testData.TestItems.Add( new TestItem{Age = 95,Name = "rsad"} );
            testDataGrid.DataContext = testData;
        }

        private void Button1Click( object sender, RoutedEventArgs e )
        {
            var testData = (TestBindable)testDataGrid.DataContext;

            //testData.TestItems = new ObservableCollection<TestItem>();
            testData.TestItems.Add( new TestItem{Age = 15, Name = "Changed!"} );
            testData.TestItems.Add( new TestItem{Age = 85,Name = "Changed!!!"} );


            //ModelController.Controller.CreateAndPlaceEntityForm();

            /*var entity2 = ModelController.Controller.AddEntityForm();
            entity2.DataEntity.Caption.Title = "Тест2";
            entity2.DataEntity.Attributes.Add(new EntityAttribute());
            var relation = ModelController.Controller.AddRelationForm();
            relation.DataEntity.Child = entity1.DataEntity;
            relation.DataEntity.Parent = entity2.DataEntity;
             * */
            //zoomSlider1.Zoom += 10;
            //test.Test += 10;
        }

        private void button1_Click_1(object sender, RoutedEventArgs e)
        {
            var testData = (TestBindable)testDataGrid.DataContext;
            //testData.RisePropertyChanged("TestItems");
            var selectedIndex = testDataGrid.SelectedIndex;
            testDataGrid.Rebind( DataGrid.ItemsSourceProperty );
            testDataGrid.SelectedIndex = selectedIndex;

        }

        private void ButtonRedoClick( object sender, RoutedEventArgs e )
        {
            //ModelController.Controller.Redo();
        }

        private void ButtonUndoClick( object sender, RoutedEventArgs e )
        {
           // ModelController.Controller.Undo();
        }


       
    }
}