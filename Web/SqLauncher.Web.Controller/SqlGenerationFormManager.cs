// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   SqlGenerationFormManager.cs
//   * Project: SqLauncher.Web.Controller
//   * Description:
//   * Created at 2011 11 25 12:59
//   * Modified at: 2011  11 25  13:09
// / ******************************************************************************/ 

using System.IO;
using System.Text;

using SqLauncher.Web.Model;
using SqLauncher.Web.UI.Model;

namespace SqLauncher.Web.Controller
{
    /// <summary>
    ///   The manager for sql geneate form handling.
    /// </summary>
    public class SqlGenerationFormManager
    {
        /// <summary>
        /// The encoding.
        /// </summary>
        private readonly Encoding _encoding;

        /// <summary>
        /// Initializes a new instance of the <see cref = "T:SqLauncher.Web.Controller.SqlGenerationFormManager" /> class.
        /// </summary>
        /// <param name="sqlGenerationForm">The generation form.</param>
        /// <param name="modelGeneratorBase">The ddl generator.</param>
        /// <param name="dataModel">The data model.</param>
        public SqlGenerationFormManager( ISqlGenerationForm sqlGenerationForm, DataModelGeneratorBase modelGeneratorBase, DataModel dataModel )
        {
            SqlGenerationForm = sqlGenerationForm;
            sqlGenerationForm.SqlGenerating += SQLGenerationFormSqlGenerating;
            ModelGenerator = modelGeneratorBase;
            DataModel = dataModel;
            _encoding = new UTF8Encoding(false);
            ModelGenerator.ERDEntityProcessed += ModelGeneratorERDEntityProcessed;
        }

        void ModelGeneratorERDEntityProcessed(object sender, ERDEntityProcessedEventArgs e)
        {
            SqlGenerationForm.DataEntity.GeneratedItems.Add( e.Entity.Caption );
        }

        /// <summary>
        /// Closes the sql form.
        /// </summary>
        public void Close()
        {
            SqlGenerationForm.SqlGenerating -= SQLGenerationFormSqlGenerating;
            ModelGenerator.ERDEntityProcessed -= ModelGeneratorERDEntityProcessed;
            SqlGenerationForm.DataEntity.GeneratedItems.Clear();
            SqlGenerationForm.CloseForm();
        }

        /// <summary>
        /// Starts show dialog.
        /// </summary>
        public void Start()
        {
            SqlGenerationForm.Start();
        }

        /// <summary>
        ///   Occurs when user want to generate sql save it.
        /// </summary>
        /// <param name = "sender"></param>
        /// <param name = "e"></param>
        private void SQLGenerationFormSqlGenerating( object sender, SqlGeneratingEventArgs e )
        {
            SqlGenerationForm.DataEntity.GeneratedItems.Clear();
            var writer = new StreamWriter( e.StreamWriter, _encoding );
            writer.Write( ModelGenerator.Generate( DataModel ) );
            writer.Flush();
            SqlGenerationForm.DataEntity.FilePath = e.FilePath;
        }

        /// <summary>
        ///   The handled form.
        /// </summary>
        public ISqlGenerationForm SqlGenerationForm { get; private set; }

        /// <summary>
        ///   The model generator.
        /// </summary>
        public DataModelGeneratorBase ModelGenerator { get; private set; }

        /// <summary>
        /// The model to convertion.
        /// </summary>
        public DataModel DataModel { get;private set; }
    }
}