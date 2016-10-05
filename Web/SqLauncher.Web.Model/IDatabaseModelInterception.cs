// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   IDatabaseModelInterception.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2011 09 25 2:01 PM
//   * Modified at: 2011  09 25  2:01 PM
// / ******************************************************************************/ 

using Microsoft.Practices.Unity;

namespace SqLauncher.Web.Model
{
    /// <summary>
    /// Represents an interface for set up all databases model.
    /// </summary>
    public interface IDatabaseModelInterception
    {
        /// <summary>
        /// Initializes the sqLite types interception.
        /// </summary>
        /// <param name="container">The unity container.</param>
        void Initialize( IUnityContainer container );
    }
}