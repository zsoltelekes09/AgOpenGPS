using System.Resources;

namespace AgOpenGPS
{
    public class String
    {
        static System.Globalization.CultureInfo Culture2;

        internal static string Get(string str)
        {
            if (Culture != null)
            {
                string LanguageString = ResourceManager.GetString(str, Culture);
                if (LanguageString == null && resourceManEn != null)
                    LanguageString = resourceManEn.GetString(str, Culture);
                if (LanguageString == null) return str;
                else return LanguageString;
            }
            else return "Error No Culture";
        }

        private static ResourceManager resourceMan;
        private static ResourceManager resourceManEn;
        public static System.Globalization.CultureInfo Culture { get; set; }


        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Advanced)]
        public static ResourceManager ResourceManager
        {
            get
            {
                if (Culture == Culture2) return resourceMan;
                else
                {
                    Culture2 = Culture;
                    resourceMan = new ResourceManager("AgOpenGPS.Language.String" + Culture.Name, typeof(String).Assembly);
                }
                if (resourceManEn is null) resourceManEn = new ResourceManager("AgOpenGPS.Language.Stringen", typeof(String).Assembly);
                return resourceMan;
            }
        }
    }
}