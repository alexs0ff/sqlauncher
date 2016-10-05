// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   RelationForm.xaml.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 08 30 11:34 PM
//   * Modified at: 2011  10 18  10:28 PM
// / ******************************************************************************/ 

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

using SqLauncher.Web.Model;
using SqLauncher.Web.UI.Common;
using SqLauncher.Web.UI.Model;

namespace SqLauncher.Web.UI
{
    /// <summary>
    ///   Represents a entity relation
    /// </summary>
    public partial class RelationForm : IRelationForm
    {
        /// <summary>
        ///   The distance between connector`s parts.
        /// </summary>
        public const double ConnectorPartsDistance = 10.0;

        public RelationForm()
        {
            InitializeComponent();
            InitLineStyle();
            InitDatshArrayForRelationTypes();

            LayoutUpdated += RelationFormFirstLayoutUpdated;
            Unloaded += RelationFormUnloaded;
        }

        #region Edit Form 

        /// <summary>
        ///   The edit element.
        /// </summary>
        private readonly RelationFormEdit _relationEdit = new RelationFormEdit();

        /// <summary>
        ///   The edit form.
        /// </summary>
        public RelationFormEdit RelationEdit
        {
            get { return _relationEdit; }
        }

        /// <summary>
        /// Showes the relation edit form .
        /// </summary>
        public void ShowEditForm()
        {
            relationSwitchEditBehavior.StartEdit( MiddlePointBetweenConnectPoints );
        }

        #endregion Edit Form

         /// <summary>
        /// Initializes instance of the form.
        /// Invokes when from has been added into the model view.
        /// </summary>
        public void Initialize()
        {
            UpdateLineStylesForRelationType();
        }

        /// <summary>
        /// Uninitializes the instance of the form.
        /// Invokes when from has been removed from the model view.
        /// </summary>
        public void Uninitialize()
        {
            
        }

        /// <summary>
        /// Occurs when layout updated.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        void RelationFormFirstLayoutUpdated(object sender, EventArgs e)
        {
            LayoutUpdated -= RelationFormFirstLayoutUpdated;

            IsLoaded = true;
            RiseElementLoaded();
        }

        /// <summary>
        ///   The start point of the line about parent canvas.
        ///   Before setting you must invoke Visual.TransformToVisual
        /// </summary>
        public RectConnector StartPoint
        {
            get { return ViewState.StartPoint; }
            set
            {
                ViewState.StartPoint = value;
                UpdateRelationPosition();
            }
        }

        /// <summary>
        ///   The destination point of the line about parent canvas.
        /// </summary>
        public RectConnector DestinationPoint
        {
            get { return ViewState.DestinationPoint; }
            set
            {
                ViewState.DestinationPoint = value;
                UpdateRelationPosition();
            }
        }

        #region Lines

        /// <summary>
        ///   The main line.
        /// </summary>
        private readonly Line _mainLine = new Line();

        /// <summary>
        ///   The connector.
        /// </summary>
        private readonly Line _childPerpendicularConnector = new Line();

        /// <summary>
        ///   The connector.
        /// </summary>
        private readonly Line _childConnector1 = new Line();

        /// <summary>
        ///   The connector.
        /// </summary>
        private readonly Line _childConnector2 = new Line();

        /// <summary>
        ///   The connector.
        /// </summary>
        private readonly Line _childParallelMark = new Line();

        /// <summary>
        ///   The connector.
        /// </summary>
        private readonly Line _parentParallelMark = new Line();

        /// <summary>
        ///   The connector.
        /// </summary>
        private readonly Line _parentPerpendicularConnector = new Line();

        /// <summary>
        ///   The name of the connector line style.
        /// </summary>
        private const string LineStyleName = "RelationLineStyle";

        /// <summary>
        ///   Initializes the all styles of line.
        /// </summary>
        private void InitLineStyle()
        {
            var reader = new XamlStyleReader();
            _mainLine.Style = reader.GetStyle(LineStyleName);
            _childPerpendicularConnector.Style = reader.GetStyle(LineStyleName);
            _childConnector1.Style = reader.GetStyle(LineStyleName);
            _childConnector2.Style = reader.GetStyle(LineStyleName);
            _childParallelMark.Style = reader.GetStyle(LineStyleName);
            _parentParallelMark.Style = reader.GetStyle(LineStyleName);
            _parentPerpendicularConnector.Style = reader.GetStyle(LineStyleName);
            relationView.Children.Add(_childPerpendicularConnector);
            relationView.Children.Add(_mainLine);
            relationView.Children.Add(_childConnector1);
            relationView.Children.Add(_childConnector2);
            relationView.Children.Add(_childParallelMark);
            relationView.Children.Add(_parentParallelMark);
            relationView.Children.Add(_parentPerpendicularConnector);
        }

        #endregion Lines

        #region Relation draw 

        /// <summary>
        /// NonIdentifying the main line style.
        /// </summary>
        private readonly DoubleCollection _nonIdentifyingDashArray = new DoubleCollection();

        /// <summary>
        /// Informative the main line style.
        /// </summary>
        private readonly DoubleCollection _informativeDashArray = new DoubleCollection();

        /// <summary>
        /// Init relation type styles dash arays.
        /// </summary>
        private void InitDatshArrayForRelationTypes()
        {
            _nonIdentifyingDashArray.Add( 8.0 );
            _nonIdentifyingDashArray.Add( 3.0 );

            _informativeDashArray.Add( 8.0 );
            _informativeDashArray.Add( 3.0 );
            _informativeDashArray.Add( 2.0 );
            _informativeDashArray.Add( 3.0 );
        }

        /// <summary>
        ///   Updates position of entity relation.
        /// </summary>
        private void UpdateRelationPosition()
        {
            if ( DestinationPoint == null || StartPoint == null ){
                return;
            }

            Point destinationPoint = UpdateDestinationLayout();
            _mainLine.X1 = destinationPoint.X;
            _mainLine.Y1 = destinationPoint.Y;
            Point startPoint = UpdateStartLayout();
            _mainLine.X2 = startPoint.X;
            _mainLine.Y2 = startPoint.Y;

            UpdateCaptionPosition();
            UpdateLegendPosition();
        }

        /// <summary>
        /// Updates the lines depend on different relation types.
        /// </summary>
        private void UpdateLineStylesForRelationType()
        {
            switch (CurrentRelationshipType)
            {
                case RelationshipType.Identifying:
                    _mainLine.StrokeDashArray = null;
                    _childParallelMark.Visibility = Visibility.Visible;
                    _parentParallelMark.Visibility = Visibility.Visible;
                    _childConnector1.Visibility = Visibility.Visible;
                    _childConnector2.Visibility = Visibility.Visible;
                    break;
                case RelationshipType.NonIdentifying:
                    _mainLine.StrokeDashArray = _nonIdentifyingDashArray;
                     _childParallelMark.Visibility = Visibility.Visible;
                    _parentParallelMark.Visibility = Visibility.Visible;
                    _childConnector1.Visibility = Visibility.Visible;
                    _childConnector2.Visibility = Visibility.Visible;
                    break;
                case RelationshipType.Informative:
                    _mainLine.StrokeDashArray = _informativeDashArray;
                    _childParallelMark.Visibility = Visibility.Collapsed;
                    _parentParallelMark.Visibility = Visibility.Collapsed;
                    _childConnector1.Visibility = Visibility.Collapsed;
                    _childConnector2.Visibility = Visibility.Collapsed;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            } //switch
        }

        /// <summary>
        ///   The destination point
        /// </summary>
        private Point _destinationPoint;


        /// <summary>
        ///   The destination point
        /// </summary>
        private Point _startPoint;

        /// <summary>
        ///   Updates position of the caption text.
        /// </summary>
        private void UpdateCaptionPosition()
        {
            if ( DestinationPoint == null || StartPoint == null ){
                return;
            } //if

            var x = ( _startPoint.X + _destinationPoint.X )/2;
            var y = ( _startPoint.Y + _destinationPoint.Y )/2;

            Canvas.SetLeft( captionBlock, x - captionBlock.ActualWidth/2 );
            Canvas.SetTop( captionBlock, y - captionBlock.ActualHeight );
        }

        /// <summary>
        /// Updates position for parent and child legends.
        /// </summary>
        private void UpdateLegendPosition()
        {
            if (DestinationPoint == null || StartPoint == null)
            {
                return;
            } //if

            var parentLegendFactors = AxesFactors.CreateLegendFactors( StartPoint.RectSide );

            var childLegendFactors = AxesFactors.CreateLegendFactors( DestinationPoint.RectSide );

            //parent
            Canvas.SetLeft( parentLegend,
                            _parentPerpendicularConnector.X1 + parentLegendFactors.X*ConnectorPartsDistance );
            Canvas.SetTop( parentLegend, _parentPerpendicularConnector.Y1 + parentLegendFactors.Y*ConnectorPartsDistance );

            //child
            if (DestinationPoint.RectSide == RectSide.Left){
                Canvas.SetLeft( childLegend,
                                _childPerpendicularConnector.X1 + childLegendFactors.X*ConnectorPartsDistance*2 );
                Canvas.SetTop(childLegend, _childPerpendicularConnector.Y1 + childLegendFactors.Y * ConnectorPartsDistance);    
            } //if
            else{
                Canvas.SetLeft(childLegend, _childPerpendicularConnector.X1 + childLegendFactors.X * ConnectorPartsDistance);
                Canvas.SetTop(childLegend, _childPerpendicularConnector.Y1 + childLegendFactors.Y * ConnectorPartsDistance);    
            } //else
        }

        /// <summary>
        ///   Updates a child`s connector
        /// </summary>
        /// <returns>The destination coordinates for the main line.</returns>
        private Point UpdateDestinationLayout()
        {
            var perpendicularFactors = AxesFactors.CreatePerpendicularFactors( DestinationPoint.RectSide );
            var connectPoint =
                _destinationPoint = CalcConnectorPoint( perpendicularFactors, DestinationPoint.MiddleSidePoint );
            var parallelFactors = AxesFactors.CreateParallelFactors( DestinationPoint.RectSide );

            _childConnector1.X1 = DestinationPoint.MiddleSidePoint.X + ( ConnectorPartsDistance*parallelFactors.X )/2;
            _childConnector1.Y1 = DestinationPoint.MiddleSidePoint.Y + ( ConnectorPartsDistance*parallelFactors.Y )/2;
            _childConnector1.X2 = connectPoint.X;
            _childConnector1.Y2 = connectPoint.Y;

            _childConnector2.X1 = DestinationPoint.MiddleSidePoint.X - ( ConnectorPartsDistance*parallelFactors.X )/2;
            _childConnector2.Y1 = DestinationPoint.MiddleSidePoint.Y - ( ConnectorPartsDistance*parallelFactors.Y )/2;
            _childConnector2.X2 = connectPoint.X;
            _childConnector2.Y2 = connectPoint.Y;

            _childPerpendicularConnector.X1 = connectPoint.X;
            _childPerpendicularConnector.Y1 = connectPoint.Y;
            _childPerpendicularConnector.X2 = DestinationPoint.MiddleSidePoint.X;
            _childPerpendicularConnector.Y2 = DestinationPoint.MiddleSidePoint.Y;

            DrawParallelMark( _childParallelMark, connectPoint, parallelFactors );

            return connectPoint;
        }

        /// <summary>
        /// Gets the destination connector point.
        /// </summary>
        public Point DestinationConnectPoint
        {
            get { return new Point( _childPerpendicularConnector.X1, _childPerpendicularConnector.Y1 ); }
        }

        /// <summary>
        ///   Draw parallel mark.
        /// </summary>
        /// <param name = "connector">The connector which will middle point of the mark.</param>
        /// <param name = "connectPoint">The connect point.</param>
        /// <param name = "parallelFactors">The current parrallel factors.</param>
        private void DrawParallelMark( Line connector, Point connectPoint, AxesFactors parallelFactors )
        {
            connector.X1 = connectPoint.X + ( ConnectorPartsDistance*parallelFactors.X )/2;
            connector.Y1 = connectPoint.Y + ( ConnectorPartsDistance*parallelFactors.Y )/2;

            connector.X2 = connectPoint.X - ( ConnectorPartsDistance*parallelFactors.X )/2;
            connector.Y2 = connectPoint.Y - ( ConnectorPartsDistance*parallelFactors.Y )/2;
        }

        /// <summary>
        ///   Updates layout the parent`s connector.
        /// </summary>
        /// <returns>The start coordinates for the main line.</returns>
        private Point UpdateStartLayout()
        {
            var perpendicularFactors = AxesFactors.CreatePerpendicularFactors( StartPoint.RectSide );
            var parallelFactors = AxesFactors.CreateParallelFactors( StartPoint.RectSide );

            var connectPoint = _startPoint = CalcConnectorPoint( perpendicularFactors, StartPoint.MiddleSidePoint );
            _parentPerpendicularConnector.X1 = connectPoint.X;
            _parentPerpendicularConnector.Y1 = connectPoint.Y;
            _parentPerpendicularConnector.X2 = StartPoint.MiddleSidePoint.X;
            _parentPerpendicularConnector.Y2 = StartPoint.MiddleSidePoint.Y;

            DrawParallelMark( _parentParallelMark, connectPoint, parallelFactors );
            return connectPoint;
        }

        /// <summary>
        /// Gets the start connector point.
        /// </summary>
        public Point StartConnectPoint
        {
            get { return new Point( _parentPerpendicularConnector.X1, _parentPerpendicularConnector.Y1 ); }
        }

        /// <summary>
        /// Gets the middle point between conectors.
        /// </summary>
        public Point MiddlePointBetweenConnectPoints
        {
            get
            {
                return new Point( ( StartConnectPoint.X + DestinationConnectPoint.X )/2,
                                  ( StartConnectPoint.Y + DestinationConnectPoint.Y )/2 );
            }
        }

        /// <summary>
        ///   Calculates a connector`s point.
        /// </summary>
        /// <param name = "factors">The perpendicular factors.</param>
        /// <param name = "middleSidePoint">The point of middle side.</param>
        /// <returns>Connector`s point.</returns>
        private Point CalcConnectorPoint( AxesFactors factors, Point middleSidePoint )
        {
            return new Point( ConnectorPartsDistance*factors.X + middleSidePoint.X,
                              ConnectorPartsDistance*factors.Y + middleSidePoint.Y );
        }

        /// <summary>
        ///   The helper for factors to calculate perpendicular coordinates.
        /// </summary>
        private class AxesFactors
        {
            /// <summary>
            ///   Initializes a new instance of the AxesFactors class.
            /// </summary>
            public AxesFactors( int x, int y )
            {
                X = x;
                Y = y;
            }

            /// <summary>
            ///   Initializes a new instance for a Perpendicular values.
            /// </summary>
            public static AxesFactors CreatePerpendicularFactors( RectSide rectSide )
            {
                AxesFactors result = null;
                switch ( rectSide ){
                    case RectSide.Left:
                        result = new AxesFactors( -1, 0 );
                        break;
                    case RectSide.Top:
                        result = new AxesFactors( 0, -1 );
                        break;
                    case RectSide.Right:
                        result = new AxesFactors( 1, 0 );
                        break;
                    case RectSide.Bottom:
                        result = new AxesFactors( 0, 1 );
                        break;
                }

                return result;
            }

            /// <summary>
            ///   Initializes a new instance for a parallel values.
            /// </summary>
            public static AxesFactors CreateParallelFactors( RectSide rectSide )
            {
                AxesFactors result = null;
                switch ( rectSide ){
                    case RectSide.Left:
                        result = new AxesFactors( 0, -1 );
                        break;
                    case RectSide.Top:
                        result = new AxesFactors( 1, 0 );
                        break;
                    case RectSide.Right:
                        result = new AxesFactors( 0, 1 );
                        break;
                    case RectSide.Bottom:
                        result = new AxesFactors( -1, 0 );
                        break;
                }

                return result;
            }

            /// <summary>
            /// Initializes a new instance for a legend values.
            /// </summary>
            /// <param name="rectSide">The rect side</param>
            /// <returns></returns>
            public static AxesFactors CreateLegendFactors(RectSide rectSide)
            {
                AxesFactors result = null;
                switch (rectSide)
                {
                    case RectSide.Left:
                        result = new AxesFactors(-1, 1);
                        break;
                    case RectSide.Top:
                        result = new AxesFactors(1, -1);
                        break;
                    case RectSide.Right:
                        result = new AxesFactors(0, 1);
                        break;
                    case RectSide.Bottom:
                        result = new AxesFactors( 1, 0 );
                        break;
                }

                return result;
            }

            /// <summary>
            ///   The X axes factor.
            /// </summary>
            public int X { get; private set; }

            /// <summary>
            ///   The Y axes factor.
            /// </summary>
            public int Y { get; private set; }
        }

        #endregion Relation draw
        
        #region Canvas Position

        /// <summary>
        ///   Returns the left position of the entity form.
        /// </summary>
        /// <returns>The position.</returns>
        public double GetLeft()
        {
            return Canvas.GetLeft( this );
        }

        /// <summary>
        ///   Sets the top position of the entity form on view model.
        /// </summary>
        /// <param name = "y">The top position</param>
        public void SetTop( double y )
        {
            Canvas.SetTop( this, y );
        }

        /// <summary>
        ///   Sets the left position of the entity form on view model.
        /// </summary>
        /// <param name = "x">The left position</param>
        public void SetLeft( double x )
        {
            Canvas.SetLeft( this, x );
        }

        /// <summary>
        ///   Returns the Z index of the entity form.
        /// </summary>
        /// <returns></returns>
        public int GetZIndex()
        {
            UIElement currentForm = this;

            if (RelationEdit.Visibility == Visibility.Visible)
            {
                currentForm = RelationEdit;
            } //if

            return Canvas.GetZIndex(currentForm);
        }

        /// <summary>
        ///   Returns the top position of the entity form.
        /// </summary>
        /// <returns></returns>
        public double GetTop()
        {
            return Canvas.GetTop( this );
        }

        #endregion Canvas Position

        /// <summary>
        ///   Checks what form has the point.
        /// </summary>
        /// <param name = "pointToCheck">Point to check.</param>
        /// <returns>True if it belongs to otherwise false</returns>
        public bool HitTest( Point pointToCheck )
        {
            var parent = ControlHelper.FindParent<Canvas>(this);
            GeneralTransform gt = parent.TransformToVisual( Application.Current.RootVisual );

            Point point = gt.Transform( pointToCheck );

            var elements = VisualTreeHelper.FindElementsInHostCoordinates(point, this);

            bool result = elements.GetEnumerator().MoveNext();

            if ( !result ){
                elements = VisualTreeHelper.FindElementsInHostCoordinates( point, RelationEdit );
                result = elements.GetEnumerator().MoveNext();
            } //if

            return result;
        }

        #region Selection 

        /// <summary>
        ///   Occurs when the form will be selected or unselected.
        /// </summary>
        public event EventHandler<SelectionStateChangedEventArgs> SelectionStateChanged
        {
            add { selectRelationFormBehavior.SelectionStateChanged += value; }
            remove { selectRelationFormBehavior.SelectionStateChanged -= value; }
        }

        /// <summary>
        ///   The flag which indicates that a form has been selected.
        /// </summary>
        public bool IsSelected
        {
            get { return selectRelationFormBehavior.SelectionIsVisible; }
            set
            {
                selectRelationFormBehavior.SelectionIsVisible = value;
            }
        }

        #endregion Selection

        #region Loaded 

        /// <summary>
        ///   Occurs when from has been loaded.
        /// </summary>
        public event EventHandler<ElementLoadedEventArgs> ElementLoaded;

        /// <summary>
        ///   The invocator of ElementLoaded event.
        /// </summary>
        private void RiseElementLoaded()
        {
            EventHandler<ElementLoadedEventArgs> handler = ElementLoaded;
            if ( handler != null ){
                handler( this, new ElementLoadedEventArgs() );
            }
        }

        /// <summary>
        /// Gets the information about loaded event has been rised.
        /// </summary>
        public bool IsLoaded { get; private set; }
      

        /// <summary>
        ///   Occurs when relation form unloaded.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The event args.</param>
        private void RelationFormUnloaded(object sender, RoutedEventArgs e)
        {
            IsLoaded = false;
        }

        #endregion Loaded

        /// <summary>
        ///   Sets or gets visibility property.
        /// </summary>
        public bool IsVisible
        {
            get { return Visibility == Visibility.Visible; }
            set { Visibility = value ? Visibility.Visible : Visibility.Collapsed; }
        }

        #region Data 

        /// <summary>
        /// The inner data context for improving performance operations.
        /// </summary>
        private RelationViewState _relationFormDataContext;

        /// <summary>
        ///   The data context.
        /// </summary>
        public IRelationViewState DataEntity
        {
            get
            {
                return _relationFormDataContext;
            }
            set
            {
                if ( DataEntity!=null ){
                    UnSubscribeForDataEntityPropetyChanges( DataEntity );
                } //if

                DataContext = value;
                _relationFormDataContext = (RelationViewState)value;

                if ( value!=null ){
                    SubscribeForDataEntityPropetyChanges(value);    
                } //if
            }
        }

        /// <summary>
        /// Unsubscribes the events.
        /// </summary>
        /// <param name="dataEntity">The source of the property change events</param>
        private void UnSubscribeForDataEntityPropetyChanges( IRelationViewState dataEntity )
        {
            if ( dataEntity.Relation!=null ){
                dataEntity.Relation.PropertyChanged -= EntityRelationPropertyChanged;
            } //if
        }

        /// <summary>
        /// Subscribes for the change events.
        /// </summary>
        /// <param name="dataEntity">The source of the property change events</param>
        private void SubscribeForDataEntityPropetyChanges(IRelationViewState dataEntity)
        {
            if ( dataEntity.Relation != null ){
                dataEntity.Relation.PropertyChanged += EntityRelationPropertyChanged;
            } //if
        }

        /// <summary>
        /// Occurs when EntityRelation property changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        void EntityRelationPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch ( e.PropertyName ){
                case  "Type":
                    UpdateLineStylesForRelationType();
                    UpdateRelationPosition();
                    break;
            } //switch
        }

        /// <summary>
        ///   The associated view state.
        /// </summary>
        public RelationViewState ViewState
        {
            get { return _relationFormDataContext; }
        }

        /// <summary>
        /// Gets the data context RelationshipType.
        /// <remarks>If data context is null will return RelationshipType.Identifying type.</remarks>
        /// </summary>
        private RelationshipType CurrentRelationshipType
        {
            get
            {
                var result = RelationshipType.Identifying;

                if ( DataEntity!=null ){
                    result = DataEntity.Relation.Type;
                } //if

                return result;
            }
        }

        #endregion Data
    }
}