// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   EntityFromPlaceHandler.cs
//   * Project: SqLauncher.Web.Controller
//   * Description:
//   * Created at 2011 09 05 2:53 PM
//   * Modified at: 2011  09 08  9:11 PM
// / ******************************************************************************/ 

using System.Windows;

using SqLauncher.Web.Controller.Commands;
using SqLauncher.Web.UI.Model;

namespace SqLauncher.Web.Controller.PlaceHandlers
{
    /// <summary>
    ///   Reprsenets the handler of new form adding.
    /// </summary>
    internal class EntityFromPlaceHandler : IPlaceHandler
    {
        /// <summary>
        ///   The handled form.
        /// </summary>
        public IEntityForm EntityForm { get; set; }

        /// <summary>
        /// The model manager.
        /// </summary>
        public ModelViewManager ModelViewManager { get; set; }

        private void CanvasMouseMove(object sender, ScaledMouseMoveEventArgs e)
        {
            EntityForm.SetLeft( e.Position.X - EntityForm.CurrentWidth/2 );
            EntityForm.SetTop( e.Position.Y - EntityForm.CurrentHeight/2 );
        }

        /// <summary>
        ///   Starts finding a new place.
        /// </summary>
        public void StartAssignNewPlace()
        {
            EntityForm.ElementLoaded += EntityFormLoadedToAssignPlace;
            ModelViewManager.ModelView.AddChild(EntityForm);
        }

        /// <summary>
        ///   Occures when new form has been load.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The event args.</param>
        private void EntityFormLoadedToAssignPlace(object sender, ElementLoadedEventArgs e)
        {
            EntityForm.ElementLoaded -= EntityFormLoadedToAssignPlace;
            ModelViewManager.ModelView.ModelMouseMove += CanvasMouseMove;
            EntityForm.MouseButtonDown += MouseLeftButtonDown;
        }

        private void MouseLeftButtonDown( object sender, MouseButtonDownEventArgs e )
        {
            CreateAndExecuteAssignMethod();
        }

        /// <summary>
        /// Creates a new AddNewERDEntity command and executes it.
        /// </summary>
        private void CreateAndExecuteAssignMethod()
        {
            var form = EntityForm;
            CleanUp();

            var point = new Point( form.GetLeft(), form.GetTop() );

            var command = new AddNewERDEntity();
            command.Controller = ModelViewManager.Controller;
            form.DataEntity.Location = point;
            command.DataModel = ModelViewManager.ModelView.DataEntity.DataModel;
            command.Done = false;
            command.Entity = form.DataEntity.Entity;
            command.EntityForm = form.DataEntity;
            ModelViewManager.Controller.ExecCommand( command );
        }

        /// <summary>
        ///   Stops all operation.
        /// </summary>
        public void Stop()
        {
            CleanUp();
        }

        private void CleanUp()
        {
            ModelViewManager.ModelView.ModelMouseMove -= CanvasMouseMove;
            if ( EntityForm != null ){
                ModelViewManager.ModelView.RemoveChild(EntityForm);
                EntityForm.MouseButtonDown -= MouseLeftButtonDown;
                EntityForm.ElementLoaded -= EntityFormLoadedToAssignPlace;
                EntityForm = null;
            }
            
        }
    }
}