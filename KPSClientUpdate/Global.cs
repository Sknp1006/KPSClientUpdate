using System;
using System.Collections;
using System.Reflection;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Xml;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace AutoUpdater.UpdateHelper
{
	/// <summary>
	/// 通用函数
	/// </summary>
	public sealed class Global
	{
		/// <summary>
		/// 根节点名称
		/// </summary>
		public const string ROOT_NODE		= "configuration";

		/// <summary>
		/// 配置文件属性名称
		/// </summary>
		public const string VALUE			= "Value";

		/// <summary>
		/// 更新日志文件
		/// </summary>
		public const string LOG_FILE		= "Update.log";

		/// <summary>
		/// 是否写日志文件
		/// </summary>
		public const bool bWriteLog = true;

		#region 按位与操作值
		/// <summary>
		/// 按位与操作值
		/// </summary>
		#endregion
		private const int XOR_VALUE						= 0x09;

		#region 对称算法初始化向量文件名称
		/// <summary>
		/// 对称算法初始化向量文件名称
		/// </summary>
		#endregion
		private const string IV_FILE					= "IV.bin";

		#region 数据加密标准 (DES) 算法的机密密钥文件
		/// <summary>
		/// 数据加密标准 (DES) 算法的机密密钥文件
		/// </summary>
		#endregion
		private const string KEY_FILE					= "Key.bin";	
	
		private static byte[] bIV = null;
		private static byte[] bKey = null;

		#region 对称算法的初始化向量
		/// <summary>
		/// 对称算法的初始化向量
		/// </summary>
		#endregion
		private static byte[] bytesIV
		{
			get
			{
				if( bIV == null )
				{
					bIV = new byte[ 8 ];
					byte[] bFileStream = ReadStreamFromFile( AssemblyPath + "\\" + IV_FILE );

					byte[] bBuffer = MD5Encrypt( System.Text.Encoding.Default.GetBytes( System.Text.Encoding.Default.GetString( bFileStream ) ) );

					Array.Copy( bBuffer,0,bIV,0,8 );    
				}
				return bIV;
			}
		}


		#region 数据加密标准 (DES) 算法的机密密钥
		/// <summary>
		/// 数据加密标准 (DES) 算法的机密密钥
		/// </summary>
		#endregion
		private static byte[] bytesKey
		{
			get
			{
				if( bKey == null )
				{
					bKey = new byte[ 16 ];
					byte[] bFileStream = ReadStreamFromFile( AssemblyPath + "\\" + KEY_FILE );

					byte[] bBuffer = MD5Encrypt( System.Text.Encoding.Default.GetBytes( System.Text.Encoding.Default.GetString( bFileStream ) ) );

					Array.Copy( bBuffer,0,bKey,0,bBuffer.Length );
				}
				return bKey;
			}
		}


		#region 对字节进行行加密或解密
		/// <summary>
		/// 对字节进行行加密或解密
		/// </summary>
		/// <param name="bStream">欲操作的字节流</param>
		/// <returns></returns>
		#endregion
		public static byte[] ConvertBytesRerverse( byte[] bStream )
		{
			byte[] bOut = new byte[ bStream.Length ];
			int nTmp = 0;
			int nByte = 0;

			for( int i = 0; i < bStream.Length; i++ )
			{
				nTmp = (int)bStream[i];
				nByte = nTmp ^ XOR_VALUE;
				bOut[i] = (byte)nByte;
			}
			return bOut;
		}


		#region 读取文件
		/// <summary>
		/// 读取文件
		/// </summary>
		/// <param name="strFile">文件名称</param>
		/// <returns></returns>
		#endregion
		public static byte[] ReadStreamFromFile( string strFile )
		{
			FileStream fs = new FileStream( strFile,FileMode.Open,FileAccess.Read );

			byte[] bReadStream = new byte[ (int)fs.Length ];

			fs.Read( bReadStream,0,bReadStream.Length );

			byte[] bBuffer = ConvertBytesRerverse( bReadStream );

			fs.Close();

			return bReadStream;
		}


		#region 对字符串进行MD5加密
		/// <summary>
		/// 对字符串进行MD5加密
		/// </summary>
		/// <param name="bInput">欲加密的字节流</param>
		/// <returns></returns>
		#endregion
		public static byte[] MD5Encrypt( byte[] bInput )
		{
			MD5CryptoServiceProvider encrypt = new MD5CryptoServiceProvider();
			byte[] bytes = encrypt.ComputeHash( bInput );
			return bytes;
		}


		/// <summary>
		/// 对字符串进行标准MD5加密
		/// </summary>
		/// <param name="strEncrypt">欲加密的字符串</param>
		/// <returns></returns>
		public static string MD5StandEncrypt( string strEncrypt )
		{
			string strMD5 = BitConverter.ToString( MD5Encrypt( System.Text.Encoding.Default.GetBytes( strEncrypt ) ) );
			return strMD5.Replace( "-","" ).ToLower();
		}


		#region 读取文件
		/// <summary>
		/// 读取文件
		/// </summary>
		/// <param name="file">文件物理路径</param>
		/// <returns>以二进制的形式反回文件大小</returns>
		#endregion
		public static byte[] ReadFile( string file )
		{
			FileStream fs = new FileStream( file,FileMode.Open,FileAccess.Read );

			byte[] bReadStream = new byte[ (int)fs.Length ];

			fs.Read( bReadStream,0,bReadStream.Length );

			fs.Close();
            
			return bReadStream;
		}


		/// <summary>
		/// 当前应用程序所在的路径
		/// </summary>
		public static string AssemblyPath
		{
			get{ return Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location );}
		}
		

		/// <summary>
		/// 获取指定目标的随机文件名
		/// </summary>
		/// <param name="strPath">目标名称</param>
		/// <param name="strPrefix">文件名前缀</param>
		/// <param name="strExt">文件扩展名</param>
		/// <remarks>
		///		若指定目标不存在，则创建
		/// </remarks>
		/// <returns></returns>
		public static string GetRandomFile( string strPath,string strPrefix,string strExt )
		{
			DateTime dt = DateTime.Now;
			string strFile = String.Format( @"{0}\{1}{2}{3}{4}{5}{6}{7}{8}.{9}",strPath.TrimEnd('\\'),strPrefix,dt.Year,dt.Month,dt.Day,dt.Hour,dt.Minute,dt.Second,RandomValue( 0,9999 ),strExt );
			while( File.Exists( strFile ) )
			{
				strFile = String.Format( @"{0}\{1}{2}{3}{4}{5}{6}{7}{8}.{9}",strPath.TrimEnd('\\'),strPrefix,dt.Year,dt.Month,dt.Day,dt.Hour,dt.Minute,dt.Second,RandomValue( 0,9999 ),strExt );
			}
			return strFile;
		}


		/// <summary>
		/// 获取指定范围内的随机数
		/// </summary>
		/// <param name="min">最小值</param>
		/// <param name="max">最大值</param>
		/// <returns></returns>
		public static int RandomValue( int min,int max )
		{
			System.Random rnd = new Random( (int)DateTime.Now.Ticks );
			return rnd.Next( min,max );
		}


		/// <summary>
		/// 解析本地更新配置文件
		/// </summary>
		/// <param name="xmlfile"></param>
		/// <returns></returns>
		public static UpdateUrlConfig ParseUpdateUrlConfig( string xmlfile )
		{
			UpdateUrlConfig objResult = null;

			XmlDocument xmlDoc = new XmlDocument();
			try
			{
				objResult = new UpdateUrlConfig();				
				xmlDoc.Load( new MemoryStream( Global.ReadFile( xmlfile ) ) );
				XmlNode rootNode = xmlDoc.SelectSingleNode( string.Format( "{0}",ROOT_NODE ) );
				if( rootNode.ChildNodes.Count != 7 )
				{
					throw new Exception( "本地更新配置文件损坏，无法读取数据！" );
				}
				objResult.strUpdateTitle	= rootNode.ChildNodes[0].Attributes[ VALUE ].InnerText;
				objResult.strUpdateUrl		= rootNode.ChildNodes[1].Attributes[ VALUE ].InnerText;
				objResult.strLastVersion	= rootNode.ChildNodes[2].Attributes[ VALUE ].InnerText;
				objResult.dtUpdateDate		= Convert.ToDateTime( rootNode.ChildNodes[3].Attributes[ VALUE ].InnerText );
				objResult.strLocalConfigFile= rootNode.ChildNodes[4].Attributes[ VALUE ].InnerText;
				objResult.schoolId          = rootNode.ChildNodes[5].Attributes[VALUE].InnerText;
				if ( rootNode.ChildNodes[6].ChildNodes.Count != 2)  // <Application>
				{
					throw new Exception( "本地更新配置文件损坏，无法读取数据！" );
				}
				objResult.ApplicationFileName = rootNode.ChildNodes[6].ChildNodes[0].Attributes[ VALUE ].InnerText;
				objResult.Parameters = rootNode.ChildNodes[6].ChildNodes[1].Attributes[ VALUE ].InnerText;
			}
			finally
			{
				xmlDoc = null;
			}
			return objResult;
		}


		#region 解析远程更新配置信息
		/// <summary>
		/// 解析远程更新配置信息
		/// </summary>
		/// <param name="xmlfile">配置文件的物理路径</param>
		/// <returns></returns>
		#endregion
		public static UpdateFileConfig ParseUpdateFileConfig( string xmlfile )
		{
			UpdateFileConfig objResult = null;
			XmlDocument xmlDoc = new XmlDocument();
			try
			{
				objResult = new UpdateFileConfig();
				xmlDoc.Load( new MemoryStream( Global.ReadFile( xmlfile ) ) );
				XmlNode rootNode = xmlDoc.SelectSingleNode( string.Format( "{0}",ROOT_NODE ) );
				if( rootNode.ChildNodes.Count != 7 )
				{
					throw new Exception( "远程更新配置文件损坏！" );
				}
                objResult.UpdateSetting			= rootNode.ChildNodes[0].Attributes[ VALUE ].InnerText == "1";
				objResult.UpdateMainVersion		= rootNode.ChildNodes[1].Attributes[ VALUE ].InnerText;
				objResult.UpdateDate			= Convert.ToDateTime( rootNode.ChildNodes[2].Attributes[ VALUE ].InnerText );
				objResult.UpdateWebPath			= rootNode.ChildNodes[3].Attributes[ VALUE ].InnerText;
				objResult.LicenceFile			= rootNode.ChildNodes[4].Attributes[ VALUE ].InnerText;
				objResult.HistoryFile			= rootNode.ChildNodes[5].Attributes[ VALUE ].InnerText;

				foreach( XmlNode configNode in rootNode.ChildNodes[6].ChildNodes )
				{
					objResult.Files.Add( ParseFileDescription( configNode ) );
				}
			}
			catch (System.Exception)
            {
				Console.WriteLine("该文件不是xml格式，尝试使用json解析");
                try
                {
					using (System.IO.StreamReader file = System.IO.File.OpenText(xmlfile))
					{
						string json = file.ReadToEnd();
						Root root = JsonConvert.DeserializeObject<Root>(json);

						objResult.UpdateSetting = root.data.updateSetting.ToString() == "1";
						objResult.UpdateMainVersion = root.data.updateMainVersion;
						objResult.UpdateDate = Convert.ToDateTime(root.data.updateDate);
						objResult.UpdateWebPath = root.data.updateWebPath;
						if (root.data.licenceFile == null)
                        {
							objResult.LicenceFile = "";
						}
                        else
                        {
							objResult.LicenceFile = root.data.licenceFile;
						}
						if (root.data.historyFile == null)
                        {
							objResult.HistoryFile = "";
						}
                        else
                        {
							objResult.HistoryFile = root.data.historyFile.ToString();
						}

						List<string> files = new List<string>();
						foreach (var item in root.data.updateFiles)
                        {
							FileDescription fileDescription = new FileDescription();
							fileDescription.FileName = item.fileName.ToString();
							fileDescription.FileSize = (int)item.fileSize;
							fileDescription.FileVersion = item.fileVersion.ToString();
							if (item.applications == null)
							{
								fileDescription.Applications = "KPScanClient.exe";  // 主程序，表示更新时要停止的程序
							}
							else
							{
								fileDescription.Applications = item.applications.ToString();

							}
							if (!files.Contains(fileDescription.FileName))
                            {
								files.Add(fileDescription.FileName);
							    objResult.Files.Add(fileDescription);
							}
						}
					}
				}
				catch
                {
					AutoUpdater.UpdateHelper.Global.WriteUpdateLog(string.Format("{0}:远程服务器异常，请稍后重试。", DateTime.Now), true);
				}
            }
			finally
			{
				xmlDoc = null;
			}
			return objResult;
		}


		#region 解析远程更新文件信息
		/// <summary>
		/// 解析远程更新文件信息
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		#endregion
		public static FileDescription ParseFileDescription( XmlNode node )
		{
			if( node.Attributes.Count != 5 )
			{
				throw new Exception( "远程更新配置文件损坏，无法读取数据！" );
			}
			FileDescription info = new FileDescription();
			info.FileName		= node.Attributes[ UpdateFileConfig.FILE_NAME ].InnerText;
			info.FileSize		= Convert.ToInt32( node.Attributes[ UpdateFileConfig.FILE_SIZE ].InnerText );
			info.FileVersion	= node.Attributes[ UpdateFileConfig.FILE_VERSION ].InnerText;
			info.Applications	= node.Attributes[ UpdateFileConfig.APPLICATIONS ].InnerText;
			info.ClientPath		= node.Attributes[ UpdateFileConfig.CLIENT_PATH ].InnerText;
			return info;
		}


		#region 更新本地配置文件信息
		/// <summary>
		/// 更新本地配置文件信息
		/// </summary>
		/// <param name="info"></param>
		/// <param name="file">本地更新配置文件</param>
		#endregion
		public static void WriteUpdateUrlFile( UpdateUrlConfig info,string file )
		{
//			string strTmpFile = Global.GetRandomFile( AssemblyPath,string.Empty,"bin" );
			CancelFileAttribute( file );
			XmlTextWriter xmlWriter = new XmlTextWriter( file,System.Text.Encoding.UTF8 );
			try
			{
				#region 写XML文件

				xmlWriter.WriteStartDocument();					//-----------Start----------		
				xmlWriter.WriteStartElement( ROOT_NODE );		//-----------configuration--

				xmlWriter.WriteStartElement( UpdateUrlConfig.UPDATE_TITLE );	//--------UpdateTitle------
				xmlWriter.WriteAttributeString( VALUE,info.strUpdateTitle );	// value
				xmlWriter.WriteEndElement();									//--------UpdateTitle------

				xmlWriter.WriteStartElement( UpdateUrlConfig.UPDATE_URL );		//--------UpdateUrl------
				xmlWriter.WriteAttributeString( VALUE,info.strUpdateUrl );		//value
				xmlWriter.WriteEndElement();										//--------UpdateUrl------
                
				xmlWriter.WriteStartElement( UpdateUrlConfig.LAST_VERSION );		//--------LastVersion---------
				xmlWriter.WriteAttributeString( VALUE,info.strLastVersion );	//value;
				xmlWriter.WriteEndElement();										//--------LastVersion---------

				xmlWriter.WriteStartElement( UpdateUrlConfig.UPDATE_DATE );		//--------UpdateDate---------
				xmlWriter.WriteAttributeString( VALUE,info.dtUpdateDate.ToString( "F" ) );	//value;
				xmlWriter.WriteEndElement();										//--------UpdateDate---------

				xmlWriter.WriteStartElement( UpdateUrlConfig.LOCAL_CONFIG_FILE );		//--------LocalConfigFile---------
				xmlWriter.WriteAttributeString( VALUE,info.strLocalConfigFile );	//value;
				xmlWriter.WriteEndElement();                                        //--------LocalConfigFile---------

				xmlWriter.WriteStartElement(UpdateUrlConfig.SCHOOLID);     //--------LocalConfigFile---------
				xmlWriter.WriteAttributeString(VALUE, info.schoolId); //value;
				xmlWriter.WriteEndElement();                                        //--------LocalConfigFile---------

				xmlWriter.WriteStartElement( UpdateUrlConfig.APPLICATION );		//--------Application---------

				xmlWriter.WriteStartElement( UpdateUrlConfig.APPLICATION_FILENAME );		//--------ApplicationFileName---------
				xmlWriter.WriteAttributeString( VALUE,info.ApplicationFileName );	//value;
				xmlWriter.WriteEndElement();										//--------ApplicationFileName---------


				xmlWriter.WriteStartElement( UpdateUrlConfig.PARAMETERS );		//--------Parameters---------
				xmlWriter.WriteAttributeString( VALUE,info.Parameters );	//value;
				xmlWriter.WriteEndElement();										//--------Parameters---------

				xmlWriter.WriteEndElement();										//--------Application---------


				xmlWriter.WriteEndElement();					//-----------configuration--
				xmlWriter.WriteEndDocument();					//-----------End------------

				#endregion

				xmlWriter.Close();

//				Global.WriteEncryptFile( strTmpFile,file );
			}
			finally
			{
//				File.Delete( strTmpFile );				
			}
		}


		#region 更新远程配置文件信息
		/// <summary>
		/// 更新远程配置文件信息
		/// </summary>
		/// <param name="info"></param>
		/// <param name="file">远程配置文件</param>
		#endregion
		public static void WriteUpdateFileConfig( UpdateFileConfig info,string file )
		{	
			CancelFileAttribute( file );
			XmlTextWriter xmlWriter = new XmlTextWriter( file,System.Text.Encoding.UTF8 );
			try
			{
				#region 写XML文件

				xmlWriter.WriteStartDocument();					//-----------Start----------		
				xmlWriter.WriteStartElement( ROOT_NODE );		//-----------configuration--

				xmlWriter.WriteStartElement( UpdateFileConfig.UPDATE_SETTING );	//--------UpdateSetting------
                xmlWriter.WriteAttributeString( VALUE,( info.UpdateSetting == true ) ? "1" : "0" );	// value
				xmlWriter.WriteEndElement();									//--------UpdateSetting------

				xmlWriter.WriteStartElement( UpdateFileConfig.UPDATE_MAIN_VERSION );//--------UpdateMainVersion------
				xmlWriter.WriteAttributeString( VALUE,info.UpdateMainVersion );	//value
				xmlWriter.WriteEndElement();										//--------UpdateMainVersion------
                
				xmlWriter.WriteStartElement( UpdateFileConfig.UPDATE_DATE );		//--------UpdateDate---------
				xmlWriter.WriteAttributeString( VALUE,info.UpdateDate.ToString( "F" ) );	//value;
				xmlWriter.WriteEndElement();										//--------UpdateDate---------

				xmlWriter.WriteStartElement( UpdateFileConfig.UPDATE_WEB_PATH );		//--------UpdateWebPath---------
				xmlWriter.WriteAttributeString( VALUE,info.UpdateWebPath );	//value;
				xmlWriter.WriteEndElement();										//--------UpdateWebPath---------

				xmlWriter.WriteStartElement( UpdateFileConfig.LICENCE_FILE );		//--------AgreementFile---------
				xmlWriter.WriteAttributeString( VALUE,info.LicenceFile );	//value;
				xmlWriter.WriteEndElement();										//--------AgreementFile---------


				xmlWriter.WriteStartElement( UpdateFileConfig.HISTORY_FILE );		//--------HistoryFile---------
				xmlWriter.WriteAttributeString( VALUE,info.HistoryFile );	//value;
				xmlWriter.WriteEndElement();										//--------HistoryFile---------



				xmlWriter.WriteStartElement( UpdateFileConfig.UPDATE_FILES );		//--------UpdateFiles--------

				foreach( FileDescription fileInfo in info.Files )
				{
					xmlWriter.WriteStartElement( UpdateFileConfig.FILE_DESCRIPTION );	//------FileDescription-----

					xmlWriter.WriteAttributeString( UpdateFileConfig.FILE_NAME,fileInfo.FileName );				//FileName
					xmlWriter.WriteAttributeString( UpdateFileConfig.FILE_SIZE,fileInfo.FileSize.ToString() );	//FileSize
					xmlWriter.WriteAttributeString( UpdateFileConfig.FILE_VERSION,fileInfo.FileVersion );			//FileVersion
					xmlWriter.WriteAttributeString( UpdateFileConfig.APPLICATIONS,fileInfo.Applications );			//Applications
					xmlWriter.WriteAttributeString( UpdateFileConfig.CLIENT_PATH,fileInfo.ClientPath );			//ClientPath

					xmlWriter.WriteEndElement();										//------FileDescription-----
				}

				xmlWriter.WriteEndElement();										//--------UpdateFiles--------

                xmlWriter.WriteEndElement();					//-----------configuration--
				xmlWriter.WriteEndDocument();					//-----------End------------

				#endregion

				xmlWriter.Close();
			}
			finally
			{
//				File.Delete( strTmpFile );				
			}
		}


		/// <summary>
		/// 比较本地与远程配置信息,获取应更新的文件列表
		/// </summary>
		/// <param name="remote"></param>
		/// <param name="local"></param>
		/// <returns></returns>
		public static ArrayList CompareLocalAndRemoteConfigInfo( UpdateFileConfig remote,UpdateFileConfig local )
		{
			AutoUpdater.UpdateHelper.Global.WriteUpdateLog( "/*********************************************/",true );
			AutoUpdater.UpdateHelper.Global.WriteUpdateLog( string.Format( "{0}:开始比较本地与远程配置信息,获取应更新的文件列表...",DateTime.Now ),true );
			FileDescription tmpFile = null;
			ArrayList alFiles = new ArrayList();
			string strPath = Global.AssemblyPath;
			string strTmp = string.Empty;

			foreach( FileDescription remoteFile in remote.Files )
			{
				//检测欲下载的文件在本地是否存在
				AutoUpdater.UpdateHelper.Global.WriteUpdateLog( string.Format( "待检测文件:{0}:{1}:{2}",remoteFile.FileName,remoteFile.ClientPath,remoteFile.ClientPath.Length == 0 ),true );
				if( remoteFile.ClientPath.Length == 0 )
				{
					strTmp = string.Format( "{0}\\{1}",strPath,remoteFile.FileName );
				}
				else
				{
					strTmp = string.Format( "{0}\\{1}\\{2}",strPath,remoteFile.ClientPath,remoteFile.FileName );
				}
				//文件存在
				AutoUpdater.UpdateHelper.Global.WriteUpdateLog( string.Format( "检测文件:{0}",strTmp ),true );
				//如果文件已经存在
				if( File.Exists( strTmp ) )
				{
					AutoUpdater.UpdateHelper.Global.WriteUpdateLog( string.Format( "文件:{0}存在.",strTmp ),true );
					tmpFile = new FileDescription();
					tmpFile.FileName = remoteFile.FileName;
					tmpFile.FileSize = (int)( new FileInfo( strTmp ) ).Length;
					tmpFile.FileVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo( strTmp ).FileVersion;
					//比较是否需要更新
					if(  tmpFile.CompareTo( remoteFile ) == 1 )
					{
						AutoUpdater.UpdateHelper.Global.WriteUpdateLog( string.Format( "文件:{0}不需更新.",remoteFile.FileName ),true );
						//不需更新
						continue;
					}
					else
					{
						AutoUpdater.UpdateHelper.Global.WriteUpdateLog( string.Format( "文件:{0}加入更新列表.",remoteFile.FileName ),true );
						alFiles.Add( remoteFile );
					}
				}
				else
				{
					AutoUpdater.UpdateHelper.Global.WriteUpdateLog( string.Format( "文件:{0}不存在,加入更新列表.",remoteFile.FileName ),true );
					alFiles.Add( remoteFile );
				}
			}
			AutoUpdater.UpdateHelper.Global.WriteUpdateLog( string.Format( "{0}:获取应更新的文件列表完成...",DateTime.Now ),true );
			AutoUpdater.UpdateHelper.Global.WriteUpdateLog( "/*********************************************/",true );

			return alFiles;
		}


		/// <summary>
		/// 解析需关闭的应用程序列表
		/// </summary>
		/// <param name="files">远程更新文件信息</param>
		/// <returns></returns>
		public static ArrayList ParseCloseApplications( UpdateFileConfig remote )
		{
			ArrayList alFiles = new ArrayList();

			string path = AssemblyPath;

			string[] arrString = null;

			string strTmp = string.Empty;

			foreach( FileDescription info in remote.Files )
			{
				if( info.Applications.Length > 0 )
				{
					//拆分字符数组
					arrString = info.Applications.Split( new char[]{ '|' } );
					for( int i = 0; i < arrString.Length; ++i )
					{
						//strTmp = string.Format( "{0}\\{1}",path,arrString[ i ] );
						strTmp = string.Format(arrString[i]);  //
						if ( !CheckStringExists( strTmp,alFiles ) )
						{
							alFiles.Add( strTmp );
						}
					}
				}
			}
			return alFiles;
		}


		/// <summary>
		/// 检查文件是否存在于指定的文件列表中
		/// </summary>
		/// <param name="file">检查的文件</param>
		/// <param name="files">文件列表</param>
		/// <returns></returns>
		public static bool CheckStringExists( string file,ArrayList files )
		{
			bool bExists = false;
			foreach( string info in files )
			{
				if( info == file )
				{
					bExists = true;
					break;
				}
			}
			return bExists;
		}
				

		/// <summary>
		/// 将文件大小从字节转换为KB
		/// </summary>
		/// <param name="lFieSize">文件大小</param>
		/// <returns></returns>
		public static long GetFileSizeOfKB( long lFileSize )
		{
			double size = lFileSize / 1024.0;
			return Convert.ToInt64( Math.Round( size,0 ) );
		}


		#region 加密字节流
		/// <summary>
		/// 加密字节流
		/// </summary>
		/// <param name="bStream">字节流</param>
		/// <returns></returns>
		#endregion
		public static byte[] EncryptStream( byte[] bStream )
		{
			MemoryStream memStream = new MemoryStream();

			RC2CryptoServiceProvider encry = new RC2CryptoServiceProvider();

			CryptoStream encryptStream = new CryptoStream( memStream,encry.CreateEncryptor( bytesKey,bytesIV ),CryptoStreamMode.Write );

			encryptStream.Write( bStream,0,bStream.Length );

			encryptStream.FlushFinalBlock();

			byte[] bEncryptStream = memStream.ToArray();

			encryptStream.Close(); 

			encry.Clear();

			memStream.Close();

			return bEncryptStream;
		}


		#region 解密字节流
		/// <summary>
		/// 解密字节流
		/// </summary>
		/// <param name="bStream">字节流</param>
		/// <returns></returns>
		#endregion
		public static byte[] DecryptStream( byte[] bStream )
		{
			MemoryStream memStream = new MemoryStream( bStream );  // 存储在内存的流

			RC2CryptoServiceProvider encry = new RC2CryptoServiceProvider();

			CryptoStream encryptStream = new CryptoStream( memStream,encry.CreateDecryptor( bytesKey,bytesIV ),CryptoStreamMode.Read );

			byte[] bEncryptStream = new byte[ bStream.Length ];

			encryptStream.Read( bEncryptStream,0,bEncryptStream.Length );

			encryptStream.Close();

			encry.Clear();

			memStream.Close();

			return bEncryptStream;
		}


		#region 将文件加密并写入到目标文件
		/// <summary>
		/// 将文件加密并写入到目标文件
		/// </summary>
		/// <param name="strSourceFile">待加密文件</param>
		/// <param name="strTargetFile">目标文件</param>
		#endregion
		public static void WriteEncryptFile( string strSourceFile,string strTargetFile )
		{			
			//读取临时文件
			FileStream fsRead = new FileStream( strSourceFile,FileMode.Open,FileAccess.Read,FileShare.None );

			byte[] bRead = new byte[ (int)fsRead.Length ];
			fsRead.Read( bRead,0,bRead.Length );

			byte[] bEncryptStream = EncryptStream( bRead );

			FileStream fsWrite = new FileStream( strTargetFile,FileMode.Create,FileAccess.Write,FileShare.None );
			fsWrite.Write( bEncryptStream,0,bEncryptStream.Length );
			fsWrite.Close();
            
			fsRead.Close();
			bRead = null;
			bEncryptStream = null;			
		}


		/// <summary>
		/// 将文件解密并写入到指定的文件
		/// </summary>
		/// <param name="strEncryptFile">加密文件</param>
		/// <param name="strTargetFile">目标文件</param>
		public static void WriteDecryptFile( string strEncryptFile,string strTargetFile )
		{
			//读取加密文件
			FileStream fsRead = new FileStream( strEncryptFile,FileMode.Open,FileAccess.Read,FileShare.None );

			byte[] bRead = new byte[ (int)fsRead.Length ];
			fsRead.Read( bRead,0,bRead.Length );

			byte[] bEncryptStream = DecryptStream( bRead );  // 解密字节流

			FileStream fsWrite = new FileStream( strTargetFile,FileMode.Create,FileAccess.Write,FileShare.None );
			fsWrite.Write( bEncryptStream,0,bEncryptStream.Length );
			fsWrite.Close();
            
			fsRead.Close();
			bRead = null;
			bEncryptStream = null;
		}


		/// <summary>
		/// 取消文件的只读和隐藏属性
		/// </summary>
		/// <param name="file"></param>
		public static void CancelFileAttribute( string file )
		{
			if( !File.Exists( file ) )
			{
				return;
			}
			FileInfo fileInfo = new FileInfo( file );
			if( ( fileInfo.Attributes & System.IO.FileAttributes.ReadOnly ) == System.IO.FileAttributes.ReadOnly )
			{
				fileInfo.Attributes -= System.IO.FileAttributes.ReadOnly;
			}
			if( ( fileInfo.Attributes & System.IO.FileAttributes.Hidden ) == System.IO.FileAttributes.Hidden )
			{
				fileInfo.Attributes -= System.IO.FileAttributes.Hidden;
			}

		}


		/// <summary>
		/// 添加更新日志
		/// </summary>
		/// <param name="strIntro"></param>
		public static void WriteUpdateLog( string strLog,bool bAppend )
		{
			StreamWriter sw = new StreamWriter( string.Format( "{0}\\{1}",Global.AssemblyPath,LOG_FILE ),bAppend,System.Text.Encoding.Default );
			sw.WriteLine( strLog );
			sw.Close();
		}

	}


	#region 本地更新配置信息
	/// <summary>
	/// 本地更新配置信息
	/// </summary>
	#endregion
	public class UpdateUrlConfig
	{
		#region 节点名称

		/// <summary>
		/// 更新列表地址
		/// </summary>
		public const string UPDATE_URL					= "UpdateUrl";

		/// <summary>
		/// 最后更新版本
		/// </summary>
		public const string LAST_VERSION				= "LastVersion";

		/// <summary>
		/// 最后更新日期
		/// </summary>
		public const string UPDATE_DATE					= "UpdateDate";

		/// <summary>
		/// 对应的本地更新文件
		/// </summary>
		public const string LOCAL_CONFIG_FILE			= "LocalConfigFile";

		/// <summary>
		/// 更新标题
		/// </summary>
		public const string UPDATE_TITLE				= "UpdateTitle";

		/// <summary>
		/// 更新完成后执行的应用程序
		/// </summary>
		public const string APPLICATION					= "Application";

		/// <summary>
		/// 应用程序名称
		/// </summary>
		public const string APPLICATION_FILENAME		= "ApplicationFileName";

		/// <summary>
		/// 应用程序的执行参数
		/// </summary>
		public const string PARAMETERS					= "Parameters";

		public const string SCHOOLID = "SchoolId";

		#endregion

		#region 字段

		/// <summary>
		/// 更新列表地址
		/// </summary>
		public string strUpdateUrl			= string.Empty;

		/// <summary>
		/// 最后更新版本
		/// </summary>
		public string strLastVersion		= string.Empty;

		/// <summary>
		/// 最后更新日期
		/// </summary>
		public DateTime dtUpdateDate		= DateTime.Now;

		/// <summary>
		/// 对应的本地更新文件
		/// </summary>
		public string strLocalConfigFile	= string.Empty;

		/// <summary>
		/// 更新显示标题
		/// </summary>
		public string strUpdateTitle		= string.Empty;

		/// <summary>
		/// 应用程序名称
		/// </summary>
		public string ApplicationFileName	= string.Empty;

		/// <summary>
		/// 应用程序的执行参数
		/// </summary>
		public string Parameters			= string.Empty;

		public string schoolId              = string.Empty;

		#endregion
		
	}


	#region 远程更新配置信息
	/// <summary>
	///  远程更新配置信息
	/// </summary>
	#endregion
	public class UpdateFileConfig
	{
		#region 节点名称

		/// <summary>
		/// 是否更新标识
		/// </summary>
		/// <remarks>
		///		0:不必更新,1:更新
		/// </remarks>
		public const string UPDATE_SETTING		= "UpdateSetting";

		/// <summary>
		/// 主程序版本
		/// </summary>
		public const string UPDATE_MAIN_VERSION	= "UpdateMainVersion";

		/// <summary>
		/// 最后更新日期
		/// </summary>
		public const string UPDATE_DATE			= "UpdateDate";

		/// <summary>
		/// 协议文件
		/// </summary>
		public const string LICENCE_FILE		= "LicenceFile";

		/// <summary>
		/// 更新历史记录文件
		/// </summary>
		public const string HISTORY_FILE		= "HistoryFile";

		/// <summary>
		/// 更新文件列表
		/// </summary>
		public const string UPDATE_FILES		= "UpdateFiles";

		/// <summary>
		/// 文件描述信息
		/// </summary>
		public const string FILE_DESCRIPTION	= "FileDescription";

		/// <summary>
		/// 更新的文件名称
		/// </summary>
		public const string FILE_NAME			= "FileName";

		/// <summary>
		/// 文件大小
		/// </summary>
		/// <remarks>
		///		单位为字节
		/// </remarks>
		public const string FILE_SIZE			= "FileSize";

		/// <summary>
		/// 文件版本
		/// </summary>
		public const string FILE_VERSION		= "FileVersion";

		/// <summary>
		/// 更新此文件是需关闭的应用程序名称
		/// </summary>
		/// <remarks>
		///	$代表当前路径
		/// </remarks>
		public const string APPLICATIONS		= "Applications";

		/// <summary>
		/// 此文件相对于客户端安装路径的存储目录
		/// </summary>
		/// <remarks>
		///	$代表客户端安装目录
		/// </remarks>
		public const string CLIENT_PATH			= "ClientPath";

		/// <summary>
		/// Web更新目录
		/// </summary>
		public const string UPDATE_WEB_PATH		= "UpdateWebPath";

		#endregion

		#region 字段

		/// <summary>
		/// 是否更新标识
		/// </summary>
		public bool UpdateSetting				= false;

		/// <summary>
		/// 主程序版本
		/// </summary>
		public string UpdateMainVersion			= string.Empty;

		/// <summary>
		/// 最后更新日期
		/// </summary>
		public DateTime UpdateDate				= DateTime.Now;

		/// <summary>
		/// Web更新目录
		/// </summary>
		public string UpdateWebPath				= string.Empty;

		/// <summary>
		/// 协议文件
		/// </summary>
		public string LicenceFile				= string.Empty;

		/// <summary>
		/// 更新历史记录文件
		/// </summary>
		public string HistoryFile				= string.Empty;

		/// <summary>
		/// 下载文件列表
		/// </summary>
		public ArrayList Files					= new ArrayList();

		#endregion
	}
	

	#region 更新文件详细信息
	/// <summary>
	/// 更新文件详细信息
	/// </summary>
	#endregion
	public class FileDescription : System.IComparable
	{
		#region 字段

		/// <summary>
		/// 更新的文件名称
		/// </summary>
		public string FileName			= string.Empty;

		/// <summary>
		/// 文件大小
		/// </summary>
		public int FileSize				= 0;

		/// <summary>
		/// 文件版本
		/// </summary>
		public string FileVersion		= string.Empty;

		/// <summary>
		/// 更新此文件是需关闭的应用程序名称
		/// </summary>
		public string Applications		= string.Empty;

		/// <summary>
		/// 此文件相对于客户端安装路径的存储目录
		/// </summary>
		public string ClientPath		= string.Empty;

		#endregion

		#region IComparable 成员

		#region 比较对象
		/// <summary>
		/// 比较对象
		/// </summary>
		/// <param name="obj">欲比较的对象</param>
		/// <remarks>
		///		0:false
		///		1:true
		/// </remarks>
		/// <returns></returns>
		#endregion
		public int CompareTo(object obj)
		{
			int nValue = 0;
			if( obj is FileDescription )
			{
				FileDescription target = (FileDescription)obj;
				//没有版本号,直接下载
				if( target.FileVersion.Length == 0 )
				{
						nValue = 1;
				}
				else if( target.FileName == this.FileName && target.FileSize == this.FileSize && target.FileVersion == this.FileVersion )
				{
					nValue = 1;
				}
			}
			return nValue;
		}


		#endregion

		public override string ToString()
		{
			return string.Format( "{0}({1})",this.FileName,this.FileSize );
		}
	}

	public class UpdateFiles
	{
		/// <summary>
		/// 
		/// </summary>
		public string fileName { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int fileSize { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string fileVersion { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string applications { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string clientPath { get; set; }
	}

	public class Data
	{
		/// <summary>
		/// 
		/// </summary>
		public List<UpdateFiles> updateFiles;
		/// <summary>
		/// 
		/// </summary>
		public int updateSetting { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string updateWebPath { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string updateMainVersion { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string updateDate { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string licenceFile { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string historyFile { get; set; }
	}

	public class Root
	{
		/// <summary>
		/// 
		/// </summary>
		public string result { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string msg { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string errorCode { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string errorMsg { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int total { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public Data data { get; set; }
	}
}
