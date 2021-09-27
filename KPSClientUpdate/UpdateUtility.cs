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
	/// 执行更新
	/// </summary>
	/// <remarks>
	///		保存临时文件的目录为:System.Environment.GetFolderPath( System.Environment.SpecialFolder.ApplicationData )
	///		即当前登陆用户的Application Data目录
	/// </remarks>
	public class UpdateUtility
	{
		#region 事件

		/// <summary>
		/// 初始化下载信息完成事件
		/// </summary>
		/// <remarks>
		///		此事件传递以ArrayList表示的应关闭的关联应用程序列表
		/// </remarks>
		public event ArrayListEventHandler InializationDownloadInfoComplete;

		/// <summary>
		/// 下载协议内容完成事件
		/// </summary>
		/// <remarks>
		///		此事件传递协议内容
		/// </remarks>
		public event TextEventHandler DownloadLicenceInfoComplete;

		/// <summary>
		/// 下载更新记录信息完成事件
		/// </summary>
		/// <remarks>
		///		此事件传递更新历史记录内容
		/// </remarks>
		public event TextEventHandler DownloadHistoryInfoComplete;

		/// <summary>
		/// 准备下载文件事件
		/// </summary>
		/// <remarks>
		///		 此事件传递待下载文件列表,类型为FileDescription的集合
		/// </remarks>
		public event ArrayListEventHandler PrepareDownloadFiles;

		/// <summary>
		/// 准备开始下载文件事件
		/// </summary>
		/// <remarks>
		///		传递准备下载文件的大小及名称
		/// </remarks>
		public event DownloadFileInitEventHandler InitlizationDownloadFile;

		/// <summary>
		/// 下载文件块完成
		/// </summary>
		/// <remarks>
		///		传递下载文件的大小及当前下载大小
		/// </remarks>
		public event DownloadFileEventHandler DownloadFileBlockComplete;

        /// <summary>
        /// 下载文件完成
        /// </summary>
        /// <remarks>
        ///		传递下载完成的文件名称
        /// </remarks>
		public event TextEventHandler DownloadFileComplete;

		/// <summary>
		/// 下载更新文件结束
		/// </summary>
		public event EventHandler DownloadFileOver;

		/// <summary>
		/// 更新本地文件完成
		/// </summary>
		public event EventHandler UpdateLocalFilesComplete;

		/// <summary>
		/// 更新本地配置信息完成
		/// </summary>
		public event EventHandler UpdateLocalConfigInfoComplete;

		/// <summary>
		/// 删除临时文件完成
		/// </summary>
		public event EventHandler  DeleteTempFileComplete;

		/// <summary>
		/// 更新完成
		/// </summary>
		public event EventHandler UpdateComplete;

		#endregion

		#region 字段

		/// <summary>
		/// 本地更新配置信息文件
		/// </summary>
		private string strUpdateUrlFile;

		/// <summary>
		/// 远程更新配置文件名称
		/// </summary>
		private string strRemoteFile;

		/// <summary>
		/// 保存远程更新文件的临时文件
		/// </summary>
		private string strTmpRemoteFile;
	
		/// <summary>
		/// 线程事件对象
		/// </summary>
		private System.Threading.ManualResetEvent alDone;

		/// <summary>
		/// 是否取消操升级
		/// </summary>
		private bool bCancel;

		/// <summary>
		/// 本地更新配置信息
		/// </summary>
		private UpdateUrlConfig localUpdateConfigInfo;

		/// <summary>
		/// 本地更新信息
		/// </summary>
		private UpdateFileConfig localUpdateInfo;

		/// <summary>
		/// 远程更新信息
		/// </summary>
		private UpdateFileConfig remoteUpdateInfo;

		/// <summary>
		/// 下载文件对象
		/// </summary>
		private DownloadFile objDownload;

		/// <summary>
		/// 本地存储文件的临时目录
		/// </summary>
		private string strLocalTempPath;

		/// <summary>
		/// 下载的更新文件列表
		/// </summary>
		private ArrayList alUpdateFiles;

		/// <summary>
		/// 是否需要更新
		/// </summary>
		private bool bEnable;

		/// <summary>
		/// 标记是否正在下载更新文件
		/// </summary>
		/// <remarks>
		///		以此值来确定是否引发DownloadFileBlock事件
		/// </remarks>
		private bool bDownloadFiles;

		#endregion

		#region 构造函数
		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="configFile">配置文件名称</param>
		/// <param name="remoteFile">远程更新文件用于保存在本地的文件名称</param>
		/// <remarks>
		///		以上两个文件均包含路径
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


		#region 属性

		/// <summary>
		/// 是否需要更新
		/// </summary>
		public bool Enable
		{
			get{ return this.bEnable; }
		}
		

		/// <summary>
		/// 远程更新信息
		/// </summary>
		public UpdateFileConfig RemoteConfigInfo
		{
			get{ return this.remoteUpdateInfo; }
		}


		/// <summary>
		/// 本地更新配置信息
		/// </summary>
		public UpdateUrlConfig LocalUpdateUrlConfigInfo
		{
			get
			{
				return this.localUpdateConfigInfo;
			}
		}

		#endregion

		#region 方法

		/// <summary>
		/// 第一步：初始化更新信息
		/// </summary>
		/// <remarks>
		///		读取本地更新配置信息，获取远程服务器的更新配置文件，并读取远程更新配置信息
		/// </remarks>
		public void InitUpdateInfo()
		{
			Global.WriteUpdateLog( string.Format( "{0}:初始化更新信息...",DateTime.Now ),true );
			//读取本地更新配置信息
			this.localUpdateConfigInfo = Global.ParseUpdateUrlConfig( this.strUpdateUrlFile );

			//设置远程更新配置文件的本地复本
			//this.strRemoteFile = string.Format( "{0}\\{1}",Global.AssemblyPath,this.localUpdateConfigInfo.strLocalConfigFile );

			//if( File.Exists( this.strRemoteFile ) )
			//{				
			//	//读取本地存储的远程更新配置信息
			//	this.localUpdateInfo = Global.ParseUpdateFileConfig( this.strRemoteFile );
			//}
			//else
			//{
			//	//本地无远程更新配置信息复本，必须更新
			//	this.bEnable = true;
			//}
			
			this.strTmpRemoteFile = Global.GetRandomFile( strLocalTempPath,string.Empty,"bin" );

			//读取远程更新文件信息
			//从远程服务器下载远程更新文件信息			
			this.objDownload.DownloadUrl = this.localUpdateConfigInfo.strUpdateUrl + "api/webcloud/app/getLatestAppPath?schoolId=" + this.localUpdateConfigInfo.schoolId + "&localVersion=" + this.localUpdateConfigInfo.strLastVersion;  // 文件配置信息
			this.objDownload.DownloadFileName = strTmpRemoteFile;
			this.objDownload.strAuthorization = this.localUpdateConfigInfo.Parameters;  // token

			this.alDone.Reset();
			
			this.objDownload.Download();  // 下载远程更新配置信息（临时）

			this.alDone.WaitOne();	//等待线程直到完成

			this.remoteUpdateInfo = Global.ParseUpdateFileConfig( strTmpRemoteFile );  //远程更新配置信息

            //检查是否必须更新
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
				// 比较版本号，如果本地版小于服务器版
				Version local = new Version(this.localUpdateConfigInfo.strLastVersion);
				Version remote = new Version(this.remoteUpdateInfo.UpdateMainVersion);
				if (local < remote)
				{
					AutoUpdater.UpdateHelper.Global.WriteUpdateLog(string.Format("{0}:本地版本号不是最新，需要更新。", DateTime.Now), true);
					this.bEnable = true;
				}
            }
            else
            {
				AutoUpdater.UpdateHelper.Global.WriteUpdateLog(string.Format("{0}:本地版本无需更新。", DateTime.Now), true);
			}

            //处此引发初始完成，并用事件通知主程序需关闭的UI程序列表,然后由UI关闭关联的应用程序
            if ( this.InializationDownloadInfoComplete != null )
			{
				this.InializationDownloadInfoComplete( new ArrayListEventArgs( Global.ParseCloseApplications( this.remoteUpdateInfo ) ) );
			}

			//删除保存远程更新文件的临时文件
			File.Delete( this.strTmpRemoteFile );
		}


		/// <summary>
		/// 第二步：显示协议信息
		/// </summary>
		public void DisplayLicenceInfo()
		{
			Global.WriteUpdateLog( string.Format( "{0}:显示协议信息...",DateTime.Now ),true );
			//如果没有协议信息，则跳过此项
			string strAgreementInfo = string.Empty;
			if( this.remoteUpdateInfo.LicenceFile.Length > 0 )
			{
				//保存协议信息的临时文件
				string strTmpAgreementFile = Global.GetRandomFile( this.strLocalTempPath,string.Empty,"tmp" );
				try
				{
					//从远程服务器下载协议信息
					this.objDownload.DownloadUrl = string.Format( "{0}/{1}",this.remoteUpdateInfo.UpdateWebPath,this.remoteUpdateInfo.LicenceFile );
					this.objDownload.DownloadFileName = strTmpAgreementFile;

					this.alDone.Reset();
					//创建线程下载文件
					this.objDownload.Download();
					this.alDone.WaitOne();	//等待线程直到完成

					//读取协议信息
					StreamReader sr = new StreamReader( strTmpAgreementFile,System.Text.Encoding.Default );
					strAgreementInfo = sr.ReadToEnd();
					sr.Close();
				}
				finally
				{
					//删除保存协议信息的临时文件
					File.Delete( strTmpAgreementFile );
				}
			}
			//引发显示协议信息事件
			if( this.DownloadLicenceInfoComplete != null )
			{
				this.DownloadLicenceInfoComplete( new TextEventArgs( strAgreementInfo ) );
			}
		}


		/// <summary>
		/// 第三步：显示更新历史记录信息
		/// </summary>
		public void DisplayHistoryInfo()
		{
			Global.WriteUpdateLog( string.Format( "{0}:显示更新历史记录信息...",DateTime.Now ),true );
			//如果没有协议信息，则跳过此项
			string strHistoryInfo = string.Empty;
			if( this.remoteUpdateInfo.HistoryFile.Length > 0 )
			{
				//保存协议信息的临时文件
				string strTmpFile = Global.GetRandomFile( strLocalTempPath,string.Empty,"tmp" );
				try
				{
					//从远程服务器下载协议信息
					this.objDownload.DownloadUrl = string.Format( "{0}/{1}",this.remoteUpdateInfo.UpdateWebPath,this.remoteUpdateInfo.HistoryFile );
					this.objDownload.DownloadFileName = strTmpFile;
					this.alDone.Reset();
					//创建线程下载文件
					this.objDownload.Download();
//					new Thread( new ThreadStart( objDownload.Download ) ).Start();
					this.alDone.WaitOne();	//等待线程直到完成
					//读取协议信息
					StreamReader sr = new StreamReader( strTmpFile,System.Text.Encoding.Default );
					strHistoryInfo = sr.ReadToEnd();
					sr.Close();
				}
				finally
				{
					//删除保存协议信息的临时文件
					File.Delete( strTmpFile );
				}
			}
            else
            {
				strHistoryInfo = "暂无历史更新信息";

			}
			//引发显示更新历史记录信息事件
			if( this.DownloadHistoryInfoComplete != null )
			{
				this.DownloadHistoryInfoComplete( new TextEventArgs( strHistoryInfo ) );
			}
		}


		/// <summary>
		/// 第四步：下载更新文件
		/// </summary>
		public void DownloadUpdateFiles()
		{
			Global.WriteUpdateLog( string.Format( "{0}:下载更新文件...",DateTime.Now ),true );

			//获取欲下载的文件列表			
			if( this.localUpdateInfo != null )	
			{
				// 对比本地和服务器文件差异（全量更新不需要）
				this.alUpdateFiles = Global.CompareLocalAndRemoteConfigInfo( this.remoteUpdateInfo,this.localUpdateInfo );
			}
			else
			{
				//本地未下载任何文件，使用远程服务器的下载列表
				alUpdateFiles = this.remoteUpdateInfo.Files;
			}
			//引发准备下载文件事件，传递下载列表至UI层
			if( this.PrepareDownloadFiles != null )
			{
				this.PrepareDownloadFiles( new ArrayListEventArgs( this.alUpdateFiles ) );
			}

			this.objDownload.CheckFileSize = true;
			this.bDownloadFiles = true;

			foreach (FileDescription file in alUpdateFiles)
			{
				//检测是否终止下载
				if (this.bCancel)
				{
					break;
				}
				Global.WriteUpdateLog(string.Format("{0}:下载文件{1}...", DateTime.Now, file.FileName), true);

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
					throw new Exception("远程文件异常，下载终止！");
                }

					
				this.objDownload.FileSize = file.FileSize;
				this.alDone.Reset();
				//引发开始下载某个文件事件
				if (this.InitlizationDownloadFile != null)
				{
					this.InitlizationDownloadFile(new DownloadFileInitEventArgs(file.FileName, file.FileSize));
				}
				this.objDownload.Download();
				this.alDone.WaitOne();
				//引发下载某个文件完成事件
				if (this.DownloadFileComplete != null)
				{
					this.DownloadFileComplete(new TextEventArgs(file.FileName));
				}
			}

			this.objDownload.CheckFileSize = false;
			this.bDownloadFiles = false;
			//引发下载更新文件完成
			if( this.DownloadFileOver != null )
			{
				this.DownloadFileOver( this,EventArgs.Empty );
			}
		}


		/// <summary>
		/// 第五步：更新本地文件,更新本地配置文件信息,删除临时文件
		/// </summary>
		public void UpdateLocalFiles()
		{
			Global.WriteUpdateLog( string.Format( "{0}:更新本地文件,更新本地配置文件信息,删除临时文件...",DateTime.Now ),true );

			string strPath = Global.AssemblyPath;
			string tmpPath = string.Empty;
			//更新本地文件
			foreach( FileDescription file in this.alUpdateFiles )
			{
				if( file.ClientPath.Length > 0 )
				{	
					//创建路径
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
			//引发更新本地完成事件
			if( this.UpdateLocalFilesComplete != null )
			{
				this.UpdateLocalFilesComplete( this,EventArgs.Empty );
			}

			//更新本地配置文件信息
			this.localUpdateConfigInfo.dtUpdateDate = this.remoteUpdateInfo.UpdateDate;
			this.localUpdateConfigInfo.strLastVersion = this.remoteUpdateInfo.UpdateMainVersion;
			Global.WriteUpdateUrlFile( this.localUpdateConfigInfo,this.strUpdateUrlFile );
			//如果远程更新文件复本不存在，则创建
			if( !File.Exists(this.strRemoteFile) )
			{
				this.strRemoteFile = string.Format( "{0}//{1}",strPath,this.localUpdateConfigInfo.strLocalConfigFile );
			}
			Global.WriteUpdateFileConfig( this.remoteUpdateInfo,this.strRemoteFile );
			//引发更新配置文件信息完成事件
			if( this.UpdateLocalConfigInfoComplete != null )
			{
				this.UpdateLocalConfigInfoComplete( this,EventArgs.Empty );
			}

			//删除临时文件
			foreach( FileDescription file in this.alUpdateFiles )
			{
				File.Delete( string.Format( "{0}\\{1}",this.strLocalTempPath,file.FileName ) );				
			}
			
			//引发删除临时文件完成事件
			if( this.DeleteTempFileComplete != null )
			{
				this.DeleteTempFileComplete( this,EventArgs.Empty );
			}
			//引发更新完成事件
			if( this.UpdateComplete != null )
			{
				this.UpdateComplete( this,EventArgs.Empty );
			}
		}

		public void callback()
        {
			// 成功回传
			string callback_url = this.localUpdateConfigInfo.strUpdateUrl + "api/webcloud/app/saveSchoolInfo?schoolId=" + this.localUpdateConfigInfo.schoolId + "&appVersion=" + this.remoteUpdateInfo.UpdateMainVersion;
			this.alDone.Reset();
			this.objDownload.Download(callback_url);
			this.alDone.WaitOne();
		}


		/// <summary>
		/// 取消更新
		/// </summary>
		public void Cancel()
		{
			this.bCancel = true;
		}


		#endregion

		#region 事件处理

		/// <summary>
		/// 处理下载完成事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void objDownload_DownloadFileComplete( object sender,System.EventArgs e )
		{
			//通知线程处理完成
			this.alDone.Set();
		}


		/// <summary>
		/// 处理下载块完成事件
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
