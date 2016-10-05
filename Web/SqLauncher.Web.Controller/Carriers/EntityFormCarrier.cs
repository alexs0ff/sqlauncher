// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   EntityFormCarrier.cs
//   * Project: SqLauncher.Web.Controller
//   * Description:
//   * Created at 2011 11 21 19:22
//   * Modified at: 2012  02 16  23:00
// / ******************************************************************************/ 

using System;
using System.Collections.Generic;
using System.Windows;

using SqLauncher.Web.Controller.Commands;
using SqLauncher.Web.UI.Model;

namespace SqLauncher.Web.Controller.Carriers
{
    /// <summary>
    ///   The carrier for entity forms moving.
    /// </summary>
    public class EntityFormCarrier : Carrier
    {
        /// <summary>
        ///   Indicates that the moving was happend.
        /// </summary>
        private bool _hasMove;

        /// <summary>
        ///   The entity forms that will be moved.
        /// </summary>
        private readonly IEnumerable<IEntityForm> _entityForms;

        /// <summary>
        ///   The handled model view.
        /// </summary>
        private readonly IModelView _modelView;

        /// <summary>
        ///   The current model controller.
        /// </summary>
        private readonly ModelController _controller;

        /// <summary>
        ///   The start position of the entity forms.
        /// </summary>
        private IDictionary<Guid, Point> _startPositions;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SqLauncher.Web.Controller.Carriers.EntityFormCarrier" /> class.
        /// </summary>
        /// <param name = "modelView">The handled model view.</param>
        /// <param name = "entityForms">The all registred entity forms</param>
        /// <param name = "controller">The controller.</param>
        public EntityFormCarrier( IEnumerable<IEntityForm> entityForms, IModelView modelView, ModelController controller )
        {
            _entityForms = entityForms;
            _modelView = modelView;
            _controller = controller;
        }

        /// <summary>
        ///   Starts the moving.
        /// </summary>
        /// <param name = "point">The start point.</param>
        public override void Start( Point point )
        {
            StartPoint = point;
            _modelView.MouseCapture();
            _startPositions = GetCurrentPositions();
        }

        /// <summary>
        ///   Returns positions for handled forms.
        /// </summary>
        /// <returns>The disctionary.</returns>
        private IDictionary<Guid, Point> GetCurrentPositions()
        {
            IDictionary<Guid, Point> result = new Dictionary<Guid, Point>();

            foreach ( var entityForm in _entityForms ){
                result.Add( entityForm.DataEntity.Entity.InnerId,
                            new Point( entityForm.GetLeft(), entityForm.GetTop() ) );
            } //foreach

            return result;
        }

        /// <summary>
        ///   Stops the moving.
        /// </summary>
        public override void Stop()
        {
            _modelView.MouseRelease();

            if ( _hasMove ){
                var command = new EntityFromsChangePosition();
                command.OldPositions = _startPositions;
                command.NewPositions = GetCurrentPositions();
                command.Done = true;
                command.ViewManager = _controller.ModelViewManager;
                _controller.ExecCommand( command );

                _hasMove = false;
            } //if
        }

        /// <summary>
        ///   Moves to next point.
        /// </summary>
        /// <param name = "newPoint">The destination.</param>
        public override void Move( Point newPoint )
        {
            _hasMove = true;
            EndPoint = newPoint;

            double x = newPoint.X - StartPoint.X;
            double y = newPoint.Y - StartPoint.Y;


            //preview of the move possibility 
            foreach ( var entityForm in _entityForms ){
                var w = x + _startPositions[entityForm.DataEntity.Entity.InnerId].X + entityForm.CurrentWidth;
                var h = y + _startPositions[entityForm.DataEntity.Entity.InnerId].Y + entityForm.CurrentHeight;

                if ( w > _modelView.CurrentWidth ){
                    x -= ( w - _modelView.CurrentWidth );
                } //if

                if ( h > _modelView.CurrentHeight ){
                    y -= ( h - _modelView.CurrentHeight );
                } //if


                if ( w - entityForm.CurrentWidth < 0 ){
                    x = -_startPositions[entityForm.DataEntity.Entity.InnerId].X;
                } //if

                if ( h - entityForm.CurrentHeight < 0 ){
                    y = -_startPositions[entityForm.DataEntity.Entity.InnerId].Y;
                } //if
            } //foreach

            foreach ( var entityForm in _entityForms ){
                entityForm.SetLeft( x + _startPositions[entityForm.DataEntity.Entity.InnerId].X );
                entityForm.SetTop( y + _startPositions[entityForm.DataEntity.Entity.InnerId].Y );
            } //foreach
        }
    }
}