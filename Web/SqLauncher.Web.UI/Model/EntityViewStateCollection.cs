// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   EntityViewStateCollection.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 11 01 22:29
//   * Modified at: 2011  11 01  22:37
// / ******************************************************************************/ 

using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

using SqLauncher.Web.Model;

namespace SqLauncher.Web.UI.Model
{
    /// <summary>
    ///   Represents the collection for entity view states.
    /// </summary>
    public class EntityViewStateCollection : ObservableCollection<IEntityViewState>
    {
        /// <summary>
        ///   The handled data model.
        /// </summary>
        public DataModel DataModel { get; set; }

        /// <summary>
        /// Gets the entity view state by entity inner Id.
        /// </summary>
        /// <param name="value">The id.</param>
        /// <returns></returns>
        public IEntityViewState this[Guid value]
        {
            get { return Items.FirstOrDefault( en => en.Entity.InnerId == value ); }
        }

        /// <summary>
        /// Adds new entity view state to collection and checks exists it.
        /// </summary>
        /// <param name="state">The entity view state</param>
        public void SafeAdd(IEntityViewState state)
        {
            if ( !Items.Contains( state ) ){
                Add( state );
            } //if
        }

        /// <summary>
        ///   Raises the <see cref = "E:System.Collections.ObjectModel.ObservableCollection`1.CollectionChanged" /> event with the provided event data.
        /// </summary>
        /// <param name = "e">The event data to report in the event.</param>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if ( DataModel == null ){
                throw new ArgumentException( "DataModel is null, before initialization required" );
            } //if

            if ( e.NewItems!=null ){
                foreach ( var newItem in e.NewItems ){
                    var entity = ( (IEntityViewState) newItem ).Entity;
                    if ( entity ==null ){
                        throw new ArgumentException( "newItem requered Entity property initialization." );
                    } //if
                    if (!DataModel.Entities.Contains(entity))
                    {
                        DataModel.Entities.Add( entity );
                    } //if 
                } //foreach
            } //if

            if ( e.OldItems != null ){

                foreach ( var oldItem in e.OldItems ){
                    DataModel.Entities.Remove( ( (IEntityViewState) oldItem ).Entity );
                } //foreach

            } //if
        }
    }
}