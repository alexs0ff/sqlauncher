// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   RelationViewStateCollection.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 11 21 19:22
//   * Modified at: 2012  01 07  12:33
// / ******************************************************************************/ 

using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

using SqLauncher.Web.Model;

namespace SqLauncher.Web.UI.Model
{
    /// <summary>
    ///   Represents the collection for relation view states.
    /// </summary>
    public class RelationViewStateCollection : ObservableCollection<IRelationViewState>
    {
        /// <summary>
        ///   The handled data model.
        /// </summary>
        public DataModel DataModel { get; set; }

        /// <summary>
        ///   Gets the entity view state by relation inner Id.
        /// </summary>
        /// <param name = "value">The entity id.</param>
        /// <returns></returns>
        public IRelationViewState this[ Guid value ]
        {
            get { return Items.FirstOrDefault( en => en.Relation.InnerId == value ); }
        }

        /// <summary>
        /// Adds new relation view state to collection and checks exists it.
        /// </summary>
        /// <param name="state">The relation view state</param>
        public void SafeAdd(IRelationViewState state)
        {
            if (!Items.Contains(state))
            {
                Add(state);
            } //if
        }

        /// <summary>
        ///   Raises the <see cref = "E:System.Collections.ObjectModel.ObservableCollection`1.CollectionChanged" /> event with the provided event data.
        /// </summary>
        /// <param name = "e">The event data to report in the event.</param>
        protected override void OnCollectionChanged( NotifyCollectionChangedEventArgs e )
        {
            if ( DataModel == null ){
                throw new ArgumentException( "DataModel is null, before initialization required" );
            } //if

            if ( e.NewItems != null ){
                foreach ( var newItem in e.NewItems ){
                    var relation = ( (IRelationViewState) newItem ).Relation;

                    if ( !DataModel.Relations.Contains( relation ) ){
                        DataModel.Relations.Add( relation );
                    } //if
                } //foreach
            } //if

            if ( e.OldItems != null ){
                foreach ( var oldItem in e.OldItems ){
                    var relation = (IRelationViewState) oldItem;
                    DataModel.Relations.Remove( relation.Relation );
                } //foreach
            } //if
        }
    }
}