// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   UserIteractionProvider.cs
//   * Project: SqLauncher.Web.Controller
//   * Description:
//   * Created at 2011 09 28 8:18 PM
//   * Modified at: 2011  09 28  8:27 PM
// / ******************************************************************************/ 

using System;
using System.Windows;
using System.Windows.Media;

using SqLauncher.Web.Controller.RibbonIteraction;
using SqLauncher.Web.UI;

namespace SqLauncher.Web.Controller
{
    /// <summary>
    ///   The provider of user iteraction information.
    /// </summary>
    public class UserIteractionProvider: DependencyObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SqLauncher.Web.Controller.UserIteractionProvider"/> class.
        /// </summary>
        public UserIteractionProvider()
        {
            ZoomAbility = false;
            ZoomPercent = 100.0;
            CanRedo = false;
            CanUndo = false;
            VersionCreateDate = null;
            ModelSize = new ModelSize();
            IteractionState = new DefaultIteractionState();
            HasOpenDocuments = false;
            HasSelectedEntities = false;
        }

        /// <summary>
        ///   The static constructor.
        /// </summary>
        static UserIteractionProvider()
        {
            Default = new UserIteractionProvider();
        }

        /// <summary>
        ///   The default provider.
        /// </summary>
        public static readonly UserIteractionProvider Default;

        /// <summary>
        /// The global iteration provider.
        /// </summary>
        private static readonly GlobalIterationProvider _globalIterationProvider = new GlobalIterationProvider();

        /// <summary>
        /// The global iteration provider.
        /// </summary>
        public static GlobalIterationProvider GlobalIterationProvider
        {
            get { return _globalIterationProvider; }
        }

        /// <summary>
        /// Gets the global iteration provider.
        /// </summary>
        public GlobalIterationProvider Global { get { return _globalIterationProvider; } }

        #region Zoom 

        public static readonly DependencyProperty ZoomAbilityProperty =
            DependencyProperty.Register( "ZoomAbility", typeof ( bool ), typeof ( UserIteractionProvider ),
                                         new PropertyMetadata( default( bool ) ) );

        /// <summary>
        ///   Indicates a possibility of zooming.
        /// </summary>
        public bool ZoomAbility
        {
            get { return (bool) GetValue( ZoomAbilityProperty ); }
            set { SetValue( ZoomAbilityProperty, value ); }
        }


        public static readonly DependencyProperty ZoomPercentProperty =
            DependencyProperty.Register( "ZoomPercent", typeof ( double ), typeof ( UserIteractionProvider ),
                                         new PropertyMetadata( default( double ), ZoomChanged ) );

        private static void ZoomChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            var provider = d as UserIteractionProvider;

            if ( provider != null ){
                provider.RiseZoomPercentChanged();
            } //if
        }


        /// <summary>
        /// Occurs when zoom has been changed.
        /// </summary>
        public event EventHandler ZoomPercentChanged;

        /// <summary>
        /// The invocator of ZoomPercentChanged event.
        /// </summary>
        private void RiseZoomPercentChanged()
        {
            EventHandler handler = ZoomPercentChanged;
            if ( handler != null ){
                handler( this, new EventArgs() );
            }
        }

        /// <summary>
        /// The current zoom percent.
        /// </summary>
        public double ZoomPercent
        {
            get { return (double) GetValue( ZoomPercentProperty ); }
            set { SetValue( ZoomPercentProperty, value ); }
        }

        #endregion Zoom

        public static readonly DependencyProperty HasOpenDocumentsProperty =
            DependencyProperty.Register( "HasOpenDocuments", typeof ( bool ), typeof ( UserIteractionProvider ),
                                                        new PropertyMetadata( default( bool ) ) );

        /// <summary>
        /// Gets or sets the open documents existing.
        /// True means what user open one or more documents
        /// </summary>
        public bool HasOpenDocuments
        {
            get { return (bool)GetValue(HasOpenDocumentsProperty); }
            set { SetValue(HasOpenDocumentsProperty, value); }
        }

        public static readonly DependencyProperty HasSelectedEntitiesProperty =
            DependencyProperty.Register( "HasSelectedEntities", typeof ( bool ), typeof ( UserIteractionProvider ),
                                                        new PropertyMetadata( default( bool ) ) );

        /// <summary>
        /// Gets or sets entity selection state.
        /// </summary>
        public bool HasSelectedEntities
        {
            get { return (bool) GetValue( HasSelectedEntitiesProperty ); }
            set { SetValue( HasSelectedEntitiesProperty, value ); }
        }

        public static readonly DependencyProperty CanRedoProperty =
            DependencyProperty.Register( "CanRedo", typeof ( bool ), typeof ( UserIteractionProvider ),
                                                        new PropertyMetadata( default( bool ) ) );

        /// <summary>
        ///   Indicates a ability do "Redo" command.
        /// </summary>
        public bool CanRedo
        {
            get { return (bool) GetValue( CanRedoProperty ); }
            set
            {
                SetValue( CanRedoProperty, value );
            }
        }

        public static readonly DependencyProperty CanUndoProperty =
            DependencyProperty.Register( "CanUndo", typeof ( bool ), typeof ( UserIteractionProvider ),
                                                        new PropertyMetadata( default( bool ) ) );


        /// <summary>
        ///   Indicates a ability do "Undo" command.
        /// </summary>
        public bool CanUndo
        {
            get { return (bool) GetValue( CanUndoProperty ); }
            set
            {
                SetValue( CanUndoProperty, value );
            }
        }

        #region Brush 

        public static readonly DependencyProperty BrushProperty =
            DependencyProperty.Register( "Brush", typeof ( Brush ), typeof ( UserIteractionProvider ),
                                         new PropertyMetadata( default( Brush ), OnBrushChange ) );

        /// <summary>
        /// Occurs when brush prperty chas been changed.
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnBrushChange( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            var provider = (UserIteractionProvider) d;
            provider.RiseBrushChanged( (Brush) e.NewValue );
        }

        /// <summary>
        /// Occurs when user changed brush by ribbon.
        /// </summary>
        internal event EventHandler<BrushChangedEventArgs> BrushChanged;

        /// <summary>
        /// The invocator of BrushChanged event.
        /// </summary>
        /// <param name="brush"></param>
        private void RiseBrushChanged( Brush brush )
        {
            EventHandler<BrushChangedEventArgs> handler = BrushChanged;
            if ( handler != null ){
                handler( this, new BrushChangedEventArgs( brush ) );
            }
        }

        /// <summary>
        /// The choised brush.
        /// </summary>
        public Brush Brush
        {
            get { return (Brush) GetValue( BrushProperty ); }
            set { SetValue( BrushProperty, value ); }
        }

        #endregion Brush

        /// <summary>
        /// The size of current model.
        /// </summary>
        public ModelSize ModelSize { get; private set; }

        #region Version

        public static readonly DependencyProperty VersionCreateDateProperty =
            DependencyProperty.Register( "VersionCreateDate", typeof ( DateTime? ), typeof ( UserIteractionProvider ),
                                                        new PropertyMetadata( null ) );

        /// <summary>
        /// The creation date of current vertion.
        /// </summary>
        public DateTime? VersionCreateDate
        {
            get { return (DateTime) GetValue( VersionCreateDateProperty ); }
            set { SetValue( VersionCreateDateProperty, value ); }
        }

        public static readonly DependencyProperty VersionNumberProperty =
            DependencyProperty.Register( "VersionNumber", typeof ( int? ), typeof ( UserIteractionProvider ),
                                                        new PropertyMetadata( null ) );

        /// <summary>
        /// The current version number.
        /// </summary>
        public int? VersionNumber
        {
            get { return (int?) GetValue( VersionNumberProperty ); }
            set { SetValue( VersionNumberProperty, value ); }
        }

        #endregion

        #region View state persistent iteraction 

        /// <summary>
        /// The model iteraction state.
        /// </summary>
        public IIteractionState IteractionState { get; set; }

        #endregion View state persistent iteraction

    }
}