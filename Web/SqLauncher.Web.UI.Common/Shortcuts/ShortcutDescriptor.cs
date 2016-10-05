// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   ShortcutDescriptor.cs
//   * Project: SqLauncher.Web.UI.Common
//   * Description:
//   * Created at 2012 02 05 23:18
//   * Modified at: 2012  02 06  20:29
// / ******************************************************************************/ 

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace SqLauncher.Web.UI.Common.Shortcuts
{
    /// <summary>
    ///   Represents the descriptor of keys combination.
    /// </summary>
    public class ShortcutDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref = "T:SqLauncher.Web.UI.Common.Shortcuts.ShortcutDescriptor" /> class.
        /// </summary>
        public ShortcutDescriptor( Key key )
        {
            Key = key;
            Modifiers = new Collection<ModifierKeys>();
        }

        /// <summary>
        /// Creates the shortcut descriptor.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Created descriptor.</returns>
        public static ShortcutDescriptor Create(Key key)
        {
            var result = new ShortcutDescriptor( key );

            if ((Keyboard.Modifiers & ModifierKeys.Alt) != 0)
            {
                result.Modifiers.Add(ModifierKeys.Alt);
            } //if

            if ((Keyboard.Modifiers & ModifierKeys.Control) != 0)
            {
                result.Modifiers.Add(ModifierKeys.Control);
            } //if

            if ((Keyboard.Modifiers & ModifierKeys.Shift) != 0)
            {
                result.Modifiers.Add(ModifierKeys.Shift);
            } //if

            return result;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SqLauncher.Web.UI.Common.Shortcuts.ShortcutDescriptor"/> class.
        /// </summary>
        public ShortcutDescriptor( Key key,ModifierKeys modifier )
        {
            Key = key;
            Modifiers = new Collection<ModifierKeys>{modifier};
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SqLauncher.Web.UI.Common.Shortcuts.ShortcutDescriptor"/> class.
        /// </summary>
        public ShortcutDescriptor(Key key,params ModifierKeys[] modifiers)
        {
            Key = key;
            Modifiers = new Collection<ModifierKeys>( modifiers );
        }

        /// <summary>
        ///   The modifiers keys.
        /// </summary>
        public ICollection<ModifierKeys> Modifiers { get; private set; }

        /// <summary>
        ///   The key.
        /// </summary>
        public Key Key { get; set; }

        /// <summary>
        ///   Determines whether the specified <see cref = "T:SqLauncher.Web.UI.Common.Shortcuts.ShortcutDescriptor" /> is equal to the current <see
        ///    cref = "T:SqLauncher.Web.UI.Common.Shortcuts.ShortcutDescriptor" />.
        /// </summary>
        /// <returns>
        ///   true if the specified <see cref = "T:SqLauncher.Web.UI.Common.Shortcuts.ShortcutDescriptor" /> is equal to the current <see
        ///    cref = "T:SqLauncher.Web.UI.Common.Shortcuts.ShortcutDescriptor" />; otherwise, false.
        /// </returns>
        /// <param name = "obj">The <see cref = "T:SqLauncher.Web.UI.Common.Shortcuts.ShortcutDescriptor" /> to compare with the current <see
        ///    cref = "T:SqLauncher.Web.UI.Common.Shortcuts.ShortcutDescriptor" />. </param>
        public override bool Equals( object obj )
        {
            if ( obj == null ){
                return false;
            } //if

            if ( obj.GetType() != GetType() ){
                return false;
            } //if

            var obj2 = (ShortcutDescriptor) obj;

            var result = false;

            if ( obj2.Key == Key ){
                result = obj2.Modifiers.Except(Modifiers).ToList().Count == 0;
            } //if

            return result;
        }

        /// <summary>
        ///   Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        ///   A hash code for the current <see cref = "T:SqLauncher.Web.UI.Common.Shortcuts.ShortcutDescriptor" />.
        /// </returns>
        public override int GetHashCode()
        {
            var result = (int) Key;

            return Modifiers.Aggregate(result, (current, modifierKey) => current ^ (int)modifierKey);
        }
    }
}