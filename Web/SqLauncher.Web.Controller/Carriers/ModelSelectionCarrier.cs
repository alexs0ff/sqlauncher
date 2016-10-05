// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   ModelSelectionCarrier.cs
//   * Project: SqLauncher.Web.Controller
//   * Description:
//   * Created at 2011 10 04 10:08 PM
//   * Modified at: 2011  10 04  10:30 PM
// / ******************************************************************************/ 

using System.Collections.Generic;
using System.Windows;

using SqLauncher.Web.UI.Model;

namespace SqLauncher.Web.Controller.Carriers
{
    /// <summary>
    ///   The handler of the model view selection.
    /// </summary>
    public class ModelSelectionCarrier : Carrier
    {
        /// <summary>
        /// Indicates that the moving was happend. 
        /// </summary>
        private bool _hasMove;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SqLauncher.Web.Controller.Carriers.ModelSelectionCarrier" /> class.
        /// </summary>
        /// <param name = "modelView">The handled model view.</param>
        /// <param name="forms">The all registred entity forms</param>
        public ModelSelectionCarrier(IModelView modelView, IEnumerable<IEntityForm> forms)
        {
            _forms = forms;
            _modelView = modelView;
        }

        /// <summary>
        ///   Starts the moving.
        /// </summary>
        /// <param name = "point">The start point.</param>
        public override void Start( Point point )
        {
            StartPoint = point;
            _modelView.MouseCapture();
            _modelView.ShowSelection();
            _hasMove = false;
        }

        /// <summary>
        ///   Moves to next point.
        /// </summary>
        /// <param name = "newPoint">The destination.</param>
        public override void Move( Point newPoint )
        {
            EndPoint = newPoint;
            _modelView.SetSelection( StartPoint, newPoint );
            _hasMove = true;
        }

        /// <summary>
        ///   Stops the moving.
        /// </summary>
        public override void Stop()
        {
            _modelView.MouseRelease();
            _modelView.HideSelection();

            if (_hasMove){
                var selectionRect = new Rect( StartPoint, EndPoint );

                foreach ( var entityForm in _forms ){
                    var intersectedRect = GetRect( entityForm );
                    intersectedRect.Intersect( selectionRect );

                    if ( intersectedRect != Rect.Empty ){
                        entityForm.IsSelected = true;
                    } //if
                } //foreach
            }
        }

        /// <summary>
        /// Calculates the bounds rect for the entity form.
        /// </summary>
        private Rect GetRect(IEntityForm form)
        {
            return new Rect(form.GetLeft(), form.GetTop(), form.CurrentWidth, form.CurrentHeight);
        }


        /// <summary>
        ///   The handled view.
        /// </summary>
        private readonly IModelView _modelView;

        /// <summary>
        /// The handled forms.
        /// </summary>
        private readonly IEnumerable<IEntityForm> _forms;
    }
}