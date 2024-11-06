
using System.Text;
using System.Runtime.InteropServices;//引用命名空间

namespace SUTAIMES.Common
{

    public class INIFile
    {
        #region "声明变量"

        /// <summary>
        /// 写入INI文件
        /// </summary>
        /// <param name="section">节点名称[如[TypeName]]</param>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="filepath">文件路径</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filepath);
        /// <summary>
        /// 读取INI文件
        /// </summary>
        /// <param name="section">节点名称</param>
        /// <param name="key">键</param>
        /// <param name="def">值</param>
        /// <param name="retval">stringbulider对象</param>
        /// <param name="size">字节大小</param>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retval, int size, string filePath);

        //private string strFilePath = "";// Application.StartupPath + "\\Config.ini.ini";//获取INI文件路径
        //private string strSec = ""; //INI文件名

        #endregion

        public bool WritePrivateINIfileString(string section, string key, string val, string filepath)
        {
            try
            {
                WritePrivateProfileString(section, key, val, filepath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string GetPrivateINIfileString(string Section, string key, string filePath)
        {

            StringBuilder temp = new StringBuilder(1024);
            GetPrivateProfileString(Section, key, "", temp, 1024, filePath);
            return temp.ToString();
        }
        ///
    }
}


