namespace SMAS.Models.Common
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;

    /// <summary>  
    /// Allow file size attribute class  
    /// </summary>  
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class AllowFileSizeAttribute : ValidationAttribute
    {
        #region Public / Protected Properties  

        /// <summary>  
        /// Gets or sets file size property. Default is 1GB (the value is in Bytes).  
        /// </summary>  
        public int FileSize { get; set; } = 1 * 1024 * 1024 * 1024;

        #endregion

        #region Is valid method  

        /// <summary>  
        /// Is valid method.  
        /// </summary>  
        /// <param name="value">Value parameter</param>  
        /// <returns>Returns - true is specify extension matches.</returns>  
        public override bool IsValid(object value)
        {
            // Initialization  
            HttpPostedFileBase file = value as HttpPostedFileBase;
            bool isValid = true;

            // Settings.  
            int allowedFileSize = this.FileSize;

            // Verification.  
            if (file != null)
            {
                // Initialization.  
                var fileSize = file.ContentLength;

                // Settings.  
                isValid = fileSize <= allowedFileSize;
            }

            // Info  
            return isValid;
        }

        #endregion
    }

    public class FileTypesAttribute : ValidationAttribute
    {
        private readonly List<string> _types;

        public FileTypesAttribute(string types)
        {
            _types = types.Split(',').ToList();
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;
            var postedFile = value as HttpPostedFileBase;
            var fileExt = System.IO.Path.GetExtension(postedFile.FileName).Substring(1);
            return _types.Contains(fileExt, StringComparer.OrdinalIgnoreCase);
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format("Invalid file type. Only {0} are supported.", String.Join(", ", _types));
        }
    }
}