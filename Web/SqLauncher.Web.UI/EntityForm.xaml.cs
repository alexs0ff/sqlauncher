// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   EntityForm.xaml.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 08 17 9:30 PM
//   * Modified at: 2011  09 13  8:31 PM
// / ******************************************************************************/ 

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using SqLauncher.Web.Model;
using SqLauncher.Web.UI.Common;
using SqLauncher.Web.UI.Model;

namespace SqLauncher.Web.UI
{
    public partial class EntityForm : IEntityForm
    {
        /// <summary>
        ///   Default constructor.
        /// </summary>
        public EntityForm()
        {
            InitializeComponent();

            _entityFormEdit = new EntityFormEdit(this);
            _entityFormEdit.Visibility = Visibility.Collapsed;

            SizeChanged += EntityFormSizeChanged;
            _viewFormVisibilityNotifier = new PropertyChangeNotifier( EntityView, "Visibility" );
            _viewFormVisibilityNotifier.ValueChanged += ViewFormVisibilityNotifierValueChanged;
            Unloaded += EntityFormUnloaded;
            MouseLeftButtonDown += EntityFormMouseLeftButtonDown;
            Loaded += EntityFormLoaded;
        }

        private void EntityFormLoaded(object sender, RoutedEventArgs e)
        {
            IsLoaded = true;
            _parentCanvas = ControlHelper.FindParent<Canvas>(this);
            _parentCanvas.Children.Add(_entityFormEdit);

            _canvasLeftChangeNotifier = new PropertyChangeNotifier(this, "(Canvas.Left)");
            _canvasLeftChangeNotifier.ValueChanged += EntityFormLeftValueChanged;
            _canvasTopChangeNotifier = new PropertyChangeNotifier(this, "(Canvas.Top)");
            _canvasTopChangeNotifier.ValueChanged += EntityFormTopValueChanged;
            
            RiseElementLoaded();
        }

        #region Loaded event 

        private void EntityFormUnloaded( object sender, RoutedEventArgs e )
        {
            IsLoaded = false;
            _parentCanvas.Children.Remove( _entityFormEdit );

            _canvasLeftChangeNotifier.ValueChanged -= EntityFormLeftValueChanged;
            _canvasLeftChangeNotifier.Dispose();
            _canvasTopChangeNotifier.ValueChanged -= EntityFormTopValueChanged;
            _canvasTopChangeNotifier.Dispose();
        }

        /// <summary>
        /// The parent canvas;
        /// </summary>
        private Canvas _parentCanvas;
        
        /// <summary>
        ///   Occurs when entity from has been loaded.
        /// </summary>
        public event EventHandler<ElementLoadedEventArgs> ElementLoaded;

        /// <summary>
        ///   The invocator of ElementLoaded event.
        /// </summary>
        public void RiseElementLoaded()
        {
            var handler = ElementLoaded;
            if ( handler != null ){
                handler( this, new ElementLoadedEventArgs() );
            }
        }

        /// <summary>
        /// Gets the information about loaded event has been rised.
        /// </summary>
        public bool IsLoaded { get; private set; }

        #endregion Loaded event

        /// <summary>
        ///   Returns a view form
        /// </summary>
        /// <returns>The view form instance.</returns>
        public FrameworkElement GetViewForm()
        {
            return EntityView;
        }

        #region Edit Form 

        /// <summary>
        /// The entity form edit.
        /// </summary>
        private readonly EntityFormEdit _entityFormEdit;

        /// <summary>
        ///   Returns a edit form
        /// TODO: make internal
        /// </summary>
        /// <returns>The edit form instance.</returns>
        public EntityFormEdit GetEditForm()
        {
            return _entityFormEdit;
        }

        #endregion Edit Form

        /// <summary>
        /// Initializes instance of the form.
        /// Invokes when from has been added into the model view.
        /// </summary>
        public void Initialize()
        {
            
        }

        /// <summary>
        /// Uninitializes the instance of the form.
        /// Invokes when from has been removed from the model view.
        /// </summary>
        public void Uninitialize()
        {
            IsSelected = false;
            IsLoaded = false;
        }

        #region Size Changed

        /// <summary>
        ///   Accures when SizeChanged event rise.
        /// </summary>
        /// <param name = "sender"></param>
        /// <param name = "e"></param>
        private void EntityFormSizeChanged( object sender, SizeChangedEventArgs e )
        {
            ProcessEntityFormPositionChanging();
            ActialSize = e.NewSize;
        }

        /// <summary>
        ///   The dependency property of ActualSize field.
        /// </summary>
        public static readonly DependencyProperty ActialSizeProperty =
            DependencyProperty.Register( "ActialSize", typeof ( Size ), typeof ( EntityForm ),
                                         new PropertyMetadata( default( Size ) ) );

        /// <summary>
        ///   Represents the actual size of form.
        /// </summary>
        public Size ActialSize
        {
            get { return (Size) GetValue( ActialSizeProperty ); }
            set { SetValue( ActialSizeProperty, value ); }
        }

        #endregion

        #region Visibility changed 

        private void ViewFormVisibilityNotifierValueChanged( object sender, EventArgs e )
        {
            RiseViewFormVisibilityChanged( EntityView.Visibility == Visibility.Visible );
        }

        /// <summary>
        ///   The notifier of viewform visibility change.
        /// </summary>
        private readonly PropertyChangeNotifier _viewFormVisibilityNotifier;

        /// <summary>
        ///   Occurs when changed visibility of ViewForm.
        /// </summary>
        public event EventHandler<VisibilityChangedEventArgs> ViewFormVisibilityChanged;

        /// <summary>
        ///   Invocator of ViewFormVisibilityChanged event.
        /// </summary>
        /// <param name = "isVisible">Visibility flag</param>
        public void RiseViewFormVisibilityChanged( bool isVisible )
        {
            var e = new VisibilityChangedEventArgs( isVisible );
            EventHandler<VisibilityChangedEventArgs> handler = ViewFormVisibilityChanged;
            if ( handler != null ){
                handler( this, e );
            }
        }

        #endregion Visibility changed

        #region Position

        /// <summary>
        ///   Occurs when entity form Canvas.Top property changed.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The event args.</param>
        private void EntityFormTopValueChanged( object sender, EventArgs e )
        {
            ProcessEntityFormPositionChanging();
        }

        /// <summary>
        ///   Occurs when entity form Canvas.Left property changed.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The event args.</param>
        private void EntityFormLeftValueChanged( object sender, EventArgs e )
        {
            ProcessEntityFormPositionChanging();
        }

        /// <summary>
        ///   Performs a operation of position cahnged events.
        /// </summary>
        internal void ProcessEntityFormPositionChanging()
        {
            var position = new Point( Canvas.GetLeft( this ), Canvas.GetTop( this ) );
            ViewState.Location = position;

            RisePositionChanging( position );
        }

        /// <summary>
        ///   The canvas.Left property listener.
        /// </summary>
        private PropertyChangeNotifier _canvasLeftChangeNotifier;

        /// <summary>
        ///   The Canvas.Top property listener.
        /// </summary>
        private PropertyChangeNotifier _canvasTopChangeNotifier;

        /// <summary>
        ///   Occurs when the entity changing self position.
        /// </summary>
        public event EventHandler<PositionChangingEventArgs> PositionChanging;

        /// <summary>
        ///   The invocator of position changng event.
        /// </summary>
        /// <param name = "position">The current position.</param>
        public void RisePositionChanging( Point position )
        {
            EventHandler<PositionChangingEventArgs> handler = PositionChanging;
            if ( handler != null ){
                handler( this, new PositionChangingEventArgs( position ) );
            }
        }

        /// <summary>
        ///   Ocures when Entity From changed its location.
        /// </summary>
        public event EventHandler<PositionChangedEventArgs> PositionChanged;

        /// <summary>
        ///   The invocator of PositionChanged event.
        /// </summary>
        /// <param name = "oldPosition">The old coordinates.</param>
        /// <param name = "newPosition">The new coordinates.</param>
        private void RisePositionChanged( Point oldPosition, Point newPosition )
        {
            EventHandler<PositionChangedEventArgs> handler = PositionChanged;
            if ( handler != null ){
                handler( this, new PositionChangedEventArgs{NewPosition = newPosition, OldPosition = oldPosition} );
            }
        }

        #endregion Position

        #region Mouse button down 

        private void EntityFormMouseLeftButtonDown( object sender, MouseButtonEventArgs e )
        {
            RiseMouseButtonDown( MouseButton.Left, e.ClickCount, e.GetPosition( this ) );
        }

        /// <summary>
        ///   Occurs when a mouse button down.
        /// </summary>
        public event EventHandler<MouseButtonDownEventArgs> MouseButtonDown;

        /// <summary>
        ///   The invocator of MouseButtonDown event.
        /// </summary>
        /// <param name = "button">The pressed button.</param>
        /// <param name = "clickCount">The click count.</param>
        /// <param name="position">The position.</param>
        public void RiseMouseButtonDown( MouseButton button, int clickCount, Point position )
        {
            EventHandler<MouseButtonDownEventArgs> handler = MouseButtonDown;
            if ( handler != null ){
                handler( this, new MouseButtonDownEventArgs( button, clickCount, position ) );
            }
        }

        #endregion Mouse button down

        #region Selection 

        /// <summary>
        ///   Occurs when the form will be selected.
        /// </summary>
        public event EventHandler<SelectionStateChangedEventArgs> SelectionStateChanged
        {
            add { selectedEntityFormBehavior.SelectionStateChanged += value; }
            remove { selectedEntityFormBehavior.SelectionStateChanged -= value; }
        }

        /// <summary>
        ///   The flag which indicates that entity form has been selected.
        /// </summary>
        public bool IsSelected
        {
            get { return selectedEntityFormBehavior.SelectionIsVisible; }
            set
            {
                //selection visible only when EntityView active
                selectedEntityFormBehavior.SelectionIsVisible = !DataEntity.IsEditing && value;
            }
        }
       
        #endregion Selection

        #region Resize 

        /// <summary>
        /// Resizes the entity form without EntitySizeChanged event rise.
        /// </summary>
        /// <param name="rect">The new size rect.</param>
        public void ForceResize( Rect rect )
        {
            selectedEntityFormBehavior.ForceResize( rect );
        }

        /// <summary>
        /// Occurs when size of the current entity form has been changed.
        /// </summary>
        public event EventHandler<EntitySizeChangedEventArgs> EntitySizeChanged
        {
            add { selectedEntityFormBehavior.SizeChanged += value; }
            remove { selectedEntityFormBehavior.SizeChanged -= value; }
        }

        #endregion Resize

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

            if ( DataEntity.IsEditing ){
                currentForm = GetEditForm();
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
        /// Starts the swivel behavior.
        /// </summary>
        internal void StartShowViewForm()
        {
            SwivelFormBehavior.Start();
        }

        /// <summary>
        ///   The actual Width
        /// </summary>
        public double CurrentWidth
        {
            get
            {
                if (double.IsNaN(EntityView.Width)){
                    return ActualWidth;
                }
                return EntityView.Width;
            }
        }

        /// <summary>
        ///   The Actual Height of the element.
        /// </summary>
        public double CurrentHeight
        {
            get
            {
                if (double.IsNaN(EntityView.Height))
                {
                    return ActualHeight;
                }
                return EntityView.Height;
            }
        }

        /// <summary>
        /// Checks what form has the point.
        /// We suppose that was passed point about parent canvas axis!
        /// </summary>
        /// <param name="pointToCheck">Point to check.</param>
        /// <returns>True if it belongs to otherwise false</returns>
        public bool HitTest( Point pointToCheck )
        {
            var parent = ControlHelper.FindParent<Canvas>( this );
            var gt = parent.TransformToVisual( Application.Current.RootVisual );
            var point = gt.Transform( pointToCheck );

            IEnumerable<UIElement> elements;

            if ( DataEntity.IsEditing ){
                elements = VisualTreeHelper.FindElementsInHostCoordinates(point, GetEditForm());
            } //if
            else{
                elements = VisualTreeHelper.FindElementsInHostCoordinates(point, this);
            } //else

            return elements.GetEnumerator().MoveNext(); 
        }

        #region Data handling 

        /// <summary>
        /// The inner data context for improving performance operations.
        /// </summary>
        private EntityViewState _entityFormDataContext;

        /// <summary>
        ///   The data context.
        /// </summary>
        public IEntityViewState DataEntity
        {
            get
            {
                return _entityFormDataContext;
            }
            set
            {
                DataContext = value;
                _entityFormDataContext = (EntityViewState)value;
            }
        }

        /// <summary>
        /// The accses for view state.
        /// </summary>
        internal EntityViewState ViewState
        {
            get { return _entityFormDataContext; }
        }

        /// <summary>
        ///   Occurs when we need to add an entity attribute.
        /// </summary>
        public event EventHandler<AddEntityAttributeEventArgs> AddEntityAttribute;

        /// <summary>
        ///   The invocator of AddEntityAttribute event.
        /// </summary>
        internal void RiseAddEntityAttribute()
        {
            EventHandler<AddEntityAttributeEventArgs> handler = AddEntityAttribute;
            if ( handler != null ){
                handler( this, new AddEntityAttributeEventArgs() );
            }
        }

        /// <summary>
        ///   Occurs when we need to delete an entity attribute.
        /// </summary>
        public event EventHandler<DeleteEntityAttributeEventArgs> DeleteEntityAttribute;

        /// <summary>
        ///   The invocator of DeleteEntityAttribute event.
        /// </summary>
        /// <param name = "entityAttribute">The entity attribute to delete.</param>
        internal void RiseDeleteEntityAttribute( EntityAttribute entityAttribute )
        {
            EventHandler<DeleteEntityAttributeEventArgs> handler = DeleteEntityAttribute;
            if ( handler != null ){
                handler( this, new DeleteEntityAttributeEventArgs( entityAttribute ) );
            }
        }

        /// <summary>
        /// Occurs when we need to add an entity index.
        /// </summary>
        public event EventHandler<AddEntityIndexEventArgs> AddEntityIndex;

        /// <summary>
        /// Rises the AddEntityIndex event.
        /// </summary>
        internal void RiseAddEntityIndex()
        {
            EventHandler<AddEntityIndexEventArgs> handler = AddEntityIndex;
            if ( handler != null ){
                handler( this, new AddEntityIndexEventArgs() );
            }
        }

        /// <summary>
        ///   Occurs when we need to delete an entity attribute.
        /// </summary>
        public event EventHandler<DeleteEntityIndexEventArgs> DeleteEntityIndex;

        /// <summary>
        /// Rises the DeleteEntityIndex event.
        /// </summary>
        /// <param name="index">The index.</param>
        internal void RiseDeleteEntityIndex(EntityIndex index)
        {
            EventHandler<DeleteEntityIndexEventArgs> handler = DeleteEntityIndex;
            if ( handler != null ){
                handler( this, new DeleteEntityIndexEventArgs( index ) );
            }
        }

        /// <summary>
        /// Occurs when we need to add an index attribute.
        /// </summary>
        public event EventHandler<AddIndexAttributeEventArgs> AddIndexAttribute;

        /// <summary>
        /// The invovator of the AddIndexAttribute event.
        /// </summary>
        /// <param name="entityAttribute">The entity attribute.</param>
        /// <param name="entityIndex">The entity index.</param>
        internal void RiseAddIndexAttribute(EntityAttribute entityAttribute, EntityIndex entityIndex)
        {
            var handler = AddIndexAttribute;
            if ( handler != null ){
                handler( this, new AddIndexAttributeEventArgs( entityAttribute, entityIndex ) );
            }
        }

        /// <summary>
        /// Occurs when we need to delete an index attribute.
        /// </summary>
        public event EventHandler<DeleteIndexAttributeEventArgs> DeleteIndexAttribute;

        /// <summary>
        /// The invocator of the DeleteIndexAttribute event.
        /// </summary>
        /// <param name="indexAttribute">The index attribute.</param>
        /// <param name="entityIndex">The index attribute.</param>
        internal void RiseDeleteIndexAttribute(IndexAttribute indexAttribute, EntityIndex entityIndex)
        {
            EventHandler<DeleteIndexAttributeEventArgs> handler = DeleteIndexAttribute;
            if ( handler != null ){
                handler( this, new DeleteIndexAttributeEventArgs( indexAttribute, entityIndex ) );
            }
        }

        /// <summary>
        /// Occurs when user want to reorder some attribute.
        /// </summary>
        public event EventHandler<EntityAttributeReorderingEventArgs> EntityAttributeReordering;

        /// <summary>
        /// Rises the EntityAttributeReordering event.
        /// </summary>
        /// <param name="attribute">The attribute to reordering.</param>
        /// <param name="currentIndex">The index of attribute.</param>
        /// <param name="newIndex">The new index of attribute.</param>
        internal void RiseEntityAttributeReordering( EntityAttribute attribute,int currentIndex, int newIndex )
        {
            EventHandler<EntityAttributeReorderingEventArgs> handler = EntityAttributeReordering;
            if ( handler != null ){
                handler( this, new EntityAttributeReorderingEventArgs( attribute, currentIndex, newIndex ) );
            }
        }

        #endregion Data handling
    }
}