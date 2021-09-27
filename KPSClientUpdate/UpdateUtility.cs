using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Reflection;
using System.Xml;

namespace AutoUpdater.UpdateHelper
{
	/// <summary>
	/// ִ�и���
	/// </summary>
	/// <remarks>
	///		������ʱ�ļ���Ŀ¼Ϊ:System.Environment.GetFolderPath( System.Environment.SpecialFolder.ApplicationData )
	///		����ǰ��½�û���Application DataĿ¼
	/// </remarks>
	public class UpdateUtility
	{
		#region �¼�

		/// <summary>
		/// ��ʼ��������Ϣ����¼�
		/// </summary>
		/// <remarks>
		///		���¼�������ArrayList��ʾ��Ӧ�رյĹ���Ӧ�ó����б�
		/// </remarks>
		public event ArrayListEventHandler InializationDownloadInfoComplete;

		/// <summary>
		/// ����Э����������¼�
		/// </summary>
		/// <remarks>
		///		���¼�����Э������
		/// </remarks>
		public event TextEventHandler DownloadLicenceInfoComplete;

		/// <summary>
		/// ���ظ��¼�¼��Ϣ����¼�
		/// </summary>
		/// <remarks>
		///		���¼����ݸ�����ʷ��¼����
		/// </remarks>
		public event TextEventHandler DownloadHistoryInfoComplete;

		/// <summary>
		/// ׼�������ļ��¼�
		/// </summary>
		/// <remarks>
		///		 ���¼����ݴ������ļ��б�,����ΪFileDescription�ļ���
		/// </remarks>
		public event ArrayListEventHandler PrepareDownloadFiles;

		/// <summary>
		/// ׼����ʼ�����ļ��¼�
		/// </summary>
		/// <remarks>
		///		����׼�������ļ��Ĵ�С������
		/// </remarks>
		public event DownloadFileInitEventHandler InitlizationDownloadFile;

		/// <summary>
		/// �����ļ������
		/// </summary>
		/// <remarks>
		///		���������ļ��Ĵ�С����ǰ���ش�С
		/// </remarks>
		public event DownloadFileEventHandler DownloadFileBlockComplete;

        /// <summary>
        /// �����ļ����
        /// </summary>
        /// <remarks>
        ///		����������ɵ��ļ�����
        /// </remarks>
		public event TextEventHandler DownloadFileComplete;

		/// <summary>
		/// ���ظ����ļ�����
		/// </summary>
		public event EventHandler DownloadFileOver;

		/// <summary>
		/// ���±����ļ����
		/// </summary>
		public event EventHandler UpdateLocalFilesComplete;

		/// <summary>
		/// ���±���������Ϣ���
		/// </summary>
		public event EventHandler UpdateLocalConfigInfoComplete;

		/// <summary>
		/// ɾ����ʱ�ļ����
		/// </summary>
		public event EventHandler  DeleteTempFileComplete;

		/// <summary>
		/// �������
		/// </summary>
		public event EventHandler UpdateComplete;

		#endregion

		#region �ֶ�

		/// <summary>
		/// ���ظ���������Ϣ�ļ�
		/// </summary>
		private string strUpdateUrlFile;

		/// <summary>
		/// Զ�̸��������ļ�����
		/// </summary>
		private string strRemoteFile;

		/// <summary>
		/// ����Զ�̸����ļ�����ʱ�ļ�
		/// </summary>
		private string strTmpRemoteFile;
	
		/// <summary>
		/// �߳��¼�����
		/// </summary>
		private System.Threading.ManualResetEvent alDone;

		/// <summary>
		/// �Ƿ�ȡ��������
		/// </summary>
		private bool bCancel;

		/// <summary>
		/// ���ظ���������Ϣ
		/// </summary>
		private UpdateUrlConfig localUpdateConfigInfo;

		/// <summary>
		/// ���ظ�����Ϣ
		/// </summary>
		private UpdateFileConfig localUpdateInfo;

		/// <summary>
		/// Զ�̸�����Ϣ
		/// </summary>
		private UpdateFileConfig remoteUpdateInfo;

		/// <summary>
		/// �����ļ�����
		/// </summary>
		private DownloadFile objDownload;

		/// <summary>
		/// ���ش洢�ļ�����ʱĿ¼
		/// </summary>
		private string strLocalTempPath;

		/// <summary>
		/// ���صĸ����ļ��б�
		/// </summary>
		private ArrayList alUpdateFiles;

		/// <summary>
		/// �Ƿ���Ҫ����
		/// </summary>
		private bool bEnable;

		/// <summary>
		/// ����Ƿ��������ظ����ļ�
		/// </summary>
		/// <remarks>
		///		�Դ�ֵ��ȷ���Ƿ�����DownloadFileBlock�¼�
		/// </remarks>
		private bool bDownloadFiles;

		#endregion

		#region ���캯��
		/// <summary>
		/// ���캯��
		/// </summary>
		/// <param name="configFile">�����ļ�����</param>
		/// <param name="remoteFile">Զ�̸����ļ����ڱ����ڱ��ص��ļ�����</param>
		/// <remarks>
		///		���������ļ�������·��
		/// </remarks>
		#endregion
		public UpdateUtility( string configFile )
		{
			this.alUpdateFiles = new ArrayList();
			this.strUpdateUrlFile = configFile;
			this.alDone = new ManualResetEvent( true );
			this.bCancel = false;
			this.strLocalTempPath = Path.GetTempPath();
			this.strTmpRemoteFile = string.Empty;
			this.objDownload = new DownloadFile();
			this.objDownload.DownloadFileComplete += new EventHandler(objDownload_DownloadFileComplete);
			this.objDownload.DownloadFileBlock += new DownloadFileEventHandler(objDownload_DownloadFileBlock);
			this.bEnable = false;
			this.bDownloadFiles = false;
		}


		#region ����

		/// <summary>
		/// �Ƿ���Ҫ����
		/// </summary>
		public bool Enable
		{
			get{ return this.bEnable; }
		}
		

		/// <summary>
		/// Զ�̸�����Ϣ
		/// </summary>
		public UpdateFileConfig RemoteConfigInfo
		{
			get{ return this.remoteUpdateInfo; }
		}


		/// <summary>
		/// ���ظ���������Ϣ
		/// </summary>
		public UpdateUrlConfig LocalUpdateUrlConfigInfo
		{
			get
			{
				return this.localUpdateConfigInfo;
			}
		}

		#endregion

		#region ����

		/// <summary>
		/// ��һ������ʼ��������Ϣ
		/// </summary>
		/// <remarks>
		///		��ȡ���ظ���������Ϣ����ȡԶ�̷������ĸ��������ļ�������ȡԶ�̸���������Ϣ
		/// </remarks>
		public void InitUpdateInfo()
		{
			Global.WriteUpdateLog( string.Format( "{0}:��ʼ��������Ϣ...",DateTime.Now ),true );
			//��ȡ���ظ���������Ϣ
			this.localUpdateConfigInfo = Global.ParseUpdateUrlConfig( this.strUpdateUrlFile );

			//����Զ�̸��������ļ��ı��ظ���
			//this.strRemoteFile = string.Format( "{0}\\{1}",Global.AssemblyPath,this.localUpdateConfigInfo.strLocalConfigFile );

			//if( File.Exists( this.strRemoteFile ) )
			//{				
			//	//��ȡ���ش洢��Զ�̸���������Ϣ
			//	this.localUpdateInfo = Global.ParseUpdateFileConfig( this.strRemoteFile );
			//}
			//else
			//{
			//	//������Զ�̸���������Ϣ�������������
			//	this.bEnable = true;
			//}
			
			this.strTmpRemoteFile = Global.GetRandomFile( strLocalTempPath,string.Empty,"bin" );

			//��ȡԶ�̸����ļ���Ϣ
			//��Զ�̷���������Զ�̸����ļ���Ϣ			
			this.objDownload.DownloadUrl = this.localUpdateConfigInfo.strUpdateUrl + "api/webcloud/app/getLatestAppPath?schoolId=" + this.localUpdateConfigInfo.schoolId + "&localVersion=" + this.localUpdateConfigInfo.strLastVersion;  // �ļ�������Ϣ
			this.objDownload.DownloadFileName = strTmpRemoteFile;
			this.objDownload.strAuthorization = this.localUpdateConfigInfo.Parameters;  // token

			this.alDone.Reset();
			
			this.objDownload.Download();  // ����Զ�̸���������Ϣ����ʱ��

			this.alDone.WaitOne();	//�ȴ��߳�ֱ�����

			this.remoteUpdateInfo = Global.ParseUpdateFileConfig( strTmpRemoteFile );  //Զ�̸���������Ϣ

            //����Ƿ�������
            //if ( !this.bEnable && this.localUpdateInfo != null )
            //{
            //	if( this.localUpdateInfo.UpdateMainVersion != this.remoteUpdateInfo.UpdateMainVersion )
            //	{
            //		this.bEnable = true;
            //	}
            //	if( this.localUpdateConfigInfo.strLastVersion != this.remoteUpdateInfo.UpdateMainVersion )
            //	{
            //		this.bEnable = true;
            //	}
            //}
            if (this.localUpdateConfigInfo.strLastVersion != this.remoteUpdateInfo.UpdateMainVersion)
            {
				// �Ƚϰ汾�ţ�������ذ�С�ڷ�������
				Version local = new Version(this.localUpdateConfigInfo.strLastVersion);
				Version remote = new Version(this.remoteUpdateInfo.UpdateMainVersion);
				if (local < remote)
				{
					AutoUpdater.UpdateHelper.Global.WriteUpdateLog(string.Format("{0}:���ذ汾�Ų������£���Ҫ���¡�", DateTime.Now), true);
					this.bEnable = true;
				}
            }
            else
            {
				AutoUpdater.UpdateHelper.Global.WriteUpdateLog(string.Format("{0}:���ذ汾������¡�", DateTime.Now), true);
			}

            //����������ʼ��ɣ������¼�֪ͨ��������رյ�UI�����б�,Ȼ����UI�رչ�����Ӧ�ó���
            if ( this.InializationDownloadInfoComplete != null )
			{
				this.InializationDownloadInfoComplete( new ArrayListEventArgs( Global.ParseCloseApplications( this.remoteUpdateInfo ) ) );
			}

			//ɾ������Զ�̸����ļ�����ʱ�ļ�
			File.Delete( this.strTmpRemoteFile );
		}


		/// <summary>
		/// �ڶ�������ʾЭ����Ϣ
		/// </summary>
		public void DisplayLicenceInfo()
		{
			Global.WriteUpdateLog( string.Format( "{0}:��ʾЭ����Ϣ...",DateTime.Now ),true );
			//���û��Э����Ϣ������������
			string strAgreementInfo = string.Empty;
			if( this.remoteUpdateInfo.LicenceFile.Length > 0 )
			{
				//����Э����Ϣ����ʱ�ļ�
				string strTmpAgreementFile = Global.GetRandomFile( this.strLocalTempPath,string.Empty,"tmp" );
				try
				{
					//��Զ�̷���������Э����Ϣ
					this.objDownload.DownloadUrl = string.Format( "{0}/{1}",this.remoteUpdateInfo.UpdateWebPath,this.remoteUpdateInfo.LicenceFile );
					this.objDownload.DownloadFileName = strTmpAgreementFile;

					this.alDone.Reset();
					//�����߳������ļ�
					this.objDownload.Download();
					this.alDone.WaitOne();	//�ȴ��߳�ֱ�����

					//��ȡЭ����Ϣ
					StreamReader sr = new StreamReader( strTmpAgreementFile,System.Text.Encoding.Default );
					strAgreementInfo = sr.ReadToEnd();
					sr.Close();
				}
				finally
				{
					//ɾ������Э����Ϣ����ʱ�ļ�
					File.Delete( strTmpAgreementFile );
				}
			}
			//������ʾЭ����Ϣ�¼�
			if( this.DownloadLicenceInfoComplete != null )
			{
				this.DownloadLicenceInfoComplete( new TextEventArgs( strAgreementInfo ) );
			}
		}


		/// <summary>
		/// ����������ʾ������ʷ��¼��Ϣ
		/// </summary>
		public void DisplayHistoryInfo()
		{
			Global.WriteUpdateLog( string.Format( "{0}:��ʾ������ʷ��¼��Ϣ...",DateTime.Now ),true );
			//���û��Э����Ϣ������������
			string strHistoryInfo = string.Empty;
			if( this.remoteUpdateInfo.HistoryFile.Length > 0 )
			{
				//����Э����Ϣ����ʱ�ļ�
				string strTmpFile = Global.GetRandomFile( strLocalTempPath,string.Empty,"tmp" );
				try
				{
					//��Զ�̷���������Э����Ϣ
					this.objDownload.DownloadUrl = string.Format( "{0}/{1}",this.remoteUpdateInfo.UpdateWebPath,this.remoteUpdateInfo.HistoryFile );
					this.objDownload.DownloadFileName = strTmpFile;
					this.alDone.Reset();
					//�����߳������ļ�
					this.objDownload.Download();
//					new Thread( new ThreadStart( objDownload.Download ) ).Start();
					this.alDone.WaitOne();	//�ȴ��߳�ֱ�����
					//��ȡЭ����Ϣ
					StreamReader sr = new StreamReader( strTmpFile,System.Text.Encoding.Default );
					strHistoryInfo = sr.ReadToEnd();
					sr.Close();
				}
				finally
				{
					//ɾ������Э����Ϣ����ʱ�ļ�
					File.Delete( strTmpFile );
				}
			}
            else
            {
				strHistoryInfo = "������ʷ������Ϣ";

			}
			//������ʾ������ʷ��¼��Ϣ�¼�
			if( this.DownloadHistoryInfoComplete != null )
			{
				this.DownloadHistoryInfoComplete( new TextEventArgs( strHistoryInfo ) );
			}
		}


		/// <summary>
		/// ���Ĳ������ظ����ļ�
		/// </summary>
		public void DownloadUpdateFiles()
		{
			Global.WriteUpdateLog( string.Format( "{0}:���ظ����ļ�...",DateTime.Now ),true );

			//��ȡ�����ص��ļ��б�			
			if( this.localUpdateInfo != null )	
			{
				// �Աȱ��غͷ������ļ����죨ȫ�����²���Ҫ��
				this.alUpdateFiles = Global.CompareLocalAndRemoteConfigInfo( this.remoteUpdateInfo,this.localUpdateInfo );
			}
			else
			{
				//����δ�����κ��ļ���ʹ��Զ�̷������������б�
				alUpdateFiles = this.remoteUpdateInfo.Files;
			}
			//����׼�������ļ��¼������������б���UI��
			if( this.PrepareDownloadFiles != null )
			{
				this.PrepareDownloadFiles( new ArrayListEventArgs( this.alUpdateFiles ) );
			}

			this.objDownload.CheckFileSize = true;
			this.bDownloadFiles = true;

			foreach (FileDescription file in alUpdateFiles)
			{
				//����Ƿ���ֹ����
				if (this.bCancel)
				{
					break;
				}
				Global.WriteUpdateLog(string.Format("{0}:�����ļ�{1}...", DateTime.Now, file.FileName), true);

				this.objDownload.DownloadFileName = string.Format("{0}\\{1}", this.strLocalTempPath, file.FileName);

				if (alUpdateFiles.Count == 1 && ((FileDescription)alUpdateFiles[0]).FileName == "KPSClient.msi")
                {
					int _finded = this.remoteUpdateInfo.UpdateWebPath.LastIndexOf("/");
					if (_finded != -1)
					{
						//this.objDownload.DownloadUrl = string.Format("{0}/app/GetAppInstallation?version=", this.remoteUpdateInfo.UpdateWebPath.Substring(0, _finded)) + ((FileDescription)alUpdateFiles[0]).FileVersion;
						this.objDownload.DownloadUrl = this.remoteUpdateInfo.UpdateWebPath;
					}
				}
				else
				{
					throw new Exception("Զ���ļ��쳣��������ֹ��");
                }

					
				this.objDownload.FileSize = file.FileSize;
				this.alDone.Reset();
				//������ʼ����ĳ���ļ��¼�
				if (this.InitlizationDownloadFile != null)
				{
					this.InitlizationDownloadFile(new DownloadFileInitEventArgs(file.FileName, file.FileSize));
				}
				this.objDownload.Download();
				this.alDone.WaitOne();
				//��������ĳ���ļ�����¼�
				if (this.DownloadFileComplete != null)
				{
					this.DownloadFileComplete(new TextEventArgs(file.FileName));
				}
			}

			this.objDownload.CheckFileSize = false;
			this.bDownloadFiles = false;
			//�������ظ����ļ����
			if( this.DownloadFileOver != null )
			{
				this.DownloadFileOver( this,EventArgs.Empty );
			}
		}


		/// <summary>
		/// ���岽�����±����ļ�,���±��������ļ���Ϣ,ɾ����ʱ�ļ�
		/// </summary>
		public void UpdateLocalFiles()
		{
			Global.WriteUpdateLog( string.Format( "{0}:���±����ļ�,���±��������ļ���Ϣ,ɾ����ʱ�ļ�...",DateTime.Now ),true );

			string strPath = Global.AssemblyPath;
			string tmpPath = string.Empty;
			//���±����ļ�
			foreach( FileDescription file in this.alUpdateFiles )
			{
				if( file.ClientPath.Length > 0 )
				{	
					//����·��
                    tmpPath = string.Format( "{0}\\{1}",strPath,file.ClientPath );
					if( !Directory.Exists( tmpPath ) )
					{
						Directory.CreateDirectory( tmpPath );
					}
					File.Copy( string.Format( "{0}\\{1}",this.strLocalTempPath,file.FileName ),string.Format( "{0}\\{1}\\{2}",strPath,file.ClientPath.Replace( "/","\\" ),file.FileName ),true );
				}
				else
				{
					File.Copy( string.Format( "{0}\\{1}",this.strLocalTempPath,file.FileName ),string.Format( "{0}\\{1}",strPath,file.FileName ),true );
				}
			}
			//�������±�������¼�
			if( this.UpdateLocalFilesComplete != null )
			{
				this.UpdateLocalFilesComplete( this,EventArgs.Empty );
			}

			//���±��������ļ���Ϣ
			this.localUpdateConfigInfo.dtUpdateDate = this.remoteUpdateInfo.UpdateDate;
			this.localUpdateConfigInfo.strLastVersion = this.remoteUpdateInfo.UpdateMainVersion;
			Global.WriteUpdateUrlFile( this.localUpdateConfigInfo,this.strUpdateUrlFile );
			//���Զ�̸����ļ����������ڣ��򴴽�
			if( !File.Exists(this.strRemoteFile) )
			{
				this.strRemoteFile = string.Format( "{0}//{1}",strPath,this.localUpdateConfigInfo.strLocalConfigFile );
			}
			Global.WriteUpdateFileConfig( this.remoteUpdateInfo,this.strRemoteFile );
			//�������������ļ���Ϣ����¼�
			if( this.UpdateLocalConfigInfoComplete != null )
			{
				this.UpdateLocalConfigInfoComplete( this,EventArgs.Empty );
			}

			//ɾ����ʱ�ļ�
			foreach( FileDescription file in this.alUpdateFiles )
			{
				File.Delete( string.Format( "{0}\\{1}",this.strLocalTempPath,file.FileName ) );				
			}
			
			//����ɾ����ʱ�ļ�����¼�
			if( this.DeleteTempFileComplete != null )
			{
				this.DeleteTempFileComplete( this,EventArgs.Empty );
			}
			//������������¼�
			if( this.UpdateComplete != null )
			{
				this.UpdateComplete( this,EventArgs.Empty );
			}
		}

		public void callback()
        {
			// �ɹ��ش�
			string callback_url = this.localUpdateConfigInfo.strUpdateUrl + "api/webcloud/app/saveSchoolInfo?schoolId=" + this.localUpdateConfigInfo.schoolId + "&appVersion=" + this.remoteUpdateInfo.UpdateMainVersion;
			this.alDone.Reset();
			this.objDownload.Download(callback_url);
			this.alDone.WaitOne();
		}


		/// <summary>
		/// ȡ������
		/// </summary>
		public void Cancel()
		{
			this.bCancel = true;
		}


		#endregion

		#region �¼�����

		/// <summary>
		/// ������������¼�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void objDownload_DownloadFileComplete( object sender,System.EventArgs e )
		{
			//֪ͨ�̴߳������
			this.alDone.Set();
		}


		/// <summary>
		/// �������ؿ�����¼�
		/// </summary>
		/// <param name="e"></param>
		private void objDownload_DownloadFileBlock( DownloadFileEventArgs e )
		{
			if( this.bDownloadFiles )
			{
				if( this.DownloadFileBlockComplete != null )
				{
					this.DownloadFileBlockComplete( e );
				}
			}
		}

		#endregion
	}
}
