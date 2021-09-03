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
	/// ͨ�ú���
	/// </summary>
	public sealed class Global
	{
		/// <summary>
		/// ���ڵ�����
		/// </summary>
		public const string ROOT_NODE		= "configuration";

		/// <summary>
		/// �����ļ���������
		/// </summary>
		public const string VALUE			= "Value";

		/// <summary>
		/// ������־�ļ�
		/// </summary>
		public const string LOG_FILE		= "Update.log";

		/// <summary>
		/// �Ƿ�д��־�ļ�
		/// </summary>
		public const bool bWriteLog = true;

		#region ��λ�����ֵ
		/// <summary>
		/// ��λ�����ֵ
		/// </summary>
		#endregion
		private const int XOR_VALUE						= 0x09;

		#region �Գ��㷨��ʼ�������ļ�����
		/// <summary>
		/// �Գ��㷨��ʼ�������ļ�����
		/// </summary>
		#endregion
		private const string IV_FILE					= "IV.bin";

		#region ���ݼ��ܱ�׼ (DES) �㷨�Ļ�����Կ�ļ�
		/// <summary>
		/// ���ݼ��ܱ�׼ (DES) �㷨�Ļ�����Կ�ļ�
		/// </summary>
		#endregion
		private const string KEY_FILE					= "Key.bin";	
	
		private static byte[] bIV = null;
		private static byte[] bKey = null;

		#region �Գ��㷨�ĳ�ʼ������
		/// <summary>
		/// �Գ��㷨�ĳ�ʼ������
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


		#region ���ݼ��ܱ�׼ (DES) �㷨�Ļ�����Կ
		/// <summary>
		/// ���ݼ��ܱ�׼ (DES) �㷨�Ļ�����Կ
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


		#region ���ֽڽ����м��ܻ����
		/// <summary>
		/// ���ֽڽ����м��ܻ����
		/// </summary>
		/// <param name="bStream">���������ֽ���</param>
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


		#region ��ȡ�ļ�
		/// <summary>
		/// ��ȡ�ļ�
		/// </summary>
		/// <param name="strFile">�ļ�����</param>
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


		#region ���ַ�������MD5����
		/// <summary>
		/// ���ַ�������MD5����
		/// </summary>
		/// <param name="bInput">�����ܵ��ֽ���</param>
		/// <returns></returns>
		#endregion
		public static byte[] MD5Encrypt( byte[] bInput )
		{
			MD5CryptoServiceProvider encrypt = new MD5CryptoServiceProvider();
			byte[] bytes = encrypt.ComputeHash( bInput );
			return bytes;
		}


		/// <summary>
		/// ���ַ������б�׼MD5����
		/// </summary>
		/// <param name="strEncrypt">�����ܵ��ַ���</param>
		/// <returns></returns>
		public static string MD5StandEncrypt( string strEncrypt )
		{
			string strMD5 = BitConverter.ToString( MD5Encrypt( System.Text.Encoding.Default.GetBytes( strEncrypt ) ) );
			return strMD5.Replace( "-","" ).ToLower();
		}


		#region ��ȡ�ļ�
		/// <summary>
		/// ��ȡ�ļ�
		/// </summary>
		/// <param name="file">�ļ�����·��</param>
		/// <returns>�Զ����Ƶ���ʽ�����ļ���С</returns>
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
		/// ��ǰӦ�ó������ڵ�·��
		/// </summary>
		public static string AssemblyPath
		{
			get{ return Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location );}
		}
		

		/// <summary>
		/// ��ȡָ��Ŀ�������ļ���
		/// </summary>
		/// <param name="strPath">Ŀ������</param>
		/// <param name="strPrefix">�ļ���ǰ׺</param>
		/// <param name="strExt">�ļ���չ��</param>
		/// <remarks>
		///		��ָ��Ŀ�겻���ڣ��򴴽�
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
		/// ��ȡָ����Χ�ڵ������
		/// </summary>
		/// <param name="min">��Сֵ</param>
		/// <param name="max">���ֵ</param>
		/// <returns></returns>
		public static int RandomValue( int min,int max )
		{
			System.Random rnd = new Random( (int)DateTime.Now.Ticks );
			return rnd.Next( min,max );
		}


		/// <summary>
		/// �������ظ��������ļ�
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
					throw new Exception( "���ظ��������ļ��𻵣��޷���ȡ���ݣ�" );
				}
				objResult.strUpdateTitle	= rootNode.ChildNodes[0].Attributes[ VALUE ].InnerText;
				objResult.strUpdateUrl		= rootNode.ChildNodes[1].Attributes[ VALUE ].InnerText;
				objResult.strLastVersion	= rootNode.ChildNodes[2].Attributes[ VALUE ].InnerText;
				objResult.dtUpdateDate		= Convert.ToDateTime( rootNode.ChildNodes[3].Attributes[ VALUE ].InnerText );
				objResult.strLocalConfigFile= rootNode.ChildNodes[4].Attributes[ VALUE ].InnerText;
				objResult.schoolId          = rootNode.ChildNodes[5].Attributes[VALUE].InnerText;
				if ( rootNode.ChildNodes[6].ChildNodes.Count != 2)  // <Application>
				{
					throw new Exception( "���ظ��������ļ��𻵣��޷���ȡ���ݣ�" );
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


		#region ����Զ�̸���������Ϣ
		/// <summary>
		/// ����Զ�̸���������Ϣ
		/// </summary>
		/// <param name="xmlfile">�����ļ�������·��</param>
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
					throw new Exception( "Զ�̸��������ļ��𻵣�" );
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
				Console.WriteLine("���ļ�����xml��ʽ������ʹ��json����");
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
								fileDescription.Applications = "KPScanClient.exe";  // �����򣬱�ʾ����ʱҪֹͣ�ĳ���
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
					AutoUpdater.UpdateHelper.Global.WriteUpdateLog(string.Format("{0}:Զ�̷������쳣�����Ժ����ԡ�", DateTime.Now), true);
				}
            }
			finally
			{
				xmlDoc = null;
			}
			return objResult;
		}


		#region ����Զ�̸����ļ���Ϣ
		/// <summary>
		/// ����Զ�̸����ļ���Ϣ
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		#endregion
		public static FileDescription ParseFileDescription( XmlNode node )
		{
			if( node.Attributes.Count != 5 )
			{
				throw new Exception( "Զ�̸��������ļ��𻵣��޷���ȡ���ݣ�" );
			}
			FileDescription info = new FileDescription();
			info.FileName		= node.Attributes[ UpdateFileConfig.FILE_NAME ].InnerText;
			info.FileSize		= Convert.ToInt32( node.Attributes[ UpdateFileConfig.FILE_SIZE ].InnerText );
			info.FileVersion	= node.Attributes[ UpdateFileConfig.FILE_VERSION ].InnerText;
			info.Applications	= node.Attributes[ UpdateFileConfig.APPLICATIONS ].InnerText;
			info.ClientPath		= node.Attributes[ UpdateFileConfig.CLIENT_PATH ].InnerText;
			return info;
		}


		#region ���±��������ļ���Ϣ
		/// <summary>
		/// ���±��������ļ���Ϣ
		/// </summary>
		/// <param name="info"></param>
		/// <param name="file">���ظ��������ļ�</param>
		#endregion
		public static void WriteUpdateUrlFile( UpdateUrlConfig info,string file )
		{
//			string strTmpFile = Global.GetRandomFile( AssemblyPath,string.Empty,"bin" );
			CancelFileAttribute( file );
			XmlTextWriter xmlWriter = new XmlTextWriter( file,System.Text.Encoding.UTF8 );
			try
			{
				#region дXML�ļ�

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


		#region ����Զ�������ļ���Ϣ
		/// <summary>
		/// ����Զ�������ļ���Ϣ
		/// </summary>
		/// <param name="info"></param>
		/// <param name="file">Զ�������ļ�</param>
		#endregion
		public static void WriteUpdateFileConfig( UpdateFileConfig info,string file )
		{	
			CancelFileAttribute( file );
			XmlTextWriter xmlWriter = new XmlTextWriter( file,System.Text.Encoding.UTF8 );
			try
			{
				#region дXML�ļ�

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
		/// �Ƚϱ�����Զ��������Ϣ,��ȡӦ���µ��ļ��б�
		/// </summary>
		/// <param name="remote"></param>
		/// <param name="local"></param>
		/// <returns></returns>
		public static ArrayList CompareLocalAndRemoteConfigInfo( UpdateFileConfig remote,UpdateFileConfig local )
		{
			AutoUpdater.UpdateHelper.Global.WriteUpdateLog( "/*********************************************/",true );
			AutoUpdater.UpdateHelper.Global.WriteUpdateLog( string.Format( "{0}:��ʼ�Ƚϱ�����Զ��������Ϣ,��ȡӦ���µ��ļ��б�...",DateTime.Now ),true );
			FileDescription tmpFile = null;
			ArrayList alFiles = new ArrayList();
			string strPath = Global.AssemblyPath;
			string strTmp = string.Empty;

			foreach( FileDescription remoteFile in remote.Files )
			{
				//��������ص��ļ��ڱ����Ƿ����
				AutoUpdater.UpdateHelper.Global.WriteUpdateLog( string.Format( "������ļ�:{0}:{1}:{2}",remoteFile.FileName,remoteFile.ClientPath,remoteFile.ClientPath.Length == 0 ),true );
				if( remoteFile.ClientPath.Length == 0 )
				{
					strTmp = string.Format( "{0}\\{1}",strPath,remoteFile.FileName );
				}
				else
				{
					strTmp = string.Format( "{0}\\{1}\\{2}",strPath,remoteFile.ClientPath,remoteFile.FileName );
				}
				//�ļ�����
				AutoUpdater.UpdateHelper.Global.WriteUpdateLog( string.Format( "����ļ�:{0}",strTmp ),true );
				//����ļ��Ѿ�����
				if( File.Exists( strTmp ) )
				{
					AutoUpdater.UpdateHelper.Global.WriteUpdateLog( string.Format( "�ļ�:{0}����.",strTmp ),true );
					tmpFile = new FileDescription();
					tmpFile.FileName = remoteFile.FileName;
					tmpFile.FileSize = (int)( new FileInfo( strTmp ) ).Length;
					tmpFile.FileVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo( strTmp ).FileVersion;
					//�Ƚ��Ƿ���Ҫ����
					if(  tmpFile.CompareTo( remoteFile ) == 1 )
					{
						AutoUpdater.UpdateHelper.Global.WriteUpdateLog( string.Format( "�ļ�:{0}�������.",remoteFile.FileName ),true );
						//�������
						continue;
					}
					else
					{
						AutoUpdater.UpdateHelper.Global.WriteUpdateLog( string.Format( "�ļ�:{0}��������б�.",remoteFile.FileName ),true );
						alFiles.Add( remoteFile );
					}
				}
				else
				{
					AutoUpdater.UpdateHelper.Global.WriteUpdateLog( string.Format( "�ļ�:{0}������,��������б�.",remoteFile.FileName ),true );
					alFiles.Add( remoteFile );
				}
			}
			AutoUpdater.UpdateHelper.Global.WriteUpdateLog( string.Format( "{0}:��ȡӦ���µ��ļ��б����...",DateTime.Now ),true );
			AutoUpdater.UpdateHelper.Global.WriteUpdateLog( "/*********************************************/",true );

			return alFiles;
		}


		/// <summary>
		/// ������رյ�Ӧ�ó����б�
		/// </summary>
		/// <param name="files">Զ�̸����ļ���Ϣ</param>
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
					//����ַ�����
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
		/// ����ļ��Ƿ������ָ�����ļ��б���
		/// </summary>
		/// <param name="file">�����ļ�</param>
		/// <param name="files">�ļ��б�</param>
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
		/// ���ļ���С���ֽ�ת��ΪKB
		/// </summary>
		/// <param name="lFieSize">�ļ���С</param>
		/// <returns></returns>
		public static long GetFileSizeOfKB( long lFileSize )
		{
			double size = lFileSize / 1024.0;
			return Convert.ToInt64( Math.Round( size,0 ) );
		}


		#region �����ֽ���
		/// <summary>
		/// �����ֽ���
		/// </summary>
		/// <param name="bStream">�ֽ���</param>
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


		#region �����ֽ���
		/// <summary>
		/// �����ֽ���
		/// </summary>
		/// <param name="bStream">�ֽ���</param>
		/// <returns></returns>
		#endregion
		public static byte[] DecryptStream( byte[] bStream )
		{
			MemoryStream memStream = new MemoryStream( bStream );  // �洢���ڴ����

			RC2CryptoServiceProvider encry = new RC2CryptoServiceProvider();

			CryptoStream encryptStream = new CryptoStream( memStream,encry.CreateDecryptor( bytesKey,bytesIV ),CryptoStreamMode.Read );

			byte[] bEncryptStream = new byte[ bStream.Length ];

			encryptStream.Read( bEncryptStream,0,bEncryptStream.Length );

			encryptStream.Close();

			encry.Clear();

			memStream.Close();

			return bEncryptStream;
		}


		#region ���ļ����ܲ�д�뵽Ŀ���ļ�
		/// <summary>
		/// ���ļ����ܲ�д�뵽Ŀ���ļ�
		/// </summary>
		/// <param name="strSourceFile">�������ļ�</param>
		/// <param name="strTargetFile">Ŀ���ļ�</param>
		#endregion
		public static void WriteEncryptFile( string strSourceFile,string strTargetFile )
		{			
			//��ȡ��ʱ�ļ�
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
		/// ���ļ����ܲ�д�뵽ָ�����ļ�
		/// </summary>
		/// <param name="strEncryptFile">�����ļ�</param>
		/// <param name="strTargetFile">Ŀ���ļ�</param>
		public static void WriteDecryptFile( string strEncryptFile,string strTargetFile )
		{
			//��ȡ�����ļ�
			FileStream fsRead = new FileStream( strEncryptFile,FileMode.Open,FileAccess.Read,FileShare.None );

			byte[] bRead = new byte[ (int)fsRead.Length ];
			fsRead.Read( bRead,0,bRead.Length );

			byte[] bEncryptStream = DecryptStream( bRead );  // �����ֽ���

			FileStream fsWrite = new FileStream( strTargetFile,FileMode.Create,FileAccess.Write,FileShare.None );
			fsWrite.Write( bEncryptStream,0,bEncryptStream.Length );
			fsWrite.Close();
            
			fsRead.Close();
			bRead = null;
			bEncryptStream = null;
		}


		/// <summary>
		/// ȡ���ļ���ֻ������������
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
		/// ��Ӹ�����־
		/// </summary>
		/// <param name="strIntro"></param>
		public static void WriteUpdateLog( string strLog,bool bAppend )
		{
			StreamWriter sw = new StreamWriter( string.Format( "{0}\\{1}",Global.AssemblyPath,LOG_FILE ),bAppend,System.Text.Encoding.Default );
			sw.WriteLine( strLog );
			sw.Close();
		}

	}


	#region ���ظ���������Ϣ
	/// <summary>
	/// ���ظ���������Ϣ
	/// </summary>
	#endregion
	public class UpdateUrlConfig
	{
		#region �ڵ�����

		/// <summary>
		/// �����б��ַ
		/// </summary>
		public const string UPDATE_URL					= "UpdateUrl";

		/// <summary>
		/// �����°汾
		/// </summary>
		public const string LAST_VERSION				= "LastVersion";

		/// <summary>
		/// ����������
		/// </summary>
		public const string UPDATE_DATE					= "UpdateDate";

		/// <summary>
		/// ��Ӧ�ı��ظ����ļ�
		/// </summary>
		public const string LOCAL_CONFIG_FILE			= "LocalConfigFile";

		/// <summary>
		/// ���±���
		/// </summary>
		public const string UPDATE_TITLE				= "UpdateTitle";

		/// <summary>
		/// ������ɺ�ִ�е�Ӧ�ó���
		/// </summary>
		public const string APPLICATION					= "Application";

		/// <summary>
		/// Ӧ�ó�������
		/// </summary>
		public const string APPLICATION_FILENAME		= "ApplicationFileName";

		/// <summary>
		/// Ӧ�ó����ִ�в���
		/// </summary>
		public const string PARAMETERS					= "Parameters";

		public const string SCHOOLID = "SchoolId";

		#endregion

		#region �ֶ�

		/// <summary>
		/// �����б��ַ
		/// </summary>
		public string strUpdateUrl			= string.Empty;

		/// <summary>
		/// �����°汾
		/// </summary>
		public string strLastVersion		= string.Empty;

		/// <summary>
		/// ����������
		/// </summary>
		public DateTime dtUpdateDate		= DateTime.Now;

		/// <summary>
		/// ��Ӧ�ı��ظ����ļ�
		/// </summary>
		public string strLocalConfigFile	= string.Empty;

		/// <summary>
		/// ������ʾ����
		/// </summary>
		public string strUpdateTitle		= string.Empty;

		/// <summary>
		/// Ӧ�ó�������
		/// </summary>
		public string ApplicationFileName	= string.Empty;

		/// <summary>
		/// Ӧ�ó����ִ�в���
		/// </summary>
		public string Parameters			= string.Empty;

		public string schoolId              = string.Empty;

		#endregion
		
	}


	#region Զ�̸���������Ϣ
	/// <summary>
	///  Զ�̸���������Ϣ
	/// </summary>
	#endregion
	public class UpdateFileConfig
	{
		#region �ڵ�����

		/// <summary>
		/// �Ƿ���±�ʶ
		/// </summary>
		/// <remarks>
		///		0:���ظ���,1:����
		/// </remarks>
		public const string UPDATE_SETTING		= "UpdateSetting";

		/// <summary>
		/// ������汾
		/// </summary>
		public const string UPDATE_MAIN_VERSION	= "UpdateMainVersion";

		/// <summary>
		/// ����������
		/// </summary>
		public const string UPDATE_DATE			= "UpdateDate";

		/// <summary>
		/// Э���ļ�
		/// </summary>
		public const string LICENCE_FILE		= "LicenceFile";

		/// <summary>
		/// ������ʷ��¼�ļ�
		/// </summary>
		public const string HISTORY_FILE		= "HistoryFile";

		/// <summary>
		/// �����ļ��б�
		/// </summary>
		public const string UPDATE_FILES		= "UpdateFiles";

		/// <summary>
		/// �ļ�������Ϣ
		/// </summary>
		public const string FILE_DESCRIPTION	= "FileDescription";

		/// <summary>
		/// ���µ��ļ�����
		/// </summary>
		public const string FILE_NAME			= "FileName";

		/// <summary>
		/// �ļ���С
		/// </summary>
		/// <remarks>
		///		��λΪ�ֽ�
		/// </remarks>
		public const string FILE_SIZE			= "FileSize";

		/// <summary>
		/// �ļ��汾
		/// </summary>
		public const string FILE_VERSION		= "FileVersion";

		/// <summary>
		/// ���´��ļ�����رյ�Ӧ�ó�������
		/// </summary>
		/// <remarks>
		///	$����ǰ·��
		/// </remarks>
		public const string APPLICATIONS		= "Applications";

		/// <summary>
		/// ���ļ�����ڿͻ��˰�װ·���Ĵ洢Ŀ¼
		/// </summary>
		/// <remarks>
		///	$����ͻ��˰�װĿ¼
		/// </remarks>
		public const string CLIENT_PATH			= "ClientPath";

		/// <summary>
		/// Web����Ŀ¼
		/// </summary>
		public const string UPDATE_WEB_PATH		= "UpdateWebPath";

		#endregion

		#region �ֶ�

		/// <summary>
		/// �Ƿ���±�ʶ
		/// </summary>
		public bool UpdateSetting				= false;

		/// <summary>
		/// ������汾
		/// </summary>
		public string UpdateMainVersion			= string.Empty;

		/// <summary>
		/// ����������
		/// </summary>
		public DateTime UpdateDate				= DateTime.Now;

		/// <summary>
		/// Web����Ŀ¼
		/// </summary>
		public string UpdateWebPath				= string.Empty;

		/// <summary>
		/// Э���ļ�
		/// </summary>
		public string LicenceFile				= string.Empty;

		/// <summary>
		/// ������ʷ��¼�ļ�
		/// </summary>
		public string HistoryFile				= string.Empty;

		/// <summary>
		/// �����ļ��б�
		/// </summary>
		public ArrayList Files					= new ArrayList();

		#endregion
	}
	

	#region �����ļ���ϸ��Ϣ
	/// <summary>
	/// �����ļ���ϸ��Ϣ
	/// </summary>
	#endregion
	public class FileDescription : System.IComparable
	{
		#region �ֶ�

		/// <summary>
		/// ���µ��ļ�����
		/// </summary>
		public string FileName			= string.Empty;

		/// <summary>
		/// �ļ���С
		/// </summary>
		public int FileSize				= 0;

		/// <summary>
		/// �ļ��汾
		/// </summary>
		public string FileVersion		= string.Empty;

		/// <summary>
		/// ���´��ļ�����رյ�Ӧ�ó�������
		/// </summary>
		public string Applications		= string.Empty;

		/// <summary>
		/// ���ļ�����ڿͻ��˰�װ·���Ĵ洢Ŀ¼
		/// </summary>
		public string ClientPath		= string.Empty;

		#endregion

		#region IComparable ��Ա

		#region �Ƚ϶���
		/// <summary>
		/// �Ƚ϶���
		/// </summary>
		/// <param name="obj">���ȽϵĶ���</param>
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
				//û�а汾��,ֱ������
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
