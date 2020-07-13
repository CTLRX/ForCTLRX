using System;
using System.IO;
using System.Text;
using System.Web;

namespace Omipay.Core
{
    public class IOHelper
    {
        /// <summary>
        /// 返回虚拟路径对应的物理路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetMapPath(string path)
        {
            if (HttpContext.Current != null)
            {
                if (path.StartsWith("/"))
                {
                    path = "~" + path;
                }
                return HttpContext.Current.Server.MapPath(path);
            }
            else
            {
                if (path.StartsWith("/"))
                {
                    path = "~" + path;
                }
                return System.Web.Hosting.HostingEnvironment.MapPath(path);
            }
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="path">文件夹路径</param>
        public static void CreateFolder( string path )
        {
            Directory.CreateDirectory( path );
        }

        /// <summary>
        /// 移动文件夹 / 重命名文件夹名称
        /// </summary>
        public static void MoveFolder( string oldpath , string newpath ) 
        {
            if( Directory.Exists( oldpath ) )
            {
                CreateFolder( newpath );
                Directory.Move( oldpath , newpath );
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path">文件路径</param>
        public static void DeleteFile( string path )
        {
            if( File.Exists( path ) )
            {
                File.Delete( path );
            }
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="despath"></param>
        public static void DeleteFolder( string despath )
        {
            if( Directory.Exists( despath ) )
            {
                Directory.Delete( despath , true );
            }
        }

        public static void WriteBase64ToFile( string base64String , string filePath )
        {
            byte[] bytes = Convert.FromBase64String( base64String );
            File.WriteAllBytes( filePath , bytes );
        }

        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="fileStream">数据流</param>
        /// <param name="path">文件路径</param>
        public static void WriteFile( Stream fileStream , string path )
        {
            BinaryReader reader = null;
            FileStream stream = null;
            try
            {
                reader = new BinaryReader( fileStream );
                byte[] buffer = new byte[ fileStream.Length ];
                reader.Read( buffer , 0 , buffer.Length );
                stream = new FileStream( path , FileMode.Create );
                stream.Write( buffer , 0 , buffer.Length );
            }
            catch
            {

            }
            finally
            {
                if( reader != null )
                {
                    reader.Close();
                }
                if( stream != null )
                {
                    stream.Close();
                }
            }
        }

        /// <summary>
        /// 读取文件文本
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string ReadText( string path , Encoding encoding = null )
        {
            if( encoding == null )
            {
                encoding = Encoding.UTF8;
            }
            using( StreamReader reader = new StreamReader( path , encoding ) )
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="path">文件路径</param>
        public static void WriteFile( byte[] data , string path )
        {
            File.WriteAllBytes( path , data );
        }

        /// <summary>
        /// 拷贝文件夹和其所有子文件夹、文件
        /// </summary>
        /// <param name="strFromPath">原路径</param>
        /// <param name="strToPath">目标路径</param>
        /// <param name="isCopyRootFolder">是否拷贝根目录文件夹</param>
        public static void CopyFolder( string strFromPath , string strToPath , bool isCopyRootFolder = true )
        {
            //如果源文件夹不存在，则创建 
            if( !Directory.Exists( strFromPath ) )
            {
                Directory.CreateDirectory( strFromPath );
            }
            if( isCopyRootFolder )
            {
                //取得要拷贝的文件夹名
                string strFolderName = strFromPath.Substring( strFromPath.LastIndexOf( "\\" ) + 1 , strFromPath.Length - strFromPath.LastIndexOf( "\\" ) - 1 );
                //如果目标文件夹中没有源文件夹则在目标文件夹中创建源文件夹
                if( !Directory.Exists( strToPath + "\\" + strFolderName ) )
                {
                    Directory.CreateDirectory( strToPath + "\\" + strFolderName );
                }
            }
            else
            {
                if( !Directory.Exists( strToPath ) )
                {
                    Directory.CreateDirectory( strToPath );
                }
            }
            //创建数组保存源文件夹下的文件名
            string[] strFiles = Directory.GetFiles( strFromPath );
            //循环拷贝文件
            for( int i = 0 ; i < strFiles.Length ; i++ )
            {
                //取得拷贝的文件名，只取文件名，地址截掉。
                string strFileName = strFiles[ i ].Substring( strFiles[ i ].LastIndexOf( "\\" ) + 1 , strFiles[ i ].Length - strFiles[ i ].LastIndexOf( "\\" ) - 1 );
                //开始拷贝文件,true表示覆盖同名文件
                if( isCopyRootFolder )
                {
                    string strFolderName = strFromPath.Substring( strFromPath.LastIndexOf( "\\" ) + 1 , strFromPath.Length - strFromPath.LastIndexOf( "\\" ) - 1 );
                    File.Copy( strFiles[ i ] , strToPath + "\\" + strFolderName + "\\" + strFileName , true );
                }
                else
                {
                    File.Copy( strFiles[ i ] , strToPath + "\\" + strFileName , true );
                }
            }
            DirectoryInfo dirInfo = new DirectoryInfo( strFromPath );
            //取得源文件夹下的所有子文件夹名称
            DirectoryInfo[] ZiPath = null;

            ZiPath = dirInfo.GetDirectories();
            for( int j = 0 ; j < ZiPath.Length ; j++ )
            {
                //获取所有子文件夹名
                string strZiPath = strFromPath + "\\" + ZiPath[ j ].ToString();
                //把得到的子文件夹当成新的源文件夹，从头开始新一轮的拷贝
                if( isCopyRootFolder )
                {
                    string strFolderName = strFromPath.Substring( strFromPath.LastIndexOf( "\\" ) + 1 , strFromPath.Length - strFromPath.LastIndexOf( "\\" ) - 1 );
                    CopyFolder( strZiPath , strToPath + "\\" + strFolderName );
                }
                else
                {
                    CopyFolder( strZiPath , strToPath );
                }
            }
        }

    }
}
