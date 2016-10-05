// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   SqLiteModelInterception.cs
//   * Project: SqLauncher.Web.Controller
//   * Description:
//   * Created at 2011 10 01 12:45
//   * Modified at: 2011  11 14  20:14
// / ******************************************************************************/ 

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

using SqLauncher.Web.Controller.XmlSerializes;
using SqLauncher.Web.Model;
using SqLauncher.Web.Model.SqLite;
using SqLauncher.Web.UI;
using SqLauncher.Web.UI.Model;
using SqLauncher.Web.UI.SqLite;

namespace SqLauncher.Web.Controller.DataModelInterceptions
{
    /// <summary>
    ///   The object interception for the SqLite database entities.
    /// </summary>
    public sealed class SqLiteModelInterception : IDatabaseModelInterception
    {
        /// <summary>
        ///   Initializes the sqLite types interception.
        /// </summary>
        /// <param name = "container">The unity container.</param>
        public void Initialize( IUnityContainer container )
        {
            container.AddNewExtension<Interception>();

            container.Configure<Interception>().
                SetInterceptorFor<ERDEntity>( new VirtualMethodInterceptor() ).
                SetInterceptorFor<EntityAttribute>( new VirtualMethodInterceptor() ).
                SetInterceptorFor<EntityIndex>( new VirtualMethodInterceptor() )
                .SetInterceptorFor<IndexAttribute>( new VirtualMethodInterceptor() )
                .SetInterceptorFor<ItemName>( new VirtualMethodInterceptor() )
                .SetInterceptorFor<Cardinality>( new VirtualMethodInterceptor() )
                .SetInterceptorFor<DataModel>( new VirtualMethodInterceptor() )
                .SetInterceptorFor<EntityRelation>( new VirtualMethodInterceptor() )
                .SetInterceptorFor<EntityViewState>( new VirtualMethodInterceptor() )
                .SetInterceptorFor<ModelViewState>( new VirtualMethodInterceptor() )
                .SetInterceptorFor<RelationViewState>( new VirtualMethodInterceptor() )
                .SetInterceptorFor<SqlGenerationFormViewState>(new VirtualMethodInterceptor())
                .SetInterceptorFor<DatabaseDocument>(new VirtualMethodInterceptor())
                .SetInterceptorFor<DatabaseVersion>(new VirtualMethodInterceptor())
                .SetInterceptorFor<SqLiteIteractionState>(new VirtualMethodInterceptor())
                .AddPolicy( "NotifyChanged" );

            container.RegisterType<DataModelGeneratorBase, SqLiteDataModelGenerator>();
            container.RegisterType<ISqlGenerationFormViewState, SqlGenerationFormViewState>();
            container.RegisterType<IModelViewState, ModelViewState>();
            container.RegisterType<IRelationViewState, RelationViewState>();
            container.RegisterType<IEntityViewState, EntityViewState>();
            container.RegisterType<IDbTypesMapper, SqLiteTypesMapper>();
            container.RegisterType<IDocumentXmlSerializer, SqLiteXmlSerializer>();

            //script generators
            container.RegisterType<ERDEntityGeneratorBase, SqLiteERDEntityGenerator>();
            container.RegisterType<EntityRelationGeneratorBase, SqLiteEntityRelationGenerator>();

            container.RegisterType<ERDEntityASCIIPainterBase, ERDEntityASCIIPainter>();

            //One instance of one model
            container.RegisterType<IIteractionState, SqLiteIteractionState>(new ContainerControlledLifetimeManager());
        }
    }
}