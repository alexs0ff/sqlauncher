// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   ResourceReader.cs
//   * Project: SqLauncher.Web.Test
//   * Description:
//   * Created at 2011 11 17 20:55
//   * Modified at: 2011  11 17  20:59
// / ******************************************************************************/ 

using System.IO;
using System.Reflection;

namespace SqLauncher.Web.Test2
{
    /// <summary>
    ///   The reader for embedded resources.
    /// </summary>
    internal class ResourceReader
    {
        private readonly string _resourceName;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SqLauncher.Web.Test2.ResourceReader" /> class.
        /// </summary>
        public ResourceReader( string resourceName )
        {
            _resourceName = resourceName;
        }

        /// <summary>
        ///   Reades specific resource.
        /// </summary>
        /// <returns>The resource string.</returns>
        public string Read()
        {
            string result = string.Empty;

            string assemblyName = Assembly.GetExecutingAssembly().FullName;
            assemblyName  = assemblyName.Substring(0, assemblyName .IndexOf(','));

            using (Stream stream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream(string.Format( "{0}.TestResources.{1}", assemblyName, _resourceName)))
            {
                if ( stream != null ){
                    using ( var reader = new StreamReader( stream ) ){
                        result = reader.ReadToEnd();
                    }
                }
            }

            return result;
        }
    }
}