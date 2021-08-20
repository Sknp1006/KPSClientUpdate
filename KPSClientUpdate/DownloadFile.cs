using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Reflection;
using System.Web;
using System.Xml;

namespace AutoUpdater.UpdateHelper
{	
	/// <summary>
	/// 下载数据事件委托对象
	/// </summary>
	public delegate void DownloadFileEventHandler( DownloadFileEventArgs e );

	/// <summary>
	/// 开始下载文件委托对象
	/// </summary>
	public delegate void DownloadFileStartEventHandler( DownloadFileStartEventArgs e );

	/// <summary>
	/// 下载文件
	/// </summary>
	public class DownloadFile : System.IDisposable
	{
		#region 事件

		/// <summary>
		/// 下载文件开始事件对象
		/// </summary>
		public event DownloadFileStartEventHandler DownloadFileStart;

		/// <summary>
		/// 下载文件事件对象
		/// </summary>
		public event DownloadFileEventHandler DownloadFileBlock;

		/// <summary>
		/// 下载文件完成事件对象
		/// </summary>
		public event EventHandler DownloadFileComplete;

		#endregion

		#region 字段

		/// <summary>
		/// 欲下载的URL
		/// </summary>
		private string strUrl;

		/// <summary>
		/// 下载完成后保存的文件名称
		/// </summary>
		private string strFile;

		/// <summary>
		/// 文件大小
		/// </summary>
		private int nFileSize;

		/// <summary>
		/// 是否检查文件大小
		/// </summary>
		private bool bCheckFileSize;

		/// <summary>
		/// web请求对象
		/// </summary>
		private System.Net.HttpWebRequest objWebRequest;

		/// <summary>
		/// web接收对象
		/// </summary>
		private System.Net.HttpWebResponse objWebResponse;

		#endregion

		#region 构造函数

		/// <summary>
		/// 默认构造函数
		/// </summary>
		public DownloadFile()
		{
			this.nFileSize = 0;
			this.bCheckFileSize = false;
			this.strFile = string.Empty;
			this.strUrl = string.Empty;
		}


		#endregion

		#region 属性

		/// <summary>
		/// 下载地址
		/// </summary>
		public string DownloadUrl
		{
			get{ return this.strUrl; }
			set{ this.strUrl = value; }
		}


		/// <summary>
		/// 下载文件
		/// </summary>
		public string DownloadFileName
		{
			get{ return this.strFile; }
			set{ this.strFile = value; }
		}


		/// <summary>
		/// 文件大小
		/// </summary>
		public int FileSize
		{
			get{ return this.nFileSize; }
			set{ this.nFileSize = value; }
		}


		/// <summary>
		/// 是否检查文件大小
		/// </summary>
		public bool CheckFileSize
		{
			get{ return this.bCheckFileSize; }
			set{ this.bCheckFileSize = value; }
		}

		#endregion

		#region 方法

		/// <summary>
		/// 下载文件
		/// </summary>
		public void Download()
		{
			FileStream fs = new FileStream( this.strFile,FileMode.Create,FileAccess.Write,FileShare.ReadWrite );
			try
			{
				this.objWebRequest = (HttpWebRequest)WebRequest.Create( this.strUrl );
				this.objWebRequest.Headers.Add("Accept-Encoding", "identity");  // 
				this.objWebRequest.AllowAutoRedirect = true;
//				int nOffset = 0;
				long nCount = 0;
				byte[] buffer = new byte[ 1024 * 1024 ];	//1MB
				int nRecv = 0;	//接收到的字节数
				this.objWebResponse = (HttpWebResponse)this.objWebRequest.GetResponse();
				Stream recvStream = this.objWebResponse.GetResponseStream();
				long nMaxLength = (int)this.objWebResponse.ContentLength;
				if (nMaxLength == -1)
                {
					nMaxLength = this.nFileSize;

				}
                else
                {
					if (this.bCheckFileSize && nMaxLength != this.nFileSize)
					{
						throw new Exception(string.Format("文件\"{0}\"被损坏,无法下载!", Path.GetFileName(this.strFile)));
					}
				}

                if ( this.DownloadFileStart != null )
					this.DownloadFileStart( new DownloadFileStartEventArgs( (int)nMaxLength ) );
				while( true )
				{
					nRecv = recvStream.Read( buffer,0,buffer.Length );
					if( nRecv == 0 )
						break;
					fs.Write( buffer,0,nRecv );
					nCount += nRecv;
					//引发下载块完成事件
					if( this.DownloadFileBlock != null )
						this.DownloadFileBlock( new DownloadFileEventArgs( (int)nMaxLength,(int)nCount ) );
				}
				recvStream.Close();	
				//引发下载完成事件
				if( this.DownloadFileComplete != null )
					this.DownloadFileComplete( this,EventArgs.Empty );
			}
			finally
			{
				fs.Close();
//				int nTmp = (int)(new FileInfo( this.strFile ).Length );
//				if( this.nFileSize != nTmp )
//				{
//					//File.Delete( this.strFile );
//					System.Windows.Forms.MessageBox.Show( string.Format( "{0}大小不正确.{1},{2}",this.strFile,this.nFileSize,nTmp ) );
//					throw new Exception( string.Format( "文件\"{0}\"被损坏,无法下载!",Path.GetFileName( this.strFile ) ) );
//				}
//
			}
		}


		#endregion

		#region IDisposable 成员

		public void Dispose()
		{
			if( this.objWebRequest != null )
				this.objWebRequest = null;
			if( this.objWebResponse != null )
				this.objWebResponse.Close();
		}

		#endregion
	}

	/// <summary>
	/// 开始下载文件事件
	/// </summary>
	public class DownloadFileStartEventArgs : System.EventArgs 
	{

		#region 事件数据

		private int nFileSize;

		public DownloadFileStartEventArgs( int filesize )
		{
			this.nFileSize = filesize;
		}


		/// <summary>
		/// 文件长度
		/// </summary>
		public int FileSize
		{
			get{ return this.nFileSize; }
		}

		#endregion
	}

	/// <summary>
	/// 下载文件数据事件
	/// </summary>
	public class DownloadFileEventArgs : System.EventArgs 
	{
		#region 事件数据

		/// <summary>
		/// 文件大小
		/// </summary>
		private int nFileSize;

		/// <summary>
		/// 当前下载数量
		/// </summary>
		private int nDownloads;

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="filesize">文件大小</param>
		/// <param name="downloads">当前下载数量</param>
		public DownloadFileEventArgs( int filesize,int downloads )
		{
			this.nFileSize = filesize;
			this.nDownloads = downloads;
		}


		/// <summary>
		/// 文件大小
		/// </summary>
		public int FileSize
		{
			get{ return this.nFileSize; }
			set{ this.nFileSize = value; }
		}


		/// <summary>
		/// 当前下载数量
		/// </summary>
		public int Downloads
		{
			get{ return this.nDownloads; }
			set{ this.nDownloads = value; }
		}

		#endregion
	}
}
