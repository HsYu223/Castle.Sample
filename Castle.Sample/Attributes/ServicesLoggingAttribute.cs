namespace Castle.Sample.Attributes
{
    /// <summary>
    /// Services Log紀錄標籤
    /// </summary>
    /// <seealso cref="System.Attribute" />
    public class ServicesLoggingAttribute : Attribute
    {
        /// <summary>
        /// Log紀錄名稱
        /// </summary>
        public string LogName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServicesLoggingAttribute"/> class.
        /// </summary>
        /// <param name="logName">Name of the log.</param>
        public ServicesLoggingAttribute(string logName)
        {
            this.LogName = logName;
        }
    }
}
