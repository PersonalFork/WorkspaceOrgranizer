using System;

using log4net;

using Prism.Regions;

namespace RFM.Common.Extensions
{
    public static class NavigationParameterExtensions
    {
        #region Private Variable Declarations.

        private static readonly ILog _logger = LogManager.GetLogger(typeof(NavigationParameterExtensions));

        #endregion

        #region Public Method Declarations.

        /// <summary>
        /// Converts an object into a Navigation Parameter.
        /// </summary>
        /// <param name="obj">The input object.</param>
        /// <param name="key">The key for object. Default value is 'data'</param>
        /// <returns></returns>
        public static NavigationParameters ToNavigationParameter(this object obj, string key = "data")
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key), "Key cannot be empty");
            }
            NavigationParameters pairs = new NavigationParameters
            {
                { key, obj }
            };
            return pairs;
        }

        /// <summary>
        /// Tries to get a strongly typed object from navigation parameter.
        /// </summary>
        /// <typeparam name="T">The type T.</typeparam>
        /// <param name="navParams">Navigation Parameters.</param>
        /// <param name="value">The output value.</param>
        /// <param name="key">The key to search for in the navigation parameter. Default value is 'data'.</param>
        /// <returns>True if value is found. False if error/not found. </returns>
        public static bool TryGetData<T>(this NavigationParameters navParams, out T value, string key = "data")
        {
            value = default(T);

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key), "Key cannot be empty");
            }
            try
            {
                if (navParams.ContainsKey(key))
                {
                    object paramValue = navParams.GetValue<object>(key);
                    if (paramValue is T tObject)
                    {
                        value = tObject;
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return false;
        }

        #endregion
    }
}
